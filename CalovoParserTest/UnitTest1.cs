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
            return System.IO.File.ReadAllText(@"..\..\..\..\assets\buli.ics");
        }

        [TestMethod]
        public void Test_GetNextEvent_WithDateFormatted_currentCalendar_returnsNextEntry()
        {
            CalovoParser p = new CalovoParser(this.GetCurrentCalendar());
            Event e = p.GetNextEvent("20210913");
            Assert.AreEqual("19.09.2021 17:30", e.datetimeFormatted);
            Assert.AreEqual("Borussia Dortmund - 1. FC Union Berlin | Bundesliga | 5. Spieltag", e.summary);
        }

        [TestMethod]
        public void Test_GetNextEventNotPlanned_WithDateFormatted_currentCalendar_returnsNextEntry()
        {
            CalovoParser p = new CalovoParser(this.GetCurrentCalendar());
            Event e = p.GetNextEvent("20211128");
            Assert.AreEqual("03.12.2021", e.datetimeFormatted);
            Assert.AreEqual("Borussia Dortmund - FC Bayern München | Bundesliga | 14. Spieltag", e.summary);
        }
    }
}
