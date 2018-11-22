//Program kopresujący algorytmem LZW
//Na lagoratoria z Algorytmów i Systemów Operacyjnych
//Autor: Krzysztof Dąbrowski
//
//Przejście na C#

using System;
using System.IO;

namespace LZW_Compersion
{
    using DataBlock = UInt32;

    class MainClass
    {
        public static void Main(string[] args)
        {
            //StreamReader data = new StreamReader("text.txt");

            //Console.WriteLine(data.ReadLine());

            //data.Close();

            string data = File.ReadAllText(@"test.txt");


            DataBlock[] compressed = LZW.Compress(data);

            using (BinaryWriter writer = new BinaryWriter(File.Open("comprssed.lzw", FileMode.Create)))
            {
                foreach (DataBlock block in compressed)
                {
                    writer.Write(block);
                }
            }

            Console.WriteLine(LZW.Decompress(compressed));

            //string myFile = File.ReadAllText("test.txt");
            //Console.WriteLine(myFile);
        }
    }
}
