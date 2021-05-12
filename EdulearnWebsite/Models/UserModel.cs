﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EdulearnWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class userModel
    {
        public int UserID { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public Nullable<int> HeadAdminID { get; set; }
        public Nullable<int> LearnerID { get; set; }
        public Nullable<int> AdminID { get; set; }
        public string confirmPass { get; set; }
        public string email { get; set; }

        public bool IsEmailVerified { get; set; } = false;
        public System.Guid ActivationCode { get; set; }

        public bool RememberMe { get; set; }

        public virtual admin admin { get; set; }
        public virtual headAdmin headAdmin { get; set; }
        public virtual learner learner { get; set; }
    }
}
