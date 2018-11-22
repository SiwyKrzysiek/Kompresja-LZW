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
            //FileInfo toTest = new FileInfo("text.txt");
            //Console.WriteLine($"File: {toTest.Name} Size: {toTest.Length}");

            string imputFile = "iliada.txt";
            string outputFile = "compressed.lzw";

            string data = File.ReadAllText(imputFile);


            DataBlock[] compressed = LZW.Compress(data);

            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                foreach (DataBlock block in compressed)
                {
                    writer.Write(block);
                }
            }

            long inputFileSize = new FileInfo(imputFile).Length;
            long outputFileSize = new FileInfo(outputFile).Length;
            double compressionDegree = (double)inputFileSize / outputFileSize * 100;

            Console.WriteLine($"Dane z pliku {imputFile} zostały skompresowane i zapisane w pliku {outputFile}");
            Console.WriteLine($"Wielkość pliku wejściowego: {inputFileSize / 1000} KB");
            Console.WriteLine($"Wielkość pliku wyjściowego: {outputFileSize / 1000} KB");
            Console.WriteLine($"Współczynnik kompresjii wynosi: {compressionDegree:0.00}%");
            

            //Console.WriteLine(LZW.Decompress(compressed));

            //string myFile = File.ReadAllText("test.txt");
            //Console.WriteLine(myFile);
        }
    }
}
