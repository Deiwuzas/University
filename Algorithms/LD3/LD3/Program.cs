using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD3
{
    public class Block
    {
        public int Weight { get; set; }
        public int Column { get; set; }
        public int Line { get; set; }
        public Block()
        {

        }
    }
    internal class Program
    {
        static int m = 7;
        static int n = 7;
        static void Main(string[] args)
        {
            Block[,] Mine = new Block[n, m];


            Block[] masMax = new Block[m];
            int max = 0;


            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int x = r.Next(1, 10);
                    Block bl = new Block();
                    bl.Weight = x;
                    bl.Column = j + 1;
                    bl.Line = i + 1;
                    Mine[i, j] = bl;
                    Console.Write(bl.Weight + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();

            DateTime start = DateTime.Now;
            for (int i = 0; i < n; i++)
            {
                Block[] mas = new Block[m];
                int Sum = 0;
                Recursion(m - 1, i, Sum, ref mas, ref max, ref masMax, n, Mine);
            }
            DateTime end = DateTime.Now;

            Console.WriteLine();
            Console.WriteLine("Ne dinaminis: " + (end-start).TotalMilliseconds);
            Console.WriteLine();

            Console.WriteLine(max);
            
            Block[] mas2 = new Block[m];

            start = DateTime.Now;


            int sumMax = 0;
            for (int i = 0; i < n; i++)
            {
                int suma3 = RecursionDynamic(0, i, ref mas2, ref max, ref masMax, n, Mine);
                if (suma3 > sumMax)
                {
                    sumMax = suma3;
                }
            }
            end = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("Dinaminis: " + (end-start).TotalMilliseconds);
            Console.WriteLine();
            Console.WriteLine(sumMax);
            Console.WriteLine();

			Console.WriteLine("žingsnis    Line     Column");
			for (int i = 0; i < m; i++)
			{
				Console.WriteLine(i + "         -      " + masMax[i].Line + "          " + masMax[i].Column);
			}

		}

        static int Recursion(int x, int y, int Sum, ref Block[] mas, ref int max, ref Block[] masMax, int n, Block[,] Mine)
        {

            mas[x] = Mine[y, x];
            Sum += Mine[y, x].Weight;
            if (Sum > max)
            {
                max = Sum;
                for (int i = 0; i < m; i++)
                {
                    if (mas[i] != null)
                    {
                        masMax[i] = mas[i];
                    }
                }
            }
            if ((x - 1) >= 0) // dar galima eiti
            {
                // jei galima kilti į viršų
                if ((y - 1) >= 0)
                {
                    Recursion(x - 1, y - 1, Sum, ref mas, ref max, ref masMax, n, Mine);
                }

                //eini viduriu
                Recursion(x - 1, y, Sum, ref mas, ref max, ref masMax, n, Mine);

                //jei galima leidiesi žemyn
                if ((y + 1) < n)
                {
                    Recursion(x - 1, y + 1, Sum, ref mas, ref max, ref masMax, n, Mine);
                }
            }
            return max;
        }

        static int RecursionDynamic(int x, int y, ref Block[] mas, ref int max, ref Block[] masMax, int n, Block[,] Mine)
        {
                int Sum = 0;
                if (x == (n - 1))
                {
                    Sum = Mine[y, x].Weight;
                }
                else
                {
                    if (y == 0) // top
                    {
                        Sum += Mine[y, x].Weight + Math.Max(RecursionDynamic(x + 1, y + 1, ref mas, ref max, ref masMax, n, Mine), RecursionDynamic(x + 1, y, ref mas, ref max, ref masMax, n, Mine));
				}
                    else if (y == (n - 1)) // middle
                    {
                        Sum += Mine[y, x].Weight + Math.Max(RecursionDynamic(x + 1, y - 1, ref mas, ref max, ref masMax, n, Mine), RecursionDynamic(x + 1, y, ref mas, ref max, ref masMax, n, Mine));
                    }
                    else // bottom
                    {
                        Sum += Mine[y, x].Weight + Math.Max(Math.Max(RecursionDynamic(x + 1, y + 1, ref mas, ref max, ref masMax, n, Mine), RecursionDynamic(x + 1, y - 1, ref mas, ref max, ref masMax, n, Mine)), RecursionDynamic(x + 1, y, ref mas, ref max, ref masMax, n, Mine));
                    }
                }


			return Sum;
		}
    }
}

