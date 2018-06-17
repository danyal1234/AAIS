using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetApp.Models;

namespace AssetApp.Models
{
    public class CompositeModel
    {
        public CompositeModel()
        {
            AppVendorModel = new AppVendor();
            AppVendorContactModel = new AppVendorContact();
            AppVendorSupportModel = new AppVendorSupport();
            AppMainModel = new AppMain();
            AppContractModel = new AppContract();
            AppSupportInfoModel = new AppSupportInfo();
            ApplicationVersionModel = new ApplicationVersion();
            AppServerDBModel = new AppServerDB();
            AppADGroupModel = new AppADGroup();
            AppAscSoftwareModel = new AppAscSoftware();
            AppAscHardwareModel = new AppAscHardware();
            AppAscRprtModel = new AppAscRprt();
        }

        public AppVendor AppVendorModel { get; set; }
        public AppVendorContact AppVendorContactModel { get; set; }
        public AppVendorSupport AppVendorSupportModel { get; set; }
        public AppContract AppContractModel { get; set; }
        public AppMain AppMainModel { get; set; }
        public AppSupportInfo AppSupportInfoModel { get; set; }
        public ApplicationVersion ApplicationVersionModel { get; set; }
        public AppServerDB AppServerDBModel { get; set; }
        public AppADGroup AppADGroupModel { get; set; }
        public AppAscSoftware AppAscSoftwareModel { get; set; }
        public AppAscHardware AppAscHardwareModel { get; set; }
        public AppAscRprt AppAscRprtModel { get; set; }

    }
}