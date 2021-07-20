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

        [TestMethod]
        public void Test_GetNextEvent_calWithOneEntry_returnsReferenceEvent()
        {
            string calendar = @"----DTSTART;TZID=Europe/Berlin:20210721T183000
SEQUENCE:0
DTEND;TZID=Europe/Berlin:20200606T203000
SUMMARY:Borussia Dortmund - Hertha BSC | Bundesliga | 30. Spieltag
DESCRIPTION:----";
            calendar = calendar.Replace(":", "#");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210720");
            Assert.AreEqual("Borussia Dortmund - Hertha BSC", e.summary);
            Assert.AreEqual("20210721T183000", e.datetime);
        }

        [TestMethod]
        public void Test_GetNextEvent_calWithOlderEntry_returnsEmptyEvent()
        {
            string calendar = @"----DTSTART;TZID=Europe/Berlin:20210721T183000
SEQUENCE:0
DTEND;TZID=Europe/Berlin:20200606T203000
SUMMARY:Borussia Dortmund - Hertha BSC | Bundesliga | 30. Spieltag
DESCRIPTION:----";
            calendar = calendar.Replace(":", "#");
            CalovoParser p = new CalovoParser(calendar);
            Event e = p.GetNextEvent("20210812");
            
            // TODO lbra fix this test by implementing GetNextEvent
            Assert.AreEqual("", e.summary);
            Assert.AreEqual("", e.datetime);
        }


    }
}
