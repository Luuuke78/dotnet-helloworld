using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CalovoParserTestMS
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_GetNextEvent_emptyCal_returnsEmptySummaryAndDate()
        {
            CalovoParser p = new CalovoParser("");
            Assert.AreEqual("", p.GetNextEvent("").summary);
        }

        private string createStringForOneEvent(string datetime, string summary) {
            return string.Format(@"----BEGIN:VEVENT
DTSTART;TZID=Europe/Berlin:{0}T183000
SEQUENCE:0
DTEND;TZID=Europe/Berlin:20200606T203000
SUMMARY:{1}
DESCRIPTION:----
END:VEVENT", datetime, summary);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithOneEntry_returnsReferenceEvent()
        {
            string summary = "Borussia Dortmund - Hertha BSC | Bundesliga | 30. Spieltag";
            string datetime = "20210721T183000";
            string calendar = this.createStringForOneEvent(datetime, summary);
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210720");
            //Assert.AreEqual("Borussia Dortmund - Hertha BSC", e.summary);
            Assert.AreEqual(datetime, e.datetime);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithOlderEntry_returnsEmptyEvent()
        {
            string calendar = this.createStringForOneEvent("20210721T183000", "BVB | Hertha | 30.");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210812");
            
            // TODO lbra fix this test by implementing GetNextEvent
            //Assert.AreEqual("", e.summary);
            Assert.AreEqual("", e.datetime);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithTwoEntriesAskForFirstOne_returnsFirstEvent()
        {
            string firstDateTime = "20210721T183000";
            string calendar = this.createStringForOneEvent(firstDateTime, "BVB | Hertha | 30.") +
                this.createStringForOneEvent("20210728T183000", "BVB | Hertha | 31.");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210601");
            Assert.AreEqual(firstDateTime, e.datetime);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithTwoEntriesAskForSecondOne_returnsSecondEvent()
        {
            string secondDateTime = "20210728T183000";
            string calendar = this.createStringForOneEvent("20210721T183000", "BVB | Hertha | 30.") +
                this.createStringForOneEvent(secondDateTime, "BVB | Hertha | 31.");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210723");
            Assert.AreEqual(secondDateTime, e.datetime);
        }


    }
}
