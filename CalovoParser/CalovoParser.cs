
using System;
using System.Collections.Generic;

public class Event
{
    public string datetime;
    public string summary;

    public Event() {
        this.datetime = "";
        this.summary = "";
    }
}
public class CalovoParser
{
    private string calendar;
    public CalovoParser(string calendar) {
        this.calendar = calendar;
    }
    public List<Event> GetAllNextEvents(string startDate) {
        List<Event> eventlist = new List<Event>();
        string datum_curr = startDate;
        while(Convert.ToBoolean(datum_curr.Length))
        {
            Event e = this.GetNextEvent(datum_curr.Substring(0, 8));
            datum_curr = e.datetime;
            eventlist.Add(e);
        }
        return eventlist;
    }
    public Event GetNextEvent(string startDate)
    {
        string text, datum, spiel, zeile, datum_k, datum_curr, datum_k_curr;
        int a, b, c, d, e;
        Event g = new Event();
        datum_curr = "";
        datum = null;
        datum_k = "";

        string[] text_gesplittet = this.calendar.Split("BEGIN:VEVENT");
        a = text_gesplittet.Length;
        for (int i = 0; i < a; i++)
        {
            text = text_gesplittet[i];
            if (text.Contains("DTSTART"))
            {

                b = text.IndexOf("DTSTART");
                if (b > -1) {
                    c = text.IndexOf("SEQUENCE");
                    zeile = text.Substring(b, c - b);
                    d = zeile.IndexOf(":");
                    Console.WriteLine("Zeile: " + zeile);
                    datum_k_curr = zeile.Substring(d + 1, 15);
                    datum_curr = zeile.Substring(d + 1, 8);
                    if (Convert.ToInt32(datum_curr) <= Convert.ToInt32(startDate)) {
                        datum_k = "";
                    }
                    else {
                        //Console.WriteLine(Convert.ToInt32(datum));
                        if (Convert.ToInt32(datum) != 0) {
                            if (Convert.ToInt32(datum_curr) <= Convert.ToInt32(datum)) {                        
                                datum_k = datum_k_curr;
                            }
                        }
                        else {
                            datum_k = datum_k_curr;
                            datum = datum_curr;
                        }                       
                    }
                    g.datetime = datum_k;
                }
                /*
                b = text.IndexOf("SUMMARY");
                if (b > -1) {
                    c = text.IndexOf("DESCRIPTION");
                    zeile = text.Substring(b, c - b);
                    d = zeile.IndexOf(":");
                    e = zeile.IndexOf("|");
                    g.summary = zeile.Substring(d + 1, e - d - 1).Trim();
                    
                    if (Convert.ToInt32(datum) < Convert.ToInt32(startDate)) {
                        g.summary = "";
                    }
                }
                */
            }
        }
        return g;
    }
}
