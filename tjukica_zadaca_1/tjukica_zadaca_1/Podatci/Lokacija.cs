using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Lokacija
    {
        public int id { get; private set; }
        private string naziv;
        private string adresa;
        private double geoSirina;
        private double geoDuzina;

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
