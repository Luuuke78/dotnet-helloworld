using System;
using System.Collections.Generic;

namespace ParserLibs
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
        private string calendar;
        public CalovoParser(string calendar) {
            this.calendar = calendar;
        }
        public List<Event> GetAllNextEvents(string startDate) {
            List<Event> eventlist = new List<Event>();
            string datum_curr = startDate;
            while(Convert.ToBoolean(datum_curr.Length))
            {
                Event e = this.GetNextEvent(datum_curr.Substring(0, 8));
                datum_curr = e.datetime;
                eventlist.Add(e);
            }
            return eventlist;
        }
        
        public string GetNextEventString(string startDate) {
            return "";
        }

        public Event GetNextEvent(string startDate)
        {
            string cal_event, date, line, datetime, date_current, datetime_current, summary, summary_current;
            Event g = new Event();

            date = null;
            datetime = "";
            summary_current = "";
            summary = "";

            string[] calendar_events = this.calendar.Split("BEGIN:VEVENT");
            int calendar_events_length = calendar_events.Length;
            for (int i = 0; i < calendar_events_length; i++)
            {
                cal_event = calendar_events[i];
                
                if (cal_event.Contains("DTSTART"))
                {
                    int dtstart_index = cal_event.IndexOf("DTSTART");
                    if (dtstart_index > -1) {
                        int sequence_index = cal_event.IndexOf("SEQUENCE");
                        line = cal_event.Substring(dtstart_index, sequence_index - dtstart_index);
                        int dtstart_content_index = line.IndexOf(":") + 1;
                        if (line.Substring(dtstart_content_index).Trim().Length > 8) {
                            datetime_current = line.Substring(dtstart_content_index, 15);
                            date_current = line.Substring(dtstart_content_index, 8);
                        }                        
                        else {
                            datetime_current = line.Substring(dtstart_content_index, 8);
                            date_current = line.Substring(dtstart_content_index, 8);
                        }

                        int summary_index = cal_event.IndexOf("SUMMARY");
                        if (summary_index > -1) {
                            int description_index = cal_event.IndexOf("DESCRIPTION");
                            line = cal_event.Substring(summary_index, description_index - summary_index);
                            int summary_content_index = line.IndexOf(":") + 1;
                            summary_current = line.Substring(summary_content_index).Trim();
                        }

                        if (Convert.ToInt32(date_current) <= Convert.ToInt32(startDate)) {
                            datetime = "";
                            summary = "";
                        }
                        else {
                            if (Convert.ToInt32(date) != 0) {
                                if (Convert.ToInt32(date_current) <= Convert.ToInt32(date)) {                        
                                    datetime = datetime_current;
                                    summary = summary_current;
                                }
                            }
                            else {
                                datetime = datetime_current;
                                date = date_current;
                                summary = summary_current;
                            }                       
                        }
                        g.datetime = datetime;
                        g.summary = summary;
                    }                
                }
            }
            return g;
        }
    }

}
