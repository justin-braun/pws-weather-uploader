using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader.Helpers
{
    internal class EventLogger
    {
        EventLog svcEventLog;
        string _logName = "";

        public EventLogger(string logName)
        {
            svcEventLog = new EventLog(logName);
            CreateLog(logName);
            _logName = logName;
        }
        public void CreateLog(string logName)
        {
            try
            {
                if (!EventLog.SourceExists(logName))
                {
                    EventLog.CreateEventSource(logName, logName);
                }

                svcEventLog.Source = logName;
                svcEventLog.Log = logName;
                this.WriteEvent(EventLogEntryType.Information, 0, $"The {logName} was successfully created.");

            }
            catch (Exception ex)
            {
                this.WriteEvent(EventLogEntryType.Error, -1, $"Exception: {ex.Message}");
            }
        }

        public void WriteEvent(EventLogEntryType eventType, int eventId, string text)
        {
            try
            {
                svcEventLog.WriteEntry(text, eventType, eventId);
            }
            catch (Exception ex)
            {
                svcEventLog.WriteEntry($"{text}{Environment.NewLine}{Environment.NewLine}Exception: {ex.Message}", EventLogEntryType.Error);
                throw;
            }
        }

        public void Dispose()
        {
            svcEventLog.Dispose();
            svcEventLog = null;
        }
    }
}
