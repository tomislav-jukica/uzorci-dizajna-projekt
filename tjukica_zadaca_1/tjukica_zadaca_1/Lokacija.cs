using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class Lokacija
    {
        private int id;
        private string naziv;
        private string adresa;
        private double geoSirina;
        private double geoDuzina;

        public static List<Lokacija> lokacije = new List<Lokacija>();

        public Lokacija(int id, string naziv, string adresa, string koordinata)
        {
            string[] temp = Array.ConvertAll(koordinata.Split(","), p => p.Trim());

            this.id = id;
            this.naziv = naziv;
            this.adresa = adresa;
            this.geoSirina = double.Parse(temp[0], System.Globalization.CultureInfo.InvariantCulture);
            this.geoDuzina = double.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
        }

    }
}
