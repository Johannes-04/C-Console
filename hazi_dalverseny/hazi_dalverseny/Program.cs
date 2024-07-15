using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazi_dalverseny
{
    class Versenyzo
    {
        //mezők
        private int rajtSzam;
        private string nev;
        private string szak;
        private int pontSzam;

        //konstruktor
        public Versenyzo(int rajtSzam, string nev, string szak)
        {
            this.rajtSzam = rajtSzam;
            this.nev = nev;
            this.szak = szak;
        }

        //metódusok
        public void PontotKap(int pont)
        {
            pontSzam += pont;
        }

        public override string ToString() 
        {
            return String.Format("{0,5}\t{1,-10}{2,-15}{3,-10}", rajtSzam,nev,szak,pontSzam + "pont"); 
                ;
        }

        //tulajdonságok
        public int RajtSzam
        {
            get { return rajtSzam; }
        }

        public string Nev
        {
            get { return nev ; }
        }

        public string Szak
        {
            get { return szak ; }
        }

        public int PontSzam
        {
            get { return pontSzam; }
        }

    }

    class VezerloOsztaly
    {
        //mezők
        private int zsuriLetszam = 5;
        private int pontHatar = 10;

        private List<Versenyzo> versenyzok = new List<Versenyzo>();

        //metódusok
        public void Start()
        {
            AdatBevitel();

            Kiiratas("\nRésztevevők:\n");
            Verseny();
            Kiiratas("\nEredmények:\n");

            Eredmenyek();
            Keresesek();
        }

        private void AdatBevitel()
        {
            Versenyzo versenyzo;
            string nev, szak;
            int sorszam = 1;

            StreamReader sr = new StreamReader("d:/versenyzok.txt");

            while (!sr.EndOfStream)
            {
                nev = sr.ReadLine();
                szak = sr.ReadLine();

                versenyzo = new Versenyzo(sorszam,nev,szak);
                versenyzok.Add(versenyzo);

                sorszam++;
            }sr.Close();
        }

        private void Kiiratas(string cim)
        {
            Console.WriteLine(cim);
            foreach (Versenyzo enekes in versenyzok)
            {
                Console.WriteLine(enekes);
            }
        }

        private void Verseny()
        {
            Random rand = new Random();
            int pont;
            foreach (Versenyzo versenyzo in versenyzok)
            {
                for (int i = 1; i < zsuriLetszam; i++)
                {
                    pont = rand.Next(pontHatar);
                    versenyzo.PontotKap(pont);
                }
            } 
        }

        private void Eredmenyek() 
        {
            Nyertes();
            Sorrend();
        }

        private void Nyertes()
        {
            int max = versenyzok[0].PontSzam;

            foreach (Versenyzo enekes in versenyzok)
            {
                if (enekes.PontSzam > max)
                {
                    max = enekes.PontSzam;
                }
            }

            Console.WriteLine("\nA legjobb(ak)\n");
            foreach (Versenyzo enekes in versenyzok)
            {
                if (enekes.PontSzam == max)
                {
                    Console.WriteLine(enekes);
                }
            }
        }

        private void Sorrend()
        {
            Versenyzo temp;
            for (int i = 0; i < versenyzok.Count - 1; i++)
            {
                for (int j = i + 1; j < versenyzok.Count; j++)
                {
                    if (versenyzok[i].PontSzam < versenyzok[j].PontSzam)
                    {
                        temp = versenyzok[i];
                        versenyzok[i] = versenyzok[j];
                        versenyzok[j] = temp;
                    }
                }
            }

            Kiiratas("\nEredménytábla\n");
        }

        private void Keresesek()
        {
            Console.WriteLine("\nAdott szakhoz tartozó énekesek keresése\n");
            Console.Write("\nKeres valakit? (i/n) ");
            char valasz;
            while (!char.TryParse(Console.ReadLine(), out valasz))
            {
                Console.Write("Egy karaktert írjon. ");
            }
            string szak;
            bool vanIlyen;

            while (valasz == 'i' || valasz == 'i')
            {
                Console.Write("Szak: ");
                szak = Console.ReadLine();
                vanIlyen = false;

                foreach (Versenyzo enekes in versenyzok) 
                {
                    if (enekes.Szak == szak)
                    {
                        Console.WriteLine(enekes);
                        vanIlyen = true;
                    }
                }

                if (!vanIlyen)
                {
                    Console.WriteLine("Erről a szakról senki sem indult.");
                }

                Console.Write("\nKeres még valakit? (i/n) ");
                valasz = char.Parse(Console.ReadLine());
            }
        }
    }

    internal class Program
    {
        
        static void Main(string[] args)
        {
            new VezerloOsztaly().Start();

            Console.ReadKey();

            //Egyetlen versenyző adatainak beolvasása és az adatok alapján a példány létrehozása
            /*
            int sorszam = 1;
            string nev, szak;

            Console.Write("Név: ");
            nev = Console.ReadLine();
            
            Console.Write("Szak: ");
            szak = Console.ReadLine();

            Versenyzo versenyzo = new Versenyzo(sorszam,nev,szak);
            */

            // Több be olvasása
            /*
            string nev, szak;
            int n = 10;
            Versenyzo[] versenyzok = new Versenyzo[n];
            int sorszam = 1;

            Console.Write("Név: ");
            nev = Console.ReadLine();
            while (nev !="") 
            {
                Console.Write("Szak: ");
                szak = Console.ReadLine();

                versenyzok[sorszam - 1] = new Versenyzo(sorszam, nev, szak);
                sorszam++;

                Console.Write("Név: ");
                nev = Console.ReadLine();
            }
            */

            // List<Versenyzo> versenyzok = new List<Versenyzo>();

            /*
            Versenyzo versenyzo;
            string nev, szak;
            int sorszam = 1;

            Console.Write("Név: ");
            nev = Console.ReadLine();
            while (nev != "")
            {
                Console.Write("Szak: ");
                szak = Console.ReadLine();

                versenyzo = new Versenyzo(sorszam,nev,szak);
                versenyzok.Add(versenyzo);

                sorszam++;

                Console.Write("Név: ");
                nev = Console.ReadLine();

            }
            */

        }

    }
}
