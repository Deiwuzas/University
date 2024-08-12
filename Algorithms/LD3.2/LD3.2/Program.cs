using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LD3._2
{
    internal class Program
    {
        static int n = 25;
        static int k = 20;
        static int maxSizeGLOBAL = n - k + 1;
        static void Main(string[] args)
        {
            //int[] RandomSequence = {18, 5, 2, 8,7,4,5,3};
            int[] RandomSequence = new int[n];
			Random r = new Random();
			for (int i = 0; i < n; i++)
            {
                
                int num = r.Next(1, 10);
                RandomSequence[i] = num;
            }

            Console.WriteLine("Duomenys:");
            foreach(int num in RandomSequence)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
                                                                       //// {9 5 2 8} n = 4, k = 3
            int[] minChapterSize = new int[k];                         ////1- 9 5 2 8 - 2 {9} {9 5}
            string[] minChapters = new string[k];                      ////2- {5 2 8} -max 2 { 2 8 } - max 1
            int[] chapterSize = new int[k];                            ////3- { 2 8 } - max 2 { 8 } - max 1
            int[] currChapterCount = new int[k];
            int[] minChapterCount = new int[k];
            

            int[] z = new int[RandomSequence.Length - 2];

            for(int j = RandomSequence.Length - 1; j > 0; j--)
            {

            }
            DateTime start = DateTime.Now;
            int minDiff = 9999;
            Recursion(RandomSequence, ref minChapterSize, ref minChapterCount, ref chapterSize, ref currChapterCount, 0, ref minDiff);
            DateTime end = DateTime.Now;
            Console.WriteLine("Laikas: " + (end-start).TotalMilliseconds);


            int currCh = 0;
            for(int i = 0; i < minChapterCount.Length; i++)
            {
                for(int j = 0; j < minChapterCount[i]; j++)
                {
                    currCh++;
                    minChapters[i] += currCh + " ";
                }
            }
            Console.WriteLine("Skyrių numeriai:");
            foreach (string element in minChapters)
            {
                Console.WriteLine(element);
            }
            Console.WriteLine();
            Console.WriteLine("Dalyse esančių psl skaičius:");
            foreach (int element in minChapterSize)
            {
                Console.WriteLine(element);
            }

        }

        static void Recursion(int[] data, ref int[] minChapterSize, ref int[] minChaptersCount, ref int[] currChapterSize,
               ref int[] currChapterCount, int currChap, ref int minDiff)
        {
              int maxSize = data.Length - (k - currChap) + 1;
                int currCount = 0;
                if (currChap < k - 1)
                {
                    for (int i = 0; i < maxSize; i++)
                    {

                        currCount++;

                        currChapterCount[currChap] = currCount;



                        int[] data2 = new int[data.Length - i - 1];
                        for (int j = data.Length - 1; j > i; j--)
                        {
                            data2[j - i - 1] = data[j];
                        }
                        

                        

                        int sum = 0;
                        for(int j = 0; j <= i; j++)
                        {
                            sum += data[j];
                        }
                        currChapterSize[currChap] = sum;

                        

                        Recursion(data2, ref minChapterSize, ref minChaptersCount, ref currChapterSize, ref currChapterCount, currChap + 1, ref minDiff);

                        int diff = CheckingDifference(currChapterSize, k);
                        if(diff < minDiff)
                        {
                            // deep copy
                            for(int j = 0; j < k; j++)
                            {
                                minChapterSize[j] = currChapterSize[j];
                                minChaptersCount[j] = currChapterCount[j];
                            }
                            minDiff = diff;
                        }
                    }
                }
                if(currChap == k - 1)
                {

                    int sum = 0;
                    for (int j = 0; j < data.Length; j++)
                    {
                        sum += data[j];
                    }
                    currChapterSize[currChap] = sum;
                    currChapterCount[currChap] = data.Length;
                }


            
            
        }



        static int CheckingDifference(int[] num, int k)
        {
            if (k == 0 || k == 1)
            {
                return 0;
            }

            int diffMax= 0;
            for (int i = 0; i < k-1; i++)
            {
                for(int j = i+1; j < k; j++)
                {
                    int diff = Math.Abs(num[i] - num[j]);
                    if (diff > diffMax)
                    {
                        diffMax = diff;
                    }
                }
            }

            return diffMax;
        }

    }


}
