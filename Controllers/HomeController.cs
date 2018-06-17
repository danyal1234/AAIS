using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;

namespace AssetApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private AppAssetEntities db = new AppAssetEntities();
        public ActionResult Index()
        {
            ViewBag.appTables = db.AppMains.ToList();

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            var user = ADUser.finduser(userPrincipalName);

            ViewBag.displayName = user.displayName;
            ViewBag.company = user.company;
            ViewBag.department = user.department;
            ViewBag.mail = user.mail;
            ViewBag.telephoneNumber = user.telephoneNumber;
            return View();
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