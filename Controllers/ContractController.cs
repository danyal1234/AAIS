using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;

namespace AssetApp.Controllers
{
    public class ContractController : Controller
    {
        private AppAssetEntities db = new AppAssetEntities();
        // GET: Contract
        public ActionResult Index(string id, string appname)
        {
            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            var user = ADUser.finduser(userPrincipalName);

            ViewBag.displayName = user.displayName;
            ViewBag.company = user.company;
            ViewBag.department = user.department;
            ViewBag.mail = user.mail;
            ViewBag.telephoneNumber = user.telephoneNumber;

            ViewBag.appname = appname;

            var main = db.AppMains.ToList();

            AppMain app = new AppMain();

            foreach (var item in main)
            {
                if (item.AppName == appname)
                {
                    app = item;
                    break;
                }
            }

            var contracts = db.AppContracts.ToList();
            
            AppContract contract;

            foreach (var item in contracts)
            {
                if (item.VenContractNum == id  && app.VenName == item.AppVendor.VendName )
                {
                    contract = item;
                    ViewBag.VenContractNum = id;
                    ViewBag.VenContractAdmin = contract.VenContractAdmin;
                    ViewBag.VenContractStart = contract.VenContractStart;
                    ViewBag.VenContractEnd = contract.VenContractEnd;
                    ViewBag.VenContractRenew = contract.VenContractRenew;
                    ViewBag.VenContractComment = contract.VenContractComment;
                    break;
                }
            }
            
            
            return View();
        }
    }
}