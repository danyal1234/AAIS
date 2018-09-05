using AssetApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetApp.Controllers
{
    public class ExpiredController : Controller
    {
        private AppAssetEntities db = new AppAssetEntities();
        // GET: Expired
        public ActionResult Index()
        {
            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            var user = ADUser.finduser(userPrincipalName);

            ViewBag.displayName = user.displayName;
            ViewBag.company = user.company;
            ViewBag.department = user.department;
            ViewBag.mail = user.mail;
            ViewBag.telephoneNumber = user.telephoneNumber;

            ViewBag.appTables = db.AppMains.ToList();

            return View();
        }
    }
}