//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AntiCorruptionSeachEngine
{
    using System;
    using System.Collections.Generic;
    
    public partial class website
    {
        public website()
        {
            this.link_country_website = new HashSet<link_country_website>();
            this.link_industry_website = new HashSet<link_industry_website>();
            this.link_website_words = new HashSet<link_website_words>();
        }
    
        public int id { get; set; }
        public string anchor { get; set; }
        public string country { get; set; }
        public string info { get; set; }
        public string title { get; set; }
        public Nullable<bool> cost { get; set; }
        public Nullable<int> rank { get; set; }
        public Nullable<int> word_rank { get; set; }
    
        public virtual ICollection<link_country_website> link_country_website { get; set; }
        public virtual ICollection<link_industry_website> link_industry_website { get; set; }
        public virtual ICollection<link_website_words> link_website_words { get; set; }
    }
}
