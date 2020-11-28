using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.State;

namespace tjukica_zadaca_1
{
    public class Korisnik
    {

        public int id { get; private set; }
        public string ime { get; private set; }
        public int brojNeispravnihVracanja { get; set; } = 0;

        //public Vozilo najamVozila { get; set; }

        public List<Vozilo> najmovi = new List<Vozilo>();
        
        public Korisnik(int id, string ime)
        {
            this.id = id;
            this.ime = ime;
        }

        public void IznajmiVozilo(Vozilo vozilo)
        {
            if(!ImaUNajmu(vozilo))
            {
                najmovi.Add(vozilo);
            }
        }

        public void VratiVozilo(Vozilo vozilo)
        {

        }
        
        public Vozilo getVoziloUNajmu(TipVozila tipVozila)
        {
            foreach (var n in najmovi)
            {
                if(n.id == tipVozila.id)
                {
                    return n;
                }
            }
            return null;
        }

        public bool ImaUNajmu(TipVozila vozilo)
        {
            foreach (Vozilo najam in najmovi)
            {
                if(najam.naziv == vozilo.naziv)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
