using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace allatverseny_oroklodes
{
    class Allat
    {
        public string Nev { get; private set; }
        public int SzuletesiEv { get; private set; }
        public int RajtSzam { get; private set; }

        public int SzepsegPont { get; private set; }
        public int ViselkedesPont { get; private set; }

        public static int AktualisEv { get; set; }
        public static int KorHatar { get; set; }

        //Konstruktor
        public Allat(int rajtSzam, string nev, int szuletesiEv)
        {
            this.RajtSzam = rajtSzam;
            this.Nev = nev;
            this.SzuletesiEv = szuletesiEv;
            
        }

        //Metódusok
        public int Kor()
        {
            return AktualisEv - SzuletesiEv;
        }

        public virtual int PontSzam()
        {
            if (Kor() > KorHatar)
            {
                return ViselkedesPont * Kor() + SzepsegPont * (KorHatar - Kor());
            }
            return 0;
        }

        public void Pontozzak(int szepsegPont, int viselkedesPont)
        {
            this.SzepsegPont = szepsegPont;
            this.ViselkedesPont = viselkedesPont;
        }

        public override string ToString()
        {
            return $"{RajtSzam}. {Nev} nevű {this.GetType().Name.ToLower()} pontszáma: {PontSzam()} pont";
        }
    }

    class Kutya : Allat
    {
        public int GazdaViszonyPont { get; private set; }
        public bool KapottViszonyPontot { get; private set; }

        //Kontsruktor
        public Kutya(int rajtSzam, string nev, int szulEv) : base(rajtSzam, nev, szulEv) { }

        //Metódusok
        public void ViszonyPontozas(int gazdaViszonyPont)
        {
            this.GazdaViszonyPont = gazdaViszonyPont;
            KapottViszonyPontot = true;
        }

        public override int PontSzam()
        {
            int pont = 0;
            if (KapottViszonyPontot)
            {
                pont = base.PontSzam() + GazdaViszonyPont;
            }
            return pont;
        }
    }

    class Macska : Allat
    {
        public bool VanMacskaSzallitoDoboz { get; set; }

        //Konstruktor
        public Macska(int rajtSzam, string nev, int szulEv, bool vanMacskaSzallitoDoboz) : base(rajtSzam, nev, szulEv)
        {
            this.VanMacskaSzallitoDoboz = VanMacskaSzallitoDoboz;
        }

        //Metódusok
        public override int PontSzam()
        {
            if (VanMacskaSzallitoDoboz)
            {
                return base.PontSzam();
            }
            return 0;
        }

    }

    class Vezerles
    {
        private List<Allat> allatok = new List<Allat>();

        public void Start()
        {
            Allat.AktualisEv = 2015;
            Allat.KorHatar = 10;

            //Proba();

            Regisztracio();
            Kiiratas("A regisztrált versenyzők");
            Verseny();
            Kiiratas("A verseny eredmény");
        }

        private void Proba()
        {
            Allat allat1, allat2;

            string nev1 = "Pamacs", nev2 = "Bolhazsák";
            int szulEv1 = 2010, szulEv2 = 2011;
            bool vanDoboz = true;
            int rajtSzam = 1;

            int szepsegPont = 5, viselkedesPont = 4, viszonyPont = 6;

            allat1 = new Kutya(rajtSzam, nev1, szulEv2);
            rajtSzam++;

            allat2 = new Macska(rajtSzam, nev2 , szulEv2, vanDoboz);

            //Verseny
            allat2.Pontozzak(szepsegPont, viselkedesPont);

            if (allat1 is Kutya)
            {
                (allat1 as Kutya).ViszonyPontozas(viszonyPont);
            }
            allat1.Pontozzak(szepsegPont, viselkedesPont);

            Console.WriteLine("A regisztrált állatok:");
            Console.WriteLine(allat1);
            Console.WriteLine(allat2);
        }

        private void Regisztracio()
        {
            StreamReader sr = new StreamReader("d:/allatok.txt");

            string fajta, nev;
            int rajtSzam = 1, szulEv;
            bool vanDoboz;

            while (!sr.EndOfStream)
            {
                fajta = sr.ReadLine();
                nev = sr.ReadLine();
                szulEv = int.Parse(sr.ReadLine());

                if (fajta == "kutya" || fajta == "Kutya") 
                {
                    allatok.Add(new Kutya(rajtSzam, nev, szulEv));
                }
                else
                {
                    vanDoboz = bool.Parse(sr.ReadLine());
                    allatok.Add(new Macska(rajtSzam, nev, szulEv, vanDoboz));
                }
                rajtSzam++;
            }sr.Close();
        }

        private void Verseny()
        {
            Random rand = new Random();
            int hatar = 11;
            foreach (Allat item in allatok)
            {
                if (item is Kutya)
                {
                    (item as Kutya).ViszonyPontozas(rand.Next(hatar));
                }
                item.Pontozzak(rand.Next(hatar), rand.Next(hatar));
            }
        }

        private void Kiiratas(string cim)
        {
            Console.WriteLine(cim);
            foreach (Allat item in allatok)
            {
                Console.WriteLine(item);
            }
        }

    }
        internal class Program
    {
        static void Main(string[] args)
        {
            new Vezerles().Start();

            Console.ReadKey();
        }
    }
}
