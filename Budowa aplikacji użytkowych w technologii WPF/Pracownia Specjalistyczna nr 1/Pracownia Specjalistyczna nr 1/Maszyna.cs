using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pracownia_Specjalistyczna_nr_1
{
    internal class Maszyna
    {
        private List<string> list = new List<string>();
        Random rnd;

        public Maszyna()
        {
            rnd = new Random();
        }

        public void Dodaj(string item)
        {
            list.Add(item);
        }

        public string Losuj()
        {
            int number = rnd.Next(0, list.Count);
            string tekst = list[number];
            list.Remove(tekst);
            return tekst;
        }

        public bool Kupony()
        {
            if (list.Count == 0) return false;
            else return true;
        }

        public List<string> ListKupony()
        {
            return list;
        }
    }
}