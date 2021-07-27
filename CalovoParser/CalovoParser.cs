
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
        string text, datum, spiel, zeile;
        int a, b, c, d, e;
        Event g = new Event();
        datum = "";

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
                    g.datetime = zeile.Substring(d + 1, 15);
                    datum = zeile.Substring(d + 1, 8);
                    Console.WriteLine(datum);
                    if (Convert.ToInt32(datum) < Convert.ToInt32(startDate)) {
                        g.datetime = "";
                    }
                }

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
            }
        }
        return g;
    }
}
