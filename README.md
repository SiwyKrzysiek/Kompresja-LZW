# Algorytm kompresji LZW
Realizacja algorytmu na laboratoraia z algorytmówi i struktur danych.

## Główne funkcje
* Kompresja
* Dekompresja
* Obliczenie współczynnika kompresji

## Użycie
| Parametr                             | Opis                                                              |
|--------------------------------------|-------------------------------------------------------------------|
| -h                                   | Wyświetla pomoc                                                   |
| -c <plik_wejściowy> <plik_wyjściowy> | Kompresuje plik wejściowy i zapisuje rezultat w pliku wynikowym   |
| -d <plik_wejściowy> <plik_wyjściowy> | Dekompresuje plik wejściowy i zapisuje rezultat w pliku wynikowym |

## Dodatkowe informacje
Algorytm kożysta z kodowania znaków UTF-16 przez co każdy kod ma wielkość 32 bitów.  
Pozwala to na uzyskanie takich samych wyników niezależnie od języka w jakim został napisany orginalny tekst.
Takie podejście jest jednak mniej efektywne dla tekstów składających się jedynie ze znaków ASCII. Dla takich danych lepiej byłoby zastosować 8-bitowy kod znaku i 16-bitowy kod ciągu.

--------------------
**Autor**: Krzysztof Dąbrowski