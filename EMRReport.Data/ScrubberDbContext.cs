using EMRReport.DataContracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMRReport.Data
{
    public sealed class ScrubberDbContext : DbContext
    {
        public ScrubberDbContext(DbContextOptions<ScrubberDbContext> options) : base(options)
        { }
        public DbSet<ScrubberErrorEntity> ScrubberError { get; set; }
        public DbSet<ValidatorErrorEntity> ValidatorError { get; set; }
        public DbSet<ClaimEntity> Claim { get; set; }
        public DbSet<ClaimActivityEntity> ClaimActivity { get; set; }
        public DbSet<ClaimEncounterEntity> ClaimEncounter { get; set; }
        public DbSet<ClaimDiagnosisEntity> ClaimDiagnosis { get; set; }
        public DbSet<ClaimActivityObservationEntity> ClaimObservation { get; set; }
        public DbSet<ClaimBasketEntity> ClaimBasket { get; set; }
        public DbSet<BasketGroupEntity> BasketGroup { get; set; }
        public DbSet<FacilityEntity> Facility { get; set; }
        public DbSet<PayerReceiverEntity> PayerReceiver { get; set; }
        public DbSet<RegulatoryEntity> Regulatory { get; set; }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<RulesEntity> Rules { get; set; }
        public DbSet<AddressEntity> Address { get; set; }
        public DbSet<GroupEntity> Group { get; set; }
        public DbSet<CompanyEntity> Company { get; set; }
        public DbSet<CompanyRoleEntity> CompanyRole { get; set; }
        public DbSet<CompanyRoleFacilityEntity> CompanyRoleFacility { get; set; }
        public DbSet<ControlEntity> Control { get; set; }
        public DbSet<CountryEntity> Country { get; set; }
        public DbSet<GroupControlEntity> GroupControl { get; set; }
        public DbSet<RoleGroupEntity> RoleGroup { get; set; }
        public DbSet<OrganizationEntity> Organization { get; set; }
        public DbSet<FacilityTypeEntity> FacilityType { get; set; }
        public DbSet<StateEntity> State { get; set; }
        public DbSet<EncounterTypeEntity> EncounterType { get; set; }
        public DbSet<FacilityCategoryEntity> FacilityCategory { get; set; }
        public DbSet<InsuranceClassificationEntity> InsuranceClassification { get; set; }
        public DbSet<ValidatorTransactionEntity> ValidatorTransaction { get; set; }
        public DbSet<ValidatorCPTEntity> ValidatorCPT { get; set; }
        public DbSet<ValidatorICDEntity> ValidatorICD { get; set; }
        public DbSet<ActivityEntity> Activity { get; set; }
        public DbSet<SettingsEntity> Settings { get; set; }
        public DbSet<UserHistoryEntity> UserHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScrubberErrorEntity>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.ScrubberErrorID,
                    e.ClaimBasketID,
                    e.XMLClaimTagID,
                    e.ScrubberPrefixType,
                    e.ScrubberErrorCategory,
                    e.IsHit,
                    e.CreatedBy,
                    e.RoleID,
                    e.IsValidated,
                    e.Year,
                    e.Month,
                    e.Week,
                    e.Day
                }).HasName("PK_dbo.ScrubberErrorMaster");
                entity.ToTable("ScrubberErrorMaster");
                entity.Property(e => e.ScrubberErrorID).HasColumnName("ScrubberErrorID").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.XMLClaimTagID).HasColumnName("Claim_Id");
                entity.Property(e => e.RoleID).HasColumnName("RoleID");
                entity.Property(e => e.ActivityClinician).HasMaxLength(100);
                entity.Property(e => e.ActivityID).HasColumnName("ActivityID").HasMaxLength(100);
                entity.Property(e => e.ActivityStart).HasMaxLength(25);
                entity.Property(e => e.Age).HasMaxLength(25);
                entity.Property(e => e.ClaimGross).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ClaimID).HasColumnName("ClaimID").HasMaxLength(100);
                entity.Property(e => e.ClaimNet).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CPTS).HasColumnName("CPTS").HasMaxLength(4000);
                entity.Property(e => e.DispositionFlag).HasMaxLength(20);
                entity.Property(e => e.EmiratesID).HasColumnName("EmiratesID").HasMaxLength(25);
                entity.Property(e => e.EncounterEnd).HasMaxLength(25);
                entity.Property(e => e.EncounterEndType).HasMaxLength(4);
                entity.Property(e => e.EncounterStartType).HasMaxLength(4);
                entity.Property(e => e.ErrorCode1).HasColumnName("ErrorCPT1").HasMaxLength(100);
                entity.Property(e => e.ErrorCode2).HasColumnName("ErrorCPT2").HasMaxLength(400);
                entity.Property(e => e.ErrorDate).HasColumnType("datetime");
                entity.Property(e => e.ErrorHit).HasMaxLength(4);
                entity.Property(e => e.FileName).HasMaxLength(1000);
                entity.Property(e => e.Gender).HasMaxLength(50);
                entity.Property(e => e.MemberID).HasColumnName("MemberID").HasMaxLength(50);
                entity.Property(e => e.Message).HasMaxLength(1000);
                entity.Property(e => e.CodingTips).HasMaxLength(4000);
                entity.Property(e => e.MRN).HasColumnName("MRN").HasMaxLength(50);
                entity.Property(e => e.Net).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.OrderingClinician).HasMaxLength(100);
                entity.Property(e => e.PatientShare).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PrimaryICD).HasColumnName("PrimaryICD").HasMaxLength(50);
                entity.Property(e => e.PriorAuthorizationID).HasColumnName("PriorAuthorizationID").HasMaxLength(100);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ReasonForVisitICDS).HasColumnName("ReasonForVisitICDS").HasMaxLength(1000);
                entity.Property(e => e.ReceiverID).HasColumnName("ReceiverID").HasMaxLength(20);
                entity.Property(e => e.SecondaryICDS).HasColumnName("SecondaryICDS").HasMaxLength(4000);
                entity.Property(e => e.SenderID).HasColumnName("SenderID").HasMaxLength(20);
                entity.Property(e => e.ServiceCode).HasMaxLength(100);
                entity.Property(e => e.Start).HasMaxLength(25);
                entity.Property(e => e.TransactionDate).HasMaxLength(25);
                entity.Property(e => e.VAT).HasColumnName("VAT").HasColumnType("decimal(18, 2)");
            });
            modelBuilder.Entity<ValidatorErrorEntity>(entity =>
            {
                entity.HasKey(e => e.ValidatorErrorID).HasName("PK_dbo.ICDValidatorErrorMaster");
                entity.ToTable("ICDValidatorErrorMaster");
                entity.Property(e => e.ValidatorErrorID).HasColumnName("ICDErrorID");
                entity.Property(e => e.CaseNumber).HasColumnName("CaseNumber").HasMaxLength(4000);
                entity.Property(e => e.ErrorCode1).HasColumnName("ErrorICD1").HasMaxLength(400);
                entity.Property(e => e.ErrorCode2).HasColumnName("ErrorICD2").HasMaxLength(400);
                entity.Property(e => e.PrefixType).HasColumnName("ScrubberPrefixType");
                entity.Property(e => e.ErrorCategory).HasColumnName("ScrubberErrorCategory");
                entity.Property(e => e.Message).HasMaxLength(1000);
                entity.Property(e => e.CodingTips).HasColumnName("CodingTips").HasMaxLength(4000);
                entity.Property(e => e.ValidatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidatorTransactionID).HasColumnName("ValidatorTransctionID");

                entity.HasOne(d => d.userEntityValidated)
                   .WithMany(p => p.validatorErrorEntityList)
                   .HasForeignKey(d => d.ValidatedBy)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_dbo.ICDValidatorErrorMaster_dbo.UserMaster_ValidatedBy");
            });
            modelBuilder.Entity<ClaimEntity>(entity =>
            {
                entity.HasKey(e => new { e.ClaimEntityID, e.ClaimBasketID, e.XMLClaimTagID })
                  .HasName("PK_dbo.ScrubClaimBasketClaimTrans");
                entity.ToTable("ScrubClaimBasketClaimTrans");
                entity.Property(e => e.ClaimEntityID).HasColumnName("ScrubClaimBasketClaimTransID").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.XMLClaimTagID).HasColumnName("Claim_Id");
                entity.Property(e => e.ClaimGross).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ClaimID).HasColumnName("ClaimID").HasMaxLength(100);
                entity.Property(e => e.PayerID).HasColumnName("PayerID").HasMaxLength(50);
                entity.Property(e => e.ClaimNet).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.EmiratesIDNumber).HasColumnName("EmiratesIDNumber").HasMaxLength(25);
                entity.Property(e => e.MemberFormatID).HasColumnName("MemberFormatID").HasMaxLength(10);
                entity.Property(e => e.MemberID).HasColumnName("MemberID").HasMaxLength(50);
                entity.Property(e => e.PatientShare).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PrimaryICD).HasColumnName("PrimaryICD").HasMaxLength(50);
                entity.Property(e => e.ReasonForVisitICDS).HasColumnName("ReasonForVisitICDS").HasMaxLength(1000);
                entity.Property(e => e.SecondaryICDS).HasColumnName("SecondaryICDS").HasMaxLength(4000);
                entity.Property(e => e.ServiceCodes).HasMaxLength(4000);

            });
            modelBuilder.Entity<ClaimActivityEntity>(entity =>
            {
                entity.HasKey(e => new { e.ClaimActivityID, e.ClaimBasketID, e.XMLClaimTagID, e.ActivityJoinId })
                  .HasName("PK_dbo.ScrubClaimBasketActivityTrans");
                entity.ToTable("ScrubClaimBasketActivityTrans");
                entity.Property(e => e.ClaimActivityID).HasColumnName("ScrubClaimBasketActivityTransID").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.XMLClaimTagID).HasColumnName("Claim_Id");
                entity.Property(e => e.ActivityClinician).HasMaxLength(100);
                entity.Property(e => e.ActivityClinicianLicense).HasMaxLength(100);
                entity.Property(e => e.ActivityID).HasColumnName("ActivityID").HasMaxLength(100);
                entity.Property(e => e.XMLActivityTagID).HasColumnName("Activity_Id");
                entity.Property(e => e.ClinicianLicense).HasMaxLength(100);
                entity.Property(e => e.Net).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.OrderingClinician).HasMaxLength(100);
                entity.Property(e => e.PriorAuthorizationID).HasColumnName("PriorAuthorizationID").HasMaxLength(100);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ServiceCode).HasMaxLength(100);
                entity.Property(e => e.Start).HasMaxLength(25);
                entity.Property(e => e.VAT).HasColumnName("VAT").HasColumnType("decimal(18, 2)");
            });
            modelBuilder.Entity<ClaimEncounterEntity>(entity =>
            {
                entity.HasKey(e => new { e.ClaimEncounterID, e.ClaimBasketID, e.EncounterType })
                  .HasName("PK_dbo.ScrubClaimBasketEncounterTrans");
                entity.ToTable("ScrubClaimBasketEncounterTrans");
                entity.Property(e => e.ClaimEncounterID).HasColumnName("ScrubClaimBasketEncounterID").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.XMLClaimTagID).HasColumnName("Claim_Id");
                entity.Property(e => e.EncounterEndType).HasMaxLength(3);
                entity.Property(e => e.EncounterStartType).HasMaxLength(3);
                entity.Property(e => e.End).HasMaxLength(25);
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.IsDOS).HasColumnName("IsDOS");
                entity.Property(e => e.Network).HasMaxLength(200);
                entity.Property(e => e.PatientID).HasColumnName("PatientID").HasMaxLength(50);
                entity.Property(e => e.PlanId).HasMaxLength(200);
                entity.Property(e => e.Start).HasMaxLength(25);
                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });
            modelBuilder.Entity<ClaimDiagnosisEntity>(entity =>
            {
                entity.HasKey(e => new { e.ClaimDiagnosisID, e.ClaimBasketID, e.XMLClaimTagID })
                   .HasName("PK_dbo.ScrubClaimBasketDiagnosisTrans");
                entity.ToTable("ScrubClaimBasketDiagnosisTrans");
                entity.Property(e => e.ClaimDiagnosisID).HasColumnName("ScrubClaimBasketDiagnosisTransID").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.XMLClaimTagID).HasColumnName("Claim_Id");
                entity.Property(e => e.DiagnosisCode).HasMaxLength(100);
                entity.Property(e => e.Type).HasMaxLength(25);
            });
            modelBuilder.Entity<ClaimActivityObservationEntity>(entity =>
            {
                entity.HasKey(e => new { e.ClaimActivityObservationID, e.ClaimBasketID, e.XMLActivityTagID })
                .HasName("PK_dbo.ScrubClaimBasketObservationTrans");
                entity.ToTable("ScrubClaimBasketObservationTrans");
                entity.Property(e => e.ClaimActivityObservationID).HasColumnName("ScrubClaimBasketObservationTransID").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.XMLActivityTagID).HasColumnName("Activity_Id");
                entity.Property(e => e.Code).HasMaxLength(100);
                entity.Property(e => e.Type).HasMaxLength(100);
                entity.Property(e => e.Value).HasMaxLength(200);
                entity.Property(e => e.ValueType).HasMaxLength(100);
            });
            modelBuilder.Entity<ClaimBasketEntity>(entity =>
            {
                entity.HasKey(e => e.ClaimBasketID).HasName("PK_dbo.ScrubClaimBasketMaster");
                entity.ToTable("ScrubClaimBasketMaster");
                entity.Property(e => e.ClaimBasketID).HasColumnName("ScrubClaimBasketID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DispositionFlag).HasMaxLength(150);
                entity.Property(e => e.FacilityID).HasColumnName("FacilityID");
                entity.Property(e => e.FileName).HasMaxLength(1000);
                entity.Property(e => e.XMLFileName).HasColumnName("GeneratedXMLFileName").HasMaxLength(450);
                entity.Property(e => e.PayerReceiverID).HasColumnName("PayerReceiverID");
                entity.Property(e => e.ReceiverID).HasColumnName("ReceiverID").HasMaxLength(50);
                entity.Property(e => e.RegulatoryID).HasColumnName("RegulatoryID");
                entity.Property(e => e.BasketGroupID).HasColumnName("ScrubBasketGroupID");
                entity.Property(e => e.SenderID).HasColumnName("SenderID").HasMaxLength(50);
                entity.Property(e => e.TransactionDate).HasMaxLength(25);

                entity.HasOne(d => d.UserEntityCreated)
                   .WithMany(p => p.claimBasketEntityList)
                   .HasForeignKey(d => d.CreatedBy)
                   .HasConstraintName("FK_dbo.ScrubClaimBasketMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.facilityEntity)
                    .WithMany(p => p.claimBasketEntityList)
                    .HasForeignKey(d => d.FacilityID)
                    .HasConstraintName("FK_dbo.ScrubClaimBasketMaster_dbo.FacilityMaster_FacilityID");

                entity.HasOne(d => d.payerReceiverEntity)
                    .WithMany(p => p.claimBasketEntityList)
                    .HasForeignKey(d => d.PayerReceiverID)
                    .HasConstraintName("FK_dbo.ScrubClaimBasketMaster_dbo.PayerReceiverMaster_PayerReceiverID");

                entity.HasOne(d => d.regulatoryEntity)
                    .WithMany(p => p.claimBasketEntityList)
                    .HasForeignKey(d => d.RegulatoryID)
                    .HasConstraintName("FK_dbo.ScrubClaimBasketMaster_dbo.RegulatoryMaster_RegulatoryID");

                entity.HasOne(d => d.basketGroupEntity)
                    .WithMany(p => p.claimBasketEntityList)
                    .HasForeignKey(d => d.BasketGroupID)
                    .HasConstraintName("FK_dbo.ScrubClaimBasketMaster_dbo.ScrubBasketGroupMaster_ScrubBasketGroupID");
            });
            modelBuilder.Entity<BasketGroupEntity>(entity =>
            {
                entity.HasKey(e => e.BasketGroupID).HasName("PK_dbo.ScrubBasketGroupMaster");
                entity.ToTable("ScrubBasketGroupMaster");
                entity.Property(e => e.BasketGroupID).HasColumnName("ScrubBasketGroupID");
            });
            modelBuilder.Entity<FacilityEntity>(entity =>
            {
                entity.HasKey(e => e.FacilityID).HasName("PK_dbo.FacilityMaster");
                entity.ToTable("FacilityMaster");
                entity.Property(e => e.FacilityID).HasColumnName("FacilityID");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.HasIndex(e => e.LinkedToFacilityID).HasName("IX_LinkedToFacilityID");
                entity.Property(e => e.FacilityWiseLandingCode).HasMaxLength(50);
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");
                entity.HasIndex(e => e.OrganizationID).HasName("IX_OrganizationID");
                entity.Property(e => e.OrganizationID).HasColumnName("OrganizationID");
                entity.HasIndex(e => e.CompanyID).HasName("IX_CompanyID");
                entity.Property(e => e.CompanyID).HasColumnName("CompanyID");
                entity.HasIndex(e => e.RegulatoryID).HasName("IX_RegulatoryID");
                entity.Property(e => e.RegulatoryID).HasColumnName("RegulatoryID");
                entity.HasIndex(e => e.Status).HasName("IX_Status");
                entity.Property(e => e.ProductivityReportDate).HasColumnType("datetime");
                entity.HasIndex(e => e.FacilityName).HasName("IX_Facility").IsUnique();
                entity.Property(e => e.FacilityName).HasColumnName("Facility").IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.FacilityCode).HasName("IX_FacilityCode").IsUnique();
                entity.Property(e => e.FacilityCode).IsRequired().HasMaxLength(100);
                //entity.HasIndex(e => e.FacilitySingleFacilitySingleId).HasName("IX_FacilitySingle_FacilitySingleID");
                entity.HasIndex(e => e.IsDOS).HasName("IX_IsDOS");
                entity.Property(e => e.IsDOS).HasColumnName("IsDOS");
                entity.HasIndex(e => e.IsAbuDhabiDOS).HasName("IX_IsAbuDhabiDOS");
                entity.Property(e => e.IsAbuDhabiDOS).HasColumnName("IsAbuDhabiDOS");
                entity.HasIndex(e => e.FacilityTypeID).HasName("IX_FacilityTypeID");
                entity.HasIndex(e => e.IsEnabledAutoDownload).HasName("IX_IsEnabledAutoDownload");
                entity.HasIndex(e => e.IsEnabledAutoReconcile).HasName("IX_IsEnabledAutoReconcile");
                entity.HasIndex(e => e.IsInsta).HasName("IX_IsInsta");
                entity.Property(e => e.ClaimCount).HasDefaultValueSql("((0))");
                entity.Property(e => e.SubscriptionEndDate).HasColumnType("datetime");
                entity.Property(e => e.SubscriptionStartDate).HasColumnType("datetime");

                entity.Property(e => e.VisCoreLisenceTypeID).HasColumnName("VisCoreLisenceTypeID");
                entity.Property(e => e.WebServicePassword).HasMaxLength(250);
                entity.Property(e => e.WebServiceUserName).HasMaxLength(250);

                entity.HasOne(d => d.companyEntity)
                    .WithMany(p => p.facilityEntityList)
                    .HasForeignKey(d => d.CompanyID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.FacilityMaster_dbo.CompanyMaster_CompanyID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.facilityEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.FacilityMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.facilityEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.FacilityMaster_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.facilityEntityParent)
                   .WithMany(p => p.facilityEntityParentList)
                   .HasForeignKey(d => d.LinkedToFacilityID)
                   .HasConstraintName("FK_dbo.FacilityMaster_dbo.FacilityMaster_LinkedToFacilityID");

                entity.HasOne(d => d.organizationEntity)
                    .WithMany(p => p.facilityEntityList)
                    .HasForeignKey(d => d.OrganizationID)
                    .HasConstraintName("FK_dbo.FacilityMaster_dbo.OrganizationMaster_OrganizationID");

                entity.HasOne(d => d.facilityTypeEntity)
                   .WithMany(p => p.facilityEntityList)
                   .HasForeignKey(d => d.FacilityTypeID)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_dbo.FacilityMaster_dbo.FacilityTypeMaster_FacilityTypeID");

                entity.HasOne(d => d.regulatoryEntity)
                    .WithMany(p => p.facilityEntityList)
                    .HasForeignKey(d => d.RegulatoryID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.FacilityMaster_dbo.RegulatoryMaster_RegulatoryID");

                /* entity.HasOne(d => d.FacilitySingleFacilitySingle)
                     .WithMany(p => p.FacilityMaster)
                     .HasForeignKey(d => d.FacilitySingleFacilitySingleId)
                     .HasConstraintName("FK_dbo.FacilityMaster_dbo.FacilitySingleMaster_FacilitySingle_FacilitySingleID");
                */
            });
            modelBuilder.Entity<PayerReceiverEntity>(entity =>
            {
                entity.HasKey(e => e.PayerReceiverID).HasName("PK_dbo.PayerReceiverMaster");
                entity.ToTable("PayerReceiverMaster");
                entity.Property(e => e.PayerReceiverID).HasColumnName("PayerReceiverID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.FacilityID).HasColumnName("FacilityID");
                entity.Property(e => e.InsuranceClassificationID).HasColumnName("InsuranceClassificationID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.PayerReceiverName).HasColumnName("PayerReceiver").IsRequired().HasMaxLength(450);
                entity.Property(e => e.PayerReceiverIdentification).HasMaxLength(100);
                entity.Property(e => e.PayerReceiverIdentificationValidate).HasMaxLength(100);
                entity.Property(e => e.PayerReceiverShortName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ReceiverID).HasColumnName("ReceiverID").HasMaxLength(450);
                entity.Property(e => e.RecieverFacilityID).HasColumnName("RecieverFacilityID");
                entity.Property(e => e.RegulatoryID).HasColumnName("RegulatoryID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.payerReceiverCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.PayerReceiverMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                   .WithMany(p => p.payerReceiverModifiedList)
                   .HasForeignKey(d => d.ModifiedBy)
                   .HasConstraintName("FK_dbo.PayerReceiverMaster_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.facilityEntity)
                    .WithMany(p => p.payerReceiverList)
                    .HasForeignKey(d => d.FacilityID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.PayerReceiverMaster_dbo.FacilityMaster_FacilityID");

                entity.HasOne(d => d.insuranceClassificationEntity)
                    .WithMany(p => p.payerReceiverEntity)
                    .HasForeignKey(d => d.InsuranceClassificationID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.PayerReceiverMaster_dbo.InsuranceClassificationMaster_InsuranceClassificationID");

                entity.HasOne(d => d.regulatoryEntity)
                    .WithMany(p => p.payerReceiverList)
                    .HasForeignKey(d => d.RegulatoryID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.PayerReceiverMaster_dbo.RegulatoryMaster_RegulatoryID");
            });
            modelBuilder.Entity<InsuranceClassificationEntity>(entity =>
            {
                entity.HasKey(e => e.InsuranceClassificationId).HasName("PK_dbo.InsuranceClassificationMaster");
                entity.ToTable("InsuranceClassificationMaster");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.InsuranceClassification).HasName("IX_InsuranceClassification").IsUnique();
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");
                entity.Property(e => e.InsuranceClassificationId).HasColumnName("InsuranceClassificationID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.InsuranceClassification).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.insuranceClassificationEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.InsuranceClassificationMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.insuranceClassificationEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.InsuranceClassificationMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<RegulatoryEntity>(entity =>
            {
                entity.HasKey(e => e.RegulatoryID).HasName("PK_dbo.RegulatoryMaster");
                entity.ToTable("RegulatoryMaster");
                entity.Property(e => e.RegulatoryID).HasColumnName("RegulatoryID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RegulatoryName).HasColumnName("Regulatory").IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.regulatoryEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.RegulatoryMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.regulatoryEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.RegulatoryMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<AddressEntity>(entity =>
            {
                entity.HasKey(e => e.AddressID).HasName("PK_dbo.AddressMaster");
                entity.ToTable("AddressMaster");
                entity.Property(e => e.AddressID).HasColumnName("AddressID");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).HasColumnName("Phone");
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Mobile).HasColumnName("Mobile");
                entity.Property(e => e.Mobile).HasMaxLength(15);
                entity.Property(e => e.Fax).HasColumnName("Fax");
                entity.Property(e => e.FullAddress).HasColumnName("FullAddress");
                entity.Property(e => e.FullAddress).HasMaxLength(500);
                entity.Property(e => e.StreetName).HasColumnName("StreetName");
                entity.Property(e => e.StreetName).HasMaxLength(100);
                entity.Property(e => e.CityName).HasColumnName("CityName");
                entity.Property(e => e.CityName).HasMaxLength(100);
                entity.Property(e => e.CountryID).HasColumnName("CountryID");
                entity.Property(e => e.StateID).HasColumnName("StateID");
                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.HasIndex(e => e.Email).HasName("IX_Email").IsUnique();
                entity.HasIndex(e => e.Phone).HasName("IX_Phone");
                entity.HasIndex(e => e.Mobile).HasName("IX_Mobile");
                entity.HasIndex(e => e.StreetName).HasName("IX_StreetName");
                entity.HasIndex(e => e.CityName).HasName("IX_CityName");
                entity.HasIndex(e => e.CountryID).HasName("IX_CountryID");
                entity.HasIndex(e => e.StateID).HasName("IX_StateID");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");

                entity.HasOne(d => d.countryEntity)
                  .WithMany(p => p.addressEntityList)
                  .HasForeignKey(d => d.CountryID)
                  .HasConstraintName("FK_dbo.AddressMaster_dbo.CountryMaster_CountryID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.addressEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_dbo.AddressMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.addressEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.AddressMaster_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.stateEntity)
                    .WithMany(p => p.addressEntityList)
                    .HasForeignKey(d => d.StateID)
                    .HasConstraintName("FK_dbo.AddressMaster_dbo.StateMaster_StateID");
            });
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.UserID).HasName("PK_dbo.UserMaster");
                entity.ToTable("UserMaster");
                entity.Property(e => e.UserID).HasColumnName("UserID");
                entity.HasIndex(e => e.UserName).HasName("IX_UserName");
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AddressID).HasColumnName("AddressID");
                entity.Property(e => e.CompanyRoleID).HasColumnName("CompanyRoleID");
                entity.Property(e => e.IsSignUp).HasColumnName("IsSignUp");
                entity.Property(e => e.SignUpStatus).HasColumnName("SignUpStatus");
                entity.Property(e => e.AuthorityType).HasColumnName("AuthorityType");
                entity.Property(e => e.RuleVersion).HasColumnName("RuleVersion");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Reason).HasColumnName("Reason").HasMaxLength(4000);
                entity.Property(e => e.ClientFacilityCode).HasColumnName("ClientFacilityCode").HasMaxLength(100);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Password).IsRequired().HasMaxLength(250);
                entity.Property(e => e.UserTypeID).HasColumnName("UserTypeID");



                entity.HasOne(d => d.addressEntity)
                    .WithMany(p => p.userEntityList)
                    .HasForeignKey(d => d.AddressID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.UserMaster_dbo.AddressMaster_AddressID");

                entity.HasOne(d => d.companyRoleEntity)
                    .WithMany(p => p.userEntityList)
                    .HasForeignKey(d => d.CompanyRoleID)
                    .HasConstraintName("FK_dbo.UserMaster_dbo.CompanyRoleMaster_CompanyRoleID");

                entity.HasOne(d => d.userEntityParentCreated)
               .WithMany(p => p.userEntityParentCreatedList)
               .HasForeignKey(d => d.CreatedBy)
               .HasConstraintName("FK_dbo.UserMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityParentModified)
                    .WithMany(p => p.userEntityParentModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.UserMaster_dbo.UserMaster_ModifiedBy");

            });
            modelBuilder.Entity<ControlEntity>(entity =>
            {
                entity.HasKey(e => e.ControlId).HasName("PK_dbo.ControlMaster");
                entity.ToTable("ControlMaster");
                entity.Property(e => e.ControlId).HasColumnName("ControlID");
                entity.Property(e => e.ControlName).HasMaxLength(200);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.controlEntityList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.ControlMaster_dbo.UserMaster_CreatedBy");
            });
            modelBuilder.Entity<OrganizationEntity>(entity =>
            {
                entity.HasKey(e => e.OrganizationID).HasName("PK_dbo.OrganizationMaster");
                entity.ToTable("OrganizationMaster");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");
                entity.HasIndex(e => e.OrganizationName).HasName("IX_OrganizationName").IsUnique();
                entity.Property(e => e.OrganizationID).HasColumnName("OrganizationID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.OrganizationName).HasMaxLength(100);
                entity.Property(e => e.SubscriptionEndDate).HasColumnType("datetime");
                entity.Property(e => e.SubscriptionStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.organizationEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.OrganizationMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.organizationEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.OrganizationMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<CompanyEntity>(entity =>
            {
                entity.HasKey(e => e.CompanyID).HasName("PK_dbo.CompanyMaster");
                entity.ToTable("CompanyMaster");
                entity.Property(e => e.CompanyID).HasColumnName("CompanyID");
                entity.Property(e => e.CompanyName).HasColumnName("CompanyName");
                entity.Property(e => e.CompanyLicenseToken).HasMaxLength(100);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CompanyWiseLandingCode).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.LicenseEndDate).HasColumnType("datetime");
                entity.Property(e => e.LicenseStartDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.MrngenerationTypeId).HasColumnName("MRNGenerationTypeID");
                entity.Property(e => e.NotificationFromEmailPassword).HasMaxLength(200);
                entity.Property(e => e.NotificationFromEmailUserName).HasMaxLength(200);
                entity.Property(e => e.TopNotificationEmails).HasMaxLength(2000);
                entity.Property(e => e.ViscoreLicenseTypeId).HasColumnName("ViscoreLicenseTypeID");
                //entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                //entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.companyEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.companyEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.CompanyMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<GroupControlEntity>(entity =>
            {
                entity.HasKey(e => e.GroupControlId).HasName("PK_dbo.GroupControlTrans");
                entity.ToTable("GroupControlTrans");
                entity.Property(e => e.GroupControlId).HasColumnName("GroupControlID");
                entity.Property(e => e.ControlId).HasColumnName("ControlID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.controlEntity)
                    .WithMany(p => p.groupControlEntityList)
                    .HasForeignKey(d => d.ControlId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.GroupControlTrans_dbo.ControlMaster_ControlID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.GroupControlEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.GroupControlTrans_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.groupEntity)
                    .WithMany(p => p.groupControlEntityList)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.GroupControlTrans_dbo.GroupMaster_GroupID");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.GroupControlEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.GroupControlTrans_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<GroupEntity>(entity =>
            {
                entity.HasKey(e => e.GroupId).HasName("PK_dbo.GroupMaster");
                entity.ToTable("GroupControlTrans");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.GroupClass).HasMaxLength(150);
                entity.Property(e => e.GroupName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.ParentGroupId).HasColumnName("ParentGroupID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.groupEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.GroupMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.groupEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.GroupMaster_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.groupEntityParent)
                    .WithMany(p => p.goupEntityParentList)
                    .HasForeignKey(d => d.ParentGroupId)
                    .HasConstraintName("FK_dbo.GroupMaster_dbo.GroupMaster_ParentGroupID");
            });
            modelBuilder.Entity<CompanyRoleFacilityEntity>(entity =>
            {
                entity.HasKey(e => e.CompanyRoleFacilityId).HasName("PK_dbo.CompanyRoleFacilityTrans");
                entity.Property(e => e.CompanyRoleFacilityId).HasColumnName("CompanyRoleFacilityTransID");
                entity.ToTable("CompanyRoleFacilityTrans");
                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
                entity.Property(e => e.CompanyRoleId).HasColumnName("CompanyRoleID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.companyEntity)
                    .WithMany(p => p.CompanyRoleFacilityEntityList)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyRoleFacilityTrans_dbo.CompanyMaster_CompanyID");

                entity.HasOne(d => d.companyRoleEntity)
                    .WithMany(p => p.companyRoleFacilityEntityList)
                    .HasForeignKey(d => d.CompanyRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyRoleFacilityTrans_dbo.CompanyRoleMaster_CompanyRoleID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.companyRoleFacilityEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyRoleFacilityTrans_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                     .WithMany(p => p.companyRoleFacilityEntityModifiedList)
                     .HasForeignKey(d => d.ModifiedBy)
                     .HasConstraintName("FK_dbo.CompanyRoleFacilityTrans_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.facilityEntity)
                    .WithMany(p => p.companyRoleFacilityEntityList)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyRoleFacilityTrans_dbo.FacilityMaster_FacilityID");
            });
            modelBuilder.Entity<RoleGroupEntity>(entity =>
            {
                entity.HasKey(e => e.RoleGroupId).HasName("PK_dbo.RoleGroupTrans");
                entity.ToTable("RoleGroupTrans");
                entity.Property(e => e.RoleGroupId).HasColumnName("RoleGroupID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.roleGroupEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.RoleGroupTrans_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.roleGroupEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.RoleGroupTrans_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.groupEntity)
                    .WithMany(p => p.roleGroupEntityList)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.RoleGroupTrans_dbo.GroupMaster_GroupID");

                entity.HasOne(d => d.companyRoleEntity)
                    .WithMany(p => p.roleGroupEntityList)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.RoleGroupTrans_dbo.CompanyRoleMaster_RoleID");
            });
            modelBuilder.Entity<CompanyRoleEntity>(entity =>
            {
                entity.HasKey(e => e.CompanyRoleId).HasName("PK_dbo.CompanyRoleMaster");
                entity.ToTable("CompanyRoleMaster");
                entity.Property(e => e.CompanyRoleId).HasColumnName("CompanyRoleID");
                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
                entity.Property(e => e.CompanyRole).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.companyEntity)
                    .WithMany(p => p.companyRoleEntityList)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyRoleMaster_dbo.CompanyMaster_CompanyID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.companyRoleEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CompanyRoleMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.companyRoleEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.CompanyRoleMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<ActivityRulesEntity>(entity =>
            {
                entity.HasKey(e => e.ActivityRulesID).HasName("PK_dbo.ScrubberActivityRulesMaster");
                entity.ToTable("ScrubberActivityRulesMaster");
                entity.HasIndex(e => e.ActivityType).HasName("IX_ActivityType");
                entity.HasIndex(e => e.AgeMax).HasName("IX_AgeMax");
                entity.HasIndex(e => e.AgeMin).HasName("IX_AgeMin");
                entity.Property(e => e.ActivityMax).HasColumnName("ActivityMax");
                entity.Property(e => e.ActivityClinicMax).HasColumnName("ActivityClinicMax");
                entity.Property(e => e.ActivityRulesID).HasColumnName("ScrubberActivityRulesID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.GroupID).HasColumnName("GroupID");
                entity.Property(e => e.GroupCount).HasColumnName("GroupCount");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.Message).HasMaxLength(2000);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RuleActivity).HasMaxLength(100);
                entity.Property(e => e.RulesID).HasColumnName("ScrubberRulesID");

                entity.HasOne(d => d.userEntityCreated)
                     .WithMany(p => p.activityRulesEntityCreatedList)
                     .HasForeignKey(d => d.CreatedBy)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_dbo.ScrubberActivityRulesMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.activityRulesEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.ScrubberActivityRulesMaster_dbo.UserMaster_ModifiedBy");

                entity.HasOne(d => d.rulesEntity)
                    .WithMany(p => p.activityRulesEntityList)
                    .HasForeignKey(d => d.ActivityRulesID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.ScrubberActivityRulesMaster_dbo.ScrubberRulesMaster_ScrubberRulesID");
            });
            modelBuilder.Entity<CountryEntity>(entity =>
            {
                entity.HasKey(e => e.CountryId).HasName("PK_dbo.CountryMaster");
                entity.ToTable("CountryMaster");
                entity.Property(e => e.CountryId).HasColumnName("CountryID");
                entity.Property(e => e.Country).IsRequired().HasMaxLength(150);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.countryEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.CountryMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.countryEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.CountryMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<RulesEntity>(entity =>
            {
                entity.HasKey(e => e.RulesID).HasName("PK_dbo.ScrubberRulesMaster");
                entity.ToTable("ScrubberRulesMaster");
                entity.HasIndex(e => e.IsAUHRule).HasName("IX_IsAUHRule");
                entity.HasIndex(e => e.IsBothRule).HasName("IX_IsBothRule");
                entity.HasIndex(e => e.IsDOSRule).HasName("IX_IsDOSRule");
                entity.HasIndex(e => e.VStatus).HasName("IX_VStatus");
                entity.Property(e => e.RulesID).HasColumnName("ScrubberRulesID");
                entity.Property(e => e.RuleName).HasColumnName("RuleName");
                entity.Property(e => e.ScrubberErrorCategory).HasColumnName("ScrubberErrorCategory");
                entity.Property(e => e.ScrubberPrefixType).HasColumnName("ScrubberPrefixType");
                entity.Property(e => e.AuthorityType).HasColumnName("AuthorityType");
                entity.Property(e => e.RuleVersion).HasColumnName("RuleVersion");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RulePrefix).HasMaxLength(450);
                entity.Property(e => e.RulePXID).HasColumnName("RulePXID").HasMaxLength(25);
                entity.Property(e => e.Classification).HasColumnName("Classification").HasMaxLength(15);
                entity.Property(e => e.CodingTips).HasColumnName("CodingTips");
                entity.Property(e => e.PayerIDS).HasColumnName("PayerIDS");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.rulesEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.ScrubberRulesMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.rulesEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.ScrubberRulesMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<StateEntity>(entity =>
            {
                entity.HasKey(e => e.StateId).HasName("PK_dbo.StateMaster");
                entity.ToTable("StateMaster");
                entity.Property(e => e.StateId).HasColumnName("StateID");
                entity.Property(e => e.CountryId).HasColumnName("CountryID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.State).IsRequired().HasMaxLength(150);
                /*
                entity.HasOne(d => d.Country)
                    .WithMany(p => p.StateMaster)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.StateMaster_dbo.CountryMaster_CountryID");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.StateMasterCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.StateMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.StateMasterModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.StateMaster_dbo.UserMaster_ModifiedBy");*/
            });
            modelBuilder.Entity<GroupEntity>(entity =>
            {
                entity.HasKey(e => e.GroupId).HasName("PK_dbo.GroupMaster");
                entity.ToTable("GroupMaster");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.GroupClass).HasMaxLength(150);
                entity.Property(e => e.GroupName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.ParentGroupId).HasColumnName("ParentGroupID");

                /* entity.HasOne(d => d.CreatedByNavigation)
                     .WithMany(p => p.GroupMasterCreatedByNavigation)
                     .HasForeignKey(d => d.CreatedBy)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_dbo.GroupMaster_dbo.UserMaster_CreatedBy");

                 entity.HasOne(d => d.ModifiedByNavigation)
                     .WithMany(p => p.GroupMasterModifiedByNavigation)
                     .HasForeignKey(d => d.ModifiedBy)
                     .HasConstraintName("FK_dbo.GroupMaster_dbo.UserMaster_ModifiedBy");

                 entity.HasOne(d => d.ParentGroup)
                     .WithMany(p => p.InverseParentGroup)
                     .HasForeignKey(d => d.ParentGroupId)
                     .HasConstraintName("FK_dbo.GroupMaster_dbo.GroupMaster_ParentGroupID");*/
            });
            modelBuilder.Entity<FacilityCategoryEntity>(entity =>
            {
                entity.HasKey(e => e.FacilityCategoryId).HasName("PK_dbo.FacilityCategoryMaster");
                entity.ToTable("FacilityCategoryMaster");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.FacilityCategory).HasName("IX_FacilityCategory").IsUnique();
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");
                entity.Property(e => e.FacilityCategoryId).HasColumnName("FacilityCategoryID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.FacilityCategory).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.facilityCategoryEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.FacilityCategoryMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.facilityCategoryEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.FacilityCategoryMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<FacilityTypeEntity>(entity =>
            {
                entity.HasKey(e => e.FacilityTypeID).HasName("PK_dbo.FacilityTypeMaster");
                entity.ToTable("FacilityTypeMaster");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.FacilityCategoryID).HasName("IX_FacilityCategoryID");
                entity.HasIndex(e => e.FacilityTypeName).HasName("IX_FacilityType").IsUnique();
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");
                entity.Property(e => e.FacilityTypeName).HasColumnName("FacilityType");
                entity.Property(e => e.FacilityTypeID).HasColumnName("FacilityTypeID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.FacilityCategoryID).HasColumnName("FacilityCategoryID");
                entity.Property(e => e.FacilityTypeName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.facilityTypeEntitCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.FacilityTypeMaster_dbo.UserMaster_CreatedBy");

                /*  entity.HasOne(d => d.FacilityCategory)
                      .WithMany(p => p.FacilityTypeMaster)
                      .HasForeignKey(d => d.FacilityCategoryId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_dbo.FacilityTypeMaster_dbo.FacilityCategoryMaster_FacilityCategoryID");*/

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.facilityTypeEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.FacilityTypeMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<StateEntity>(entity =>
            {
                entity.HasKey(e => e.StateId).HasName("PK_dbo.StateMaster");
                entity.ToTable("StateMaster");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");
                entity.HasIndex(e => new { e.State, e.CountryId }).HasName("IX_State_CountryID").IsUnique();
                entity.Property(e => e.StateId).HasColumnName("StateID");
                entity.Property(e => e.CountryId).HasColumnName("CountryID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.State).IsRequired().HasMaxLength(150);

                entity.HasOne(d => d.countryEntity)
                    .WithMany(p => p.stateEntityList)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.StateMaster_dbo.CountryMaster_CountryID");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.stateEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.StateMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.stateEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.StateMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<EncounterTypeEntity>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("PK_dbo.EncounterTypeMaster");
                entity.ToTable("EncounterTypeMaster");
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.EncounterTypeID).HasColumnName("EncounterType").IsRequired();
                entity.Property(e => e.EncounterType).HasColumnName("EncounterType").IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.userEntityCreated)
                     .WithMany(p => p.encounterTypeEntityCreatedList)
                     .HasForeignKey(d => d.CreatedBy)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_dbo.EncounterTypeMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.encounterTypeEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.EncounterTypeMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<EncounterTypeEntity>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("PK_dbo.EncounterTypeMaster");
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.ToTable("EncounterTypeMaster");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.EncounterType).IsRequired().HasMaxLength(200);
                entity.Property(e => e.EncounterTypeID).HasColumnName("EncounterTypeID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.HasIndex(e => e.CreatedBy).HasName("IX_CreatedBy");
                entity.HasIndex(e => e.EncounterType).HasName("IX_EncounterType").IsUnique();
                entity.HasIndex(e => e.EncounterTypeID).HasName("IX_EncounterTypeID").IsUnique();
                entity.HasIndex(e => e.ModifiedBy).HasName("IX_ModifiedBy");

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.encounterTypeEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.EncounterTypeMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.encounterTypeEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.EncounterTypeMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<ValidatorTransactionEntity>(entity =>
            {
                entity.HasKey(e => e.ValidatorTransactionID).HasName("PK_dbo.ValidatorTransactionMaster");
                entity.ToTable("ValidatorTransactionMaster");
                entity.Property(e => e.Status).HasColumnName("Status");
            });
            modelBuilder.Entity<ValidatorICDEntity>(entity =>
            {
                entity.HasKey(e => new { e.ValidatorICDID, e.ValidatorTransactionID }).HasName("PK_dbo.ValidatorICDMaster");
                entity.ToTable("ValidatorICDMaster");
                entity.Property(e => e.ICD).HasMaxLength(200);
                entity.Property(e => e.Primary).HasColumnName("Primary");
                entity.Property(e => e.Secondary).HasColumnName("Secondary");
                entity.Property(e => e.ReasonForVisit).HasColumnName("ReasonForVisit");
                entity.Property(e => e.Gender).HasColumnName("Gender");
                entity.Property(e => e.Age).HasColumnName("Age");
                entity.Property(e => e.Status).HasColumnName("Status");
            });
            modelBuilder.Entity<ValidatorCPTEntity>(entity =>
            {
                entity.HasKey(e => new { e.ValidatorCPTID, e.ValidatorTransactionID }).HasName("PK_dbo.ValidatorCPTMaster");
                entity.ToTable("ValidatorCPTMaster");
                entity.Property(e => e.CPT).HasMaxLength(200);
                entity.Property(e => e.Quantity).HasColumnName("Quantity");
                entity.Property(e => e.ActivityType).HasColumnName("ActivityType");
                entity.Property(e => e.Net).HasColumnName("Net");
                entity.Property(e => e.Gender).HasColumnName("Gender");
                entity.Property(e => e.Age).HasColumnName("Age");
                entity.Property(e => e.Status).HasColumnName("Status");
            });
            modelBuilder.Entity<ActivityEntity>(entity =>
            {
                entity.HasKey(e => new { e.ActivityID }).HasName("PK_dbo.ActivityMaster");
                entity.ToTable("ActivityMaster");
                entity.Property(e => e.ActivityName).HasMaxLength(100);
                entity.Property(e => e.ActivityNumber).HasColumnName("ActivityNumber");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.HasOne(d => d.userEntityCreated)
                   .WithMany(p => p.activityEntityCreatedList)
                   .HasForeignKey(d => d.CreatedBy)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_dbo.ActivityMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.activityEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.ActivityMaster_dbo.UserMaster_ModifiedBy");
            });
            modelBuilder.Entity<SettingsEntity>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("PK_dbo.SettingsMaster");
                entity.ToTable("SettingsMaster");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.ProjectConstantDBID).HasColumnName("ProjectConstantDBID").HasMaxLength(100);
                entity.Property(e => e.ProjectConstantID).HasColumnName("ProjectConstantID");
                entity.Property(e => e.ProjectConstantName).HasMaxLength(100);

                entity.HasOne(d => d.userEntityCreated)
                    .WithMany(p => p.settingsEntityCreatedList)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.SettingsMaster_dbo.UserMaster_CreatedBy");

                entity.HasOne(d => d.userEntityModified)
                    .WithMany(p => p.settingsEntityModifiedList)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_dbo.SettingsMaster_dbo.UserMaster_ModifiedBy");
            });

            modelBuilder.Entity<UserHistoryEntity>(entity =>
            {
                entity.HasKey(e => e.UserHistoryID).HasName("PK_dbo.UserHistoryMaster");
                entity.ToTable("UserHistoryMaster");
                entity.Property(e => e.LoginSessionID).HasMaxLength(50);
                entity.Property(e => e.LoginIPAddress).HasMaxLength(50);
                entity.Property(e => e.Browser).HasMaxLength(200);
                entity.Property(e => e.Platform).HasMaxLength(200);

                entity.HasOne(d => d.userEntity)
                   .WithMany(p => p.userHistoryEntityList)
                   .HasForeignKey(d => d.LoginUserID)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_dbo.UserHistoryMaster_dbo.UserMaster_LoginUserID");

                entity.HasOne(d => d.companyRoleEntity)
                    .WithMany(p => p.userHistoryEntityList)
                    .HasForeignKey(d => d.LoginRoleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.UserHistoryMaster_dbo.CompanyRoleMaster_LoginRoleID");

            });
        }
    }

}