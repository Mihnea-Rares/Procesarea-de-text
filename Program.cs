using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;


namespace ProiectAPD
{
    
    class Program
    {
       
        static void Main(string[] args)
        {
            
            string filePath = "date.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fișierul nu există!");
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();

            foreach (string line in File.ReadLines(filePath))
            {
                string[] words = line.Split(new[] { ' ', '.', ',', '!' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string cleanWord = word.ToLower();
                    if (wordCounts.ContainsKey(cleanWord))
                        wordCounts[cleanWord]++;
                    else
                        wordCounts[cleanWord] = 1;
                }
            }

            stopwatch.Stop(); 
            Console.WriteLine($"Gata! Timp: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}


