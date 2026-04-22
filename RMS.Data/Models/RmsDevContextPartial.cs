using Microsoft.EntityFrameworkCore;
using RMS.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Data.Models
{
    // This is an extension to the existing RmsDevContext partial class
    public partial class RmsDevContext
    {
        // Add a DbSet for EmployeeDetailDto
        // Since it's a partial class, it will be considered part of the RmsDevContext
        public virtual DbSet<EmployeeDetailDto> EmployeeDetailDtos { get; set; }       
        public virtual DbSet<rptContractBillingDto> rptContractBillingDtos { get; set; }
        public virtual DbSet<RevisionDetailsDto> RevisionDetailsDtos { get; set; }
        public virtual DbSet<rptContractBillingProvisionDto> rptContractBillingProvisionDtos { get; set; }
        public virtual DbSet<rptContractBillingActualDto> rptContractBillingActualDtos { get; set; }
        public virtual DbSet<ContractlistDto> ContractlistDtos { get; set; }
        public virtual DbSet<BillingDropDownDto> BillingDropDownDtos { get; set; }
        public virtual DbSet<ResourceInfoForHRDto> ResourceInfoForHRDtos { get; set; }
        public virtual DbSet<AuthorizePagesForEmployeeModuleDto> AuthorizePagesForEmployeeModuleDtos { get; set; }
        public virtual DbSet<TimeSheetEmployeeDetailDto> TimeSheetEmployeeDetailDtos { get; set; }
        public virtual DbSet<OafListDto> OafListDtos { get; set; }
        public virtual DbSet<ContractendingsoonDto> ContractendingsoonDto { get; set; }
        public virtual DbSet<TimesheetHRExcelDto> TimesheetHRExcelDto { get; set; }





        // Use another partial method to configure the model without a key
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDetailDto>().HasNoKey();
            modelBuilder.Entity<rptContractBillingDto>().HasNoKey();
            modelBuilder.Entity<RevisionDetailsDto>().HasNoKey();
            modelBuilder.Entity<rptContractBillingProvisionDto>().HasNoKey();
            modelBuilder.Entity<ContractlistDto>().HasNoKey();
            modelBuilder.Entity<BillingDropDownDto>().HasNoKey();
            modelBuilder.Entity<rptContractBillingActualDto>().HasNoKey();
            modelBuilder.Entity<ResourceInfoForHRDto>().HasNoKey();
            modelBuilder.Entity<AuthorizePagesForEmployeeModuleDto>().HasNoKey();
            modelBuilder.Entity<TimeSheetEmployeeDetailDto>().HasNoKey();
            modelBuilder.Entity<OafListDto>().HasNoKey();
            modelBuilder.Entity<ContractendingsoonDto>().HasNoKey();
            modelBuilder.Entity<TimesheetHRExcelDto>().HasNoKey();

        }
    }
}