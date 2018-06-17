using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;

namespace AssetApp.Controllers
{
    public class InfoController : Controller
    {
        private AppAssetEntities db = new AppAssetEntities();
        // GET: About
        [HttpGet]
        public ActionResult Index(string id)
        {
            var main = db.AppMains.ToList();

            AppMain app;

            foreach (var item in main)
            {
                if (item.AppName == id)
                {
                    app = item;
                    ViewBag.app = app;
                    ViewBag.versions = app.ApplicationVersions;
                    ViewBag.ADGroups = app.AppADGroups;
                    ViewBag.ascReports = app.AppAscRprts;
                    ViewBag.ascSoftware = app.AppAscSoftwares;
                    ViewBag.ascHardware = app.AppAscHardwares;
                    ViewBag.appSupport = app.AppSupportInfoes;
                    ViewBag.appdatabase = app.AppServerDBs;
                    ViewBag.vendor = app.AppVendor;
                    ViewBag.appcontacts = app.AppVendor.AppVendorContacts;
                    ViewBag.appcontracts = app.AppVendor.AppContracts;
                    ViewBag.appVendorSupport = app.AppVendor.AppVendorSupports;
                    break;
                }
            }
            
            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            var user = ADUser.finduser(userPrincipalName);

            ViewBag.displayName = user.displayName;
            ViewBag.company = user.company;
            ViewBag.department = user.department;
            ViewBag.mail = user.mail;
            ViewBag.telephoneNumber = user.telephoneNumber;

            ViewBag.appname =  id;

            return View();
        }

        public ActionResult Version(string id, string appname)
        {

            var versions= db.ApplicationVersions.ToList();

            foreach (var item in versions)
            {
                if (item.AppVersion == id && item.AppMain.AppName == appname)
                {
                    ViewBag.version = item.AppVersion;
                    ViewBag.IE = item.AppIE;
                    ViewBag.InstallDate = item.AppInstallDate;
                    ViewBag.UpgradeDate = item.AppUpgradeDate;
                    ViewBag.ServerName = item.AppServerDB.AppServerName;
                    ViewBag.ServerType = item.AppServerDB.AppServerType;
                    ViewBag.ServerOS = item.AppServerDB.AppServerOS;
                    ViewBag.ServerURL = item.AppServerDB.AppServerURL;
                    ViewBag.ServerIE = item.AppServerDB.AppServerIE;
                    ViewBag.DBName = item.AppServerDB.AppDBName;
                    ViewBag.DBType = item.AppServerDB.AppDBType;
                    break;
                }
            }


            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            var user = ADUser.finduser(userPrincipalName);

            ViewBag.displayName = user.displayName;
            ViewBag.company = user.company;
            ViewBag.department = user.department;
            ViewBag.mail = user.mail;
            ViewBag.telephoneNumber = user.telephoneNumber;

            ViewBag.appname = appname;

            return View();
        }
    }
}