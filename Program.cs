using System;

namespace helloworld
{
    class Program
    {


        static void Main(string[] args)
        {
            string calendar = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//calovo//Calendar Publishing 1.14
METHOD:PUBLISH
CALSCALE:GREGORIAN
X-MICROSOFT-CALSCALE:GREGORIAN
X-WR-CALNAME:Borussia Dortmund
X-WR-CALDESC:Alle Bundesliga-Spieltermine von Borussia Dortmund immer aktue
 ll im Kalender.
X-WR-TIMEZONE:Europe/Berlin
X-WR-RELCALID:7d4e9bd7.ecdae2a7@2704.calovo.com
X-PUBLISHED-TTL:PT30M
BEGIN:VEVENT
UID:5d15eb45c48f4@2704.calovo
DTSTART;TZID=Europe/Berlin:20200606T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20200606T203000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Hertha BSC | Bundesliga | 30. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n30. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20200604T090112Z
CREATED:20190628T102613Z
LAST-MODIFIED:20200604T090112Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Hertha BSC | Bundesliga | 30. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5d15eb4799858@2704.calovo
DTSTART;TZID=Europe/Berlin:20200613T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20200613T173000
LOCATION:Merkur Spielarena
SUMMARY:Fortuna Düsseldorf - Borussia Dortmund | Bundesliga | 31. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n31. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20200613T100139Z
CREATED:20190628T102615Z
LAST-MODIFIED:20200613T100139Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Fortuna Düsseldorf - Borussia Dortmund | Bundesliga 
 | 31. Spieltag
END:VALARM
END:VEVENT";
            
        string[] text_gesplittet;
        string text, datum, spiel, zeile;
        int a, b, c, d, e;

            text_gesplittet = calendar.Split("BEGIN:VEVENT");
            a = text_gesplittet.Length;
            for (int i = 0; i < a; i++)
            {
                text = text_gesplittet[i];
                if (text.Contains("DTSTART"))
                {
                    //Console.WriteLine(text);
                    b = text.IndexOf("DTSTART");
                    c = text.IndexOf("SEQUENCE");
                    zeile = text.Substring(b, c - b);
                    d = zeile.IndexOf(":");
                    datum = zeile.Substring(d + 1, 15);
                    Console.WriteLine(datum);
                }
                if (text.Contains("SUMMARY"))
                {
                    b = text.IndexOf("SUMMARY");
                    c = text.IndexOf("DESCRIPTION");
                    zeile = text.Substring(b, c - b);
                    d = zeile.IndexOf(":");
                    e = zeile.IndexOf("|");
                    spiel = zeile.Substring(d + 1, e - d - 1);
                    Console.WriteLine(spiel);
                }
            }
        }
    }
}
