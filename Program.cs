//Program kopresujący algorytmem LZW
//Na lagoratoria z Algorytmów i Systemów Operacyjnych
//Autor: Krzysztof Dąbrowski
//
//Przejście na C#

using System;

namespace LZW_Compersion
{
    using DataBlock = UInt32;

    class MainClass
    {
        public static void Main(string[] args)
        {
            ConsoleInterface.HandleCommandlineArguments(args);
        }
    }
}
