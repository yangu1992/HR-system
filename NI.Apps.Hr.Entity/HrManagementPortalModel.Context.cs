﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NI.Apps.Hr.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HrDbContext : DbContext
    {
        public HrDbContext()
            : base("name=HrDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Table_ChildrenInfo> Table_ChildrenInfo { get; set; }
        public virtual DbSet<Table_CostCenter> Table_CostCenter { get; set; }
        public virtual DbSet<Table_Department> Table_Department { get; set; }
        public virtual DbSet<Table_EducationInfo> Table_EducationInfo { get; set; }
        public virtual DbSet<Table_FamilyMembersInfo> Table_FamilyMembersInfo { get; set; }
        public virtual DbSet<Table_Headcount> Table_Headcount { get; set; }
        public virtual DbSet<Table_Offer> Table_Offer { get; set; }
        public virtual DbSet<Table_OfferCycle> Table_OfferCycle { get; set; }
        public virtual DbSet<Table_PersonelInfo> Table_PersonelInfo { get; set; }
        public virtual DbSet<Table_ReportingInfo> Table_ReportingInfo { get; set; }
        public virtual DbSet<Table_Resignation> Table_Resignation { get; set; }
        public virtual DbSet<Table_Role> Table_Role { get; set; }
        public virtual DbSet<Table_SalaryInfo> Table_SalaryInfo { get; set; }
        public virtual DbSet<Table_Survey> Table_Survey { get; set; }
        public virtual DbSet<Table_WorkExperienceInfo> Table_WorkExperienceInfo { get; set; }
        public virtual DbSet<Table_TypeRoleAccess> Table_TypeRoleAccess { get; set; }
        public virtual DbSet<Table_UserRole> Table_UserRole { get; set; }
        public virtual DbSet<Table_InternalLevel> Table_InternalLevel { get; set; }
        public virtual DbSet<Table_Employee> Table_Employee { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<xprcFile_Result> xprcFile(string txt, Nullable<int> hasTitle, Nullable<int> isShow)
        {
            var txtParameter = txt != null ?
                new ObjectParameter("txt", txt) :
                new ObjectParameter("txt", typeof(string));
    
            var hasTitleParameter = hasTitle.HasValue ?
                new ObjectParameter("hasTitle", hasTitle) :
                new ObjectParameter("hasTitle", typeof(int));
    
            var isShowParameter = isShow.HasValue ?
                new ObjectParameter("isShow", isShow) :
                new ObjectParameter("isShow", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<xprcFile_Result>("xprcFile", txtParameter, hasTitleParameter, isShowParameter);
        }
    
        public virtual int Proc_InsertNewAccount(ObjectParameter returnVal, Nullable<int> offerID, string domainLogin, Nullable<System.DateTime> contractStartDate)
        {
            var offerIDParameter = offerID.HasValue ?
                new ObjectParameter("OfferID", offerID) :
                new ObjectParameter("OfferID", typeof(int));
    
            var domainLoginParameter = domainLogin != null ?
                new ObjectParameter("DomainLogin", domainLogin) :
                new ObjectParameter("DomainLogin", typeof(string));
    
            var contractStartDateParameter = contractStartDate.HasValue ?
                new ObjectParameter("ContractStartDate", contractStartDate) :
                new ObjectParameter("ContractStartDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Proc_InsertNewAccount", returnVal, offerIDParameter, domainLoginParameter, contractStartDateParameter);
        }
    
        public virtual int Proc_InsertAddressBook(Nullable<int> offerID, Nullable<int> sID)
        {
            var offerIDParameter = offerID.HasValue ?
                new ObjectParameter("OfferID", offerID) :
                new ObjectParameter("OfferID", typeof(int));
    
            var sIDParameter = sID.HasValue ?
                new ObjectParameter("SID", sID) :
                new ObjectParameter("SID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Proc_InsertAddressBook", offerIDParameter, sIDParameter);
        }
    }
}
