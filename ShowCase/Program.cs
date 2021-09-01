using System;
using ParserLibs;


using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World");
        string text = System.IO.File.ReadAllText(@"assets\buli.ics");
        CalovoParser cp = new CalovoParser(text);
        List<Event> events = cp.GetAllNextEvents("20210827");
        Console.WriteLine(events[0].datetime);
    }
}
