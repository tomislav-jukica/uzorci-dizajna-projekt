using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Helpers
{
    public class ConsoleWriter
    {
        private static ConsoleWriter baza = null;

        private ConsoleWriter() { } //konstruktor

        public static ConsoleWriter getInstance()
        {
            if (baza == null)
            {
                baza = new ConsoleWriter();
            }
            return baza;
        }

        public void Write(string msg, bool error = true)
        {
            if (error) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
