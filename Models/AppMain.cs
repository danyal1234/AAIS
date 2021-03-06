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
    
    public partial class AppMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppMain()
        {
            this.AppAscHardwares = new HashSet<AppAscHardware>();
            this.AppAscSoftwares = new HashSet<AppAscSoftware>();
            this.AppContracts = new HashSet<AppContract>();
            this.ApplicationVersions = new HashSet<ApplicationVersion>();
            this.AppSupportInfoes = new HashSet<AppSupportInfo>();
        }
    
        public int AppID { get; set; }
        public string AppName { get; set; }
        public string AppDesc { get; set; }
        public string AppType { get; set; }
        public string DeployType { get; set; }
        public string LiscType { get; set; }
        public Nullable<int> LiscNum { get; set; }
        public string UserNum { get; set; }
        public string AppAccess { get; set; }
        public int VenIDFK { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Expired { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppAscHardware> AppAscHardwares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppAscSoftware> AppAscSoftwares { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppContract> AppContracts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationVersion> ApplicationVersions { get; set; }
        public virtual AppVendor AppVendor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppSupportInfo> AppSupportInfoes { get; set; }
    }
}
