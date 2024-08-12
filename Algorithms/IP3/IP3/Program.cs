using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;



namespace IP3
{
    /// <summary>
    /// Class for location's data
    /// </summary>
    class Data
    {
        public string Location { get; set; } // Name of the location
        public Int64 ID { get; set; } // ID of the location
        public double X { get; set; } // X coordinates of the locations
        public double Y { get; set; } // Y coordinates of the location

		/// <summary>
		/// Calculating the Price for traveling from point A to B
		/// </summary>
		/// <param name="d">Location object to travel to</param>
		/// <returns>cost of traveling to given location</returns>
		public double calculatePrice(Data d)
        {
            double Distance = Math.Sqrt(Math.Pow(d.X - this.X, 2) + Math.Pow(d.Y - this.Y, 2));
            double Price = Math.Sqrt(Distance);

            return Price;
        }
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
        const string DataFile = "../../data.txt"; // Location of data file 
        static int Population = 100; // Size of the population
        static int Start = 1; // Starting location
        static int Generations = 10; // Number of generations
        static int Elite = 10; // Number of the elite
        static Random r = new Random();
        static List<Data> CheapestPath = new List<Data>(); 

        static void Main(string[] args)
        {


            Data[] data = new Data[314];
            bool[] visited = new bool[314];
            visited[Start - 1] = true;
            ReadingFile(DataFile, data);
            int Position = Start - 1;

            for(int i = 0; i < 314; i++)
            {
                visited[i] = false;
            }
            visited[Position] = true;
            

            CheapestPath = CreateGnome(data);

            List<List<Data>> Generation = new List<List<Data>>(); // creating 1st generation
            for(int i = 0; i < Population; i++)
            {
                Generation.Add(CreateGnome(data));
            }
            Sort(Generation);


            DateTime start = DateTime.Now;

            // Evolutionary algorithm
            for (int i = 0; i < Generations; i++)
            {
                // Creating new generation and adding elite into it
                List<List<Data>> newGen = new List<List<Data>>();
                for(int j = 0; j < Elite; j++)
                {
                    newGen.Add(Generation[j]);
                }


                // Generating rest of the generation
                for(int j = 0; j < Population - Elite; j++)
                {
                    int rand = r.Next(0, 50);
                    List<Data> p1 = Generation[rand];
                    rand = r.Next(0, 50);
                    List<Data> p2 = Generation[rand];
                    List<Data> child = Mate(p1, p2, data);
                    newGen.Add(child);
                }
                Sort(newGen);

                
                for(int j = 0; j < Population; j++)
                {
                    Generation[j] = newGen[j];
                }
            }

            DateTime end = DateTime.Now;

            Console.WriteLine((end-start).TotalSeconds);
            CheapestPath = Generation[0];
            Console.WriteLine(FitnessScore(CheapestPath));


        }

        
        static void Sort(List<List<Data>> Data)
        {
            bool sorted = true;
            while (sorted)
            {
                sorted = false;
                for(int i = 1; i < Data.Count; i++)
                {
                    List<Data> d1 = Data[i - 1];
                    List<Data> d2 = Data[i];
                    if(FitnessScore(d1) > FitnessScore(d2))
                    {
                        sorted = true;
                        var temp = d1;
                        Data[i - 1] = Data[i];
                        Data[i] = temp;
                    }
                }
            }
        }

		/// <summary>
		/// Generates a child list by combining genes from two parent lists using crossover and mutation.
		/// </summary>
		/// <param name="p1">First parent list.</param>
		/// <param name="p2">Second parent list.</param>
		/// <param name="data">Array of all possible Data elements.</param>
		/// <returns>Child list created from p1 and p2.</returns>
		static List<Data> Mate(List<Data> p1, List<Data> p2, Data[] data)
        {
            List<Data> child = new List<Data>();
            child.Add(data[Start-1]);
            while(child.Count < 149)
            {
                double d = r.NextDouble();
                if (d < 0.45)
                {
                    //p1
                    if (!(child.Contains(p1[child.Count])))
                    {
                        child.Add(p1[child.Count]);
                    }
                    else
                    {
                        Data DataFile = MutatedGene(data);
                        if (!(child.Contains(DataFile)))
                        {
                            child.Add(DataFile);
                        }
                        
                    }
                }
                else if (d > 0.90)
                {
                    if (!(child.Contains(p2[child.Count])))
                    {
                        child.Add(p2[child.Count]);
                    }
                    else
                    {
                        Data DataFile = MutatedGene(data);
                        if (!(child.Contains(DataFile)))
                        {
                            child.Add(DataFile);
                        }
                        
                    }
                }
                else
                {
                    Data DataFile = MutatedGene(data);
                    if (!(child.Contains(DataFile)))
                    {
                        child.Add(DataFile);
                    }
                }
            }
            child.Add(data[Start-1]);

            return child;
            
        }
		/// <summary>
		/// Selects a random gene (Data object) for mutation.
		/// </summary>
		/// <param name="Data">Array of all possible Data elements.</param>
		/// <returns>A randomly selected Data element.</returns>
		static Data MutatedGene(Data[] Data)
        {
            int rand = r.Next(0, 314);
            return Data[rand];

        }


		/// <summary>
		/// Creates a new random path (gnome) from the available Data elements.
		/// </summary>
		/// <param name="DataFile">Array of all possible Data elements.</param>
		/// <returns>A new list representing a random path.</returns>
		static List<Data> CreateGnome(Data[] DataFile)
        {
            List<Data> naujasKelias = new List<Data>();
            naujasKelias.Add(DataFile[Start - 1]);
            int count = 1;
            while(count < 149)
            {
                Data d = MutatedGene(DataFile);
                if (!(naujasKelias.Contains(d)))
                {
                    naujasKelias.Add(d);
                    count++;
                }

            }
            naujasKelias.Add(DataFile[Start - 1]);
            return naujasKelias;
        }


		/// <summary>
		/// Calculates the fitness score of a given path.
		/// </summary>
		/// <param name="DataFile">List representing the path.</param>
		/// <returns>The fitness score based on the path's total cost.</returns>
		static double FitnessScore(List<Data> DataFile)
        {
            double sum = 0;
            for(int i = 0; i < 149; i++)
            {
                sum += DataFile[i].calculatePrice(DataFile[i + 1]);
            }
            return sum;
        }


		/// <summary>
		/// Reads data from a file and populates the data array.
		/// </summary>
		/// <param name="fileName">Name of the file to read from.</param>
		/// <param name="data">Array to store the Data elements.</param>
		static void ReadingFile(string fileName, Data[] data)
        {
            int i = 0;
            StreamReader r = new StreamReader(fileName);
            string line;
            while ((line = r.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                string pavadinimas = parts[0];
                Int64 id = Int64.Parse(parts[1]);
                double x = Double.Parse(parts[2]);
                double y = Double.Parse(parts[3]);
                Data d = new Data(pavadinimas, id, x, y);
                data[i] = d;
                i++;

            }
        }


    }
}
