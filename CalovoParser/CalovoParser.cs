
using System;

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
        Console.WriteLine(calendar);
        this.calendar = calendar;
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
                    datum_k_curr = zeile.Substring(d + 1, 15);
                    datum_curr = zeile.Substring(d + 1, 8);
                    Console.WriteLine(datum_curr);
                    if (Convert.ToInt32(datum_curr) < Convert.ToInt32(startDate)) {
                        datum_k = "";
                    }
                    else {
                        Console.WriteLine(Convert.ToInt32(datum));
                        if (Convert.ToInt32(datum) != 0) {
                            if (Convert.ToInt32(datum_curr) < Convert.ToInt32(datum)) {                        
                                datum_k = datum_k_curr;
                            }
                        }
                        else {
                            Console.WriteLine(datum_curr);
                            Console.WriteLine(datum_k);
                            datum_k = datum_k_curr;
                            datum = datum_curr;
                            Console.WriteLine(datum_k);
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
