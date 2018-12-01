using System;
using System.IO;
using System.Linq;

namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine(File.ReadLines("input.txt").Select(int.Parse).Sum());
        }
    }
}
