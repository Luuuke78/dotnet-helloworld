using System;
using System.Collections.Generic;

namespace ParserLibs
{
    public class Event
    {
        public string datetime;
        public string summary;
        public string opponent;
        public bool homematch;

        public Event() {
            this.datetime = "";
            this.summary = "";
            this.opponent = "";
            this.homematch = false;
        }

        public string GetDateTimeFormatted() {
            string datetime = this.datetime;
            string datetimeFormatted;
            if (datetime.Length == 0) {
                datetimeFormatted = "";
            } 
            else {
                if (datetime.Length > 8) {
                    datetimeFormatted = datetime.Substring(6, 2) + "." + datetime.Substring(4, 2) + "." + datetime.Substring(0, 4) + " " +
                    datetime.Substring(9, 2) + ":" + datetime.Substring(11, 2);
                }
                else {
                    datetimeFormatted = datetime.Substring(6, 2) + "." + datetime.Substring(4, 2) + "." + datetime.Substring(0, 4);
                }
            }
            return datetimeFormatted;
        }
        
        public string GetSummaryFormatted() {
            return this.summary.Replace("\r\n ","").Replace("\r\n","").Replace("*","").Trim();
        }
        
        public string GetWeekday() {
            if (datetime.Length > 8) {
                DateTime dt = DateTime.ParseExact(this.datetime, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                return dt.ToString("ddd");
            } else {
                DateTime dt = DateTime.ParseExact(this.datetime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                return dt.ToString("ddd");
            }
            return "";
        }

    }
    public class CalovoParser
    {
        private string calendar;
        public CalovoParser(string calendar) {
            this.calendar = calendar;
        }
        public List<Event> GetAllNextEvents(string startDate, bool onlyHome) {
            List<Event> eventlist = new List<Event>();
            string datum_curr = startDate;
            while(Convert.ToBoolean(datum_curr.Length))
            {
                Event e = this.GetNextEvent(datum_curr.Substring(0, 8));
                datum_curr = e.datetime;
                if (onlyHome == false){
                    eventlist.Add(e);
                }
                else {
                    if (e.homematch == true) {
                        eventlist.Add(e);
                    }
                }
            }
            return eventlist;            
        }                

        public string GetNextEventString(string startDate) {
            return "";
        }

        public Event GetNextEvent(string startDate)
        {
            string cal_event, date, line, datetime, date_current, datetime_current, summary, summary_current;
            string opponent, opponent_current;
            bool homematch, homematch_current;
            Event g = new Event();

            date = null;
            datetime = "";
            summary_current = "";
            summary = "";
            opponent = "";
            opponent_current = "";
            homematch = false;
            homematch_current = false;

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
                            int firstteam_content_index = summary_current.IndexOf("-");
                            int secondteam_content_index = summary_current.IndexOf("|");
                            if (firstteam_content_index > -1 & secondteam_content_index > -1) {
                                string firstteam = summary_current.Replace("*","").Substring(0, firstteam_content_index - 1).Trim();
                                string secondteam = summary_current.Substring(firstteam_content_index + 1, secondteam_content_index - firstteam_content_index - 1).Trim();
                                if (firstteam == "Borussia Dortmund") {
                                    opponent_current = secondteam;
                                    homematch_current = true;
                                }
                                else {
                                    opponent_current = firstteam;
                                    homematch_current = false;
                                }
                            }
                        }

                        if (Convert.ToInt32(date_current) <= Convert.ToInt32(startDate)) {
                            datetime = "";
                            summary = "";
                            opponent = "";
                            homematch = false;
                        }
                        else {
                            if (Convert.ToInt32(date) != 0) {
                                if (Convert.ToInt32(date_current) <= Convert.ToInt32(date)) {                        
                                    datetime = datetime_current;
                                    summary = summary_current;
                                    opponent = opponent_current;
                                    homematch = homematch_current;
                                }
                            }
                            else {
                                datetime = datetime_current;
                                date = date_current;
                                summary = summary_current;
                                opponent = opponent_current;
                                homematch = homematch_current;
                            }                       
                        }
                        g.datetime = datetime;
                        g.summary = summary;
                        g.opponent = opponent;
                        g.homematch = homematch;
                    }                
                }
            }
            return g;
        }

        
    }

}
