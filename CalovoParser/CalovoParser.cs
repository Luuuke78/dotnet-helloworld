﻿
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

        b = this.calendar.IndexOf("DTSTART");
        if (b > -1) {
            c = this.calendar.IndexOf("SEQUENCE");
            zeile = this.calendar.Substring(b, c - b);
            d = zeile.IndexOf("#");
            g.datetime = zeile.Substring(d + 1, 15);
            datum = zeile.Substring(d + 1, 8);

            if (Convert.ToInt32(datum) < Convert.ToInt32(startDate)) {
                g.datetime = "";
            }
        }

        b = this.calendar.IndexOf("SUMMARY");
        if (b > -1) {
            c = this.calendar.IndexOf("DESCRIPTION");
            zeile = this.calendar.Substring(b, c - b);
            d = zeile.IndexOf("#");
            e = zeile.IndexOf("|");
            g.summary = zeile.Substring(d + 1, e - d - 1).Trim();
            
            if (Convert.ToInt32(datum) < Convert.ToInt32(startDate)) {
                g.summary = "";
            }
        }
        return g;
    }
}
