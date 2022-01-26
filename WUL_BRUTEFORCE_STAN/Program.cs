using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WUL_BRUTEFORCE_STAN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Words File Path: ");
            string wpath = Console.ReadLine();
            string[] words = File.ReadAllLines(wpath);
            Console.WriteLine($"File Loaded: Registered {words.Length} words!");
            Console.WriteLine();

            Console.Write("Enter Word Length: ");
            int length = Convert.ToInt32(Console.ReadLine());
            List<string> pwords = words.ToList().FindAll(x => x.Length == length);
            List<string> uwords = new List<string> { };
            pwords.ForEach(x => uwords.Add(x.ToLower()));
            if (uwords.Count < 1)
            {
                Console.WriteLine("Invalid Length");
            } else
            {
                Console.WriteLine($"Found {uwords.Count} possible words.");
                Console.WriteLine();
                string solution = SolveWord(uwords);

                Console.WriteLine("-------------------------");
                Console.WriteLine($"Riddle Solved: {solution}");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Press any key to exit.");
            }

            Console.ReadKey();
        }

        public static string SolveWord(List<string> words)
        {
            List<string> blacklist = new List<string> { };
            string startword = "";
            while (true)
            {
                Console.WriteLine("Solving...");
                Console.WriteLine("Finding Optimal Start");
                int maxv = 0;
                startword = "";
                words.ForEach(x =>
                {
                    if (GDF(x) > maxv && !blacklist.Contains(x))
                    {
                        startword = x;
                        maxv = GDF(x);
                    }
                });
                Console.WriteLine("-------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Found Word: {startword}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------");
                Console.WriteLine("Press any key to continue.");
                Console.Write("Does this word work? (y/n): ");
                if (Console.ReadLine() == "y")
                {
                    Console.WriteLine();
                    break;
                    
                }
                else
                {
                    blacklist.Add(startword);
                }
            } 

            char[] solution = new char[startword.Length];
            List<char> invalid = new List<char> { };
            List<char> excluded = new List<char> { };

            int attempts = 1;
            while (true) {
                Console.WriteLine("Please enter the status of letters as presented.");
                Console.WriteLine("x - gray, u - orange, c - green, b - blacklist word");
                Console.WriteLine("-------------------------");
                int lc = 0;
                foreach (char c in startword)
                {
                    if (solution[lc] == c)
                    {
                        Console.WriteLine($"{c}: Skipping (Already Correct)");
                    } else
                    {
                        Console.Write($"{c}: ");
                        string option = Console.ReadLine();
                        if (option == "b")
                        {
                            blacklist.Add(startword);
                            break;
                        }
                        switch(option)
                        {
                            case "x":
                                if (!excluded.Contains(c) && !solution.Contains(c) && !invalid.Contains(c))
                                {
                                    excluded.Add(c);
                                }
                                break;
                            case "u":
                                if (!invalid.Contains(c))
                                {
                                    invalid.Add(c);
                                }
                                break;
                            case "c":
                                invalid.Remove(c);
                                solution[lc] = c;
                                break;
                        }
                    }
                    lc++;
                }
                Console.WriteLine("-------------------------");
                Console.Write("Current Status: ");
                foreach (char c in solution)
                {
                    if (c == '\x0000')
                    {
                        Console.Write("_");
                    } else
                    {
                        Console.Write(c);
                    }
                }
                Console.WriteLine("\n\n");

                Console.WriteLine("Solving...");
                Console.WriteLine("Finding Optimal Word");

                words.ForEach(x => {
                    bool fitinitial1 = true;
                    int ic = 0;
                    foreach (char w in solution)
                    {
                        if (w != '\x0000' && w != x[ic])
                        {
                            fitinitial1 = false;
                        }
                        ic++;
                    }
                    if (blacklist.Contains(x))
                    {
                        fitinitial1 = false;
                    }
                    if (fitinitial1 == true)
                    {
                        bool ispossible = true;
                        foreach (char c in x)
                        {
                            if (excluded.Contains(c))
                            {
                                ispossible = false;
                            }
                        }
                        if (ispossible == true && (invalid.All(y => x.Contains(y))))
                        {
                            startword = x;
                            blacklist.Add(x);
                            return;
                        }
                    }
                });

                if (!solution.Contains('\x0000'))
                {
                    return new string(solution);
                }

                Console.WriteLine("-------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Found Word: {startword}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------");
                Console.WriteLine("Press any key to continue.");
                Console.WriteLine();

                attempts++;
            }
        }

        public static int GDF(string word)
        {
            return vowels.ToList().FindAll(x => word.Contains(x)).Count;
        }

        public static char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
    }
}
