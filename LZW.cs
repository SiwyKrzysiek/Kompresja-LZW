using System;
using System.Collections.Generic;
using System.Text;

namespace LZW_Compersion
{
    using DataBlock = UInt32; //Ustalenie jak będą zapisywane kody

    public static class LZW
    {
        public static DataBlock[] Compress(string data)
        {
            if (DataBlock.MaxValue <= char.MaxValue)
                throw new Exception("To small DataBlock set in code");
            if (data.Length == 0) return new DataBlock[0]; //Edge case dla pustych danych

            List<DataBlock> result = new List<DataBlock>(); //Wynik kodowania
            Dictionary<string, DataBlock> dictionary = CreateDictionary(); //Utworzenie początkowego słownika

            string currentSequence = data[0].ToString(); //Wczytanie pierwszego znaku do kolejki aktualnie kodowanych znaków
            for (int i = 1; i < data.Length; i++) //Od 2 elementu, bo piewszy już został wczytany
            {
                string sequenceWithNewSymbol = currentSequence + data[i]; //Ciąg z aktualnie przetwarzanym znakiem

                if (dictionary.ContainsKey(sequenceWithNewSymbol)) //Sprawdzenie czy przedłużony ciąg jest w słowniku
                {
                    currentSequence = sequenceWithNewSymbol; //Jeśli aktualny ciąg jest już w słowniku to dodajemy aktualny znak do aktualnego ciągu
                }
                else //Nie ma kodu dla rozszeżonego ciągu
                {
                    result.Add(dictionary[currentSequence]); //Wypisanie kodu dla ciągu, który do tej pory był w słowniki

                    dictionary.Add(sequenceWithNewSymbol, (DataBlock)dictionary.Count); //Dodanie nowego ciągu do słownika
                    currentSequence = data[i].ToString(); //Zresetowanie ciągu kodowanych znaków
                }
            }

            result.Add(dictionary[currentSequence]); //Dopisanie pozosatałego ciągu znaków

            return result.ToArray();
        }

        public static string Decompress(DataBlock[] compressedData)
        {
            if (DataBlock.MaxValue <= char.MaxValue)
                throw new Exception("To small DataBlock set in code");
            if (compressedData.Length == 0)
                return "";

            StringBuilder result = new StringBuilder();
            Dictionary<DataBlock, string> dictionary = CreateDecmpressionDictionary();

            DataBlock previousCode = compressedData[0];
            result.Append(dictionary[previousCode]);

            for (int i = 1; i < compressedData.Length; i++)
            {
                DataBlock newCode = compressedData[i];
                string pc = dictionary[previousCode];

                if (dictionary.ContainsKey(newCode))
                {
                    dictionary.Add((DataBlock)dictionary.Count, pc + dictionary[newCode][0]);
                    result.Append(dictionary[newCode]);
                }
                else
                {
                    dictionary.Add((DataBlock)dictionary.Count, pc + pc[0]);
                    result.Append(pc + pc[0]);
                }

                previousCode = newCode;
            }

            return result.ToString();
        }

        private static Dictionary<string, DataBlock> CreateDictionary() //Wypełani słownik pojedyńczymi znakami
        {
            Dictionary<string, DataBlock> dictionary = new Dictionary<string, DataBlock>();

            for (char i = char.MinValue; i < char.MaxValue; i++) //Wpisanie wszystkich możliwych pojednyńczych znaków do słownika
                dictionary.Add(i.ToString(), (DataBlock)dictionary.Count); //Generowanie kodów polega na przypisaniu aktualnej wielkości słownika
                                                                           //Ponieważ słownik nigdy nie maleje kody są unikalne
            return dictionary;
        }

        private static Dictionary<DataBlock, string> CreateDecmpressionDictionary()
        {
            Dictionary<DataBlock, string> dictionary = new Dictionary<DataBlock, string>();

            for (char i = char.MinValue; i < char.MaxValue; i++)
                dictionary.Add((DataBlock)dictionary.Count, i.ToString());

            return dictionary;
        }
    }
}
