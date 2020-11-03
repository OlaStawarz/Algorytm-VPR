using System;
using IronXL;
using System.Linq;

namespace lab1
{
    class Program
    {
        public static double shortestDistance = 0;
        public static double longestDistance = 0;
        public static int[] permutation;
        public static int[] randomPermutation;

        public static WorkBook workbook;
        public static WorkSheet sheet;

        public static string[,] distances;
        public static string[] cities;
        public static int count;
        public static string unit;

        static void Main(string[] args)
        {

            readData();            

            permutation = new int[count];

            for (int i = 0; i < count; i++)
            {
                permutation[i] = i + 1;
            }

            VRP vrp = new VRP();
            SA sa = new SA();

            Random random = new Random();
            randomPermutation = permutation.OrderBy(x => random.Next()).ToArray();

            Console.WriteLine("Podstawowy algorytm: " + vrp.calculate(randomPermutation, distances, cities, unit));

            Console.WriteLine("Ograniczenie pojemnościowe");
            vrp.calculateWithCapacity(permutation, distances, cities, unit);
            Console.WriteLine("Ograniczenie czasowe");
            vrp.calculateWithTimeLimit(permutation, distances, cities, unit, count);

            Console.WriteLine("Algorytm 2/3 odlegości: ");
            greedyAlgorithm();

            Console.WriteLine("SA");
            randomPermutation = sa.SA_Algorithm(randomPermutation, distances, cities, unit, count);

            greedyAlgorithm();
               
          
        }

        public static void readData ()
        {
            workbook = WorkBook.Load("PL.csv");
            sheet = workbook.WorkSheets.First();

            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            count = sheet["A1"].IntValue;
            unit = sheet["A2"].StringValue;

            distances = new string[count, count];
            cities = new string[count];

            for (int i = 0; i < count; i++)
            {
                string upperBound = Char.ToString(alpha[i]) + (count + 2);
                string range = Char.ToString(alpha[i]) + "3:" + upperBound;

                int k = 0;
                foreach (var cell in sheet[range])
                {
                    distances[k, i] = cell.Text;
                    k++;
                }

            }

            string range2 = "B" + (count + 3) + ":B" + (2 * count + 2);
            int w = 0;
            foreach (var cell in sheet[range2])
            {
                cities[w] = cell.Text;
                w++;
            }
        }

        public static void greedyAlgorithm() {


            int[] copiedPermutation = randomPermutation;
            int baseCity = randomPermutation[0];
            int index = randomPermutation[0];
            double sum = 0;

            randomPermutation = randomPermutation.Where(val => val != baseCity).ToArray();

            int numberOfCar = 1;
            index = countLongestDistance(index);

           /* Console.WriteLine(numberOfCar + " samochód");
            Console.WriteLine(cities[baseCity - 1]);
            Console.WriteLine(cities[index - 1]);*/
            sum += longestDistance;

            int temp = index;
            int pom = 0;

            while (randomPermutation.Length != 1)
            {

                if (shortestDistance > ((double)2 / 3) * longestDistance)
                {
                    numberOfCar++;
                    sum += longestDistance;
                    /*Console.WriteLine(cities[baseCity - 1]);
                    Console.WriteLine();
                    Console.WriteLine(numberOfCar + " samochód");
                    Console.WriteLine(cities[baseCity - 1]);*/

                    index = countLongestDistance(baseCity);
                    //Console.WriteLine(cities[index - 1]);
                    sum += longestDistance;

                    index = countShortestDistance(index);

                    if (shortestDistance <= ((double)2 / 3) * longestDistance || randomPermutation.Length == 1)
                    {
                        //Console.WriteLine(cities[index - 1]);
                        sum += shortestDistance;
                    }

                }
                else
                {

                    if (!(shortestDistance == 0))
                        randomPermutation = randomPermutation.Where(val => val != index).ToArray();

                    index = countShortestDistance(index);
                    pom = index;

                    if (shortestDistance <= ((double)2 / 3) * longestDistance)
                    {
                        //Console.WriteLine(cities[index - 1]);
                        sum += shortestDistance;
                    }
                    pom = 0;

                }

            }

            //Console.WriteLine(cities[baseCity - 1]);
            sum += Convert.ToDouble(distances[randomPermutation[0] - 1, baseCity - 1]);
            //Console.WriteLine();
            Console.WriteLine("Liczba samochodów: " + numberOfCar);
            Console.WriteLine("Długość trasy: " + sum);
           /* Console.WriteLine();
            Console.WriteLine();*/


            randomPermutation = copiedPermutation;
            shortestDistance = 0;
            longestDistance = 0;

        }

        public static int countShortestDistance(int baseCity)
        {
            int index = 0;
            shortestDistance = double.MaxValue;

            for (int j = 0; j < randomPermutation.Length; j++)
            {
                if (Convert.ToDouble(distances[baseCity - 1, randomPermutation[j] - 1]) < shortestDistance)
                {
                    shortestDistance = Convert.ToDouble(distances[baseCity - 1, randomPermutation[j] - 1]);
                   // Console.WriteLine(permutation[j]);
                    index = randomPermutation[j];

                }
            }

            /*Console.WriteLine(shortestDistance);
            Console.WriteLine(index);*/
            return index;
        }

        public static int countLongestDistance (int baseCity)
        {

            int index = 0;
            longestDistance = 0;

            for (int i = 0; i < randomPermutation.Length; i++)
            {
                if (Convert.ToDouble(distances[baseCity - 1, randomPermutation[i] - 1]) > longestDistance)
                {
                    longestDistance = Convert.ToDouble(distances[baseCity - 1, randomPermutation[i] - 1]);
                  //  Console.WriteLine(permutation[i]);
                    index = randomPermutation[i];

                }
            }

            /*Console.WriteLine(longestDistance);
            Console.WriteLine(index);*/
            randomPermutation = randomPermutation.Where(val => val != index).ToArray();
            return index;
        }


    }




}
