using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AssetApp.Models;
using System.Net;
using HtmlAgilityPack;
using System.Data.SqlClient;

namespace AssetApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        private AppAssetEntities db = new AppAssetEntities();
        public ActionResult Index()
        {
            ViewBag.appTables = db.AppMains.ToList();

            List<AppContract> expiringContracts = new List<AppContract>();

            foreach (var item in db.AppContracts.ToList())
            {
                if (item.VenContractEnd == null)
                {
                    continue;
                }
                DateTime date = item.VenContractEnd.Value;
                DateTime trialPeriod = DateTime.Now.AddMonths(12);
                if (trialPeriod > date)
                {
                    expiringContracts.Add(item);
                }
            }
            ViewBag.totalContracts = expiringContracts.Count;
            expiringContracts.Reverse();
            ViewBag.expiringContracts = expiringContracts;
            if(db.AppContracts.ToList().Count == 0)
            {
                ViewBag.cnpercent = 0;
            }
            else
            {
                int cnpercent = (int)Math.Round((double)(100 * expiringContracts.Count()) / (db.AppContracts.ToList().Count));
                ViewBag.cnpercent = cnpercent;
            }
            
            
            DateTime today = DateTime.Now;

            WebClient webClient = new WebClient();
            webClient.UseDefaultCredentials = true;
            string page = webClient.DownloadString("http://teamsites.mississauga.ca/sites/103/Lists/Work%20Request%20List/All%20Requests.aspx#InplviewHash9f256e31-665a-4323-859d-5e8e7883e827=CascDelWarnMessage%3D1-FirstRow%3D301-SortField%3D-SortDir%3D-FilterField1%3DIT%255Fx0020%255FSection-FilterValue1%3DCPS%252F%2520CPS%252FCMO%2520Services");

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            List<List<HtmlNode>> table = doc.DocumentNode.SelectSingleNode("//table")
                        .Descendants("tr")
                        .Skip(1)
                        .Where(tr => tr.Elements("td").Count() > 1)
                        .Select(tr => tr.Elements("td").ToList())
                        .ToList();

            List<List<HtmlNode>> wRequests = new List<List<HtmlNode>>();

            foreach (var item in table)
            {
                if (item.Count > 2 && item[21].InnerText.Trim() != "")
                {
                    wRequests.Add(item);
                }
            }

            wRequests.Reverse();

            ViewBag.workRequest = wRequests;
            ViewBag.wrcount = wRequests.Count;
            if (table.Count - 2 <= 0)
            {
                ViewBag.wrpercent = 0;
            }
            else
            {
                int wrpercent = (int)Math.Round((double)(100 * wRequests.Count) / (table.Count - 2));
                ViewBag.wrpercent = wrpercent;
            }
            

            List<AppMain> newlyAdded = new List<AppMain>();
            foreach (var item in db.AppMains.ToList())
            {
                DateTime date = item.CreatedDate.Value;
                DateTime trialPeriod = date.AddMonths(1);
                if (trialPeriod > DateTime.Now)
                {
                    newlyAdded.Add(item);
                }
            }

            newlyAdded.Reverse();

            ViewBag.newlyadded = newlyAdded;
            ViewBag.addedcount = newlyAdded.Count;

            int addedpercent = (int)Math.Round((double)(100 * newlyAdded.Count) / db.AppMains.ToList().Count);
            ViewBag.addedpercent = addedpercent;

            List<AppMain> newlymodified = new List<AppMain>();
            foreach (var item in db.AppMains.ToList())
            {
                if (item.ModifiedDate == null)
                {
                    continue;
                }
                DateTime installationDate = item.ModifiedDate.Value;
                DateTime trialPeriodEnd = installationDate.AddDays(7);
                if (trialPeriodEnd > DateTime.Now)
                {
                    newlymodified.Add(item);
                }
            }

            newlymodified.Reverse();

            ViewBag.newlymodified = newlymodified;
            ViewBag.modifiedcount = newlymodified.Count;

            int modifiedpercent = (int)Math.Round((double)(100 * newlymodified.Count) / db.AppMains.ToList().Count);
            ViewBag.modifiedpercent = modifiedpercent;

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

        public ActionResult addError(string id, string appname)
        {
            string venderror = id;
            string apperror = appname;

            ViewBag.venderror = venderror;
            ViewBag.apperror = apperror;

            return View();
        }

        public ActionResult Expire(string appid)
        {
            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppMain SET Expired = 'Y', ModifiedBy = @mb, ModifiedDate = @md WHERE AppID = '" + appid + "'";
                
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Unexpire(string appid)
        {
            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppMain SET Expired = 'N', ModifiedBy = @mb, ModifiedDate = @md WHERE AppID = '" + appid + "'";

                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string appid, string venid, string vendordelete)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppAscSoftware WHERE AppIDFK = '"+ appid +"'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppAscHardware WHERE AppIDFK = '" + appid + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppContract WHERE AppIDFK = '" + appid + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            foreach(var item in db.ApplicationVersions.ToList())
            {
                if(item.AppIDFK == Int32.Parse(appid))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM AppServerDB WHERE VersionIDFK = '" + item.VersionID + "'";
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM ApplicationVersion WHERE AppIDFK = '" + appid + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppSupportInfo WHERE AppIDFK = '" + appid + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppMain WHERE AppID = '" + appid + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            if (vendordelete == "Y")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM AppVendorSupport WHERE VenIDFK = '" + venid + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM AppVendorContact WHERE VenIDFK = '" + venid + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM AppVendor WHERE VenID = '" + venid + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}