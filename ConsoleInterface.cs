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
            switch (args[0])
            {
                case "-c":
                    TryToCompress(args[1], args[2]);
                    break;
                case "-d":
                    TryToDecompress(args[1], args[2]);
                    break;
                case "-demo":
                    RunDemo();
                    break;
                case "-h":
                default:
                    DisplayHelp();
                    break;
            }
        }

        private static void DisplayHelp()
        {
            string helpMessage =
                "POMOC\n\n" +
                "| Parametr                             | Opis                                                              |\n" +
                "|--------------------------------------|-------------------------------------------------------------------|\n" +
                "| -h                                   | Wyświetla pomoc                                                   |\n" +
                "| -c <plik_wejściowy> <plik_wyjściowy> | Kompresuje plik wejściowy i zapisuje rezultat w pliku wynikowym   |\n" +
                "| -d <plik_wejściowy> <plik_wyjściowy> | Dekompresuje plik wejściowy i zapisuje rezultat w pliku wynikowym |\n" +
                "| -demo                                | Uruchamia demonstrację działania programu                         |\n";

            Console.WriteLine(helpMessage);
        }

        private static void RunDemo()
        {
            Console.WriteLine("DEMO\n--------------------------------------\n");

            string iliada = "iliada.txt", compressed1 = "iliada.lzw", decompressed1 = "ZDEKOMPRESOWANE_iliada.txt";
            string harryPotter = "Harry_Potter.txt", compressed2 = "Harry_Potter.lzw", decompressed2 = "ZDEKOMPRESOWANE_Harry_Potter";

            if (!File.Exists(iliada))
            {
                Console.WriteLine($"Demo wymaka pliku {iliada} w katalogu z programem");
            }

            Console.WriteLine("W ramach demonstracji zostanie przetworzoan Iliada Homera i pierwsza część Harego Potera\n");

            Console.WriteLine("ILIADA: \n");
            TryToCompress(iliada, compressed1);
            TryToDecompress(compressed1, decompressed1);

            Console.WriteLine("\nHARRY POTTER: \n");
            TryToCompress(harryPotter, compressed2);
            TryToDecompress(compressed2, decompressed2);
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
            Console.WriteLine($"Dane z pliku {inputFile} zostały zdekompresowane i zapisane w pliku {outputFile}\n");
        }

        private static void DisplayCompressionInfo(string inputFile, string outputFile)
        {
            long inputFileSize = new FileInfo(inputFile).Length;
            long outputFileSize = new FileInfo(outputFile).Length;
            double compressionDegree = (double)inputFileSize / outputFileSize * 100;

            Console.WriteLine($"Dane z pliku {inputFile} zostały skompresowane i zapisane w pliku {outputFile}");
            Console.WriteLine($"Wielkość pliku wejściowego: {inputFileSize / 1000} KB");
            Console.WriteLine($"Wielkość pliku wyjściowego: {outputFileSize / 1000} KB");
            Console.WriteLine($"Współczynnik kompresjii wynosi: {compressionDegree:0.00}%\n");
        }
    }
}
