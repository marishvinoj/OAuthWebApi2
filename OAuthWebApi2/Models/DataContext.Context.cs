﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OAuthWebApi2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OauthApiEntities : DbContext
    {
        public OauthApiEntities()
            : base("name=OauthApiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ApiUser> ApiUsers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User_Roles> User_Roles { get; set; }
    }
}
