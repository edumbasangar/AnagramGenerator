using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace AnagramGenerator
{
    class Program
    {
        private static PhysicalFileProvider _fileProvider =
            new PhysicalFileProvider(Directory.GetCurrentDirectory());

        static void Main(string[] args)
        {
            Console.WriteLine("Please make sure dictionary.txt file is present in the Debug folder :");
            var d = ReadAsync();
            d.Wait();

            // Read in user input and show anagrams.
            string line;
            Console.WriteLine("Please Enter the Text :");
            while ((line = Console.ReadLine()) != null)
            {
                Show(d.Result, line);
            }

        }

        static async Task<Dictionary<string, string>> ReadAsync()
        {
            var d = new Dictionary<string, string>();

            String result;

            //var inputFile = _fileProvider.GetDirectoryContents();

            using (StreamReader reader = File.OpenText("dictionary.txt"))
            {
                Console.WriteLine("Opened file.");
                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string strAlphabetize = Alphabetize(result);
                    string strDictionaryOut;
                    if (d.TryGetValue(strAlphabetize, out strDictionaryOut))
                    {
                        d[strAlphabetize] = strDictionaryOut + "," + result;
                    }
                    else
                    {
                        d.Add(strAlphabetize, result);
                    }
                }

            }

            return d;
        }

        static string Alphabetize(string s)
        {
            char[] a = s.ToCharArray();
            Array.Sort(a);
            return new string(a);
        }

        static void Show(Dictionary<string, string> d, string w)
        {
            string v;
            if (d.TryGetValue(Alphabetize(w), out v))
            {
                Console.WriteLine(v);
            }
            else
            {
                Console.WriteLine("-");
            }
        }
    }
}
