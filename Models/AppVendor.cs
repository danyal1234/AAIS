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
    
    public partial class AppVendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppVendor()
        {
            this.AppContracts = new HashSet<AppContract>();
            this.AppMains = new HashSet<AppMain>();
            this.AppVendorContacts = new HashSet<AppVendorContact>();
            this.AppVendorSupports = new HashSet<AppVendorSupport>();
        }
    
        public int VenID { get; set; }
        public string VendName { get; set; }
        public string VendAddr { get; set; }
        public string VendURL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppContract> AppContracts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppMain> AppMains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppVendorContact> AppVendorContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppVendorSupport> AppVendorSupports { get; set; }
    }
}