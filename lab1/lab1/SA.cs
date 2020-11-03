using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;


namespace lab1
{
    class SA
    {

        private VRP vrp;

        public int[] SA_Algorithm(int[] permutation, string[,] distances, string[] cities, string unit, int count)
        {

            vrp = new VRP();

            double T_end = 0.01;
            double T = 100;
            int L = count;
            double sum, newSum;

            double e = 2.71828;

            int[] newPermutation = permutation;
            int[] finalPermutation = permutation;

            int i, j;

            while (T > T_end) 
            { 
                foreach (int p in permutation)
                {
                    i = new Random().Next(0, permutation.Length - 1);
                    j = new Random().Next(0, permutation.Length - 1);

                    newPermutation = swap(permutation, i, j);

                    sum = vrp.calculate(permutation, distances, cities, unit);
                    newSum = vrp.calculate(newPermutation, distances, cities, unit);

                    if (newSum > sum)
                    {
                        double r = new Random().NextDouble();
                        double delta = sum - newSum;

                        if (r >= (Math.Pow(e, ((double)delta / T))))
                        {
                            permutation = newPermutation;
                        }

                    }

                    permutation = newPermutation;
                    if (vrp.calculate(permutation, distances, cities, unit) 
                        < vrp.calculate(finalPermutation, distances, cities, unit))
                    {
                        finalPermutation = permutation;
                    }


                }

                T = reduceTemperature(T);
            }

            return finalPermutation;
        }

        private int[] swap (int[] permutation, int i, int j)
        {
            int temp = permutation[i];
            permutation[i] = permutation[j];
            permutation[j] = temp;

            return permutation;
        }

        private double reduceTemperature (double T)
        {
            return 0.97 * T;
        }
        
    }
}
