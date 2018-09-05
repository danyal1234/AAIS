using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;

namespace AssetApp.Controllers
{
    public class EditController : Controller
    {
        private AppAssetEntities db = new AppAssetEntities();
        public ActionResult Index(string id)
        {
            var main = db.AppMains.ToList();

            foreach (var item in main)
            {
                if (item.AppName == id)
                {
                    ViewBag.app = item;
                    ViewBag.appSupport = item.AppSupportInfoes;
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

            ViewBag.appname = id;

            return View();
        }
        
        public ActionResult ContractEdit(string contractSelect, string convendor)
        {
            var contracts = db.AppContracts.ToList();

            ViewBag.appname = convendor;

            if (contractSelect == "Add New")
            {
                ViewBag.contractid = "add";
            }
            else
            {
                foreach (var item in contracts)
                {
                    if (item.VenContractNum == contractSelect && item.AppMain.AppName == convendor)
                    {
                        ViewBag.name = contractSelect;
                        ViewBag.contractid = item.VenContractID;
                        ViewBag.appname = item.AppMain.AppName;
                        ViewBag.admin = item.VenContractAdmin;
                        ViewBag.startdate = item.VenContractStart;
                        ViewBag.enddate = item.VenContractEnd;
                        ViewBag.renewaldate = item.VenContractRenew;
                        ViewBag.description = item.VenContractComment;
                        break;
                    }
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

            return View();
        }
        
        public ActionResult ContactEdit(string contactSelect, string appname, string venid)
        {
            ViewBag.appname = appname;
            ViewBag.venid = venid;

            var contacts = db.AppVendorContacts.ToList();

            string[] words = contactSelect.Split(' ');

            foreach (var item in contacts)
            {
                if (item.VenContactFN == words[0] && item.VenContactLN == words[1])
                {
                    ViewBag.contactID = item.VenContactID;
                    ViewBag.fname = words[0];
                    ViewBag.lname = words[1];
                    ViewBag.contitle = item.VenContactTitle;
                    ViewBag.email = item.VenContactEmail;
                    ViewBag.num = item.VenContactNum;
                    ViewBag.status = item.VenContactStatus;
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

            return View();
        }
        
        public ActionResult VersionEdit(string versionSelect, string versionApp)
        {
            var versions = db.ApplicationVersions.ToList();

            ViewBag.appname = versionApp;

            foreach (var item in versions)
            {
                if (item.AppVersion + " " + item.AppIE == versionSelect && item.AppMain.AppName == versionApp)
                {
                    ViewBag.version = item.AppVersion;
                    ViewBag.versionID = item.VersionID;
                    ViewBag.AppIE = item.AppIE;
                    ViewBag.installDate = item.AppInstallDate;
                    ViewBag.upgradeDate = item.AppUpgradeDate;
                    ViewBag.VersionStatus = item.VersionStatus;
                    ViewBag.ServerName = item.AppServerDBs;
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

            return View();
        }

        public ActionResult VendorEdit(string id, string appName)
        {
            var vendors = db.AppVendors.ToList();

            foreach (var item in vendors)
            {
                if (item.VendName == id)
                {
                    ViewBag.name = id;
                    ViewBag.venid = item.VenID;
                    ViewBag.description = item.VendName;
                    ViewBag.url = item.VendURL;
                    ViewBag.address = item.VendAddr;
                    ViewBag.support = item.AppVendorSupports;
                    ViewBag.supportcount = item.AppVendorSupports.Count;
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

            ViewBag.appname = appName;

            return View();
        }
        
        public ActionResult Softwares(string softwareSelect, string softwareApp)
        {
            var softwares = db.AppAscSoftwares.ToList();

            ViewBag.appname = softwareApp;

            foreach (var item in softwares)
            {
                if (item.AppAscSoftwareName == softwareSelect && item.AppMain.AppName == softwareApp)
                {
                    ViewBag.id = item.AppAscSoftID;
                    ViewBag.name = item.AppAscSoftwareName;
                    ViewBag.version = item.AppAscSoftwareVersion;
                    ViewBag.proddate = item.ProductionDate;
                    ViewBag.expiredate = item.ExpiryDate;
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

            return View();
        }

        public ActionResult Hardwares(string hardwareName, string hardwareApp)
        {
            var hardwares = db.AppAscHardwares.ToList();

            ViewBag.appname = hardwareApp;

            foreach (var item in hardwares)
            {
                if (item.HardwareName == hardwareName && item.AppMain.AppName == hardwareApp)
                {
                    ViewBag.id = item.AppADID;
                    ViewBag.name = hardwareName;
                    ViewBag.type = item.HardwareType;
                    ViewBag.comments = item.Comments;
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

            return View();
        }

        public ActionResult editServer(string id, string appname)
        {
            var servers = db.AppServerDBs.ToList();

            ViewBag.versionid = appname;
            

            if (id != "add")
            {
                foreach (var item in servers)
                {
                    if (item.AppServerID == Int32.Parse(id))
                    {
                        ViewBag.id = id;
                        ViewBag.AppServerName = item.AppServerName;
                        ViewBag.AppServerType = item.AppServerType;
                        ViewBag.AppServerOS = item.AppServerOS;
                        ViewBag.AppServerURL = item.AppServerURL;
                        ViewBag.AppDBName = item.AppDBName;
                        ViewBag.AppDBType = item.AppDBType;
                        ViewBag.AppServerIE = item.AppServerIE;
                        break;
                    }
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

            return View();
        }

    }
}