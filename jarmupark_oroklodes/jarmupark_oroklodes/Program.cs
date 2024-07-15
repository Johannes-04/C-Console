using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarmupark_oroklodes
{
    abstract class Jarmu
    {
        public string Azonosito { get; private set; }
        public string Redszam { get; set; }
        public int GyartasiEv { get; private set; }
        public double Fogyasztas { get; set; }

        public double FutottKm { get; private set; }
        public int AktualisKoltseg { get; private set; }
        public bool Szabad { get; private set; }

        public static int AktualisEv { get; set; }
        public static int AlapDij { get; set; }
        public static double HaszonKulcs { get; set; }

        public Jarmu(string azonosit, string redszam, int gyartasiEv, double fogyasztas)
        {
            this.Azonosito = azonosit;
            this.Redszam = redszam;
            this.GyartasiEv = gyartasiEv;
            this.Fogyasztas = fogyasztas;
            this.Szabad = true;
        }

        public Jarmu(string azonosit, string redszam, int gyartasiEv)
        {
            this.Azonosito = azonosit;
            this.Redszam = redszam;
            this.GyartasiEv = gyartasiEv;
            this.Szabad = true;
        }

        //matódusok
        public int Kor()
        {
            return AktualisEv - GyartasiEv;
        }

        public bool Fuvaroz(double ut, int benzinAr)
        {
            if (Szabad)
            {
                FutottKm += ut;
                AktualisKoltseg = (int)(benzinAr * ut * Fogyasztas / 100);
                Szabad = false;
                return true;
            }
            return false;
        }

        public virtual int BerletDij()
        {
            return (int)(AlapDij + AktualisKoltseg + AktualisKoltseg * HaszonKulcs / 100);
        }

        public void Vegzett()
        {
            Szabad = true;
        }

        public override string ToString()
        {
            return $"\nA{this.GetType().Name.ToLower()} azonosítója: {Azonosito}\nkora: {Kor()}\nfogyasztása: {Fogyasztas} 1/100 km\na km-óra állása: {FutottKm} km";
        }

    }
    class Busz : Jarmu
    {
        public int Ferohely { get; private set; }
        public static double Szoros {  get; set; }

        public Busz(string azonosit, string redszam, int gyartasiEv, double fogyasztas, int ferohely) : base(azonosit, redszam, gyartasiEv, fogyasztas)
        {
            this.Ferohely = ferohely;
        }

        public Busz(string azonosit, string redszam, int gyartasiEv, int ferohely) : base(azonosit, redszam, gyartasiEv)
        {
            this.Ferohely = ferohely;
        }

        //metódusok

        public override int BerletDij()
        {
            return (int)(base.BerletDij() + Ferohely * Szoros);
        }

        public override string ToString()
        {
            return $"{base.ToString()}\n\tférőhelyek száma: {Ferohely}";
        }
    }

    class TeherAuto : Jarmu
    {
        public double TeherBiras { get; private set; }
        public static double Szoros { get; set; }

        public TeherAuto(string azonosit, string redszam, int gyartasiEv, double fogyasztas, double teherBiras) : base(azonosit, redszam, gyartasiEv, fogyasztas)
        {
            this.TeherBiras = teherBiras;
        }

        public TeherAuto(string azonosit, string redszam, int gyartasiEv, double teherBiras) : base(azonosit, redszam, gyartasiEv)
        {
            this.TeherBiras = teherBiras;
        }

        //metódusok
        public override int BerletDij()
        {
            return (int)(base.BerletDij());
        }

        public override string ToString()
        {
            return $"{base.ToString()}\n\tteherbírás: {TeherBiras} tonna";
        }
    }

    class Vezerles
    {
        private List<Jarmu> jarmuvek = new List<Jarmu>();
        private string BUSZ = "busz";
        private string TEHER_AUTO = "teherautó";

        public void Indit()
        {
            StatikusBeallitas();
            AdatBevitel();
            Kiir("A regisztrált járművek: ");
            Mukodtet();
            Kiir("\nA működés után állapot: ");
            AtlagKor();
            LegtobbKilometer();
            Rendez();
        }

        private void StatikusBeallitas()
        {
            Jarmu.AktualisEv = 2015;
            Jarmu.AlapDij = 1000;
            Jarmu.HaszonKulcs = 10;

            Busz.Szoros = 15;
            TeherAuto.Szoros = 8.5;
        }

        private void AdatBevitel()
        {
            string tipus, redszam, azonosito;
            int gyartEv, ferohely;
            double fogyasztas, teherbiras;

            StreamReader sr = new StreamReader("d:/jarmuvek.txt");

            int sorszam = 1;

            while(!sr.EndOfStream) 
            {
                tipus = sr.ReadLine();
                Console.WriteLine(tipus);
                redszam = sr.ReadLine();
                gyartEv = int.Parse(sr.ReadLine());
                fogyasztas = double.Parse(sr.ReadLine());
                azonosito = tipus.Substring(0, 1).ToUpper() + sorszam;

                if(tipus == BUSZ)
                {
                    ferohely = int.Parse(sr.ReadLine());
                    jarmuvek.Add(new Busz(azonosito,redszam, gyartEv, fogyasztas, ferohely));
                }
                else if(tipus == TEHER_AUTO)
                {
                    teherbiras = double.Parse(sr.ReadLine());
                    jarmuvek.Add(new TeherAuto(azonosito, redszam, gyartEv, fogyasztas, teherbiras));
                }sorszam++;
            }sr.Close();

        }

        private void Kiir(string cim)
        {
            Console.WriteLine(cim);
            foreach(Jarmu jarmu in jarmuvek)
            {
                Console.WriteLine(jarmu);
            }
        }

        private void Mukodtet()
        {
            int osszKoltseg = 0, osszBevetel = 0;

            Random rand = new Random();
            int alsoBenzinAr = 400;
            int felsoBenzinar = 420;
            double utMax = 300;
            int mukodesHatar = 200;
            int jarmuIndex;

            Jarmu jarmu;
            int fuvarSzam = 0;

            for (int i = 0; i < (int)rand.Next(mukodesHatar); i++)
            {
                jarmuIndex = rand.Next(jarmuvek.Count);
                jarmu = jarmuvek[jarmuIndex];
                if (jarmu.Fuvaroz(rand.NextDouble() * utMax, rand.Next(alsoBenzinAr, felsoBenzinar)))
                {
                    fuvarSzam++;
                    Console.WriteLine($"\nAz induló jármű redszáma: {jarmu.Redszam}\nAz aktuális fuvarozási költség: {jarmu.AktualisKoltseg} Ft.\nAz aktuális bevétel: {jarmu.BerletDij()} Ft.");

                    osszBevetel += jarmu.BerletDij();
                    osszKoltseg += jarmu.AktualisKoltseg;

                }

                jarmuIndex = rand.Next(jarmuvek.Count);
                jarmuvek[jarmuIndex].Vegzett();
            }

            Console.WriteLine($"\n\nA cég teljes költsége: {osszKoltseg} Ft.\n\nTeljes bevétel: {osszBevetel} Ft.\n\nHaszna: {osszBevetel - osszKoltseg} Ft.");
            Console.WriteLine($"\nA fuvarok száma: {fuvarSzam}");
        }

        private void AtlagKor()
        {
            double osszKor = 0;
            int darab = 0;
            foreach (Jarmu jarmu in jarmuvek)
            {
                osszKor += jarmu.Kor();
                darab++;
            }

            if (darab > 0)
            {
                Console.WriteLine($"\nA járművek átlag kora: {osszKor / darab} év.");
            }
            else
            {
                Console.WriteLine("Nincsenek járművek.");
            }
        }

        private void LegtobbKilometer()
        {
            double max = jarmuvek[0].FutottKm;
            foreach (Jarmu jarmu in jarmuvek)
            {
                if (jarmu.FutottKm > max)
                {
                    max = jarmu.FutottKm;
                }
            }

            Console.WriteLine("\nA legtöbbet futott jármű(vek) {0: 000.00} km-rel:", max);
            foreach (Jarmu jarmu in jarmuvek)
            {
                if (jarmu.FutottKm == max)
                {
                    Console.WriteLine(jarmu.Redszam);
                }
            }
        }

        private void Rendez()
        {
            Jarmu temp;

            for (int i = 0; (i < jarmuvek.Count - 1); i++)
            {
                for (int j = i + 1; (j < jarmuvek.Count); j++)
                {
                    if (jarmuvek[i].Fogyasztas > jarmuvek[j].Fogyasztas)
                    {
                        temp = jarmuvek[i];
                        jarmuvek[i] = jarmuvek[j];
                        jarmuvek[j] = temp;
                    }
                }
            }
            Console.WriteLine("\nA járművek fogyasztása szerint rendezve: ");
            foreach (Jarmu jarmu in jarmuvek)
            {
                Console.WriteLine("{0,-10} {1:00.0} liter / 100 km.", jarmu.Redszam, jarmu.Fogyasztas);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            new Vezerles().Indit();

            Console.ReadKey();
        }
    }
}
