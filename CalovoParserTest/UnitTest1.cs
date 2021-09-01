using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ParserLibs;

namespace CalovoParserTests
{
    [TestClass]
    public class CalovoParserTest
    {
        [TestMethod]
        public void Test_GetNextEvent_emptyCal_returnsEmptySummaryAndDate()
        {
            CalovoParser p = new CalovoParser("");
            Assert.AreEqual("", p.GetNextEvent("").summary);
        }

        private string createStringForOneEvent(string datetime, string summary) {
            return string.Format(@"----BEGIN:VEVENT
DTSTART;TZID=Europe/Berlin:{0}
SEQUENCE:0
DTEND;TZID=Europe/Berlin:20200606T203000
SUMMARY:{1}
DESCRIPTION:----
END:VEVENT", datetime, summary);
        }
        

        [TestMethod]
        public void Test_GetNextEvent_calWithOneEntry_returnsReferenceEvent()
        {
            string summary = "Sample summary...";
            string datetime = "20210721T183000";
            string calendar = this.createStringForOneEvent(datetime, summary);
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210720");
            Assert.AreEqual(datetime, e.datetime);
            Assert.AreEqual(summary, e.summary);
        }
        [TestMethod]
        public void Test_GetNextEvent_calWithOneEntryButOnlyDate_returnsReferenceEvent()
        {
            string summary = "Sample summary...";
            string datetime = "20210721";
            string calendar = this.createStringForOneEvent(datetime, summary);
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210720");
            Assert.AreEqual(datetime, e.datetime);
            Assert.AreEqual(summary, e.summary);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithOlderEntry_returnsEmptyEvent()
        {
            string calendar = this.createStringForOneEvent("20210721T183000", "BVB | Hertha | 30.");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210812");
            Assert.AreEqual("", e.datetime);
            Assert.AreEqual("", e.summary);
        }
        

        [TestMethod]
        public void Test_GetNextEvent_calWithTwoEntriesAskForFirstOne_returnsFirstEvent()
        {
            string firstDateTime = "20210721T183000";
            string calendar = this.createStringForOneEvent(firstDateTime, "Summary1") +
                this.createStringForOneEvent("20210728T183000", "Summary2");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210601");
            Assert.AreEqual(firstDateTime, e.datetime);
            Assert.AreEqual("Summary1", e.summary);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithTwoEntriesAskForSecondOne_returnsSecondEvent()
        {
            string secondDateTime = "20210728T183000";
            string calendar = this.createStringForOneEvent("20210721T183000", "Summary1") +
                this.createStringForOneEvent(secondDateTime, "Summary2");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210723");
            Assert.AreEqual(secondDateTime, e.datetime);
            Assert.AreEqual("Summary2", e.summary);
        }

        [TestMethod]
        public void Test_GetAllNextEvents_calOneOlderEntryTwoNextEntries_returnsOnlyNextEntries()
        {
            string oldDateTime = "20210728T183000";
            string nextDateTime0 = "20210828T183000";
            string nextDateTime1 = "20210928T183000";
            string nextDateTime2 = "20211028T183000";
            string calendar = 
                this.createStringForOneEvent(oldDateTime, "Summary") +
                this.createStringForOneEvent(nextDateTime0, "Summary0") +
                this.createStringForOneEvent(nextDateTime1, "Summary1") +
                this.createStringForOneEvent(nextDateTime2, "Summary2");
            CalovoParser p = new CalovoParser(calendar);
            List<Event> eventlist = p.GetAllNextEvents("20210801");
            Assert.AreEqual(nextDateTime0, eventlist[0].datetime);
            Assert.AreEqual(nextDateTime1, eventlist[1].datetime);
            Assert.AreEqual(nextDateTime2, eventlist[2].datetime);
            Assert.AreEqual("Summary0", eventlist[0].summary);
            Assert.AreEqual("Summary1", eventlist[1].summary);
            Assert.AreEqual("Summary2", eventlist[2].summary);
        }

        [TestMethod]
        public void Test_GetAllNextEvents_currentCalendar_returnsValidEntries()
        {
            CalovoParser p = new CalovoParser(this.GetCurrentCalendar());
            List<Event> events = p.GetAllNextEvents("20210825");
            // Console.WriteLine(events);   
            foreach(Event e in events) {
                Console.WriteLine(e.summary);
            }
            Assert.AreEqual(33, events.Count);
            Assert.AreEqual("20210827T203000", events[0].datetime);
            // Assert.AreEqual("20210827T203000", events[0].datetime);
        }
        

        private string GetCurrentCalendar() {
            return string.Format(@"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//calovo//Calendar Publishing 1.14
METHOD:PUBLISH
CALSCALE:GREGORIAN
X-MICROSOFT-CALSCALE:GREGORIAN
X-WR-CALNAME:Borussia Dortmund
X-WR-CALDESC:Alle Bundesliga-Spieltermine von Borussia Dortmund immer aktue
 ll im Kalender.
X-WR-TIMEZONE:Europe/Berlin
X-WR-RELCALID:f01a37ea.782562b9@2704.calovo.com
X-PUBLISHED-TTL:PT30M
BEGIN:VEVENT
UID:5f2d27702e1c0@2704.calovo
DTSTART;TZID=Europe/Berlin:20200919T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20200919T203000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Borussia Mönchengladbach | Bundesliga | 1. Spi
 eltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n1. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20200917T070430Z
CREATED:20200807T100536Z
LAST-MODIFIED:20200917T070430Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Borussia Mönchengladbach | Bunde
 sliga | 1. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d277304e00@2704.calovo
DTSTART;TZID=Europe/Berlin:20200926T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20200926T173000
LOCATION:WWK ARENA
SUMMARY:FC Augsburg - Borussia Dortmund | Bundesliga | 2. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n2. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20200923T051052Z
CREATED:20200807T100539Z
LAST-MODIFIED:20200923T051052Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: FC Augsburg - Borussia Dortmund | Bundesliga | 2. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f5c5a5aeabac@2704.calovo
DTSTART;TZID=Europe/Berlin:20200930T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20200930T223000
LOCATION:Allianz Arena
SUMMARY:FC Bayern München - Borussia Dortmund | Supercup
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\nFC Bayern München vs. Borussia Dortmund\n\n\n\n\nAnbieter-Impressum: h
 ttps://www.bundesliga.com/de/bundesliga/info/impressum/\n\n\ncalfeed lösc
 hen? Anleitung: bit.ly/calfeed_loeschen
DTSTAMP:20200923T051301Z
CREATED:20200912T051922Z
LAST-MODIFIED:20200923T051301Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: FC Bayern München - Borussia Dortmund | Supercup
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27736d869@2704.calovo
DTSTART;TZID=Europe/Berlin:20201003T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201003T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Sport-Club Freiburg | Bundesliga | 3. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n3. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20200923T051052Z
CREATED:20200807T100539Z
LAST-MODIFIED:20200923T051052Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Sport-Club Freiburg | Bundesliga 
 | 3. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27752d5d9@2704.calovo
DTSTART;TZID=Europe/Berlin:20201017T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201017T173000
LOCATION:PreZero Arena
SUMMARY:TSG Hoffenheim - Borussia Dortmund | Bundesliga | 4. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n4. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20201015T140426Z
CREATED:20200807T100541Z
LAST-MODIFIED:20201015T140426Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: TSG Hoffenheim - Borussia Dortmund | Bundesliga | 4. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d2776e8fa9@2704.calovo
DTSTART;TZID=Europe/Berlin:20201024T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201024T203000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - FC Schalke 04 | Bundesliga | 5. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n5. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20201023T050541Z
CREATED:20200807T100542Z
LAST-MODIFIED:20201023T050541Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - FC Schalke 04 | Bundesliga | 5. S
 pieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d2779d96c7@2704.calovo
DTSTART;TZID=Europe/Berlin:20201031T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201031T173000
LOCATION:SchücoArena
SUMMARY:DSC Arminia Bielefeld - Borussia Dortmund | Bundesliga | 6. Spielta
 g
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n6. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20201026T220256Z
CREATED:20200807T100545Z
LAST-MODIFIED:20201026T220256Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: DSC Arminia Bielefeld - Borussia Dortmund | Bundeslig
 a | 6. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d2779ee44e@2704.calovo
DTSTART;TZID=Europe/Berlin:20201107T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201107T203000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - FC Bayern München | Bundesliga | 7. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n7. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20201026T220431Z
CREATED:20200807T100545Z
LAST-MODIFIED:20201026T220431Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - FC Bayern München | Bundesliga |
  7. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d277c9d1f2@2704.calovo
DTSTART;TZID=Europe/Berlin:20201121T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201121T223000
LOCATION:Olympiastadion
SUMMARY:Hertha BSC - Borussia Dortmund | Bundesliga | 8. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n8. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20201119T110520Z
CREATED:20200807T100548Z
LAST-MODIFIED:20201119T110520Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Hertha BSC - Borussia Dortmund | Bundesliga | 8. Spie
 ltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d277d447de@2704.calovo
DTSTART;TZID=Europe/Berlin:20201128T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201128T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - 1. FC Köln | Bundesliga | 9. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n9. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20201119T110342Z
CREATED:20200807T100549Z
LAST-MODIFIED:20201119T110342Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - 1. FC Köln | Bundesliga | 9. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d277f75d78@2704.calovo
DTSTART;TZID=Europe/Berlin:20201205T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201205T173000
LOCATION:Deutsche Bank Park
SUMMARY:Eintracht Frankfurt - Borussia Dortmund | Bundesliga | 10. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n10. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20201203T160127Z
CREATED:20200807T100551Z
LAST-MODIFIED:20201203T160127Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Eintracht Frankfurt - Borussia Dortmund | Bundesliga 
 | 10. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d278077f09@2704.calovo
DTSTART;TZID=Europe/Berlin:20201212T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201212T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - VfB Stuttgart | Bundesliga | 11. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n11. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20201211T110145Z
CREATED:20200807T100552Z
LAST-MODIFIED:20201211T110145Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - VfB Stuttgart | Bundesliga | 11. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27838f161@2704.calovo
DTSTART;TZID=Europe/Berlin:20201215T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201215T223000
LOCATION:wohninvest WESERSTADION
SUMMARY:SV Werder Bremen - Borussia Dortmund | Bundesliga | 12. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n12. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20201211T110147Z
CREATED:20200807T100555Z
LAST-MODIFIED:20201211T110147Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: SV Werder Bremen - Borussia Dortmund | Bundesliga | 1
 2. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27851ae2f@2704.calovo
DTSTART;TZID=Europe/Berlin:20201218T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20201218T223000
LOCATION:An der Alten Försterei
SUMMARY:1. FC Union Berlin - Borussia Dortmund | Bundesliga | 13. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n13. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20201217T150309Z
CREATED:20200807T100557Z
LAST-MODIFIED:20201217T150309Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: 1. FC Union Berlin - Borussia Dortmund | Bundesliga |
  13. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d2786478fe@2704.calovo
DTSTART;TZID=Europe/Berlin:20210103T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210103T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - VfL Wolfsburg | Bundesliga | 14. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n14. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20201223T120214Z
CREATED:20200807T100558Z
LAST-MODIFIED:20201223T120214Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - VfL Wolfsburg | Bundesliga | 14. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d2787d1587@2704.calovo
DTSTART;TZID=Europe/Berlin:20210109T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210109T203000
LOCATION:Red Bull Arena
SUMMARY:RB Leipzig - Borussia Dortmund | Bundesliga | 15. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n15. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210107T140246Z
CREATED:20200807T100559Z
LAST-MODIFIED:20210107T140246Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: RB Leipzig - Borussia Dortmund | Bundesliga | 15. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d2789ae361@2704.calovo
DTSTART;TZID=Europe/Berlin:20210116T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210116T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - 1. FSV Mainz 05 | Bundesliga | 16. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n16. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210116T130328Z
CREATED:20200807T100601Z
LAST-MODIFIED:20210116T130328Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - 1. FSV Mainz 05 | Bundesliga | 16
 . Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d278ba3ebb@2704.calovo
DTSTART;TZID=Europe/Berlin:20210119T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210119T223000
LOCATION:BayArena
SUMMARY:Bayer 04 Leverkusen - Borussia Dortmund | Bundesliga | 17. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n17. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210116T130330Z
CREATED:20200807T100603Z
LAST-MODIFIED:20210116T130330Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Bayer 04 Leverkusen - Borussia Dortmund | Bundesliga 
 | 17. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d278d0237a@2704.calovo
DTSTART;TZID=Europe/Berlin:20210122T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210122T223000
LOCATION:BORUSSIA-PARK
SUMMARY:Borussia Mönchengladbach - Borussia Dortmund | Bundesliga | 18. Sp
 ieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n18. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210120T100400Z
CREATED:20200807T100605Z
LAST-MODIFIED:20210120T100400Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Mönchengladbach - Borussia Dortmund | Bunde
 sliga | 18. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d278ecb1da@2704.calovo
DTSTART;TZID=Europe/Berlin:20210130T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210130T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - FC Augsburg | Bundesliga | 19. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n19. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210120T100256Z
CREATED:20200807T100606Z
LAST-MODIFIED:20210120T100256Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - FC Augsburg | Bundesliga | 19. Sp
 ieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d279122714@2704.calovo
DTSTART;TZID=Europe/Berlin:20210206T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210206T173000
LOCATION:Schwarzwald-Stadion
SUMMARY:Sport-Club Freiburg - Borussia Dortmund | Bundesliga | 20. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n20. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210203T200504Z
CREATED:20200807T100609Z
LAST-MODIFIED:20210203T200504Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Sport-Club Freiburg - Borussia Dortmund | Bundesliga 
 | 20. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d279254b9c@2704.calovo
DTSTART;TZID=Europe/Berlin:20210213T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210213T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - TSG Hoffenheim | Bundesliga | 21. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n21. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210211T150041Z
CREATED:20200807T100610Z
LAST-MODIFIED:20210211T150041Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - TSG Hoffenheim | Bundesliga | 21.
  Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d279502d20@2704.calovo
DTSTART;TZID=Europe/Berlin:20210220T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210220T203000
LOCATION:VELTINS-Arena
SUMMARY:FC Schalke 04 - Borussia Dortmund | Bundesliga | 22. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n22. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210211T150150Z
CREATED:20200807T100613Z
LAST-MODIFIED:20210211T150150Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: FC Schalke 04 - Borussia Dortmund | Bundesliga | 22. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d279607269@2704.calovo
DTSTART;TZID=Europe/Berlin:20210227T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210227T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - DSC Arminia Bielefeld | Bundesliga | 23. Spielt
 ag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n23. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210211T150044Z
CREATED:20200807T100614Z
LAST-MODIFIED:20210211T150044Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - DSC Arminia Bielefeld | Bundeslig
 a | 23. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27978c097@2704.calovo
DTSTART;TZID=Europe/Berlin:20210306T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210306T203000
LOCATION:Allianz Arena
SUMMARY:FC Bayern München - Borussia Dortmund | Bundesliga | 24. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n24. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210305T170226Z
CREATED:20200807T100615Z
LAST-MODIFIED:20210305T170226Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: FC Bayern München - Borussia Dortmund | Bundesliga |
  24. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27992c67f@2704.calovo
DTSTART;TZID=Europe/Berlin:20210313T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210313T203000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Hertha BSC | Bundesliga | 25. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n25. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210312T110354Z
CREATED:20200807T100617Z
LAST-MODIFIED:20210312T110354Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Hertha BSC | Bundesliga | 25. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d279c293fe@2704.calovo
DTSTART;TZID=Europe/Berlin:20210320T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210320T173000
LOCATION:RheinEnergieSTADION
SUMMARY:1. FC Köln - Borussia Dortmund | Bundesliga | 26. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n26. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210319T150204Z
CREATED:20200807T100620Z
LAST-MODIFIED:20210319T150204Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: 1. FC Köln - Borussia Dortmund | Bundesliga | 26. Sp
 ieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d279c986dc@2704.calovo
DTSTART;TZID=Europe/Berlin:20210403T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210403T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Eintracht Frankfurt | Bundesliga | 27. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n27. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210325T140711Z
CREATED:20200807T100620Z
LAST-MODIFIED:20210325T140711Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Eintracht Frankfurt | Bundesliga 
 | 27. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a01a17a@2704.calovo
DTSTART;TZID=Europe/Berlin:20210410T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210410T203000
LOCATION:Mercedes-Benz Arena
SUMMARY:VfB Stuttgart - Borussia Dortmund | Bundesliga | 28. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n28. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210409T210244Z
CREATED:20200807T100624Z
LAST-MODIFIED:20210409T210244Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: VfB Stuttgart - Borussia Dortmund | Bundesliga | 28. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a039d8c@2704.calovo
DTSTART;TZID=Europe/Berlin:20210418T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210418T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - SV Werder Bremen | Bundesliga | 29. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n29. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210416T120133Z
CREATED:20200807T100624Z
LAST-MODIFIED:20210416T120133Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - SV Werder Bremen | Bundesliga | 2
 9. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a23b679@2704.calovo
DTSTART;TZID=Europe/Berlin:20210421T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210421T223000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - 1. FC Union Berlin | Bundesliga | 30. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n30. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210416T120057Z
CREATED:20200807T100626Z
LAST-MODIFIED:20210416T120057Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - 1. FC Union Berlin | Bundesliga |
  30. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a44ff53@2704.calovo
DTSTART;TZID=Europe/Berlin:20210424T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210424T173000
LOCATION:Volkswagen Arena
SUMMARY:VfL Wolfsburg - Borussia Dortmund | Bundesliga | 31. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n31. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210422T140105Z
CREATED:20200807T100628Z
LAST-MODIFIED:20210422T140105Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: VfL Wolfsburg - Borussia Dortmund | Bundesliga | 31. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a5cc7ec@2704.calovo
DTSTART;TZID=Europe/Berlin:20210508T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210508T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - RB Leipzig | Bundesliga | 32. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n32. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210503T130107Z
CREATED:20200807T100629Z
LAST-MODIFIED:20210503T130107Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - RB Leipzig | Bundesliga | 32. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a899800@2704.calovo
DTSTART;TZID=Europe/Berlin:20210516T180000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210516T200000
LOCATION:OPEL ARENA
SUMMARY:1. FSV Mainz 05 - Borussia Dortmund | Bundesliga | 33. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n33. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210503T130146Z
CREATED:20200807T100632Z
LAST-MODIFIED:20210503T130146Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: 1. FSV Mainz 05 - Borussia Dortmund | Bundesliga | 33
 . Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:5f2d27a9493db@2704.calovo
DTSTART;TZID=Europe/Berlin:20210522T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210522T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Bayer 04 Leverkusen | Bundesliga | 34. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n34. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210503T130120Z
CREATED:20200807T100633Z
LAST-MODIFIED:20210503T130120Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Bayer 04 Leverkusen | Bundesliga 
 | 34. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d1b555372b5@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20210625
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20210626
URL:http://www.bundesliga.com/de/saison-2021-22-spielplan-veroeffentlichung
LOCATION:Dein Kalender aktualisiert sich automatisch.
SUMMARY:Spielplan-Veröffentlichung 2021/2022
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\nDein Bundesliga-Spielplan 2021/2022 ist bald da. Wenn am Freitag\, 25.0
 6. die Termine für die nächste Saison bekannt gegeben werden\, erscheine
 n alle Daten auch zeitnah in deinem Kalender-Abonnement. Dazu musst du nic
 hts tun! Der genaue Zeitpunkt des Updates ist auch abhängig von deinen pe
 rsönlichen Kalender-Einstellungen. Für Ungeduldige: aktualisiere deine K
 alender-App nach Bekanntgabe gerne manuell.\n\nZur Eventübersicht: http:/
 /www.bundesliga.com/de/saison-2021-22-spielplan-veroeffentlichung\n\n\n\n\
 nAnbieter-Impressum: https://www.bundesliga.com/de/bundesliga/info/impress
 um/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_loeschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210622T100301Z
CREATED:20210622T100301Z
LAST-MODIFIED:20210622T100301Z
BEGIN:VALARM
TRIGGER:-P0DT6H0M0S
ACTION:DISPLAY
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\nDein Bundesliga-Spielplan 2021/2022 ist bald da. Wenn am Freitag\, 25.0
 6. die Termine für die nächste Saison bekannt gegeben werden\, erscheine
 n alle Daten auch zeitnah in deinem...
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b481e7ab6@2704.calovo
DTSTART;TZID=Europe/Berlin:20210814T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210814T203000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Eintracht Frankfurt | Bundesliga | 1. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n1. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20210726T170753Z
CREATED:20210625T104833Z
LAST-MODIFIED:20210726T170753Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Eintracht Frankfurt | Bundesliga 
 | 1. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60e5bf57f3018@2704.calovo
DTSTART;TZID=Europe/Berlin:20210817T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210817T223000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - FC Bayern München | Supercup
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\nBorussia Dortmund vs. FC Bayern München\n\n\n\n\nAnbieter-Impressum: h
 ttps://www.bundesliga.com/de/bundesliga/info/impressum/\n\n\ncalfeed lösc
 hen? Anleitung: bit.ly/calfeed_loeschen
DTSTAMP:20210726T170803Z
CREATED:20210707T145103Z
LAST-MODIFIED:20210726T170803Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - FC Bayern München | Supercup
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b487e4ef0@2704.calovo
DTSTART;TZID=Europe/Berlin:20210821T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210821T173000
LOCATION:Schwarzwald-Stadion
SUMMARY:Sport-Club Freiburg - Borussia Dortmund | Bundesliga | 2. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n2. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20210726T170550Z
CREATED:20210625T104839Z
LAST-MODIFIED:20210726T170550Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Sport-Club Freiburg - Borussia Dortmund | Bundesliga 
 | 2. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b48a7079b@2704.calovo
DTSTART;TZID=Europe/Berlin:20210827T203000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210827T223000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - TSG Hoffenheim | Bundesliga | 3. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n3. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20210726T170755Z
CREATED:20210625T104842Z
LAST-MODIFIED:20210726T170755Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - TSG Hoffenheim | Bundesliga | 3. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b48ebdb0b@2704.calovo
DTSTART;TZID=Europe/Berlin:20210911T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210911T173000
LOCATION:BayArena
SUMMARY:Bayer 04 Leverkusen - Borussia Dortmund | Bundesliga | 4. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n4. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20210726T170559Z
CREATED:20210625T104846Z
LAST-MODIFIED:20210726T170559Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Bayer 04 Leverkusen - Borussia Dortmund | Bundesliga 
 | 4. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b491cf948@2704.calovo
DTSTART;TZID=Europe/Berlin:20210919T173000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210919T193000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - 1. FC Union Berlin | Bundesliga | 5. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n5. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20210726T170757Z
CREATED:20210625T104849Z
LAST-MODIFIED:20210726T170757Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - 1. FC Union Berlin | Bundesliga |
  5. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b49848a8b@2704.calovo
DTSTART;TZID=Europe/Berlin:20210925T183000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20210925T203000
LOCATION:BORUSSIA-PARK
SUMMARY:Borussia Mönchengladbach - Borussia Dortmund | Bundesliga | 6. Spi
 eltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n6. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/
 bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfee
 d_loeschen
DTSTAMP:20210726T170759Z
CREATED:20210625T104856Z
LAST-MODIFIED:20210726T170759Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Mönchengladbach - Borussia Dortmund | Bunde
 sliga | 6. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b49b7e9b8@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211001
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211004
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - FC Augsburg | Bundesliga | 7. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n7. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht fe
 stgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bunde
 sliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_loe
 schen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170609Z
CREATED:20210625T104859Z
LAST-MODIFIED:20210726T170609Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - FC Augsburg | Bundesliga | 7. S
 pieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b49f4b4c6@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211015
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211018
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - 1. FSV Mainz 05 | Bundesliga | 8. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n8. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht fe
 stgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bunde
 sliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_loe
 schen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170612Z
CREATED:20210625T104903Z
LAST-MODIFIED:20210726T170612Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - 1. FSV Mainz 05 | Bundesliga | 
 8. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4a7ab8c0@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211022
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211025
LOCATION:SchücoArena
SUMMARY:* DSC Arminia Bielefeld - Borussia Dortmund | Bundesliga | 9. Spiel
 tag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n9. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht fe
 stgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bunde
 sliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_loe
 schen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170617Z
CREATED:20210625T104911Z
LAST-MODIFIED:20210726T170617Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * DSC Arminia Bielefeld - Borussia Dortmund | Bundesl
 iga | 9. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4a953e36@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211029
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211101
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - 1. FC Köln | Bundesliga | 10. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n10. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170620Z
CREATED:20210625T104913Z
LAST-MODIFIED:20210726T170620Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - 1. FC Köln | Bundesliga | 10. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4af5bb3f@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211105
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211108
LOCATION:Red Bull Arena
SUMMARY:* RB Leipzig - Borussia Dortmund | Bundesliga | 11. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n11. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170624Z
CREATED:20210625T104919Z
LAST-MODIFIED:20210726T170624Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * RB Leipzig - Borussia Dortmund | Bundesliga | 11. S
 pieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4b46af95@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211119
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211122
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - VfB Stuttgart | Bundesliga | 12. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n12. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170627Z
CREATED:20210625T104924Z
LAST-MODIFIED:20210726T170627Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - VfB Stuttgart | Bundesliga | 12
 . Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4bc3c0f1@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211126
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211129
LOCATION:Volkswagen Arena
SUMMARY:* VfL Wolfsburg - Borussia Dortmund | Bundesliga | 13. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n13. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170631Z
CREATED:20210625T104932Z
LAST-MODIFIED:20210726T170631Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * VfL Wolfsburg - Borussia Dortmund | Bundesliga | 13
 . Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4bf36ee4@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211203
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211206
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - FC Bayern München | Bundesliga | 14. Spielta
 g
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n14. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170634Z
CREATED:20210625T104935Z
LAST-MODIFIED:20210726T170634Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - FC Bayern München | Bundesliga
  | 14. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4c82fa8b@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211210
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211213
LOCATION:Vonovia Ruhrstadion
SUMMARY:* VfL Bochum 1848 - Borussia Dortmund | Bundesliga | 15. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n15. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170638Z
CREATED:20210625T104944Z
LAST-MODIFIED:20210726T170638Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * VfL Bochum 1848 - Borussia Dortmund | Bundesliga | 
 15. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4c9446e3@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211214
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211216
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - SpVgg Greuther Fürth | Bundesliga | 16. Spie
 ltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n16. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170641Z
CREATED:20210625T104945Z
LAST-MODIFIED:20210726T170641Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - SpVgg Greuther Fürth | Bundesl
 iga | 16. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4d138958@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20211217
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20211220
LOCATION:Olympiastadion
SUMMARY:* Hertha BSC - Borussia Dortmund | Bundesliga | 17. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n17. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170643Z
CREATED:20210625T104953Z
LAST-MODIFIED:20210726T170643Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Hertha BSC - Borussia Dortmund | Bundesliga | 17. S
 pieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4d4cabd1@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220107
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220110
LOCATION:Deutsche Bank Park
SUMMARY:* Eintracht Frankfurt - Borussia Dortmund | Bundesliga | 18. Spielt
 ag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n18. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170646Z
CREATED:20210625T104956Z
LAST-MODIFIED:20210726T170646Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Eintracht Frankfurt - Borussia Dortmund | Bundeslig
 a | 18. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4da13581@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220114
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220117
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - Sport-Club Freiburg | Bundesliga | 19. Spielt
 ag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n19. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170647Z
CREATED:20210625T105002Z
LAST-MODIFIED:20210726T170647Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - Sport-Club Freiburg | Bundeslig
 a | 19. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4e2b0c75@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220121
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220124
LOCATION:PreZero Arena
SUMMARY:* TSG Hoffenheim - Borussia Dortmund | Bundesliga | 20. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n20. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170650Z
CREATED:20210625T105010Z
LAST-MODIFIED:20210726T170650Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * TSG Hoffenheim - Borussia Dortmund | Bundesliga | 2
 0. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4e584ef6@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220204
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220207
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - Bayer 04 Leverkusen | Bundesliga | 21. Spielt
 ag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n21. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170654Z
CREATED:20210625T105013Z
LAST-MODIFIED:20210726T170654Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - Bayer 04 Leverkusen | Bundeslig
 a | 21. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4eb751ef@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220211
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220214
LOCATION:An der Alten Försterei
SUMMARY:* 1. FC Union Berlin - Borussia Dortmund | Bundesliga | 22. Spielta
 g
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n22. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170658Z
CREATED:20210625T105019Z
LAST-MODIFIED:20210726T170658Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * 1. FC Union Berlin - Borussia Dortmund | Bundesliga
  | 22. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4ef79e1d@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220218
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220221
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - Borussia Mönchengladbach | Bundesliga | 23. 
 Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n23. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170701Z
CREATED:20210625T105023Z
LAST-MODIFIED:20210726T170701Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - Borussia Mönchengladbach | Bun
 desliga | 23. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4f771345@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220225
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220228
LOCATION:WWK ARENA
SUMMARY:* FC Augsburg - Borussia Dortmund | Bundesliga | 24. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n24. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170705Z
CREATED:20210625T105031Z
LAST-MODIFIED:20210726T170705Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * FC Augsburg - Borussia Dortmund | Bundesliga | 24. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4faf363c@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220304
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220307
LOCATION:MEWA ARENA
SUMMARY:* 1. FSV Mainz 05 - Borussia Dortmund | Bundesliga | 25. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n25. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170708Z
CREATED:20210625T105035Z
LAST-MODIFIED:20210726T170708Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * 1. FSV Mainz 05 - Borussia Dortmund | Bundesliga | 
 25. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b4fcc2be9@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220311
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220314
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - DSC Arminia Bielefeld | Bundesliga | 26. Spie
 ltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n26. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170711Z
CREATED:20210625T105036Z
LAST-MODIFIED:20210726T170711Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - DSC Arminia Bielefeld | Bundesl
 iga | 26. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b50409440@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220318
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220321
LOCATION:RheinEnergieSTADION
SUMMARY:* 1. FC Köln - Borussia Dortmund | Bundesliga | 27. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n27. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170715Z
CREATED:20210625T105044Z
LAST-MODIFIED:20210726T170715Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * 1. FC Köln - Borussia Dortmund | Bundesliga | 27. 
 Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b5056369d@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220401
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220404
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - RB Leipzig | Bundesliga | 28. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n28. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170717Z
CREATED:20210625T105045Z
LAST-MODIFIED:20210726T170717Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - RB Leipzig | Bundesliga | 28. S
 pieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b50c6dbaa@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220408
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220411
LOCATION:Mercedes-Benz Arena
SUMMARY:* VfB Stuttgart - Borussia Dortmund | Bundesliga | 29. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n29. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170721Z
CREATED:20210625T105052Z
LAST-MODIFIED:20210726T170721Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * VfB Stuttgart - Borussia Dortmund | Bundesliga | 29
 . Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b50e6e3f3@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220416
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220418
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - VfL Wolfsburg | Bundesliga | 30. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n30. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170724Z
CREATED:20210625T105054Z
LAST-MODIFIED:20210726T170724Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - VfL Wolfsburg | Bundesliga | 30
 . Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b512803d9@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220422
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220425
LOCATION:Allianz Arena
SUMMARY:* FC Bayern München - Borussia Dortmund | Bundesliga | 31. Spielta
 g
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n31. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170727Z
CREATED:20210625T105058Z
LAST-MODIFIED:20210726T170727Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * FC Bayern München - Borussia Dortmund | Bundesliga
  | 31. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b516eb755@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220429
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220501
LOCATION:SIGNAL IDUNA PARK
SUMMARY:* Borussia Dortmund - VfL Bochum 1848 | Bundesliga | 32. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n32. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170730Z
CREATED:20210625T105102Z
LAST-MODIFIED:20210726T170730Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * Borussia Dortmund - VfL Bochum 1848 | Bundesliga | 
 32. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b520e36b8@2704.calovo
DTSTART;TZID=Europe/Berlin;VALUE=DATE:20220506
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin;VALUE=DATE:20220509
LOCATION:Sportpark Ronhof | Thomas Sommer
SUMMARY:* SpVgg Greuther Fürth - Borussia Dortmund | Bundesliga | 33. Spie
 ltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n33. Spieltag\n\nAchtung: Der endgültige Spieltermin wurde noch nicht f
 estgelegt.\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de/bund
 esliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfeed_lo
 eschen
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
DTSTAMP:20210726T170735Z
CREATED:20210625T105112Z
LAST-MODIFIED:20210726T170735Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: * SpVgg Greuther Fürth - Borussia Dortmund | Bundesl
 iga | 33. Spieltag
END:VALARM
END:VEVENT
BEGIN:VEVENT
UID:60d5b521291b1@2704.calovo
DTSTART;TZID=Europe/Berlin:20220514T153000
SEQUENCE:0
TRANSP:TRANSPARENT
STATUS:CONFIRMED
DTEND;TZID=Europe/Berlin:20220514T173000
LOCATION:SIGNAL IDUNA PARK
SUMMARY:Borussia Dortmund - Hertha BSC | Bundesliga | 34. Spieltag
DESCRIPTION:Dieser Kalenderservice wird dir präsentiert von bundesliga.de\
 n\n34. Spieltag\n\n\n\n\nAnbieter-Impressum: https://www.bundesliga.com/de
 /bundesliga/info/impressum/\n\n\ncalfeed löschen? Anleitung: bit.ly/calfe
 ed_loeschen
DTSTAMP:20210726T170737Z
CREATED:20210625T105113Z
LAST-MODIFIED:20210726T170737Z
BEGIN:VALARM
TRIGGER:-P0DT1H0M0S
ACTION:DISPLAY
DESCRIPTION:Reminder: Borussia Dortmund - Hertha BSC | Bundesliga | 34. Spi
 eltag
END:VALARM
END:VEVENT
BEGIN:VTIMEZONE
TZID:Europe/Berlin
X-LIC-LOCATION:Europe/Berlin
BEGIN:DAYLIGHT
TZNAME:CEST
TZOFFSETFROM:+0100
TZOFFSETTO:+0200
DTSTART:19810329T030000
RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTH=3;BYDAY=-1SU
END:DAYLIGHT
BEGIN:STANDARD
TZNAME:CET
TZOFFSETFROM:+0200
TZOFFSETTO:+0100
DTSTART:19961027T030000
RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTH=10;BYDAY=-1SU
END:STANDARD
END:VTIMEZONE
END:VCALENDAR");
        }

    }
}
