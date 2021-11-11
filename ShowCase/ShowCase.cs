using System;
using ParserLibs;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;



class ShowCase
{
    static readonly HttpClient client = new HttpClient();

    public static async Task MainAsync()
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

        CalovoParser cp = new CalovoParser(text);
        List<Event> events = cp.GetAllNextEvents(DateTime.Today.ToString("yyyyMMdd"), true);
        
        for (int i = 0; i < events.Count; i++) {
            if (events[i].datetime.Length > 8) {
                Console.WriteLine(events[i].GetDateTimeFormatted() + " Uhr : " + events[i].opponent);    
            }
            else {
                Console.WriteLine(events[i].GetDateTimeFormatted() + "           : " + events[i].opponent);
            }
        }
    }

    static void Main2(string[] args)
    {  
        MainAsync().Wait();
    }
}
