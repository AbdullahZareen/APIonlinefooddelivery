//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fypapi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Resturant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resturant()
        {
            this.FoodItems = new HashSet<FoodItem>();
        }
    
        public int rid { get; set; }
        public string rcname { get; set; }
        public string rcaddress { get; set; }
        public string rccity { get; set; }
        public string rcemail { get; set; }
        public string rpassword { get; set; }
        public string rclongitude { get; set; }
        public string rclattitude { get; set; }
        public string rcImage { get; set; }
        public string ownername { get; set; }
        public string Category { get; set; }
        public Nullable<int> rcnumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodItem> FoodItems { get; set; }
    }
}