using System;
using ParserLibs;


using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        string text = System.IO.File.ReadAllText(@"assets\buli.ics");
        CalovoParser cp = new CalovoParser(text);
        List<Event> events = cp.GetAllNextEvents("20210827");
        
        for (int i = 0; i < 10; i++) {
            Console.WriteLine(events[i].datetime + ": " + events[i].summary);
        }
    }
}
