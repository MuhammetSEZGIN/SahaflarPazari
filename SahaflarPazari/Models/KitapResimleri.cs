//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SahaflarPazari.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KitapResimleri
    {
        public int ResimId { get; set; }
        public int KitapId { get; set; }
        public string DataYolu { get; set; }
    
        public virtual Kitap Kitap { get; set; }
    }
}
