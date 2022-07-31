using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebAppNew.Models;
using System.Configuration;

namespace MVCWebAppNew.Controllers
{
    public class HomeController : Controller

    { 
        public ActionResult Index()
        {
            var model = new AppConfigurationViewModel
            {
                BackgroundColor = AppConfiguration.BackgroundColor,
                FontColor = AppConfiguration.FontColor,
                FontSize = AppConfiguration.FontSize,
                Message = AppConfiguration.Message,
                url = AppConfiguration.url,
                token = AppConfiguration.token
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}