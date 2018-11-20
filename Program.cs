//Program kopresujący algorytmem LZW
//Na lagoratoria z Algorytmów i Systemów Operacyjnych
//Autor: Krzysztof Dąbrowski
//
//Przejście na C#

using System;
using System.IO;

namespace LZW_Compersion
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //StreamReader data = new StreamReader("text.txt");

            //Console.WriteLine(data.ReadLine());

            //data.Close();

            string data = File.ReadAllText(@"test.txt");


            //string myFile = File.ReadAllText("test.txt");
            //Console.WriteLine(myFile);
        }
    }
}
