//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssetApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AppVendorContact
    {
        public int VenContactID { get; set; }
        public string VenContactFN { get; set; }
        public string VenContactLN { get; set; }
        public string VenContactTitle { get; set; }
        public string VenContactEmail { get; set; }
        public string VenContactNum { get; set; }
        public string VenContactStatus { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int VenIDFK { get; set; }
    
        public virtual AppVendor AppVendor { get; set; }
    }
}
