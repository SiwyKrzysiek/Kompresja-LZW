using System;
using System.IO;
using System.Collections.Generic;

namespace LZW_Compersion
{
    using DataBlock = UInt32;

    public static class ConsoleInterface
    {
        public static void HandleCommandlineArguments(string[] args)
        {
            Mode mode;
            switch (args[0])
            {
                case "-c":
                    TryToCompress(args[1], args[2]);
                    break;
                case "-d":
                    TryToDecompress(args[1], args[2]);
                    break;
                case "-h":
                default:
                    mode = Mode.Help;
                    break;
            }
        }

        private static void TryToCompress(string inputFile, string outputFile)
        {
            string data;

            try
            {
                data = File.ReadAllText(inputFile);
                if (data.Length <= 0) throw new IOException();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Nie udało się odnaleźć wskazanego pliku");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Wystąpił błąd podczas czytana pliku z danymi");
                return;
            }

            DataBlock[] compressed = LZW.Compress(data);

            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    foreach (DataBlock block in compressed)
                    {
                        writer.Write(block);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystąpił błąd podczas zapisu do pliku");
                return;
            }

            DisplayCompressionInfo(inputFile, outputFile);
        }

        private static void TryToDecompress(string inputFile, string outputFile)
        {
            List<DataBlock> data = new List<DataBlock>();

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
                {
                    long position = 0;
                    long length = reader.BaseStream.Length;

                    while (position < length)
                    {
                        data.Add(reader.ReadUInt32());
                        position += sizeof(DataBlock);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Nie udało się odnaleźć wskazanego pliku");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Wystąpił błąd podczas czytana pliku z danymi");
                return;
            }

            string decompressed = LZW.Decompress(data.ToArray());

            try
            {
                File.WriteAllText(outputFile, decompressed);
            }
            catch (Exception)
            {
                Console.WriteLine("Wystąpił błąd podczas zapisu do pliku");
                return;
            }

            DisplayDecompressionInfo(inputFile, outputFile);
        }

        private static void DisplayDecompressionInfo(string inputFile, string outputFile)
        {
            Console.WriteLine($"Dane z pliku {inputFile} zostały zdekompresowane i zapisane w pliku {outputFile}");
        }

        private static void DisplayCompressionInfo(string inputFile, string outputFile)
        {
            long inputFileSize = new FileInfo(inputFile).Length;
            long outputFileSize = new FileInfo(outputFile).Length;
            double compressionDegree = (double)inputFileSize / outputFileSize * 100;

            Console.WriteLine($"Dane z pliku {inputFile} zostały skompresowane i zapisane w pliku {outputFile}");
            Console.WriteLine($"Wielkość pliku wejściowego: {inputFileSize / 1000} KB");
            Console.WriteLine($"Wielkość pliku wyjściowego: {outputFileSize / 1000} KB");
            Console.WriteLine($"Współczynnik kompresjii wynosi: {compressionDegree:0.00}%");
        }

        enum Mode
        {
            Help,
            Compress,
            Decompress,
            Demo
        }
    }
}
