using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MVCWebAppNew.Models
{
    public static class AppConfiguration
    {
        public static int Count { get; set; }
        public static string url { get; set; }
        public static string token { get; set; }
        public static string BackgroundColor { get; set; }
        public static string FontColor { get; set; }
        public static int FontSize { get; set; }
        public static string Message { get; set; }
    }

    public class AppConfigurationViewModel
    {
        public int Count { get; set; }
        public string url { get; set; }
        public string token { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public int FontSize { get; set; }
        public string Message { get; set; }
    }
}