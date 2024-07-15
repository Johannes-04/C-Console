using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace allatmenhely_alapitvany
{

    class Allat
    {
        //mezők
        private string nev;
        private int szuletesiEv;

        private int szepsegPont, viselkedesPont;
        private int pontSzam;

        private static int aktualisEv;
        private static int korHatar;

        // konstruktor
        public Allat(string nev, int szuletesiEv)
        {
            this.nev = nev;
            this.szuletesiEv = szuletesiEv;
        }

        // metódusok
        public int Kor()
        {
            return aktualisEv - szuletesiEv;
        }

        public void Pontozzak(int szepsegPont, int viselkedesPont)
        {
            this.szepsegPont = szepsegPont;
            this.viselkedesPont = viselkedesPont;
            if (Kor() <= korHatar)
            {
                pontSzam = viselkedesPont * Kor() + szepsegPont * (korHatar - Kor());
            }
        }

        public override string ToString()
        {
            return $"{nev} pontszáma: {pontSzam} pont";
        }

        // tulajdonságok

        // kívülről nem változtatható értékek

        public string Nev
        {
            get { return nev; }
        }

        public int SzuletesiEv
        {
            get { return szuletesiEv; }
        }

        public int SzepsegPont
        {
            get { return szepsegPont; }
        }

        public int ViselkedesPont
        {
            get { return viselkedesPont; }
        }

        public int PontSzam
        {
            get { return pontSzam; }
        }

        // kívülről változtatható értékek

        public static int AktualisEv
        {
            get { return aktualisEv; }
            set { aktualisEv = value; }
        }

        public static int KorHatar
        {
            get { return KorHatar; }
            set { korHatar = value; }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // az allat nevű változó deklarálása
            Allat allat;

            int aktvEv = 2015, korhatar = 10;

            string nev;
            int szulEv;
            int szepseg, viselkedes;

            // Az aktuális év és a korhatár megadása
            Allat.AktualisEv = aktvEv;
            Allat.KorHatar = korhatar;

            // csak egyetlen példány kipróbálása:

            nev = "Vakarcs";
            szulEv = 2010;
            szepseg = 5;
            viselkedes = 3;

            // az allat példány létrehozása
            allat = new Allat(nev, szulEv);

            // a pontozási metódus meghívádsa
            allat.Pontozzak(szepseg, viselkedes);

            Console.WriteLine(allat);

            Console.ReadLine();
        }
    }
}