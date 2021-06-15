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
LOCATION:Merkur Spielarena";

        //{
            
        //}
        string[] rest;

            //calendar.Substring()
            rest = calendar.Split("BEGIN:VEVENT");
            Console.WriteLine(rest);
        }
    }
}
