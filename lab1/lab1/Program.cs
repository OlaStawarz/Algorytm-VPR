using System;
using IronXL;
using System.Linq;

namespace lab1
{
    class Program
    {
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

            /*for (int i = 0; i < count; i++)
            {
                Console.WriteLine(cities[i]);
            }*/

            int[] permutation = new int[] { 5, 10, 4, 12, 5 };

            double sum = 0;
            for (int p = 1; p < permutation.Length; p++)
            {
                int k = p - 1;
                int lower = permutation[k] - 1;
                int upper = permutation[p] - 1;

                Console.WriteLine(distances[lower, upper]);
                sum += Convert.ToDouble(distances[lower, upper]);

            }

            Console.WriteLine(sum);

        }
    }
}
