using System;
using System.Collections.Generic;
using System.Text;
using IronXL;
using System.Linq;
using IronXL.Xml.Wordprocessing;

namespace lab1
{
    class VRP
    {

        private static WorkBook workbook = WorkBook.Load("PLdata.csv");
        private WorkSheet sheet = workbook.WorkSheets.First();
        char[] alpha = "ABC".ToCharArray();

        public string[] readLength()
        {
           
            int count = sheet["A1"].IntValue;

            string[] length = new string[count];
  
            string range = "B2:B26";

            int k = 0;
            foreach (var s in sheet[range])
            {
                length[k] = s.Text;
                k++;
            }

            return length;
        }

        public string[] readWeight ()
        {

            int count = sheet["A1"].IntValue;
            string[] weigth = new string[count];
            string range = "C2:C26";
            int k = 0;

            foreach (var s in sheet[range])
            {
                weigth[k] = s.Text;
                k++;
            }

            return weigth;

        }

        public double calculate (int[] permutation, string[,] distances, string[] cities, string unit)
        {
           double sum = 0;
           for (int p = 1; p < permutation.Length; p++)
           {
               int k = p - 1;
               int lower = permutation[k] - 1;
               int upper = permutation[p] - 1;
                
                sum += Convert.ToDouble(distances[lower, upper]);

           }

            return sum;

        }

        public void calculateWithCapacity(int[] permutation, string[,] distances, string[] cities, 
                                            string unit)
        {

            Console.WriteLine("Podaj typ pojazdu (t lub c): ");
            string car = Console.ReadLine();

            double maxLength;
            double maxWeigth;
            
            if (car == "t")
            {
                maxLength = 16.6;
                maxWeigth = 24000;
            } else if (car == "c")
            {
                maxLength = 7.8;
                maxWeigth = 8000;
            } else
            {
                Console.WriteLine("Wprowadzono niepoprawny pojazd");
                return;
            }

            string[] lenght = readLength();
            string[] weigth = readWeight();

            double sum = 0;
            double calculatedLength = 0;
            double calculatedWeigth = 0;
           
            for (int p = 1; p < permutation.Length; p++)
            {
                int k = p - 1;
                int lower = permutation[k] - 1;
                int upper = permutation[p] - 1;

                if (calculatedLength > maxLength || calculatedWeigth > maxWeigth)
                {
                    sum += Convert.ToDouble(distances[lower, permutation[0] - 1]);
                    calculatedLength = 0;
                    calculatedWeigth = 0;
                    sum += Convert.ToDouble(distances[permutation[0] - 1, upper]);
                    calculatedLength += Convert.ToDouble(lenght[upper]);
                    calculatedWeigth += Convert.ToDouble(weigth[upper]);
                } else
                {
                    sum += Convert.ToDouble(distances[lower, upper]);
                    calculatedLength += Convert.ToDouble(lenght[upper]);
                    calculatedWeigth += Convert.ToDouble(weigth[upper]);
                }

            }

            Console.WriteLine("Przebyta odległość: " + sum + unit);
        }

        public void calculateWithTimeLimit(int[] permutation, string[,] distances, string[] cities, string unit, int count)
        {
            double[,] times = calculateTime(distances, count);
            
            double maxTime = 9;
            double shortBreak = 4.5;
            double longBreak = 11;

            bool isShortBreak = false;

            double sumTime = 0;
            double sumRoute = 0;

            int numberOfCar = 1;
        

            for (int p = 1; p < permutation.Length; p++)
            {
                int k = p - 1;
                int lower = permutation[k] - 1;
                int upper = permutation[p] - 1;

                if (sumTime > shortBreak && !isShortBreak)
                {
                    sumTime += 0.75;
                    sumRoute += Convert.ToDouble(distances[lower, upper]);
                    isShortBreak = true;
                } else if (sumTime > maxTime)
                {
                    numberOfCar++;
                    sumTime += Convert.ToDouble(times[lower, upper]);
                    sumRoute += Convert.ToDouble(distances[lower, upper]);
                }
                else
                {
                    sumTime += Convert.ToDouble(times[lower, upper]);
                    sumRoute += Convert.ToDouble(distances[lower, upper]);
                }

            }

            Console.WriteLine("Przebyta droga: " + sumRoute);
            Console.WriteLine("Czas: " + sumTime);
            Console.WriteLine("Liczba samochodów: " + numberOfCar);
        }

        public double[,] calculateTime(string[,] distances, int count)
        {
            double[,] times = new double[count, count];

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1; j++)
                {
                    if (Convert.ToDouble(distances[i, j]) > 100)
                    {
                        times[i, j] = (double)(Convert.ToDouble(distances[i, j]) / 80);
                    }
                    else
                    {
                        times[i, j] = (double)(Convert.ToDouble(distances[i, j]) / 60);
                    }
                }
            }

            return times;
        }



    }

    
}
