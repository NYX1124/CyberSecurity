//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Cybersecurity_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Score
    {
        public int id { get; set; }
        public string username { get; set; }
        public int score1 { get; set; }
        public int CategoryId { get; set; }
        public System.DateTime createDate { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Employee Employee { get; set; }
    }
}