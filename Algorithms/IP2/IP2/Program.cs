using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;




namespace IP2
{
    /// <summary>
    /// Class for location's data
    /// </summary>
    class Data
    {
        public string Location { get; set; } // Title of the location
        public Int64 ID { get; set; } // ID of the location
        public double X { get; set; } // X coordinates of the location
        public double Y { get; set; } // Y coordinates of the location


        /// <summary>
        /// Calculating the Price for traveling from point A to B
        /// </summary>
        /// <param name="d">Location object to travel to</param>
        /// <returns>cost of traveling to given location</returns>
        public double calculatePrice(Data d)
        {
            double xdiff = d.X - this.X;
            double ydiff = d.Y - this.Y;
            double xsqr = Math.Pow(xdiff, 2);
            double ysqr = Math.Pow(ydiff, 2);
            double distance = Math.Sqrt(xsqr+ysqr);
            double Price = Math.Sqrt(distance);

            return Price;
        }
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public Data(string Location, Int64 iD, double x, double y)
        {
            this.Location = Location;
            ID = iD;
            X = x;
            Y = y;
        }
    }


    internal class Program
    {
        static int Start = 1; // Starting location from data file
        const string DataFile = "../../Data.txt"; // File path to data file
        static double Shortest=0; // Full price of the cheapest path


        static int DataSize = 20; // 314 PAKEISTI IR EXCELIO FAILA
        static int AmountNeeded = 8; // 150 PAKEISTI IR EXCEL FILE
        static void Main(string[] args)
        {


            Data[] Data = new Data[DataSize];
            bool[] visited = new bool[DataSize];
            ReadingFile(DataFile, Data);
 
            //Starting position
            int Position = Start - 1;

            //declaring boolean array of visited locations of false values

            Parallel.For(0, visited.Length, i =>{
                visited[i] = false;
            });

            visited[Start - 1] = true;

            Data[] ShortestPath = new Data[AmountNeeded];

            ShortestPath[0] = Data[Position];
            ShortestPath[AmountNeeded-1] = Data[Position];




            // Calculating locally cheapest path for faster processing
            for (int i = 1; i < AmountNeeded-1; i++)
            {
                double min = double.MaxValue;
                int minInd = -1;

                Data d = Data[Position];
                for (int j = 0; j < DataSize; j++)
                {
                    if (!(visited[j]))
                    {
                        double Value = d.calculatePrice(Data[j]);
                        if (Value < min)
                        {
                            min = Value;
                            minInd = j;
                        }
                    }
                }
                ShortestPath[i] = Data[minInd];
                visited[minInd] = true;
            }
            
            Parallel.For(0, ShortestPath.Length - 1, i =>
            {
				Shortest += ShortestPath[i].calculatePrice(ShortestPath[i + 1]);
			});





            Console.WriteLine(Shortest);
            recursion(Data, ShortestPath, visited, ShortestPath, 0, 1);
            Console.WriteLine(Shortest);
        }

		static void ReadingFile(string fileName, Data[] Data)
		{
			StreamReader r = new StreamReader(fileName);
            for(int i = 0; i < DataSize; i++)
            {
                string line = r.ReadLine();
                string[] parts = line.Split('\t');
                string pavadinimas = parts[0];
                Int64 id = Int64.Parse(parts[1]);
                double x = Double.Parse(parts[2]);
                double y = Double.Parse(parts[3]);
                Data d = new Data(pavadinimas, id, x, y);
                Data[i] = d;
            }
		}

		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Data">Data array</param>
		/// <param name="Path">Current path</param>
		/// <param name="Visited">visited locations array</param>
		/// <param name="ShortestPath">Current cheapest path</param>
		/// <param name="Price">Price of current path</param>
		/// <param name="Position">Current position in the path array</param>
		static void recursion(Data[] Data, Data[] Path, bool[] Visited, Data[] ShortestPath, double Price, int Position)
        {
            if (Price >= Shortest)
            {
                return;
            }

            if(Position == AmountNeeded-1)
            {
                Price += Path[Position-1].calculatePrice(Path[Position]);
                if(Price < Shortest)
                {
                    
                    Parallel.For(0, ShortestPath.Length, i =>
                    {
                        ShortestPath[i] = Path[i];
                    });
                    Shortest = Price;
                    Console.WriteLine("New path found with the price of: " + Shortest);
                }
                return;
            }


            for (int i = 0; i < DataSize; i++)
            {

                if (!(Visited[i]))
                {
                    int naujaVieta = i;
                    bool[] Visited2 = new bool[Visited.Length];

                    
                    Parallel.For(0, Visited.Length, j =>
                    {
                        Visited2[j] = Visited[j];
                    });
                    Visited2[naujaVieta] = true;
                    Path[Position] = Data[naujaVieta];
                    double NewPrice = Price + Path[Position - 1].calculatePrice(Path[Position]);
                    recursion(Data, Path, Visited2, ShortestPath, NewPrice, Position + 1);
                }
            }

        }


    }
}
