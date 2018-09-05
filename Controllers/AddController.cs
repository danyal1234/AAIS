using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;
using System.Data.SqlClient;

namespace AssetApp.Controllers
{
    public class AddController : Controller
    {
        private AppAssetEntities db = new AppAssetEntities();
        // GET: Add
        public ActionResult Index()
        {
            ViewBag.appVendors = db.AppVendors.ToList();

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

        [HttpPost]
        public ActionResult Main(string vendorSelect,string vendorName, string street1,
            string city,string province,string zip,string vendURL, string startTime,string vendTel,
            string vendEmail, string supportComments)
        {
            ViewBag.vendorSelect = vendorSelect;
            ViewBag.vendorName = vendorName;
            ViewBag.street1 = street1;
            ViewBag.city = city;
            ViewBag.province = province;
            ViewBag.zip = zip;
            ViewBag.vendURL = vendURL;
            ViewBag.startTime = startTime;
            ViewBag.vendTel = vendTel;
            ViewBag.vendEmail = vendEmail;
            ViewBag.supportComments = supportComments;


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

        

        public ActionResult saveMain(string appName, string appDescription, string appType, string deployType,
            string liscenceType, string numbLiscence, string numbUsers, string appAccess, string vendorName, string itOwner,
            string systemAdmin, string BOG, string BOPMR, string hourstart, string afterHours,
            string maintWindow, string vendorSelect, string street1,string city, string province, string zip, string vendURL, string startTime, string vendTel,
            string vendEmail, string supportComments)
        {
            var user = User.Identity.Name.Replace("CITYACCT\\", "");
            
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            int check = 0;
            int ID = 0;

            string venderror = "no";
            string apperror = "no";

            foreach (var item in db.AppVendors.ToList())
            {
                if (item.VendName == vendorName)
                {
                    venderror = "yes";
                    check = 1;
                    break;
                }
            }

            foreach (var item in db.AppMains.ToList())
            {
                if (item.AppName == appName)
                {
                    apperror = "yes";
                    check = 1;
                    break;
                }
            }

            if(check == 1)
            {
                return Redirect("~/Home/addError/" + venderror + "/" + apperror);
            }

            if (vendorSelect.Length != 0)
            {
                ViewBag.vendor = vendorSelect;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO AppVendor (VendName, VendAddr, VendURL) VALUES ('" + vendorName + "','" + street1 + " " + city + " " + province + " " + zip + "','" + vendURL + "')";

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                foreach (var item in db.AppVendors.ToList())
                {
                    if (item.VendName == vendorName && item.VendURL == vendURL)
                    {
                        ID = item.VenID;
                        break;
                    }
                }


                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO AppVendorSupport (VenSupportHours, VenSupportNum, VenSupportEmail, VenSupportComments, VenIDFK) VALUES ('" + startTime + "','" + vendTel + "','" + vendEmail + "','" + supportComments + "','" + ID + "')";

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
                
            }
            if(vendorSelect.Length != 0)
            {
                foreach (var item in db.AppVendors.ToList())
                {
                    if (item.VendName == vendorSelect)
                    {
                        ID = item.VenID;
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in db.AppVendors.ToList())
                {
                    if (item.VendName == vendorName && item.VendURL == vendURL)
                    {
                        ID = item.VenID;
                        break;
                    }
                }
            }
            

            if (appAccess == "Yes")
            {
                appAccess = "Y";
            }
            else
            {
                appAccess = "N";
            }

            if (afterHours == "Yes")
            {
                afterHours = "Y";
            }
            else
            {
                afterHours = "N";
            }

            string commandText = "INSERT INTO AppMain (AppName, AppDesc, AppType, DeployType, LiscType, LiscNum, UserNum, AppAccess, VenIDFK, CreatedBy, CreatedDate, Expired) VALUES ('" +
                appName + "', '" + appDescription + "', '" + appType + "', '" + deployType + "', '" + liscenceType + "', '" + numbLiscence + "', '" + numbUsers + "', '"
                + appAccess + "', '" + ID + "', '" + user + "', '" + DateTime.Now +  "','N');";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            int appid = 0;

            foreach (var item in db.AppMains.ToList())
            {
                if (item.AppName == appName)
                {
                    appid = item.AppID;
                    break;
                }
            }

            commandText = "INSERT INTO AppSupportInfo (AppSupportITOwner, AppSupportAdmin, AppSupportBOG, AppSupportPMR, AppHours, AppMaintenance, AppIDFK, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES ('" +
                            itOwner + "', '" + systemAdmin + "', '" + BOG + "', '" + BOPMR + "', '" + hourstart + "', '" + maintWindow + "', '" + appid + "', '" + user + "', '"
                            + DateTime.Now + "', '','" + DateTime.Now + "');";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index", "Info", new { id = appName });
        }

   }
}
