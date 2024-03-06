using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2
{
    internal class Program
    {
        /// <summary>
        /// Klasė miesto duomenims saugoti
        /// </summary>
        class Miestas
        {
            private string pavadinimas; // Miesto pavadinimas
            private double plotas; // Miesto plotas tūkst. kv. km
            private int gyventojai; // Miesto gyventojų skaičius tūkst.

            public Miestas(string pavadinimas, double plotas, int gyventojai)
            {
                this.pavadinimas = pavadinimas;
                this.plotas = plotas;
                this.gyventojai = gyventojai;
            }
            public string ImtiPavadinima() { return pavadinimas; } // gražina pavadinimą
            public double ImtiPlota() { return plotas; } // gražina ploto reikšmę
            public int ImtiZmones() { return gyventojai; } // gražina gyventojų reikšmę
        }
        const int Mas = 100; // masyvo ilgis
        const string FD1 = "..\\..\\..\\duom1.txt"; // pirmo duomenų failo nuoroda
        const string FD2 = "..\\..\\..\\duom2.txt"; // antro duomenų failo nuoroda
        const string Rez = "..\\..\\..\\rez.txt"; // rezultatų failo nuoroda
        static void Main(string[] args)
        {
            int n1, n2; // Skaičius miestų duomenų faile
            double tankiausias1, tankiausias2; // Tankiausio miesto vieta masyve
            int gyventojai1, gyventojai2; //Valstybių bendras miestų gyventojų skaičius
            Miestas[] m1 = new Miestas[Mas]; // pirmos valstybės miestų duomenų masyvai
            string valstybe1, valstybe2; // valstybių pavadinimai
            Skaityti(FD1, m1, out valstybe1, out n1); // duomenų iš failo nuskaitymas
            Miestas[] m2 = new Miestas[Mas]; // antros valstybės miestų duomenų įvedimas
            Skaityti(FD2, m2, out valstybe2, out n2); // duomenų iš failo nuskaitymas
            tankiausias1 = Tankumas(m1, n1); // Tankiausio miesto valstybėje nustatymas
            tankiausias2 = Tankumas(m2, n2);
            Spausdinti(Rez, m1, n1, valstybe1); // Duomenų spausdinimas rezultatų faile
            Spausdinti(Rez, m2, n2, valstybe2);
            gyventojai1 = Gyventojai(m1, n1); // gyventojų kiekio valstybėse skaičiavimas
            gyventojai2 = Gyventojai(m2, n2);
            int SK; // Skaičius žmonių

            //Naujas sąrašas miestų, kuriuose yra daugiau nei nurodyta žmonių
            Miestas[] Sarasas;
            Sarasas = new Miestas[100];

            //Dialogas koncolėje, įvedimas intervalas
            Console.WriteLine("Įveskite skaičių žmonių kiekį");
            SK = int.Parse(Console.ReadLine());

            //Įvedimas faile ir sąlygos sakiniai tikrinantys, kuri valstybė turi
            //daugiau gyventojų
            using (var fr = File.AppendText(Rez))
            {
                if (gyventojai1 > gyventojai2) fr.WriteLine("Valstybė turinti daugiau " 
                    + "gyventojų: " +
                    "{0}", valstybe1);
                else if (gyventojai1 < gyventojai2) fr.WriteLine("Valstybė turinti " +
                    "daugiau gyventojų: " +
                    "{0}", valstybe2);
                else fr.WriteLine("Abi valstybės turi po lygiai gyventojų");

            //Įvedimas faile ir sąlygos sakiniai tikrinantys, kurioje valstybėje yra
            //tankiausiai apgyvendintas miestas
                if (tankiausias1 > tankiausias2) fr.WriteLine("Valstybė, kurioje yra" +
                    " tankiausiai apgyvendintas " +
                    "miestas: {0}", valstybe1);
                else if (tankiausias1 < tankiausias2) fr.WriteLine("Valstybė, kurioje " +
                    "yra tankiausiai apgyvendintas " +
                    "miestas: {0}", valstybe2);
                else fr.WriteLine("Abiejos valstybėse tankiausiai apgyvendintų miestų " +
                    "santykis yra vienodas");


               
                
                //Spausdina gyventojų skaičių sąrašo valstybėje miestuose
                fr.WriteLine("{0} turi {1} tūkst. gyventojų šiuose miestuose", 
                    valstybe1, gyventojai1);
                fr.WriteLine("{0} turi {1} tūkst. gyventojų šiuose miestuose", 
                    valstybe2, gyventojai2);

                //Nurodo tankiausią pirmos valstybės miestą
                fr.WriteLine("Tankiausias pirmos valstybės ({0}) miestas - {1}.", 
                    valstybe1, TankiausiasMiest(m1, n1, valstybe1));
                fr.WriteLine();

                //Sąrašas miestų, kuriuose yra daugiau nei nurodyta
                //dialoge gyventojų
                fr.WriteLine("Sąrašas miestų ir jų duomenų, kuriuose yra daugiau " +
                    "gyventojų nei {0} tūkst.", SK);
                fr.WriteLine();
                
            }

            int masilgis = 0; // Miestų, kuriuose yra daugiau nei nurodyta dialoge, pozicija masyve

            // Papildo masyvą miestais iš pirmo sąrašo
            SarasoSudarymas(m1, n1, SK, ref Sarasas, ref masilgis);

            // Papildo masyvą miestais iš antro sąrašo
            SarasoSudarymas(m2, n2, SK, ref Sarasas, ref masilgis);


            //Tikrina ar masyve yra reikšmių ir jei yra, išveda jas faile
            if (masilgis != 0)
            {
                string tekstas = "Miestų sąrašas";
                Spausdinti(Rez, Sarasas, masilgis, tekstas); 
            }
            else using(var fr = File.AppendText(Rez))
                {
                    fr.WriteLine("Tokių miestų nėra");
                }

        }
        /// <summary>
        /// Metodas nuskaityti duomenims iš failo
        /// </summary>
        /// <param name="fv">Failo pavadinimas</param>
        /// <param name="m">Miesto duomenų klasės masyvas</param>
        static void Skaityti(string fv, Miestas[] m, out string valstybe, out int i)
        {
            using (StreamReader reader = new StreamReader(fv))
            {
                string[] lines = File.ReadAllLines(fv);
                string line;
                valstybe = lines[0];
                for (i = 1; i <= lines.Length - 1; i++)
                {
                    line = lines[i];
                    Console.WriteLine(line);
                    string[] parts = line.Split(' ');
                    string pavadinimas = parts[0];
                    double plotas = double.Parse(parts[1]);
                    int gyventojai = int.Parse(parts[2]);
                    m[i - 1] = new Miestas(pavadinimas, plotas, gyventojai);
                }
                i -= 1;
            }
        }

        /// <summary>
        /// Metodas surasti tankiausią miestą duomenų faile
        /// </summary>
        /// <param name="fv">Failo pavadinimas</param>
        /// <param name="m">Objekto klasės masyvas</param>
        /// <param name="n">Kintamasis rodantis kiek miestų yra 
        /// duomenų faile</param>
        /// <param name="miestosk">Kintamasis rodantis kuris miestas 
        /// masyve yra tankiausias</param>
        /// <returns></returns>
        static double Tankumas(Miestas[] m, int n1)
        {
            double a = 0.0;
            for (int i = 0; i < n1; i++)
            {
                if (m[i].ImtiZmones() / m[i].ImtiPlota() > a)
                {
                    a = m[i].ImtiZmones() / m[i].ImtiPlota();
                }
            }
            return a;
        }
        /// <summary>
        /// Metodas spausidnti duomenims
        /// </summary>
        /// <param name="fv">Rezultatų failo pavadinimas</param>
        /// <param name="m">Klasė su duomenimis</param>
        /// <param name="n">Kintamasis nurodantis kiek miestų 
        /// yra rinkinyje</param>
        /// <param name="valstybe">Valstybės pavadinimas</param>
        static void Spausdinti(string fv, Miestas[] m, int n, string valstybe)
        {
            const string virsus = "-------------------------------------------------\n" +
                                  "|  Miestas  | Plotas(kv.km) | Gyventojai(tūkst.)|\n" +
                                  "-------------------------------------------------";
            using (var fr = File.AppendText(fv))
            {
                fr.WriteLine("{0}", valstybe);
                fr.WriteLine(virsus);
                for (int i = 0; i < n; i++)
                {
                    fr.WriteLine("|{0,-11}|{1,15:f2}|{2,19:d}|", m[i].ImtiPavadinima(), 
                        m[i].ImtiPlota(), m[i].ImtiZmones());
                    fr.WriteLine("-------------------------------------------------");
                }

            }
        }
        /// <summary>
        /// Gyventojų skaičiavimas valstybėje
        /// </summary>
        /// <param name="m">Valstybės miestų masyvas</param>
        /// <param name="n">Valstybės miestų saraše skaičius</param>
        /// <returns></returns>
        static int Gyventojai(Miestas[] m, int n)
        {
            int gyventojai = 0;
            for (int i = 0; i < n; i++)
            {
                gyventojai += m[i].ImtiZmones();
            }
            return gyventojai;
        }
        /// <summary>
        /// Sudaro sąrašą miestų, kuriuose yra daugiau gyventojų 
        /// nei nurodyta dialoge.
        /// </summary>
        /// <param name="m">Miesto duomenų masyvas</param>
        /// <param name="n"></param>
        /// <param name="SK">Skaičius gyventojų, kuris yra reikalingas 
        /// norint miestui
        ///                  patekti į sąrašą</param>
        /// <param name="Sarasas">Naujas duomenų masyvas, kuriame 
        /// surašyti miestai turintys
        ///                       daugiau gyventojų nei nurodyta</param>
        /// <param name="masilgis">Miesto duomenų pozicija naujame 
        /// masyve</param>
        static void SarasoSudarymas(Miestas[] m, int n, int SK, 
            ref Miestas[] Sarasas, ref int masilgis)
        {
            for (int i = 0; i < n; i++)
            {
                if (m[i].ImtiZmones() >= SK)
                {
                    Sarasas[masilgis] = new Miestas(m[i].ImtiPavadinima(), 
                        m[i].ImtiPlota(), m[i].ImtiZmones());
                    masilgis++;
                }
            }
        }

        /// <summary>
        /// Tankiausio miesto valstybėje radimas
        /// </summary>
        /// <param name="m">Miesto duomenų masyvas</param>
        /// <param name="n">Miestų saraše kiekis</param>
        /// <param name="valstybe">Valstybės pavadinimas</param>
        /// <returns></returns>
        static string TankiausiasMiest(Miestas[] m, int n, string valstybe)
        {
            double a = 0.0;
            int k = 0;
            for (int i = 0; i < n; i++)
            {
                if ((m[i].ImtiZmones() / m[i].ImtiPlota()) > a)
                {
                    a = m[i].ImtiZmones() / m[i].ImtiPlota();
                    k = i;
                }
            }
            return m[k].ImtiPavadinima();
        }






    }
}

    

