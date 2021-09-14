using System;
using ParserLibs;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;



class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task MainAsync()
    {
        string text = "";
        try	
        {
            HttpResponseMessage response = await client.GetAsync("http://i.cal.to/ical/2704/bundesliga/borussia-dortmund/6be29136.4e662db2-12985be5.ics");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            text = responseBody;
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
        }

        //string text = System.IO.File.ReadAllText(@"assets\buli.ics");
        CalovoParser cp = new CalovoParser(text);
        List<Event> events = cp.GetAllNextEvents("20210827");
        
        for (int i = 0; i < 18; i++) {
            if (events[i].datetime.Length > 8) {
                Console.WriteLine(events[i].GetDateTimeFormatted() + " Uhr : " + events[i].GetSummaryFormatted());    
            }
            else {
                Console.WriteLine(events[i].GetDateTimeFormatted() + "           : " + events[i].GetSummaryFormatted());
            }
        }
    }

    static void Main(string[] args)
    {  
        MainAsync().Wait();
    }
}
