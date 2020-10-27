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

        static void Main(string[] args)
        {
            WorkBook workbook = WorkBook.Load("PL.csv");
            WorkSheet sheet = workbook.WorkSheets.First();

            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                  
            int count = sheet["A1"].IntValue;
            string unit = sheet["A2"].StringValue;
            
            string[,] distances = new string[count, count];
            string[] cities = new string[count];

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


            /*for (int i = 0; i < count; i++)
            {
                Console.WriteLine("________");
                for (int j = 0; j < count; j++)
                {
                    Console.WriteLine(distances[i, j]);
                }
            }*/

           /* for (int i = 0; i < count; i++)
            {
                Console.WriteLine(cities[i]);
            }*/

            //UWAGA - w ostatniej iteracji jest błąd!!
            permutation = new int[] { 5, 2, 1, 3, 4 };

            for (int i = 0; i < permutation.Length; i++)
            {
                int baseCity = permutation[i];
                int index = permutation[i];

                permutation = permutation.Where(val => val != baseCity).ToArray();

                int numberOfCar = 1;
                index = countLongestDistance(distances, index);

                Console.WriteLine(numberOfCar + " samochód");
                Console.WriteLine(cities[baseCity - 1]);
                Console.WriteLine(cities[index - 1]);

                while (permutation.Length != 1)
                {

                    if (shortestDistance > ((double)2 / 3) * longestDistance)
                    {
                        numberOfCar++;
                        Console.WriteLine(cities[baseCity - 1]);
                        Console.WriteLine();
                        Console.WriteLine(numberOfCar + " samochód");
                        Console.WriteLine(cities[baseCity - 1]);

                        index = countLongestDistance(distances, baseCity);
                        Console.WriteLine(cities[index - 1]);
                        index = countShortestDistance(distances, index);
                        
                        if (shortestDistance <= ((double)2 / 3) * longestDistance || permutation.Length == 1)
                            Console.WriteLine(cities[index - 1]);
                    }
                    else
                    {

                        if (!(shortestDistance == 0))
                            permutation = permutation.Where(val => val != index).ToArray();

                        index = countShortestDistance(distances, index);

                        if (shortestDistance <= ((double)2 / 3) * longestDistance)
                            Console.WriteLine(cities[index - 1]);
                        
                    }

                }

                Console.WriteLine(cities[baseCity - 1]);
                Console.WriteLine();
                Console.WriteLine("Liczba samochodów: " + numberOfCar);
                Console.WriteLine();
                Console.WriteLine();
                permutation = new int[] { 5, 2, 1, 3, 4 };
            }

            
           


            /*Console.WriteLine(newDistances.Length);
            foreach (double d in newDistances)
            {
                Console.WriteLine(d);
            }*/


            /*double longestDistance = 0; 
            double sum = 0;
            for (int p = 1; p < permutation.Length; p++)
            {
                int k = p - 1;
                int lower = permutation[k] - 1;
                int upper = permutation[p] - 1;
                Console.WriteLine(distances[lower, upper]);
                //sum += Convert.ToDouble(distances[lower, upper]);
                if (Convert.ToDouble(distances[lower, upper]) > longestDistance)
                {
                    longestDistance = Convert.ToDouble(distances[lower, upper]);
                }

            }

            Console.WriteLine(longestDistance);*/
            /*Console.WriteLine("Trasa: ");
            foreach (int i in permutation) {
                Console.WriteLine(cities[i-1]);
            }*/

            //Console.WriteLine("Przebyta odległość: " + sum + unit);

        }

        public static int countShortestDistance(string[,] distances, int baseCity)
        {
            int index = 0;
            shortestDistance = double.MaxValue;

            for (int j = 0; j < permutation.Length; j++)
            {
                if (Convert.ToDouble(distances[baseCity - 1, permutation[j] - 1]) < shortestDistance)
                {
                    shortestDistance = Convert.ToDouble(distances[baseCity - 1, permutation[j] - 1]);
                   // Console.WriteLine(permutation[j]);
                    index = permutation[j];

                }
            }

            /*Console.WriteLine(shortestDistance);
            Console.WriteLine(index);*/
            return index;
        }

        public static int countLongestDistance (string[,] distances, int baseCity)
        {

            int index = 0;
            longestDistance = 0;

            for (int i = 0; i < permutation.Length; i++)
            {
                if (Convert.ToDouble(distances[baseCity - 1, permutation[i] - 1]) > longestDistance)
                {
                    longestDistance = Convert.ToDouble(distances[baseCity - 1, permutation[i] - 1]);
                  //  Console.WriteLine(permutation[i]);
                    index = permutation[i];

                }
            }

            /*Console.WriteLine(longestDistance);
            Console.WriteLine(index);*/
            permutation = permutation.Where(val => val != index).ToArray();
            return index;
        }


    }




}
