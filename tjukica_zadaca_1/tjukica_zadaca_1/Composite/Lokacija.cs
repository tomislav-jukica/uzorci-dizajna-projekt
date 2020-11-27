﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Composite
{
    public class Lokacija : TvrtkaComponent
    {
        public string naziv { get; private set; }
        private string adresa;
        private double geoSirina;
        private double geoDuzina;

        public Lokacija(int id, string naziv, string adresa, string koordinata, TvrtkaComponent nadredeni):base(id, nadredeni)
        {
            string[] temp = Array.ConvertAll(koordinata.Split(","), p => p.Trim());
            this.naziv = naziv;
            this.adresa = adresa;
            this.geoSirina = double.Parse(temp[0], System.Globalization.CultureInfo.InvariantCulture);
            this.geoDuzina = double.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
            this.nadredeni = nadredeni;
        }

        public override TvrtkaComponent getComponent(int componentId)
        {
            throw new NotImplementedException();
        }

        public override string getComponentName()
        {
            return naziv;
        }

        public override TvrtkaComponent getParentComponent()
        {
            return nadredeni;
        }
    }
}