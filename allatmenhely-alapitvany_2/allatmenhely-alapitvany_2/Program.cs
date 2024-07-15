using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace allatmenhely_alapitvany_2
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

        // konstruktors
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
        private static void AllatVerseny()
        {
            // az allat nevű változó deklarálása
            Allat allat;
            string nev;
            int szulEv;
            int szepseg, viselkedes;
            int velatlenPontHatar = 10;

            // egy Random példány létrehozása
            Random rand = new Random();

            // számoláshoz szükséges kezdőértékek beállítása
            int osszesVersenyzo = 0;
            int osszesPont = 0;
            int legtobbPont = 0;

            Console.WriteLine("Kezdődik a verseny");

            char tovabb = 'i';
            while (tovabb == 'i')
            {
                Console.WriteLine("Az állat neve: ");
                nev = Console.ReadLine();

                Console.WriteLine("születési éve: ");
                while (!int.TryParse(Console.ReadLine(), out szulEv)
                    || szulEv <= 0
                    || szulEv > Allat.AktualisEv)
                {
                    Console.WriteLine("Hibás adat, kérem újra.");
                }

                // véletlen pontértékek
                szepseg = rand.Next(velatlenPontHatar + 1);
                viselkedes = rand.Next(velatlenPontHatar + 1);

                // az állat példány létrehozása
                allat = new Allat(nev, szulEv);

                //a pontozási metódus meghívása
                allat.Pontozzak(szepseg, viselkedes);

                Console.WriteLine(allat);

                //számítások

                osszesVersenyzo++;
                osszesPont += allat.PontSzam;
                if (legtobbPont < allat.PontSzam)
                {
                    legtobbPont = allat.PontSzam;
                }

                Console.WriteLine("Van még állat? (i/n) ");

                // alakítása át ellenőrzött beolvaásra
                tovabb = char.Parse(Console.ReadLine());
            }
            // eredmény kiíratása
            Console.WriteLine($"\nÖsszesen {osszesVersenyzo} versenyző volt,\nösszpontszámuk: {osszesPont} pont,\nlegnagyobb pontszám: {legtobbPont}");
        }

        static void Main(string[] args)
        {
            int aktEv = 2015, korhatar = 10;

            // Az aktuális év és a korhatár megadása
            Allat.AktualisEv = aktEv;
            Allat.KorHatar = korhatar;

            AllatVerseny();

            Console.ReadLine();

        }
    }
}