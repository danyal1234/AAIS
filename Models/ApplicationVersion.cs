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
    
    public partial class ApplicationVersion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicationVersion()
        {
            this.AppServerDBs = new HashSet<AppServerDB>();
        }
    
        public int VersionID { get; set; }
        public string AppVersion { get; set; }
        public string AppIE { get; set; }
        public Nullable<System.DateTime> AppInstallDate { get; set; }
        public Nullable<System.DateTime> AppUpgradeDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int AppIDFK { get; set; }
        public string VersionStatus { get; set; }
    
        public virtual AppMain AppMain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppServerDB> AppServerDBs { get; set; }
    }
}
