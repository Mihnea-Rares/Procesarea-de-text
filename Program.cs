using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcesareText_Paralela
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "date.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Eroare: Fișierul nu a fost găsit!");
                return;
            }

            Console.WriteLine("Începem procesarea PARALELĂ ...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Dicționarul global în care adunăm totul la final
            ConcurrentDictionary<string, int> globalWordCounts = new ConcurrentDictionary<string, int>();

            // Citim liniile și folosim Thread-Local Storage pentru a evita blocajele
            Parallel.ForEach(
                File.ReadLines(filePath),

                // 1. Inițializăm un dicționar local pentru fiecare Thread
                () => new Dictionary<string, int>(),

                // 2. Procesăm liniile: fiecare thread scrie doar în dicționarul său local
                (line, loopState, localDictionary) =>
                {
                    string[] words = line.Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '(', ')' },
                                              StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        string cleanWord = word.ToLower();
                        if (localDictionary.ContainsKey(cleanWord))
                            localDictionary[cleanWord]++;
                        else
                            localDictionary[cleanWord] = 1;
                    }
                    return localDictionary;
                },

                // 3. La finalul fiecărui Thread, adunăm dicționarul local în cel global
                (localDictionary) =>
                {
                    foreach (var kvp in localDictionary)
                    {
                        globalWordCounts.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => oldValue + kvp.Value);
                    }
                }
            );

            stopwatch.Stop();

            Console.WriteLine($"Timp execuție PARALEL OPTIMIZAT: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Cuvinte unice găsite: {globalWordCounts.Count}");
        }
    }
}