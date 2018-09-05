using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;
using System.Data.SqlClient;

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
            
            foreach (var item in main)
            {
                if (String.Compare(id, item.AppName, true) == 0)
                {
                    ViewBag.app = item;
                    ViewBag.versions = item.ApplicationVersions;
                    ViewBag.ascSoftware = item.AppAscSoftwares;
                    ViewBag.ascHardware = item.AppAscHardwares;
                    ViewBag.appSupport = item.AppSupportInfoes;
                    ViewBag.vendor = item.AppVendor;
                    ViewBag.appcontacts = item.AppVendor.AppVendorContacts;
                    ViewBag.appcontracts = item.AppContracts;
                    ViewBag.appVendorSupport = item.AppVendor.AppVendorSupports;
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

        [HttpPost]
        public ActionResult saveVersion(string versionName, string versionIE, string installDate, string upgradeDate, 
            string versionStatus, string appName, string versionid)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            if (versionStatus == "Active")
            {
                versionStatus = "A";
            }
            else
            {
                versionStatus = "E";
            }

            int appid = 0;

            foreach (var item in db.AppMains.ToList())
            {
                if (item.AppName == appName)
                {
                    appid = item.AppID;
                }
            }

            if (versionid.Length == 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO ApplicationVersion (AppVersion, AppIE, AppInstallDate, AppUpgradeDate, VersionStatus, CreatedBy, CreatedDate, AppIDFK) VALUES(@vn, @ai, @ad, @au, @vs, @mb, @md, @ap); ";

                    command.Parameters.AddWithValue("@vn", versionName);
                    command.Parameters.AddWithValue("@ai", versionIE);
                    command.Parameters.AddWithValue("@vs", versionStatus);
                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    command.Parameters.AddWithValue("@ap", appid);

                    if (installDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@ad", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ad", installDate);
                    }

                    if (upgradeDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@au", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@au", upgradeDate);
                    }

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID='" + appid + "'";

                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Redirect("~/Info/Index/" + appName + "/");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE ApplicationVersion SET AppVersion = @vn, AppIE = @ai, AppInstallDate = @ad, AppUpgradeDate = @au, VersionStatus = @vs, ModifiedBy = @mb, ModifiedDate = @md WHERE VersionID='" + versionid + "'";

                command.Parameters.AddWithValue("@vn", versionName);
                command.Parameters.AddWithValue("@ai", versionIE);
                command.Parameters.AddWithValue("@vs", versionStatus);
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                if (installDate.Length == 0)
                {
                    command.Parameters.AddWithValue("@ad", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@ad", installDate);
                }

                if (upgradeDate.Length == 0)
                {
                    command.Parameters.AddWithValue("@au", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@au", upgradeDate);
                }

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID='" + appid + "'";

                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appName + "/");
        }

        [HttpPost]
        public ActionResult deleteVersion(string id, string appname)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppServerDB WHERE VersionIDFK = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM ApplicationVersion WHERE VersionID = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        [HttpPost]
        public ActionResult saveContract(string contractNum, string contractAdmin, string startDate, string endDate,
            string renewalDate, string description, string appname, string contractid)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");
            int appid = 0;

            foreach (var item in db.AppMains.ToList())
            {
                if (item.AppName == appname)
                {
                    appid = item.AppID;
                }
            }

            if (contractid == "add")
            {
                

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO AppContract (VenContractNum, VenContractAdmin, VenContractStart, VenContractEnd, VenContractRenew, VenContractComment, CreatedBy, CreatedDate, AppIDFK) VALUES(@vn, @va, @vs, @ve, @vr, @vc, @cb, @cd, @ai); ";

                    command.Parameters.AddWithValue("@vn", contractNum);
                    command.Parameters.AddWithValue("@va", contractAdmin);
                    if (startDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@vs", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@vs", startDate);
                    }
                    if (endDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@ve", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ve", endDate);
                    }
                    if (renewalDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@vr", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@vr", renewalDate);
                    }
                    command.Parameters.AddWithValue("@vc", description);
                    command.Parameters.AddWithValue("@cb", userPrincipalName);
                    command.Parameters.AddWithValue("@cd", DateTime.Now);
                    command.Parameters.AddWithValue("@ai", appid);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID='" + appid + "'";

                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Redirect("~/Info/Index/" + appname + "/");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppContract SET VenContractNum = @vn, VenContractAdmin = @va, VenContractStart = @vs, VenContractEnd = @ve, VenContractRenew = @vr, VenContractComment = @vc, ModifiedBy = @mb, ModifiedDate = @md WHERE VenContractID ='" + contractid + "'";

                    command.Parameters.AddWithValue("@vn", contractNum);
                    command.Parameters.AddWithValue("@va", contractAdmin);
                    if (startDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@vs", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@vs", startDate);
                    }
                    if (endDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@ve", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ve", endDate);
                    }
                    if (renewalDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@vr", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@vr", renewalDate);
                    }
                    command.Parameters.AddWithValue("@vc", description);
                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID='" + appid + "'";

                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        [HttpPost]
        public ActionResult deleteContract(string contractid, string appname)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppContract WHERE VenContractID = '" + contractid + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        [HttpPost]
        public ActionResult saveContact(string id, string venid, string fName, string lName, string contitle, string contactTele, string contactEmail, string status, string appName)
        {
            if (status == "Active")
            {
                status = "Y";
            }
            else
            {
                status = "N";
            }

            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");
            
            if(id.Length == 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO AppVendorContact (VenContactFN, VenContactLN, VenContactTitle, VenContactEmail, VenContactNum, VenContactStatus, CreatedBy, CreatedDate, VenIDFK) VALUES(@fn, @ln, @ct, @ce, @cn, @cs, @cb, @cd, @vi); ";

                    command.Parameters.AddWithValue("@fn", fName);
                    command.Parameters.AddWithValue("@ln", lName);
                    command.Parameters.AddWithValue("@ct", contitle);
                    command.Parameters.AddWithValue("@ce", contactEmail);
                    command.Parameters.AddWithValue("@cn", contactTele);
                    command.Parameters.AddWithValue("@cs", status);
                    command.Parameters.AddWithValue("@cb", userPrincipalName);
                    command.Parameters.AddWithValue("@cd", DateTime.Now);
                    command.Parameters.AddWithValue("@vi", venid);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                return Redirect("~/Info/Index/" + appName + "/");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppVendorContact SET VenContactFN = @fn, VenContactLN = @ln, VenContactTitle = @ct, VenContactEmail = @ce, VenContactNum = @cn, VenContactStatus = @cs,  ModifiedBy = @mb, ModifiedDate = @md WHERE VenContactID = '" + id + "'";

                    command.Parameters.AddWithValue("@fn", fName);
                    command.Parameters.AddWithValue("@ln", lName);
                    command.Parameters.AddWithValue("@ct", contitle);
                    command.Parameters.AddWithValue("@ce", contactEmail);
                    command.Parameters.AddWithValue("@cn", contactTele);
                    command.Parameters.AddWithValue("@cs", status);
                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
                
            }


            return Redirect("~/Info/Index/" + appName + "/");
        }

        [HttpPost]
        public ActionResult deleteContact(string id, string appname)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppVendorContact WHERE VenContactID = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        [HttpPost]
        public ActionResult saveSoftware(string appname, string softwareName, string softwareVersion, string prodDate, string expiryDate, string id)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            int appid = 0;

            foreach (var item in db.AppMains.ToList())
            {
                if (item.AppName == appname)
                {
                    appid = item.AppID;
                }
            }

            if (id.Length == 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText = "INSERT INTO AppAscSoftware (AppAscSoftwareName, AppAscSoftwareVersion, ProductionDate, ExpiryDate, CreatedBy, CreatedDate, AppIDFK) VALUES (@sn, @sv, @pd, @ed, @mb, @md, @ai)";

                    command.Parameters.AddWithValue("@sn", softwareName);
                    command.Parameters.AddWithValue("@sv", softwareVersion);
                    if(prodDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@pd", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@pd", prodDate);
                    }
                    if (expiryDate.Length == 0)
                    {
                        command.Parameters.AddWithValue("@ed", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ed", expiryDate);
                    }
                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    command.Parameters.AddWithValue("@ai", appid);

                    connection.Open();
                    
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID = '" + appid + "'";
                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Redirect("~/Info/Index/" + appname + "/");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {

                command.CommandText = "UPDATE AppAscSoftware SET AppAscSoftwareName = @sn, AppAscSoftwareVersion = @sv, ProductionDate = @pd, ExpiryDate = @ed, ModifiedBy = @mb, ModifiedDate = @md WHERE AppAscSoftID = '" + id + "'";

                command.Parameters.AddWithValue("@sn", softwareName);
                command.Parameters.AddWithValue("@sv", softwareVersion);
                if (prodDate.Length == 0)
                {
                    command.Parameters.AddWithValue("@pd", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@pd", prodDate);
                }
                if (expiryDate.Length == 0)
                {
                    command.Parameters.AddWithValue("@ed", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@ed", expiryDate);
                }
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID = '" + appid + "'";
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        public ActionResult deleteSoftware(string id, string appname)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppAscSoftware WHERE AppAscSoftID = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        [HttpPost]
        public ActionResult saveHardware(string hardwareName, string hardwareType, string comments, string appname, string id)
        {

            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            int appid = 0;

            foreach (var item in db.AppMains.ToList())
            {
                if (item.AppName == appname)
                {
                    appid = item.AppID;
                }
            }

            if (id.Length == 0)
            {
                

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText = "INSERT INTO AppAscHardware (HardwareName, HardwareType, Comments, CreatedBy, CreatedDate, AppIDFK) VALUES (@hn, @ht, @cm, @mb, @md, @ai)";

                    command.Parameters.AddWithValue("@hn", hardwareName);
                    command.Parameters.AddWithValue("@ht", hardwareType);
                    command.Parameters.AddWithValue("@cm", comments);
                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    command.Parameters.AddWithValue("@ai", appid);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID='" + appid + "'";

                    command.Parameters.AddWithValue("@mb", userPrincipalName);
                    command.Parameters.AddWithValue("@md", DateTime.Now);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Redirect("~/Info/Index/" + appname + "/");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppAscHardware SET HardwareName = @hn, HardwareType = @ht, Comments = @cm,  ModifiedBy = @mb, ModifiedDate = @md WHERE AppADID = '" + id + "'";

                command.Parameters.AddWithValue("@hn", hardwareName);
                command.Parameters.AddWithValue("@ht", hardwareType);
                command.Parameters.AddWithValue("@cm", comments);
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppMain SET ModifiedBy = @mb, ModifiedDate = @md WHERE AppID='" + appid + "'";

                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        public ActionResult deleteHardware(string id, string appname)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppAscHardware WHERE AppADID = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return Redirect("~/Info/Index/" + appname + "/");
        }

        [HttpPost]
        public ActionResult saveVendor(string vendName, string vendAddress, string vendURL, string supportHRS, string vendPhone,
            string vendEmail, string vendSupport, string appName, string vendorid, string supportID)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppVendor SET VendName = @vn, VendAddr = @va, VendURL = @vu WHERE VenID ='" + vendorid + "'";

                command.Parameters.AddWithValue("@vn", vendName);
                command.Parameters.AddWithValue("@va", vendAddress);
                command.Parameters.AddWithValue("@vu", vendURL);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

           if(supportID == null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO AppVendorSupport (VenSupportHours, VenSupportNum, VenSupportEmail, VenSupportComments, VenIDFK) VALUES (@vh, @vn, @ve, @vc, @vi)";

                    command.Parameters.AddWithValue("@vh", supportHRS);
                    command.Parameters.AddWithValue("@vn", vendPhone);
                    command.Parameters.AddWithValue("@ve", vendEmail);
                    command.Parameters.AddWithValue("@vc", vendSupport);
                    command.Parameters.AddWithValue("@vi", vendorid);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE AppVendorSupport SET VenSupportHours = @vh, VenSupportNum = @vn, VenSupportEmail = @ve, VenSupportComments = @vc WHERE VenSupportID ='" + supportID + "'";

                    command.Parameters.AddWithValue("@vh", supportHRS);
                    command.Parameters.AddWithValue("@vn", vendPhone);
                    command.Parameters.AddWithValue("@ve", vendEmail);
                    command.Parameters.AddWithValue("@vc", vendSupport);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }


            return Redirect("~/Info/Index/" + appName + "/");
        }


        [HttpPost]
        public ActionResult saveMain(string appName, string ogApp, string appDesc, string appType, string deployType,
            string liscenceType, string numLiscences, string numUsers, string appAccess, string itOwner,
            string systemAdmin, string BOG, string PMR, string supportHours, string afterHours,
            string maintWindow)
        {
            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            if (appAccess == "Yes")
            {
                appAccess = "Y";
            }
            else
            {
                appAccess = "N";
            }
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppMain SET AppName = @an, AppDesc = @ad, AppType = @at, DeployType = @dt, LiscType = @lt, LiscNum = @ln, UserNum = @un, AppAccess = @aa, ModifiedBy = @mb, ModifiedDate = @md WHERE AppID = '" + ogApp + "'";
                command.Parameters.AddWithValue("@an", appName);
                command.Parameters.AddWithValue("@ad", appDesc);
                command.Parameters.AddWithValue("@at", appType);
                command.Parameters.AddWithValue("@dt", deployType);
                command.Parameters.AddWithValue("@lt", liscenceType);
                command.Parameters.AddWithValue("@ln", numLiscences);
                command.Parameters.AddWithValue("@un", numUsers);
                command.Parameters.AddWithValue("@aa", appAccess);
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            var ID = -1;

            foreach (var item in db.AppSupportInfoes.ToList())
            {
                if (item.AppMain.AppName == appName)
                {
                    ID = item.AppSupportID;
                    break;
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppSupportInfo SET AppSupportITOwner = @io, AppSupportAdmin = @sa, AppSupportBOG = @bog, AppSupportPMR = @pmr, AppHours = @hrs, AppMaintenance = @am, ModifiedBy = @mb, ModifiedDate = @md WHERE AppSupportID='" + ID + "'";

                command.Parameters.AddWithValue("@io", itOwner);
                command.Parameters.AddWithValue("@sa", systemAdmin);
                command.Parameters.AddWithValue("@bog", BOG);
                command.Parameters.AddWithValue("@pmr", PMR);
                command.Parameters.AddWithValue("@hrs", supportHours);
                command.Parameters.AddWithValue("@am", maintWindow);
                command.Parameters.AddWithValue("@mb", userPrincipalName);
                command.Parameters.AddWithValue("@md", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }

            return Redirect("~/Info/Index/" + appName + "/");
        }

        public ActionResult Version(string id, string appname)
        {

            var versions= db.ApplicationVersions.ToList();

            foreach (var item in versions)
            {
                if (item.AppVersion + item.AppIE == id && item.AppMain.AppName == appname)
                {
                    ViewBag.version = item.AppVersion;
                    ViewBag.versionid = item.VersionID;
                    ViewBag.IE = item.AppIE;
                    ViewBag.InstallDate = item.AppInstallDate;
                    ViewBag.UpgradeDate = item.AppUpgradeDate;
                    ViewBag.servers = item.AppServerDBs;
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
        

        [HttpPost]
        public ActionResult saveServer(string serverName, string serverType, string serverOS, string serverURL, string dbname, string dbtype, string serverie, string id, string versionid)
        {

            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            var userPrincipalName = User.Identity.Name;
            userPrincipalName = User.Identity.Name.Replace("CITYACCT\\", "");

            string appname = "";
            string versionname = "";
            string versionenviro = "";
            
            foreach(var item in db.ApplicationVersions.ToList())
            {
                if(item.VersionID == Int32.Parse(versionid)){
                    appname = item.AppMain.AppName;
                    versionname = item.AppVersion;
                    versionenviro = item.AppIE;
                }
            }

            if (id.Length == 0)
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText = "INSERT INTO AppServerDB (AppServerName, AppServerType, AppServerOS, AppServerURL, AppDBType, AppDBName, AppServerIE, VersionIDFK) VALUES (@sn, @st, @os, @su, @dt, @dn, @si, @vi)";

                    command.Parameters.AddWithValue("@sn", serverName);
                    command.Parameters.AddWithValue("@st", serverType);
                    command.Parameters.AddWithValue("@os", serverOS);
                    command.Parameters.AddWithValue("@su", serverURL);
                    command.Parameters.AddWithValue("@dt", dbtype);
                    command.Parameters.AddWithValue("@dn", dbname);
                    command.Parameters.AddWithValue("@si", serverie);
                    command.Parameters.AddWithValue("@vi", versionid);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return Redirect("~/Info/Version/" + versionname + versionenviro + "/" + appname + "/");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE AppServerDB SET AppServerName = @sn, AppServerType = @st, AppServerOS = @os, AppServerURL = @su, AppDBType = @dt, AppDBName = @dn, AppServerIE = @si WHERE AppServerID = '" + id + "'";

                command.Parameters.AddWithValue("@sn", serverName);
                command.Parameters.AddWithValue("@st", serverType);
                command.Parameters.AddWithValue("@os", serverOS);
                command.Parameters.AddWithValue("@su",serverURL);
                command.Parameters.AddWithValue("@dt", dbtype);
                command.Parameters.AddWithValue("@dn", dbname);
                command.Parameters.AddWithValue("@si", serverie);
                
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            
            return Redirect("~/Info/Version/" + versionname + versionenviro + "/"+appname+"/");
        }

        [HttpPost]
        public ActionResult deleteServer(string id, string versionid)
        {
            string connectionString = @"data source=INTSQLDBS91DVM\DEV014A;initial catalog=AppAsset;persist security info=True;user id=AppAssetADM;password=admappasset;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM AppServerDB WHERE AppServerID = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            string appname = "";
            string versionname = "";
            string versionenviro = "";

            foreach (var item in db.ApplicationVersions.ToList())
            {
                if (item.VersionID == Int32.Parse(versionid))
                {
                    appname = item.AppMain.AppName;
                    versionname = item.AppVersion;
                    versionenviro = item.AppIE;
                }
            }

            return Redirect("~/Info/Version/" + versionname + versionenviro + "/" + appname + "/");
        }

    }
}