using System;

namespace Parser.Services
{
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
        public Event GetNextEvent()
        {
            Event g = new Event();
            return g;
        }
    }
}
