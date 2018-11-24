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
            if (DataBlock.MaxValue <= char.MaxValue) //Upewnienie się, że typ danych na kody jest dbrze wybrany
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
            if (DataBlock.MaxValue <= char.MaxValue) //Upewnienie się, że typ danych na kody jest dbrze wybrany
                throw new Exception("To small DataBlock set in code");
            if (compressedData.Length == 0) //Obsłużenie pustych danych
                return "";

            StringBuilder result = new StringBuilder(); //Zmienna na budowanie wyniku dekompresji
            Dictionary<DataBlock, string> dictionary = CreateDecmpressionDictionary(); //Utworzenie słownika do dekompresjii

            DataBlock previousCode = compressedData[0]; //Na początek wczytuję pierwszy kod
            result.Append(dictionary[previousCode]); //Wypisanie znaku zakodowanego pierwszym kodem

            for (int i = 1; i < compressedData.Length; i++) //Dopóki są jeszcze kody
            {
                DataBlock newCode = compressedData[i]; //Zapisanie aktualnie przetwarzanego kodu
                string previousSymbol = dictionary[previousCode]; //Zobaczenie jaki ciąg znaków kodował poprzedni kod

                if (dictionary.ContainsKey(newCode)) //Jeżeli słownik zawiera aktualnie wczytany kod
                {
                    dictionary.Add((DataBlock)dictionary.Count, previousSymbol + dictionary[newCode][0]);
                    result.Append(dictionary[newCode]);
                }
                else
                {
                    dictionary.Add((DataBlock)dictionary.Count, previousSymbol + previousSymbol[0]);
                    result.Append(previousSymbol + previousSymbol[0]);
                }

                previousCode = newCode;
            }

            return result.ToString();
        }

        /// <summary>
        /// //Wypełana słownik pojedyńczymi znakami
        /// </summary>
        /// <returns>Słownik do kompresji</returns>
        private static Dictionary<string, DataBlock> CreateDictionary() 
        {
            Dictionary<string, DataBlock> dictionary = new Dictionary<string, DataBlock>(); //Utworzenie pustego słownika

            for (char i = char.MinValue; i < char.MaxValue; i++) //Wpisanie wszystkich możliwych pojednyńczych znaków do słownika
                dictionary.Add(i.ToString(), (DataBlock)dictionary.Count); //Generowanie kodów polega na przypisaniu aktualnej wielkości słownika
                                                                           //Ponieważ słownik nigdy nie maleje kody są unikalne
            return dictionary;
        }

        /// <summary>
        /// Wypełana słownik kodami pojedyńczych znaków
        /// </summary>
        /// <returns>Słownik do dekompresjii</returns>
        private static Dictionary<DataBlock, string> CreateDecmpressionDictionary()
        {
            Dictionary<DataBlock, string> dictionary = new Dictionary<DataBlock, string>(); //Utworzenie pustego słownika

            for (char i = char.MinValue; i < char.MaxValue; i++) //Wpisanie do słownika kodów wszystkich pojedyńczych znaków
                dictionary.Add((DataBlock)dictionary.Count, i.ToString()); //Generowanie kodów polega na przypisaniu aktualnej wielkości słownika
                                                                           //Ponieważ słownik nigdy nie maleje kody są unikalne

            return dictionary;
        }
    }
}
