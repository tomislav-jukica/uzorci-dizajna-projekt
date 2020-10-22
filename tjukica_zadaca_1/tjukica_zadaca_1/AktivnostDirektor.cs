﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class AktivnostDirektor
    {
        public static Aktivnost Pregled(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
        {
            return new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo).build();
        }
        public static Aktivnost Kraj(int idAktivnosti, DateTime vrijeme)
        {
            return new AktivnostBuilder(idAktivnosti, vrijeme).build();
        }
    }
}