using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public static class Logger
    {
        public static void LogToScreen(string text)
        {
            Console.WriteLine($"{DateTime.Now.ToString()} - {text}");
        }
    }
}
