# Algorytm kompresji LZW
Realizacja algorytmu na laboratoraia z algorytmówi i struktur danych.  
By zobaczyć działanie programu należy odpalic w terminalu program _LZW_Compersion.exe_

## Główne funkcje
* Kompresja
* Dekompresja
* Obliczenie współczynnika kompresji

## Użycie
| Parametr                                     | Opis                                                              |
|----------------------------------------------|-------------------------------------------------------------------|
| **-h**                                       | Wyświetla pomoc                                                   |
| **-c** <_plik_wejściowy_> <_plik_wyjściowy_> | Kompresuje plik wejściowy i zapisuje rezultat w pliku wynikowym   |
| **-d** <_plik_wejściowy_> <_plik_wyjściowy_> | Dekompresuje plik wejściowy i zapisuje rezultat w pliku wynikowym |
| **-demo**                                    | Uruchamia demonstrację działania programu                         |

## Dodatkowe informacje
Algorytm kożysta z kodowania znaków UTF-16 przez co każdy kod ma wielkość 32 bitów.  
Pozwala to na uzyskanie takich samych wyników niezależnie od języka w jakim został napisany orginalny tekst.
Takie podejście jest jednak mniej efektywne dla tekstów składających się jedynie ze znaków ASCII. Dla takich danych lepiej byłoby zastosować 8-bitowy kod znaku i 16-bitowy kod ciągu.

--------------------
**Autor**: Krzysztof Dąbrowski