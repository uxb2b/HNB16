using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ModelCore.DataModel;

public partial class LcEntityDbContext : DbContext
{
    public LcEntityDbContext(DbContextOptions<LcEntityDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlertDataQueue> AlertDataQueue { get; set; }

    public virtual DbSet<AmendingLcApplication> AmendingLcApplication { get; set; }

    public virtual DbSet<AmendingLcInformation> AmendingLcInformation { get; set; }

    public virtual DbSet<AmendingLcRegistry> AmendingLcRegistry { get; set; }

    public virtual DbSet<ApLogCategory> ApLogCategory { get; set; }

    public virtual DbSet<ApplicantBeneficiaryPair> ApplicantBeneficiaryPair { get; set; }

    public virtual DbSet<AttachableDocument> AttachableDocument { get; set; }

    public virtual DbSet<BankData> BankData { get; set; }

    public virtual DbSet<BankInbox> BankInbox { get; set; }

    public virtual DbSet<BankUser> BankUser { get; set; }

    public virtual DbSet<BankUserBranch> BankUserBranch { get; set; }

    public virtual DbSet<BankUserDeputy> BankUserDeputy { get; set; }

    public virtual DbSet<BankUserLogin> BankUserLogin { get; set; }

    public virtual DbSet<BankUserRole> BankUserRole { get; set; }

    public virtual DbSet<BeneficiaryData> BeneficiaryData { get; set; }

    public virtual DbSet<BeneficiaryGroup> BeneficiaryGroup { get; set; }

    public virtual DbSet<BeneficiaryServiceGroup> BeneficiaryServiceGroup { get; set; }

    public virtual DbSet<BeneficiaryTransferInto> BeneficiaryTransferInto { get; set; }

    public virtual DbSet<CommonAppLog> CommonAppLog { get; set; }

    public virtual DbSet<CommonCALog> CommonCALog { get; set; }

    public virtual DbSet<CreditApplicationDocumentary> CreditApplicationDocumentary { get; set; }

    public virtual DbSet<CreditCancellation> CreditCancellation { get; set; }

    public virtual DbSet<CreditCancellationInfo> CreditCancellationInfo { get; set; }

    public virtual DbSet<CreditLoan> CreditLoan { get; set; }

    public virtual DbSet<CurrencyType> CurrencyType { get; set; }

    public virtual DbSet<CustomerInbox> CustomerInbox { get; set; }

    public virtual DbSet<CustomerOfBranch> CustomerOfBranch { get; set; }

    public virtual DbSet<CustomerOfBranchVersion> CustomerOfBranchVersion { get; set; }

    public virtual DbSet<DataPortLog> DataPortLog { get; set; }

    public virtual DbSet<DataProcessLog> DataProcessLog { get; set; }

    public virtual DbSet<DefaultSpecificNotes> DefaultSpecificNotes { get; set; }

    public virtual DbSet<DisabledBranch> DisabledBranch { get; set; }

    public virtual DbSet<DocumentDispatch> DocumentDispatch { get; set; }

    public virtual DbSet<DocumentOwner> DocumentOwner { get; set; }

    public virtual DbSet<DocumentProcessLog> DocumentProcessLog { get; set; }

    public virtual DbSet<DocumentType> DocumentType { get; set; }

    public virtual DbSet<Documentary> Documentary { get; set; }

    public virtual DbSet<DocumentaryAllowance> DocumentaryAllowance { get; set; }

    public virtual DbSet<DocumentaryDenial> DocumentaryDenial { get; set; }

    public virtual DbSet<DocumentaryLevel> DocumentaryLevel { get; set; }

    public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }

    public virtual DbSet<FpgBeneficiaryStatus> FpgBeneficiaryStatus { get; set; }

    public virtual DbSet<FpgLcItem> FpgLcItem { get; set; }

    public virtual DbSet<FpgLcMonthlyReport> FpgLcMonthlyReport { get; set; }

    public virtual DbSet<FpgLcMonthlySummary> FpgLcMonthlySummary { get; set; }

    public virtual DbSet<FpgNegoDraft> FpgNegoDraft { get; set; }

    public virtual DbSet<FpgNegoRemittance> FpgNegoRemittance { get; set; }

    public virtual DbSet<FpgNegoRemittanceDispatch> FpgNegoRemittanceDispatch { get; set; }

    public virtual DbSet<FpgNegoRemittanceLog> FpgNegoRemittanceLog { get; set; }

    public virtual DbSet<GoodsDetail> GoodsDetail { get; set; }

    public virtual DbSet<GroupDepartment> GroupDepartment { get; set; }

    public virtual DbSet<GuaranteeDeposit> GuaranteeDeposit { get; set; }

    public virtual DbSet<LcItems> LcItems { get; set; }

    public virtual DbSet<LetterOfCredit> LetterOfCredit { get; set; }

    public virtual DbSet<LetterOfCreditExtension> LetterOfCreditExtension { get; set; }

    public virtual DbSet<LetterOfCreditVersion> LetterOfCreditVersion { get; set; }

    public virtual DbSet<LevelExpression> LevelExpression { get; set; }

    public virtual DbSet<NegoAffair> NegoAffair { get; set; }

    public virtual DbSet<NegoDraft> NegoDraft { get; set; }

    public virtual DbSet<NegoDraftAcceptance> NegoDraftAcceptance { get; set; }

    public virtual DbSet<NegoDraftDeal> NegoDraftDeal { get; set; }

    public virtual DbSet<NegoDraftExtension> NegoDraftExtension { get; set; }

    public virtual DbSet<NegoHost> NegoHost { get; set; }

    public virtual DbSet<NegoInvoice> NegoInvoice { get; set; }

    public virtual DbSet<NegoInvoiceDetail> NegoInvoiceDetail { get; set; }

    public virtual DbSet<NegoLC> NegoLC { get; set; }

    public virtual DbSet<NegoLoan> NegoLoan { get; set; }

    public virtual DbSet<NegoPrompt> NegoPrompt { get; set; }

    public virtual DbSet<NegoPromptRequestQueue> NegoPromptRequestQueue { get; set; }

    public virtual DbSet<Organization> Organization { get; set; }

    public virtual DbSet<OrganizationBranchSettings> OrganizationBranchSettings { get; set; }

    public virtual DbSet<OrganizationExtension> OrganizationExtension { get; set; }

    public virtual DbSet<OrganizationStatus> OrganizationStatus { get; set; }

    public virtual DbSet<ReceivedDataQueue> ReceivedDataQueue { get; set; }

    public virtual DbSet<ResponseDataQueue> ResponseDataQueue { get; set; }

    public virtual DbSet<RevisionLog> RevisionLog { get; set; }

    public virtual DbSet<ServiceDataQueue> ServiceDataQueue { get; set; }

    public virtual DbSet<SpecificNotes> SpecificNotes { get; set; }

    public virtual DbSet<UserRole> UserRole { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlertDataQueue>(entity =>
        {
            entity.HasKey(e => e.DataPortLogID);

            entity.Property(e => e.DataPortLogID).ValueGeneratedNever();

            entity.HasOne(d => d.DataPortLog).WithOne(p => p.AlertDataQueue)
                .HasForeignKey<AlertDataQueue>(d => d.DataPortLogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlertDataQueue_DataPortLog");
        });

        modelBuilder.Entity<AmendingLcApplication>(entity =>
        {
            entity.HasKey(e => e.DocumentaryID);

            entity.HasIndex(e => e.ApplicationDate, "IX_AmendingLcApplication");

            entity.HasIndex(e => e.AmendmentNo, "IX_AmendingLcApplication_1");

            entity.Property(e => e.DocumentaryID).ValueGeneratedNever();
            entity.Property(e => e.AmendmentNo).HasMaxLength(64);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AttachableDocumentID).HasComment("參考開狀申請檢附文件");
            entity.Property(e => e.FileName)
                .HasMaxLength(64)
                .HasComment("CDS修狀申請之申請書檔名");
            entity.Property(e => e.Instruction)
                .HasMaxLength(512)
                .HasComment("非本行制式特別指示之申請原因及依據");
            entity.Property(e => e.IsAccepted).HasComment("修狀受益人已接受\r\n1:是\r\n其它:否");
            entity.Property(e => e.LcItemsID).HasComment("參考金額、日期申請資料");
            entity.Property(e => e.OverTheCounter).HasComment("註記臨櫃申請\r\n1:是\r\n0或NULL:否");
            entity.Property(e => e.PrintNotice).HasComment("已列修狀通知書次數");
            entity.Property(e => e.SourceID).HasComment("參考修狀參照到的版本來源，可以是原開狀申請書或前一次修狀申請書");
            entity.Property(e => e.SpecificNotesID).HasComment("參考特別指示條款");
            entity.Property(e => e.SysDocID)
                .HasMaxLength(32)
                .HasComment("Deprecated");
            entity.Property(e => e.TransactionMessage)
                .HasMaxLength(64)
                .HasComment("Deprecated");

            entity.HasOne(d => d.AttachableDocument).WithMany(p => p.AmendingLcApplication)
                .HasForeignKey(d => d.AttachableDocumentID)
                .HasConstraintName("FK_AmendingLcApplication_AttachableDocument");

            entity.HasOne(d => d.Documentary).WithOne(p => p.AmendingLcApplication)
                .HasForeignKey<AmendingLcApplication>(d => d.DocumentaryID)
                .HasConstraintName("FK_AmendingLcApplication_Documentary");

            entity.HasOne(d => d.LcItems).WithMany(p => p.AmendingLcApplication)
                .HasForeignKey(d => d.LcItemsID)
                .HasConstraintName("FK_AmendingLcApplication_LcItems");

            entity.HasOne(d => d.Source).WithMany(p => p.AmendingLcApplication)
                .HasPrincipalKey(p => p.VersionID)
                .HasForeignKey(d => d.SourceID)
                .HasConstraintName("FK_AmendingLcApplication_LetterOfCreditVersion");

            entity.HasOne(d => d.SpecificNotes).WithMany(p => p.AmendingLcApplication)
                .HasForeignKey(d => d.SpecificNotesID)
                .HasConstraintName("FK_AmendingLcApplication_SpecificNotes");
        });

        modelBuilder.Entity<AmendingLcInformation>(entity =>
        {
            entity.HasKey(e => e.AmendingLcApplicationID);

            entity.Property(e => e.AmendingLcApplicationID).ValueGeneratedNever();
            entity.Property(e => e.AmendingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InformationNo)
                .IsRequired()
                .HasMaxLength(32);

            entity.HasOne(d => d.AmendingLcApplication).WithOne(p => p.AmendingLcInformation)
                .HasForeignKey<AmendingLcInformation>(d => d.AmendingLcApplicationID)
                .HasConstraintName("FK_AmendingLcInformation_AmendingLcApplication");
        });

        modelBuilder.Entity<AmendingLcRegistry>(entity =>
        {
            entity.HasKey(e => e.RegistrationID);

            entity.Property(e => e.交易憑證編號).HasMaxLength(32);
            entity.Property(e => e.作業資訊組負責人).HasMaxLength(20);
            entity.Property(e => e.匯率).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.增加信用狀記帳金額).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.延長信用狀原因).HasMaxLength(24);
            entity.Property(e => e.延長匯票期限原因).HasMaxLength(24);
            entity.Property(e => e.改狀手續費).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.改狀費金額).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.放款作業專員).HasMaxLength(20);
            entity.Property(e => e.沖銷原因).HasMaxLength(24);
            entity.Property(e => e.沖銷存入保證金金額).HasColumnType("decimal(18, 4)");
        });

        modelBuilder.Entity<ApLogCategory>(entity =>
        {
            entity.HasKey(e => e.CODE_KEY).HasName("PK_AP_LOG_CATEGORY");

            entity.Property(e => e.CODE_KEY).HasMaxLength(32);
            entity.Property(e => e.CODE_VALUE).HasMaxLength(64);
            entity.Property(e => e.MEMO1).HasMaxLength(256);
            entity.Property(e => e.MEMO2).HasMaxLength(256);
        });

        modelBuilder.Entity<ApplicantBeneficiaryPair>(entity =>
        {
            entity.HasKey(e => new { e.ApplicantID, e.BeneID });

            entity.Property(e => e.BeneficiaryChiefEmail).HasMaxLength(64);
            entity.Property(e => e.BeneficiaryChiefTitle).HasMaxLength(64);

            entity.HasOne(d => d.Applicant).WithMany(p => p.ApplicantBeneficiaryPair)
                .HasForeignKey(d => d.ApplicantID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicantBeneficiaryPair_Organization");

            entity.HasOne(d => d.Bene).WithMany(p => p.ApplicantBeneficiaryPair)
                .HasForeignKey(d => d.BeneID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicantBeneficiaryPair_BeneficiaryData");
        });

        modelBuilder.Entity<AttachableDocument>(entity =>
        {
            entity.HasKey(e => e.AttachmentID);

            entity.Property(e => e.其他).HasMaxLength(256);
        });

        modelBuilder.Entity<BankData>(entity =>
        {
            entity.HasKey(e => e.BankCode)
                .HasName("PK0")
                .IsClustered(false);

            entity.Property(e => e.BankCode).HasMaxLength(10);
            entity.Property(e => e.Address).HasMaxLength(64);
            entity.Property(e => e.BranchName).HasMaxLength(64);
            entity.Property(e => e.CRC_Branch).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(24);
        });

        modelBuilder.Entity<BankInbox>(entity =>
        {
            entity.HasKey(e => e.MsgID);

            entity.Property(e => e.BankCode).HasMaxLength(10);
            entity.Property(e => e.MsgContent).HasMaxLength(50);
            entity.Property(e => e.MsgDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BankCodeNavigation).WithMany(p => p.BankInbox)
                .HasForeignKey(d => d.BankCode)
                .HasConstraintName("FK_BankInbox_BankData");

            entity.HasOne(d => d.Documentary).WithMany(p => p.BankInbox)
                .HasForeignKey(d => d.DocumentaryID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BankInbox_Documentary");
        });

        modelBuilder.Entity<BankUser>(entity =>
        {
            entity.HasKey(e => e.PID);

            entity.HasIndex(e => e.USER_ID, "IX_BankUser").IsUnique();

            entity.Property(e => e.PID).HasMaxLength(64);
            entity.Property(e => e.EMAIL).HasMaxLength(256);
            entity.Property(e => e.PASSWORD).HasMaxLength(64);
            entity.Property(e => e.ROLE_ID).HasMaxLength(16);
            entity.Property(e => e.STATUS_CODE).HasMaxLength(16);
            entity.Property(e => e.TEL_NO).HasMaxLength(16);
            entity.Property(e => e.USER_NAME).HasMaxLength(64);

            entity.HasOne(d => d.ROLE).WithMany(p => p.BankUser)
                .HasForeignKey(d => d.ROLE_ID)
                .HasConstraintName("FK_BankUser_BankUserRole");

            entity.HasOne(d => d.SUPERVISORNavigation).WithMany(p => p.InverseSUPERVISORNavigation)
                .HasPrincipalKey(p => p.USER_ID)
                .HasForeignKey(d => d.SUPERVISOR)
                .HasConstraintName("FK_BankUser_BankUser");
        });

        modelBuilder.Entity<BankUserBranch>(entity =>
        {
            entity.HasKey(e => new { e.BRANCH_ID, e.USER_ID });

            entity.Property(e => e.BRANCH_ID).HasMaxLength(10);

            entity.HasOne(d => d.BRANCH).WithMany(p => p.BankUserBranch)
                .HasForeignKey(d => d.BRANCH_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankUserBranch_BankData");

            entity.HasOne(d => d.USER).WithMany(p => p.BankUserBranch)
                .HasPrincipalKey(p => p.USER_ID)
                .HasForeignKey(d => d.USER_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankUserBranch_BankUser");
        });

        modelBuilder.Entity<BankUserDeputy>(entity =>
        {
            entity.HasKey(e => e.USER_ID);

            entity.Property(e => e.USER_ID).ValueGeneratedNever();
            entity.Property(e => e.DEPUTY_ID)
                .IsRequired()
                .HasMaxLength(64);

            entity.HasOne(d => d.DEPUTY).WithMany(p => p.BankUserDeputyDEPUTY)
                .HasForeignKey(d => d.DEPUTY_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankUserDeputy_BankUser1");

            entity.HasOne(d => d.USER).WithOne(p => p.BankUserDeputyUSER)
                .HasPrincipalKey<BankUser>(p => p.USER_ID)
                .HasForeignKey<BankUserDeputy>(d => d.USER_ID)
                .HasConstraintName("FK_BankUserDeputy_BankUser");
        });

        modelBuilder.Entity<BankUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.USER_ID, e.LAST_LOGIN_TIME });

            entity.Property(e => e.LAST_LOGIN_TIME).HasColumnType("datetime");

            entity.HasOne(d => d.USER).WithMany(p => p.BankUserLogin)
                .HasPrincipalKey(p => p.USER_ID)
                .HasForeignKey(d => d.USER_ID)
                .HasConstraintName("FK_BankUserLogin_BankUser");
        });

        modelBuilder.Entity<BankUserRole>(entity =>
        {
            entity.HasKey(e => e.RoleID);

            entity.Property(e => e.RoleID).HasMaxLength(16);
            entity.Property(e => e.Memo).HasMaxLength(256);
            entity.Property(e => e.RoleName).HasMaxLength(64);
        });

        modelBuilder.Entity<BeneficiaryData>(entity =>
        {
            entity.HasKey(e => e.OrganizationID);

            entity.Property(e => e.OrganizationID).ValueGeneratedNever();
            entity.Property(e => e.AppAccount).HasMaxLength(20);
            entity.Property(e => e.AppCountersign).HasDefaultValue(true);
            entity.Property(e => e.Approver01).HasMaxLength(20);
            entity.Property(e => e.Approver02).HasMaxLength(20);
            entity.Property(e => e.NegoCenter).HasMaxLength(10);

            entity.HasOne(d => d.CustomerOfBranchVersion).WithMany(p => p.BeneficiaryData)
                .HasForeignKey(d => d.CustomerOfBranchVersionID)
                .HasConstraintName("FK_BeneficiaryData_CustomerOfBranchVersion");

            entity.HasOne(d => d.Log).WithMany(p => p.BeneficiaryData)
                .HasForeignKey(d => d.LogID)
                .HasConstraintName("FK_BeneficiaryData_RevisionLog");

            entity.HasOne(d => d.NegoCenterNavigation).WithMany(p => p.BeneficiaryData)
                .HasForeignKey(d => d.NegoCenter)
                .HasConstraintName("FK_BeneficiaryData_BankData");

            entity.HasOne(d => d.Organization).WithOne(p => p.BeneficiaryData)
                .HasForeignKey<BeneficiaryData>(d => d.OrganizationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BeneficiaryData_Organization1");
        });

        modelBuilder.Entity<BeneficiaryGroup>(entity =>
        {
            entity.HasKey(e => e.GroupID);

            entity.HasIndex(e => e.GroupName, "IX_BeneficiaryGroup").IsUnique();

            entity.Property(e => e.GroupID).ValueGeneratedNever();
            entity.Property(e => e.GroupName)
                .IsRequired()
                .HasMaxLength(64);

            entity.HasOne(d => d.Service).WithMany(p => p.BeneficiaryGroup)
                .HasForeignKey(d => d.ServiceID)
                .HasConstraintName("FK_BeneficiaryGroup_BeneficiaryServiceGroup");
        });

        modelBuilder.Entity<BeneficiaryServiceGroup>(entity =>
        {
            entity.HasKey(e => e.ServiceID);

            entity.Property(e => e.ServiceID).ValueGeneratedNever();
            entity.Property(e => e.ConfirmUrl).HasMaxLength(256);
            entity.Property(e => e.PostUrl).HasMaxLength(256);
            entity.Property(e => e.ServiceName).HasMaxLength(64);
        });

        modelBuilder.Entity<BeneficiaryTransferInto>(entity =>
        {
            entity.HasKey(e => e.AccountID);

            entity.Property(e => e.AccountNo)
                .IsRequired()
                .HasMaxLength(32);
            entity.Property(e => e.BankCode)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasOne(d => d.BankCodeNavigation).WithMany(p => p.BeneficiaryTransferInto)
                .HasForeignKey(d => d.BankCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BeneficiaryTransferInto_BankData");

            entity.HasOne(d => d.Bene).WithMany(p => p.BeneficiaryTransferInto)
                .HasForeignKey(d => d.BeneID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BeneficiaryTransferInto_Organization");
        });

        modelBuilder.Entity<CommonAppLog>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.Property(e => e.ACTION_CATEGORY).HasMaxLength(32);
            entity.Property(e => e.ACTION_MESSAGE).HasMaxLength(255);
            entity.Property(e => e.ACTION_RESULT).HasMaxLength(16);
            entity.Property(e => e.LOG_SEQ).HasMaxLength(255);
            entity.Property(e => e.LOG_TIME).HasColumnType("datetime");
            entity.Property(e => e.USER_PID).HasMaxLength(64);

            entity.HasOne(d => d.ACTION_CATEGORYNavigation).WithMany(p => p.CommonAppLog)
                .HasForeignKey(d => d.ACTION_CATEGORY)
                .HasConstraintName("FK_CommonAppLog_AP_LOG_CATEGORY");

            entity.HasOne(d => d.USER_P).WithMany(p => p.CommonAppLog)
                .HasForeignKey(d => d.USER_PID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CommonAppLog_BankUser");
        });

        modelBuilder.Entity<CommonCALog>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.Property(e => e.LOG_CATEGORY).HasMaxLength(32);
            entity.Property(e => e.LOG_CIPHER).IsUnicode(false);
            entity.Property(e => e.LOG_DOC).IsUnicode(false);
            entity.Property(e => e.LOG_MESSAGE).HasMaxLength(256);
            entity.Property(e => e.LOG_RESULT).HasMaxLength(64);
            entity.Property(e => e.LOG_SEQ).HasMaxLength(256);
            entity.Property(e => e.LOG_TIME).HasColumnType("datetime");
            entity.Property(e => e.LOG_TYPE).HasMaxLength(32);
            entity.Property(e => e.USER_PID).HasMaxLength(64);

            entity.HasOne(d => d.LOG_CATEGORYNavigation).WithMany(p => p.CommonCALog)
                .HasForeignKey(d => d.LOG_CATEGORY)
                .HasConstraintName("FK_CommonCALog_AP_LOG_CATEGORY");

            entity.HasOne(d => d.USER_P).WithMany(p => p.CommonCALog)
                .HasForeignKey(d => d.USER_PID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CommonCALog_BankUser");
        });

        modelBuilder.Entity<CreditApplicationDocumentary>(entity =>
        {
            entity.HasKey(e => e.DocumentaryID).HasName("PK_CreditApplyingBook");

            entity.ToTable(tb => tb.HasComment("開狀申請書"));

            entity.HasIndex(e => e.DocumentaryID, "IX_CreditApplicationDocumentary");

            entity.HasIndex(e => e.ApplicationNo, "IX_CreditApplicationDocumentary_1").IsUnique();

            entity.Property(e => e.DocumentaryID).ValueGeneratedNever();
            entity.Property(e => e.AdvisingBankCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.ApplicantDetailsID).HasComment("參考申請人歷史記錄檔");
            entity.Property(e => e.ApplicantID)
                .HasDefaultValueSql("(N'')")
                .HasComment("參考BeneficiaryData");
            entity.Property(e => e.ApplicationDate)
                .HasDefaultValueSql("((0))")
                .HasColumnType("datetime");
            entity.Property(e => e.ApplicationNo)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.AtSight).HasDefaultValue(true);
            entity.Property(e => e.AttachableDocumentID).HasComment("參考開狀申請檢附文件");
            entity.Property(e => e.BeneDetailsID).HasComment("參考受益人歷史記錄檔");
            entity.Property(e => e.BeneficiaryID).HasComment("參考BeneficiaryData");
            entity.Property(e => e.FileName)
                .HasMaxLength(64)
                .HasComment("CDS開狀申請之申請書檔名");
            entity.Property(e => e.Instrunction)
                .HasMaxLength(200)
                .HasComment("非本行制式特別指示之申請原因及依據");
            entity.Property(e => e.IsAccepted).HasComment("信用狀受益人已接受\r\n1:是\r\n其它:否");
            entity.Property(e => e.IssuingBankCode)
                .IsRequired()
                .HasMaxLength(10)
                .HasComment("參考BeneficiaryData");
            entity.Property(e => e.LcItemsID).HasComment("參考金額、日期申請資料");
            entity.Property(e => e.OverTheCounter).HasComment("註記臨櫃申請\r\n1:是\r\n0或NULL:否");
            entity.Property(e => e.PayableBankCode)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("")
                .HasComment("即為付款人(付款行與原意不符)");
            entity.Property(e => e.SpecificNotesID).HasComment("參考特別指示條款");

            entity.HasOne(d => d.AdvisingBankCodeNavigation).WithMany(p => p.CreditApplicationDocumentaryAdvisingBankCodeNavigation)
                .HasForeignKey(d => d.AdvisingBankCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditApplicationDocumentary_BankData2");

            entity.HasOne(d => d.ApplicantDetails).WithMany(p => p.CreditApplicationDocumentaryApplicantDetails)
                .HasForeignKey(d => d.ApplicantDetailsID)
                .HasConstraintName("FK_CreditApplicationDocumentary_CustomerOfBranchVersion");

            entity.HasOne(d => d.AttachableDocument).WithMany(p => p.CreditApplicationDocumentary)
                .HasForeignKey(d => d.AttachableDocumentID)
                .HasConstraintName("FK_CreditApplicationDocumentary_AttachableDocument");

            entity.HasOne(d => d.BeneDetails).WithMany(p => p.CreditApplicationDocumentaryBeneDetails)
                .HasForeignKey(d => d.BeneDetailsID)
                .HasConstraintName("FK_CreditApplicationDocumentary_CustomerOfBranchVersion1");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.CreditApplicationDocumentary)
                .HasForeignKey(d => d.BeneficiaryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditApplicationDocumentary_BeneficiaryData");

            entity.HasOne(d => d.Documentary).WithOne(p => p.CreditApplicationDocumentary)
                .HasForeignKey<CreditApplicationDocumentary>(d => d.DocumentaryID)
                .HasConstraintName("FK_CreditApplicationDocumentary_Documentary");

            entity.HasOne(d => d.LcItems).WithMany(p => p.CreditApplicationDocumentary)
                .HasForeignKey(d => d.LcItemsID)
                .HasConstraintName("FK_CreditApplicationDocumentary_LcItems");

            entity.HasOne(d => d.PayableBankCodeNavigation).WithMany(p => p.CreditApplicationDocumentaryPayableBankCodeNavigation)
                .HasForeignKey(d => d.PayableBankCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditApplicationDocumentary_BankData");

            entity.HasOne(d => d.SpecificNotes).WithMany(p => p.CreditApplicationDocumentary)
                .HasForeignKey(d => d.SpecificNotesID)
                .HasConstraintName("FK_CreditApplyingBook_SpecificNotes");

            entity.HasOne(d => d.CustomerOfBranch).WithMany(p => p.CreditApplicationDocumentary)
                .HasForeignKey(d => new { d.IssuingBankCode, d.ApplicantID })
                .HasConstraintName("FK_CreditApplicationDocumentary_CustomerOfBranch");
        });

        modelBuilder.Entity<CreditCancellation>(entity =>
        {
            entity.HasKey(e => e.DocumentaryID).HasName("PK_CreditCancelList");

            entity.HasIndex(e => e.LcID, "IX_CreditCancellation");

            entity.Property(e => e.DocumentaryID).ValueGeneratedNever();
            entity.Property(e => e.FileName)
                .HasMaxLength(64)
                .HasComment("CDS註申請之申請書檔名");
            entity.Property(e => e.LcID).HasComment("參考註銷信用狀來源");
            entity.Property(e => e.SysDocID)
                .HasMaxLength(32)
                .HasDefaultValueSql("((0))")
                .HasComment("Deprecated");
            entity.Property(e => e.status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("((50))")
                .IsFixedLength()
                .HasComment("Deprecated");
            entity.Property(e => e.申請日期).HasColumnType("datetime");
            entity.Property(e => e.註銷申請號碼)
                .IsRequired()
                .HasMaxLength(40)
                .HasComment("參考註銷登錄(L4500)");

            entity.HasOne(d => d.Documentary).WithOne(p => p.CreditCancellation)
                .HasForeignKey<CreditCancellation>(d => d.DocumentaryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditCancellation_Documentary");

            entity.HasOne(d => d.Lc).WithMany(p => p.CreditCancellation)
                .HasForeignKey(d => d.LcID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditCancellation_LetterOfCredit");
        });

        modelBuilder.Entity<CreditCancellationInfo>(entity =>
        {
            entity.HasKey(e => e.CancellationID);

            entity.Property(e => e.CancellationID).ValueGeneratedNever();
            entity.Property(e => e.CancellationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Cancellation).WithOne(p => p.CreditCancellationInfo)
                .HasForeignKey<CreditCancellationInfo>(d => d.CancellationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditCancellationInfo_CreditCancellation");
        });

        modelBuilder.Entity<CreditLoan>(entity =>
        {
            entity.HasKey(e => e.LoadID);

            entity.Property(e => e.AcceptApprovedNo).HasMaxLength(64);
            entity.Property(e => e.AcceptStampTaxFlag).HasMaxLength(64);
            entity.Property(e => e.ApplyDate).HasColumnType("datetime");
            entity.Property(e => e.ApproveManagerId1).HasMaxLength(64);
            entity.Property(e => e.ApproveManagerId2).HasMaxLength(64);
            entity.Property(e => e.AuditManager).HasMaxLength(64);
            entity.Property(e => e.AuditOfficer).HasMaxLength(64);
            entity.Property(e => e.BatchInsuranceApprovedNo).HasMaxLength(64);
            entity.Property(e => e.CheckAml).HasMaxLength(16);
            entity.Property(e => e.CheckNo1).HasMaxLength(64);
            entity.Property(e => e.CheckNo2).HasMaxLength(64);
            entity.Property(e => e.CheckTransferAccountNo1).HasMaxLength(64);
            entity.Property(e => e.CheckTransferAccountNo2).HasMaxLength(64);
            entity.Property(e => e.DemandDepositAccountNo).HasMaxLength(64);
            entity.Property(e => e.FiaTxseq).HasMaxLength(64);
            entity.Property(e => e.Fiaacc).HasMaxLength(64);
            entity.Property(e => e.Fiaacmo).HasMaxLength(64);
            entity.Property(e => e.FoaOsq).HasMaxLength(64);
            entity.Property(e => e.GuaranteeCondition).HasMaxLength(64);
            entity.Property(e => e.LcApprovedNo).HasMaxLength(64);
            entity.Property(e => e.LcStampTaxFlag).HasMaxLength(64);
            entity.Property(e => e.LcaNo).HasMaxLength(64);
            entity.Property(e => e.LcrNo).HasMaxLength(64);
            entity.Property(e => e.LoanAccountId).HasMaxLength(64);
            entity.Property(e => e.LoanManagerId).HasMaxLength(64);
            entity.Property(e => e.MrateType).HasMaxLength(64);
            entity.Property(e => e.OtherAppointment).HasMaxLength(256);
            entity.Property(e => e.RateMemo).HasMaxLength(256);
            entity.Property(e => e.RefuseId).HasMaxLength(64);
            entity.Property(e => e.RefuseName).HasMaxLength(64);
            entity.Property(e => e.ReturnAccount).HasMaxLength(64);
            entity.Property(e => e.RiskKind1).HasMaxLength(64);
            entity.Property(e => e.RiskKind2).HasMaxLength(64);
            entity.Property(e => e.RiskKind3).HasMaxLength(64);
            entity.Property(e => e.RiskKind4).HasMaxLength(64);
            entity.Property(e => e.SmegCase).HasMaxLength(64);
            entity.Property(e => e.SmegNoticeno).HasMaxLength(64);

            entity.HasOne(d => d.Amending).WithMany(p => p.CreditLoan)
                .HasForeignKey(d => d.AmendingID)
                .HasConstraintName("FK_CreditLoan_AmendingLcApplication");

            entity.HasOne(d => d.App).WithMany(p => p.CreditLoan)
                .HasForeignKey(d => d.AppID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CreditLoan_CreditApplicationDocumentary");
        });

        modelBuilder.Entity<CurrencyType>(entity =>
        {
            entity.HasKey(e => e.CurrencyID).HasName("PK_currency_type");

            entity.Property(e => e.CurrencyID).ValueGeneratedNever();
            entity.Property(e => e.AbbrevName).HasMaxLength(6);
            entity.Property(e => e.CurrencyName).HasMaxLength(8);
        });

        modelBuilder.Entity<CustomerInbox>(entity =>
        {
            entity.HasKey(e => e.MsgID);

            entity.HasIndex(e => e.ReceiptNo, "IX_CustomerInbox");

            entity.Property(e => e.MsgDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReceiptNo).HasMaxLength(10);

            entity.HasOne(d => d.Company).WithMany(p => p.CustomerInbox)
                .HasForeignKey(d => d.CompanyID)
                .HasConstraintName("FK_CustomerInbox_Organization");

            entity.HasOne(d => d.Documentary).WithMany(p => p.CustomerInbox)
                .HasForeignKey(d => d.DocumentaryID)
                .HasConstraintName("FK_CustomerInbox_Documentary");
        });

        modelBuilder.Entity<CustomerOfBranch>(entity =>
        {
            entity.HasKey(e => new { e.BankCode, e.OrganizationID });

            entity.Property(e => e.BankCode).HasMaxLength(10);
            entity.Property(e => e.Addr).HasMaxLength(256);
            entity.Property(e => e.CompanyName).HasMaxLength(128);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.PayableAccount).HasMaxLength(24);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Undertaker).HasMaxLength(20);

            entity.HasOne(d => d.BankCodeNavigation).WithMany(p => p.CustomerOfBranch)
                .HasForeignKey(d => d.BankCode)
                .HasConstraintName("FK_CustomerOfBranch_BankData");

            entity.HasOne(d => d.CurrentLevelNavigation).WithMany(p => p.CustomerOfBranch)
                .HasForeignKey(d => d.CurrentLevel)
                .HasConstraintName("FK_CustomerOfBranch_LevelExpression");

            entity.HasOne(d => d.CustomerOfBranchVersion).WithMany(p => p.CustomerOfBranch)
                .HasForeignKey(d => d.CustomerOfBranchVersionID)
                .HasConstraintName("FK_CustomerOfBranch_CustomerOfBranchVersion");

            entity.HasOne(d => d.Organization).WithMany(p => p.CustomerOfBranch)
                .HasForeignKey(d => d.OrganizationID)
                .HasConstraintName("FK_CustomerOfBranch_Organization");
        });

        modelBuilder.Entity<CustomerOfBranchVersion>(entity =>
        {
            entity.HasKey(e => e.VersionID);

            entity.Property(e => e.Addr).HasMaxLength(256);
            entity.Property(e => e.BankCode).HasMaxLength(10);
            entity.Property(e => e.CompanyName).HasMaxLength(128);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactName).HasMaxLength(50);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.EnglishAddr).HasMaxLength(256);
            entity.Property(e => e.EnglishName).HasMaxLength(50);
            entity.Property(e => e.EnglishRegAddr).HasMaxLength(256);
            entity.Property(e => e.Fax).HasMaxLength(50);
            entity.Property(e => e.OldBankCode).HasMaxLength(10);
            entity.Property(e => e.PID).HasMaxLength(50);
            entity.Property(e => e.PayableAccount).HasMaxLength(24);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RegAddr).HasMaxLength(256);
            entity.Property(e => e.Undertaker).HasMaxLength(20);
            entity.Property(e => e.UndertakerID).HasMaxLength(50);
            entity.Property(e => e.UndertakerName).HasMaxLength(50);
        });

        modelBuilder.Entity<DataPortLog>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.HasIndex(e => e.Direction, "IX_DataPortLog");

            entity.HasIndex(e => e.Catalog, "IX_DataPortLog_1");

            entity.HasIndex(e => e.FpgFileName, "IX_DataPortLog_2");

            entity.HasIndex(e => e.FpgTableName, "IX_DataPortLog_3");

            entity.Property(e => e.FpgFileName).HasMaxLength(64);
            entity.Property(e => e.FpgTableName).HasMaxLength(64);
            entity.Property(e => e.TransportTime).HasColumnType("datetime");

            entity.HasOne(d => d.ExceptionLog).WithMany(p => p.DataPortLog)
                .HasForeignKey(d => d.ExceptionLogID)
                .HasConstraintName("FK_DataPortLog_ExceptionLog");

            entity.HasOne(d => d.Service).WithMany(p => p.DataPortLog)
                .HasForeignKey(d => d.ServiceID)
                .HasConstraintName("FK_DataPortLog_BeneficiaryServiceGroup");
        });

        modelBuilder.Entity<DataProcessLog>(entity =>
        {
            entity.HasKey(e => e.ProcID);

            entity.HasIndex(e => e.DataPortLogID, "IX_DataProcessLog");

            entity.Property(e => e.ProcessDate).HasColumnType("datetime");

            entity.HasOne(d => d.DataPortLog).WithMany(p => p.DataProcessLog)
                .HasForeignKey(d => d.DataPortLogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataProcessLog_DataPortLog");

            entity.HasOne(d => d.ExceptionLog).WithMany(p => p.DataProcessLog)
                .HasForeignKey(d => d.ExceptionLogID)
                .HasConstraintName("FK_DataProcessLog_ExceptionLog");
        });

        modelBuilder.Entity<DefaultSpecificNotes>(entity =>
        {
            entity.HasKey(e => e.CompanyID);

            entity.Property(e => e.CompanyID).ValueGeneratedNever();

            entity.HasOne(d => d.Company).WithOne(p => p.DefaultSpecificNotes)
                .HasForeignKey<DefaultSpecificNotes>(d => d.CompanyID)
                .HasConstraintName("FK_DefaultSpecificNotes_Organization");

            entity.HasOne(d => d.Note).WithMany(p => p.DefaultSpecificNotes)
                .HasForeignKey(d => d.NoteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DefaultSpecificNotes_SpecificNotes");
        });

        modelBuilder.Entity<DisabledBranch>(entity =>
        {
            entity.HasKey(e => e.BankCode);

            entity.Property(e => e.BankCode).HasMaxLength(10);

            entity.HasOne(d => d.BankCodeNavigation).WithOne(p => p.DisabledBranch)
                .HasForeignKey<DisabledBranch>(d => d.BankCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DisabledBranch_BankData");
        });

        modelBuilder.Entity<DocumentDispatch>(entity =>
        {
            entity.HasKey(e => e.DocumentaryID);

            entity.Property(e => e.DocumentaryID).ValueGeneratedNever();

            entity.HasOne(d => d.Documentary).WithOne(p => p.DocumentDispatch)
                .HasForeignKey<DocumentDispatch>(d => d.DocumentaryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentDispatch_Documentary");
        });

        modelBuilder.Entity<DocumentOwner>(entity =>
        {
            entity.HasKey(e => e.DocID);

            entity.Property(e => e.DocID).ValueGeneratedNever();

            entity.HasOne(d => d.Doc).WithOne(p => p.DocumentOwner)
                .HasForeignKey<DocumentOwner>(d => d.DocID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentOwner_Documentary");

            entity.HasOne(d => d.Owner).WithMany(p => p.DocumentOwner)
                .HasForeignKey(d => d.OwnerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentOwner_Organization");
        });

        modelBuilder.Entity<DocumentProcessLog>(entity =>
        {
            entity.HasKey(e => new { e.DocID, e.ProcessDate });

            entity.Property(e => e.ProcessDate).HasColumnType("datetime");

            entity.HasOne(d => d.Doc).WithMany(p => p.DocumentProcessLog)
                .HasForeignKey(d => d.DocID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProcessLog_Documentary");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.TypeID);

            entity.ToTable(tb => tb.HasComment("文件類型"));

            entity.Property(e => e.TypeID).ValueGeneratedNever();
            entity.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(128)
                .HasComment("文件類別名稱");
        });

        modelBuilder.Entity<Documentary>(entity =>
        {
            entity.HasKey(e => e.DocID);

            entity.ToTable(tb => tb.HasComment("系統文件主體"));

            entity.Property(e => e.AttachedFile)
                .HasMaxLength(256)
                .HasComment("附件檔存檔路徑");
            entity.Property(e => e.CurrentLevel)
                .HasDefaultValue(0)
                .HasComment("目前狀態");
            entity.Property(e => e.DesiredDate)
                .HasComment("預約日期")
                .HasColumnType("datetime");
            entity.Property(e => e.DocDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("文件日期")
                .HasColumnType("datetime");
            entity.Property(e => e.DocType).HasComment("文件類型索引");
            entity.Property(e => e.SysDocID)
                .HasMaxLength(32)
                .HasComment("Deprecated");

            entity.HasOne(d => d.CurrentLevelNavigation).WithMany(p => p.Documentary)
                .HasForeignKey(d => d.CurrentLevel)
                .HasConstraintName("FK_Documentary_LevelExpression");

            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.Documentary)
                .HasForeignKey(d => d.DocType)
                .HasConstraintName("FK_Documentary_DocumentType");
        });

        modelBuilder.Entity<DocumentaryAllowance>(entity =>
        {
            entity.HasKey(e => new { e.DocID, e.ApprovalDate });

            entity.ToTable(tb => tb.HasComment("文件核准記錄"));

            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("核准日期")
                .HasColumnType("datetime");
            entity.Property(e => e.Approver)
                .HasMaxLength(16)
                .HasComment("核准人");
            entity.Property(e => e.Memo)
                .HasMaxLength(256)
                .HasComment("備註");

            entity.HasOne(d => d.Doc).WithMany(p => p.DocumentaryAllowance)
                .HasForeignKey(d => d.DocID)
                .HasConstraintName("FK_DocumentaryAllowance_Documentary");
        });

        modelBuilder.Entity<DocumentaryDenial>(entity =>
        {
            entity.HasKey(e => new { e.DocID, e.DenialDate });

            entity.ToTable(tb => tb.HasComment("文件退回記錄"));

            entity.Property(e => e.DenialDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("退回日期")
                .HasColumnType("datetime");
            entity.Property(e => e.Denier)
                .HasMaxLength(16)
                .HasComment("退回者");
            entity.Property(e => e.Reason)
                .HasMaxLength(32)
                .HasComment("退回原因");

            entity.HasOne(d => d.Doc).WithMany(p => p.DocumentaryDenial)
                .HasForeignKey(d => d.DocID)
                .HasConstraintName("FK_DocumentaryDenial_Documentary");
        });

        modelBuilder.Entity<DocumentaryLevel>(entity =>
        {
            entity.HasKey(e => new { e.DocumentaryID, e.LevelDate });

            entity.ToTable(tb => tb.HasComment("文件狀態變更記錄"));

            entity.HasIndex(e => e.DocumentaryID, "IX_DocumentaryLevel_DocID");

            entity.HasIndex(e => e.DocLevel, "IX_DocumentaryLevel_DocLevel");

            entity.HasIndex(e => e.LevelDate, "IX_DocumentaryLevel_LevelDate");

            entity.Property(e => e.LevelDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("變更日期")
                .HasColumnType("datetime");
            entity.Property(e => e.DocLevel)
                .HasDefaultValue(0)
                .HasComment("文件狀態");

            entity.HasOne(d => d.DocLevelNavigation).WithMany(p => p.DocumentaryLevel)
                .HasForeignKey(d => d.DocLevel)
                .HasConstraintName("FK_DocumentaryLevel_LevelExpression");

            entity.HasOne(d => d.Documentary).WithMany(p => p.DocumentaryLevel)
                .HasForeignKey(d => d.DocumentaryID)
                .HasConstraintName("FK_DocumentaryLevel_Documentary");
        });

        modelBuilder.Entity<ExceptionLog>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.Property(e => e.LogTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<FpgBeneficiaryStatus>(entity =>
        {
            entity.HasKey(e => e.BeneID).HasName("PK_FpgBeneficiaryStatus_1");

            entity.Property(e => e.BeneID).ValueGeneratedNever();
            entity.Property(e => e.押匯允差比例).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.押匯允差比例退回原因).HasMaxLength(64);

            entity.HasOne(d => d.Bene).WithOne(p => p.FpgBeneficiaryStatus)
                .HasForeignKey<FpgBeneficiaryStatus>(d => d.BeneID)
                .HasConstraintName("FK_FpgBeneficiaryStatus_BeneficiaryData");
        });

        modelBuilder.Entity<FpgLcItem>(entity =>
        {
            entity.HasKey(e => e.AppID);

            entity.Property(e => e.AppID).ValueGeneratedNever();
            entity.Property(e => e.ContactName).HasMaxLength(32);
            entity.Property(e => e.ContactPhone).HasMaxLength(32);
            entity.Property(e => e.CustomerNo).HasMaxLength(16);
            entity.Property(e => e.DepartID)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.匯票起算基準).HasMaxLength(16);
            entity.Property(e => e.押匯允差比例).HasColumnType("decimal(12, 0)");

            entity.HasOne(d => d.App).WithOne(p => p.FpgLcItem)
                .HasForeignKey<FpgLcItem>(d => d.AppID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgLcItem_CreditApplicationDocumentary");

            entity.HasOne(d => d.GroupDepartment).WithMany(p => p.FpgLcItem)
                .HasForeignKey(d => new { d.GroupID, d.DepartID })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgLcItem_GroupDepartment");
        });

        modelBuilder.Entity<FpgLcMonthlyReport>(entity =>
        {
            entity.HasKey(e => e.ReportID).HasName("PK_FpgLcMonthlyReport_1");

            entity.HasIndex(e => new { e.Year, e.Month }, "IX_FpgLcMonthlyReport");

            entity.HasIndex(e => new { e.BankCode, e.ApplicantID, e.BeneID, e.Year, e.Month, e.Catalog }, "IX_FpgLcMonthlyReport_1").IsUnique();

            entity.Property(e => e.BankCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.ProductCategory).HasMaxLength(64);

            entity.HasOne(d => d.Bene).WithMany(p => p.FpgLcMonthlyReport)
                .HasForeignKey(d => d.BeneID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgLcMonthlyReport_BeneficiaryData");

            entity.HasOne(d => d.CustomerOfBranch).WithMany(p => p.FpgLcMonthlyReport)
                .HasForeignKey(d => new { d.BankCode, d.ApplicantID })
                .HasConstraintName("FK_FpgLcMonthlyReport_CustomerOfBranch");
        });

        modelBuilder.Entity<FpgLcMonthlySummary>(entity =>
        {
            entity.HasKey(e => e.ReportID);

            entity.HasIndex(e => new { e.Year, e.Month }, "IX_FpgLcMonthlySummary");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ApplicantName).HasMaxLength(64);
            entity.Property(e => e.ApplicantNo)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.BeneficiaryNo)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.BranchName)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.ProductCategory).HasMaxLength(64);
        });

        modelBuilder.Entity<FpgNegoDraft>(entity =>
        {
            entity.HasKey(e => e.NegoDraftID);

            entity.Property(e => e.NegoDraftID).ValueGeneratedNever();
            entity.Property(e => e.匯入帳號).HasMaxLength(32);
            entity.Property(e => e.匯入戶名).HasMaxLength(32);
            entity.Property(e => e.匯入銀行代碼).HasMaxLength(10);
            entity.Property(e => e.匯入銀行名稱).HasMaxLength(64);
            entity.Property(e => e.押匯提示地點).HasMaxLength(32);
            entity.Property(e => e.營業員姓名).HasMaxLength(32);
            entity.Property(e => e.營業員連絡電話).HasMaxLength(32);

            entity.HasOne(d => d.NegoDraft).WithOne(p => p.FpgNegoDraft)
                .HasForeignKey<FpgNegoDraft>(d => d.NegoDraftID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgNegoDraft_NegoDraft");
        });

        modelBuilder.Entity<FpgNegoRemittance>(entity =>
        {
            entity.HasKey(e => e.FpgNegoDraftID);

            entity.HasIndex(e => e.Status, "IX_FpgNegoRemittance");

            entity.Property(e => e.FpgNegoDraftID).ValueGeneratedNever();
            entity.Property(e => e.RemittanceDate).HasColumnType("datetime");

            entity.HasOne(d => d.FpgNegoDraft).WithOne(p => p.FpgNegoRemittance)
                .HasForeignKey<FpgNegoRemittance>(d => d.FpgNegoDraftID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgNegoRemittance_FpgNegoDraft");
        });

        modelBuilder.Entity<FpgNegoRemittanceDispatch>(entity =>
        {
            entity.HasKey(e => e.FpgNegoRemittanceLogID);

            entity.Property(e => e.FpgNegoRemittanceLogID).ValueGeneratedNever();

            entity.HasOne(d => d.FpgNegoRemittanceLog).WithOne(p => p.FpgNegoRemittanceDispatch)
                .HasForeignKey<FpgNegoRemittanceDispatch>(d => d.FpgNegoRemittanceLogID)
                .HasConstraintName("FK_FpgNegoRemittanceDispatch_FpgNegoRemittanceLog");
        });

        modelBuilder.Entity<FpgNegoRemittanceLog>(entity =>
        {
            entity.HasKey(e => e.RemittanceID);

            entity.HasIndex(e => e.FpgNegoRemittanceID, "IX_FpgNegoRemittanceLog");

            entity.HasIndex(e => e.SeqNo, "IX_FpgNegoRemittanceLog_1");

            entity.Property(e => e.Amount)
                .HasComment("押匯金額")
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BatchNo).HasMaxLength(32);
            entity.Property(e => e.DPMTID).HasMaxLength(32);
            entity.Property(e => e.Description).HasMaxLength(128);

            entity.HasOne(d => d.DataPortLog).WithMany(p => p.FpgNegoRemittanceLog)
                .HasForeignKey(d => d.DataPortLogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgNegoRemittanceLog_DataPortLog");

            entity.HasOne(d => d.FpgNegoRemittance).WithMany(p => p.FpgNegoRemittanceLog)
                .HasForeignKey(d => d.FpgNegoRemittanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FpgNegoRemittanceLog_FpgNegoRemittance");
        });

        modelBuilder.Entity<GoodsDetail>(entity =>
        {
            entity.HasKey(e => new { e.ItemID, e.Sno });

            entity.Property(e => e.Amount).HasMaxLength(20);
            entity.Property(e => e.ProductName).HasMaxLength(512);
            entity.Property(e => e.ProductSize).HasMaxLength(20);
            entity.Property(e => e.Quantity).HasMaxLength(20);
            entity.Property(e => e.Remark).HasMaxLength(40);
            entity.Property(e => e.UnitPrice).HasMaxLength(20);

            entity.HasOne(d => d.Item).WithMany(p => p.GoodsDetail)
                .HasForeignKey(d => d.ItemID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsDetail_LcItems");
        });

        modelBuilder.Entity<GroupDepartment>(entity =>
        {
            entity.HasKey(e => new { e.GroupID, e.DepartID });

            entity.Property(e => e.DepartID).HasMaxLength(16);
            entity.Property(e => e.Department)
                .IsRequired()
                .HasMaxLength(32);

            entity.HasOne(d => d.Group).WithMany(p => p.GroupDepartment)
                .HasForeignKey(d => d.GroupID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupDepartment_BeneficiaryGroup");
        });

        modelBuilder.Entity<GuaranteeDeposit>(entity =>
        {
            entity.HasKey(e => e.DepositID);

            entity.Property(e => e.AuditManager).HasMaxLength(64);
            entity.Property(e => e.AuditOfficer).HasMaxLength(64);
            entity.Property(e => e.DraftNo).HasMaxLength(64);
            entity.Property(e => e.FiaTxseq).HasMaxLength(64);
            entity.Property(e => e.Fiaacc).HasMaxLength(64);
            entity.Property(e => e.Fiaacmo).HasMaxLength(64);
            entity.Property(e => e.Fiaacno).HasMaxLength(64);
            entity.Property(e => e.Fiadcmo).HasMaxLength(256);
            entity.Property(e => e.Fiadcno).HasMaxLength(64);
            entity.Property(e => e.Fidacc).HasMaxLength(64);
            entity.Property(e => e.FoaOsq).HasMaxLength(64);
            entity.Property(e => e.LcNo).HasMaxLength(64);
            entity.Property(e => e.LcStampTaxFlag).HasMaxLength(64);
            entity.Property(e => e.LcaNo).HasMaxLength(64);
            entity.Property(e => e.LcdNo).HasMaxLength(64);
            entity.Property(e => e.LcexpCheckNo).HasMaxLength(64);
            entity.Property(e => e.LcexpPaymentWay).HasMaxLength(64);
            entity.Property(e => e.LcexpTransferAccountNo).HasMaxLength(64);
            entity.Property(e => e.RefuseId).HasMaxLength(64);
            entity.Property(e => e.RefuseName).HasMaxLength(64);
            entity.Property(e => e.RejectReason).HasMaxLength(64);
            entity.Property(e => e.ReturnAccountNo).HasMaxLength(64);
            entity.Property(e => e.Starttime).HasColumnType("datetime");

            entity.HasOne(d => d.App).WithMany(p => p.GuaranteeDeposit)
                .HasForeignKey(d => d.AppID)
                .HasConstraintName("FK_GuaranteeDeposit_CreditApplicationDocumentary");

            entity.HasOne(d => d.Cancellation).WithMany(p => p.GuaranteeDeposit)
                .HasForeignKey(d => d.CancellationID)
                .HasConstraintName("FK_GuaranteeDeposit_CreditCancellation");

            entity.HasOne(d => d.Draft).WithMany(p => p.GuaranteeDeposit)
                .HasForeignKey(d => d.DraftID)
                .HasConstraintName("FK_GuaranteeDeposit_NegoDraft");

            entity.HasOne(d => d.Lc).WithMany(p => p.GuaranteeDeposit)
                .HasForeignKey(d => d.LcID)
                .HasConstraintName("FK_GuaranteeDeposit_LetterOfCredit");
        });

        modelBuilder.Entity<LcItems>(entity =>
        {
            entity.HasKey(e => e.ItemID);

            entity.Property(e => e.Goods).HasMaxLength(512);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentTerms).HasMaxLength(16);
            entity.Property(e => e.有效期限).HasColumnType("datetime");
            entity.Property(e => e.開狀金額)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.CurrencyType).WithMany(p => p.LcItems)
                .HasForeignKey(d => d.CurrencyTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LcItems_currency_type");
        });

        modelBuilder.Entity<LetterOfCredit>(entity =>
        {
            entity.HasKey(e => e.LcID);

            entity.HasIndex(e => e.LcNo, "IX_LetterOfCredit").IsUnique();

            entity.Property(e => e.AppCountersign).HasDefaultValue(true);
            entity.Property(e => e.LcDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LcNo)
                .IsRequired()
                .HasMaxLength(24);
            entity.Property(e => e.NotifyingBank).HasMaxLength(10);
            entity.Property(e => e.可用餘額).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Application).WithMany(p => p.LetterOfCredit)
                .HasForeignKey(d => d.ApplicationID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LetterOfCredit_CreditApplicationDocumentary");

            entity.HasOne(d => d.NotifyingBankNavigation).WithMany(p => p.LetterOfCredit)
                .HasForeignKey(d => d.NotifyingBank)
                .HasConstraintName("FK_LetterOfCredit_BankData");
        });

        modelBuilder.Entity<LetterOfCreditExtension>(entity =>
        {
            entity.HasKey(e => e.LcID);

            entity.Property(e => e.LcID).ValueGeneratedNever();

            entity.HasOne(d => d.Lc).WithOne(p => p.LetterOfCreditExtension)
                .HasForeignKey<LetterOfCreditExtension>(d => d.LcID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LetterOfCreditExtension_LetterOfCredit");
        });

        modelBuilder.Entity<LetterOfCreditVersion>(entity =>
        {
            entity.HasKey(e => new { e.LcID, e.VersionNo });

            entity.HasIndex(e => e.VersionID, "IX_LetterOfCreditVersion").IsUnique();

            entity.Property(e => e.NotifyingBank).HasMaxLength(10);
            entity.Property(e => e.VersionID).ValueGeneratedOnAdd();

            entity.HasOne(d => d.AmendingLcInformation).WithMany(p => p.LetterOfCreditVersion)
                .HasForeignKey(d => d.AmendingLcInformationID)
                .HasConstraintName("FK_LetterOfCreditVersion_AmendingLcInformation");

            entity.HasOne(d => d.AttachableDocument).WithMany(p => p.LetterOfCreditVersion)
                .HasForeignKey(d => d.AttachableDocumentID)
                .HasConstraintName("FK_LetterOfCreditVersion_AttachableDocument");

            entity.HasOne(d => d.Lc).WithMany(p => p.LetterOfCreditVersion)
                .HasForeignKey(d => d.LcID)
                .HasConstraintName("FK_LetterOfCreditVersion_LetterOfCredit");

            entity.HasOne(d => d.LcItems).WithMany(p => p.LetterOfCreditVersion)
                .HasForeignKey(d => d.LcItemsID)
                .HasConstraintName("FK_LetterOfCreditVersion_LcItems");

            entity.HasOne(d => d.SpecificNotes).WithMany(p => p.LetterOfCreditVersion)
                .HasForeignKey(d => d.SpecificNotesID)
                .HasConstraintName("FK_LetterOfCreditVersion_SpecificNotes");
        });

        modelBuilder.Entity<LevelExpression>(entity =>
        {
            entity.HasKey(e => e.LevelID);

            entity.ToTable(tb => tb.HasComment("文件狀態描述"));

            entity.Property(e => e.LevelID).ValueGeneratedNever();
            entity.Property(e => e.BusinessExpression)
                .HasMaxLength(32)
                .HasComment("狀態名稱企業端顯示使用");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasComment("狀態詳細資訊");
            entity.Property(e => e.Expression)
                .HasMaxLength(32)
                .HasComment("狀態名稱");
        });

        modelBuilder.Entity<NegoAffair>(entity =>
        {
            entity.HasKey(e => new { e.NegoDraftID, e.NegoInvoiceID });

            entity.Property(e => e.NegoAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.NegoDraft).WithMany(p => p.NegoAffair)
                .HasForeignKey(d => d.NegoDraftID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoAffair_NegoDraft");

            entity.HasOne(d => d.NegoInvoice).WithMany(p => p.NegoAffair)
                .HasForeignKey(d => d.NegoInvoiceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoAffair_NegoInvoice");
        });

        modelBuilder.Entity<NegoDraft>(entity =>
        {
            entity.HasKey(e => e.DocumentaryID)
                .HasName("PK3")
                .IsClustered(false);

            entity.HasIndex(e => e.DraftNo, "IX_NegoDraft");

            entity.HasIndex(e => e.DownloadFlag, "IX_NegoDraft_2");

            entity.HasIndex(e => e.AppYear, "IX_NegoDraft_3");

            entity.Property(e => e.DocumentaryID).ValueGeneratedNever();
            entity.Property(e => e.Amount)
                .HasComment("押匯金額")
                .HasColumnType("decimal(12, 0)");
            entity.Property(e => e.AppNoBase).HasComment("Deprecated");
            entity.Property(e => e.AppSeq).HasComment("押匯申請號碼：流水號");
            entity.Property(e => e.AppYear).HasComment("押匯申請號碼：年");
            entity.Property(e => e.BackSeal)
                .HasMaxLength(64)
                .HasComment("匯票背面圖章存檔路徑");
            entity.Property(e => e.DownloadFlag)
                .HasDefaultValue((short)0)
                .HasComment("註記CDS押匯資料已下載\r\n1：是\r\n其他：否");
            entity.Property(e => e.DraftContent).HasComment("匯票影像(PDF)數據，以BASE64編碼儲存");
            entity.Property(e => e.DraftNo)
                .IsRequired()
                .HasMaxLength(16)
                .HasComment("受益人指定的匯票號碼");
            entity.Property(e => e.FrontSeal)
                .HasMaxLength(64)
                .HasComment("匯票正面圖章存檔路徑");
            entity.Property(e => e.InvoiceCount).HasComment("檢附發票張數");
            entity.Property(e => e.InvoiceDate)
                .HasComment("匯票之發票日期")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemName)
                .HasMaxLength(128)
                .HasComment("貨品名稱");
            entity.Property(e => e.ItemQuantity)
                .HasComment("貨品數量")
                .HasColumnType("decimal(8, 1)");
            entity.Property(e => e.ItemSubtotal)
                .HasComment("貨品金額")
                .HasColumnType("decimal(12, 0)");
            entity.Property(e => e.NegoDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("押匯提示日期")
                .HasColumnType("datetime");
            entity.Property(e => e.PromptID).HasComment("參考押匯提示清單");
            entity.Property(e => e.ResourceName)
                .HasMaxLength(256)
                .HasComment("CDS匯票資源名稱");
            entity.Property(e => e.ShipmentDate)
                .HasComment("匯票出貨日期")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Documentary).WithOne(p => p.NegoDraft)
                .HasForeignKey<NegoDraft>(d => d.DocumentaryID)
                .HasConstraintName("FK_NegoDraft_Documentary");

            entity.HasOne(d => d.NegoLcVersion).WithMany(p => p.NegoDraft)
                .HasPrincipalKey(p => p.VersionID)
                .HasForeignKey(d => d.NegoLcVersionID)
                .HasConstraintName("FK_NegoDraft_LetterOfCreditVersion");

            entity.HasOne(d => d.Prompt).WithMany(p => p.NegoDraft)
                .HasForeignKey(d => d.PromptID)
                .HasConstraintName("FK_NegoDraft_NegoPrompt");
        });

        modelBuilder.Entity<NegoDraftAcceptance>(entity =>
        {
            entity.HasKey(e => e.AcceptanceID);

            entity.Property(e => e.AcceptanceID).ValueGeneratedNever();

            entity.HasOne(d => d.Acceptance).WithOne(p => p.NegoDraftAcceptance)
                .HasForeignKey<NegoDraftAcceptance>(d => d.AcceptanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoDraftAcceptance_Documentary");

            entity.HasOne(d => d.Draft).WithMany(p => p.NegoDraftAcceptance)
                .HasForeignKey(d => d.DraftID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoDraftAcceptance_NegoDraft");
        });

        modelBuilder.Entity<NegoDraftDeal>(entity =>
        {
            entity.HasKey(e => e.DraftID);

            entity.HasIndex(e => e.AcceptanceBranch, "IX_NegoDraftDeal");

            entity.Property(e => e.DraftID).ValueGeneratedNever();
            entity.Property(e => e.AcceptanceBranch)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasOne(d => d.AcceptanceBranchNavigation).WithMany(p => p.NegoDraftDeal)
                .HasForeignKey(d => d.AcceptanceBranch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoDraftDeal_BankData");

            entity.HasOne(d => d.Draft).WithOne(p => p.NegoDraftDeal)
                .HasForeignKey<NegoDraftDeal>(d => d.DraftID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoDraftDeal_NegoDraft");
        });

        modelBuilder.Entity<NegoDraftExtension>(entity =>
        {
            entity.HasKey(e => e.NegoDraftID);

            entity.Property(e => e.NegoDraftID).ValueGeneratedNever();
            entity.Property(e => e.BeneficiaryAccountNo).HasMaxLength(32);
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.LcBranch).HasMaxLength(10);
            entity.Property(e => e.NegoBranch)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasOne(d => d.LcBranchNavigation).WithMany(p => p.NegoDraftExtensionLcBranchNavigation)
                .HasForeignKey(d => d.LcBranch)
                .HasConstraintName("FK_NegoDraftExtension_BankData1");

            entity.HasOne(d => d.NegoBranchNavigation).WithMany(p => p.NegoDraftExtensionNegoBranchNavigation)
                .HasForeignKey(d => d.NegoBranch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoDraftExtension_BankData");

            entity.HasOne(d => d.NegoDraft).WithOne(p => p.NegoDraftExtension)
                .HasForeignKey<NegoDraftExtension>(d => d.NegoDraftID)
                .HasConstraintName("FK_NegoDraftExtension_NegoDraft");
        });

        modelBuilder.Entity<NegoHost>(entity =>
        {
            entity.HasKey(e => e.HostID);

            entity.Property(e => e.HostUrl).HasMaxLength(256);
        });

        modelBuilder.Entity<NegoInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceID);

            entity.HasIndex(e => e.InvoiceNo, "IX_NegoInvoice");

            entity.HasIndex(e => e.DownloadFlag, "IX_NegoInvoice_1");

            entity.Property(e => e.AttachedFile).HasMaxLength(128);
            entity.Property(e => e.ContractNo).HasMaxLength(16);
            entity.Property(e => e.DownloadFlag).HasDefaultValue((short)0);
            entity.Property(e => e.ImportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceContent).HasColumnType("text");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceNo)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.LadingNo).HasMaxLength(6);
            entity.Property(e => e.ReceiptNo).HasMaxLength(10);
            entity.Property(e => e.TaxNo).HasMaxLength(16);

            entity.HasOne(d => d.NegoDraft).WithMany(p => p.NegoInvoice)
                .HasForeignKey(d => d.NegoDraftID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NegoInvoice_NegoDraft");
        });

        modelBuilder.Entity<NegoInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.ProductID);

            entity.Property(e => e.CostAmount)
                .HasComment("金額")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.CostAmount2)
                .HasComment("金額")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.FreightAmount)
                .HasComment("總運費")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.ItemNo).HasMaxLength(16);
            entity.Property(e => e.No).HasComment("明細排列序號");
            entity.Property(e => e.OriginalPrice)
                .HasComment("原幣金額")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.Piece)
                .HasComment("數量")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.Piece2)
                .HasComment("數量")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.PieceUnit)
                .HasMaxLength(16)
                .HasComment("件數單位");
            entity.Property(e => e.PieceUnit2)
                .HasMaxLength(16)
                .HasComment("件數單位");
            entity.Property(e => e.RelateNumber)
                .HasMaxLength(64)
                .HasComment("相關號碼");
            entity.Property(e => e.Remark)
                .HasMaxLength(128)
                .HasComment("單一欄位備註");
            entity.Property(e => e.Spec)
                .HasMaxLength(128)
                .HasComment("產品規格");
            entity.Property(e => e.TaxType).HasComment("課稅別\r\n1：應稅\r\n2：零稅率\r\n3：免稅\r\n9：混合應稅與免稅或零稅率 (限收銀機發票無法分辨時使用)");
            entity.Property(e => e.UnitCost)
                .HasComment("單價")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.UnitCost2)
                .HasComment("單價")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.UnitFreight)
                .HasComment("每單位運費")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.Weight)
                .HasComment("重量")
                .HasColumnType("decimal(18, 5)");
            entity.Property(e => e.WeightUnit)
                .HasMaxLength(16)
                .HasComment("重量單位");

            entity.HasOne(d => d.Invoice).WithMany(p => p.NegoInvoiceDetail)
                .HasForeignKey(d => d.InvoiceID)
                .HasConstraintName("FK_NegoInvoiceDetail_NegoInvoice");
        });

        modelBuilder.Entity<NegoLC>(entity =>
        {
            entity.HasKey(e => e.LcID);

            entity.HasIndex(e => e.DownloadFlag, "IX_NegoLC_1");

            entity.Property(e => e.LcID).ValueGeneratedNever();
            entity.Property(e => e.AdvisingBank).HasMaxLength(10);
            entity.Property(e => e.DownloadFlag).HasDefaultValue((short)0);
            entity.Property(e => e.ImportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IssuingBank)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.LCType).HasMaxLength(3);
            entity.Property(e => e.PayableBank).HasMaxLength(10);
            entity.Property(e => e.Status).HasDefaultValue(0);

            entity.HasOne(d => d.AdvisingBankNavigation).WithMany(p => p.NegoLCAdvisingBankNavigation)
                .HasForeignKey(d => d.AdvisingBank)
                .HasConstraintName("FK_NegoLC_BankData1");

            entity.HasOne(d => d.ApplicantDetails).WithMany(p => p.NegoLCApplicantDetails)
                .HasForeignKey(d => d.ApplicantDetailsID)
                .HasConstraintName("FK_NegoLC_CustomerOfBranchVersion");

            entity.HasOne(d => d.BeneDetails).WithMany(p => p.NegoLCBeneDetails)
                .HasForeignKey(d => d.BeneDetailsID)
                .HasConstraintName("FK_NegoLC_CustomerOfBranchVersion1");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.NegoLC)
                .HasForeignKey(d => d.BeneficiaryID)
                .HasConstraintName("FK_NegoLC_BeneficiaryData");

            entity.HasOne(d => d.Lc).WithOne(p => p.NegoLC)
                .HasForeignKey<NegoLC>(d => d.LcID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoLC_LetterOfCredit");

            entity.HasOne(d => d.PayableBankNavigation).WithMany(p => p.NegoLCPayableBankNavigation)
                .HasForeignKey(d => d.PayableBank)
                .HasConstraintName("FK_NegoLC_BankData2");

            entity.HasOne(d => d.CustomerOfBranch).WithMany(p => p.NegoLC)
                .HasForeignKey(d => new { d.IssuingBank, d.CompanyID })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NegoLC_CustomerOfBranch");
        });

        modelBuilder.Entity<NegoLoan>(entity =>
        {
            entity.HasKey(e => e.LoanID);

            entity.Property(e => e.ConfirmDate).HasColumnType("datetime");
            entity.Property(e => e.FiaDate).HasMaxLength(64);
            entity.Property(e => e.FiaTxseq).HasMaxLength(64);
            entity.Property(e => e.OtherAppointment).HasMaxLength(64);
            entity.Property(e => e.RateMemo).HasMaxLength(64);
            entity.Property(e => e.RejectDate).HasMaxLength(64);
            entity.Property(e => e.RejectReason).HasMaxLength(64);

            entity.HasOne(d => d.Draft).WithMany(p => p.NegoLoan)
                .HasForeignKey(d => d.DraftID)
                .HasConstraintName("FK_NegoLoan_NegoDraft");
        });

        modelBuilder.Entity<NegoPrompt>(entity =>
        {
            entity.HasKey(e => e.PromptID);

            entity.Property(e => e.ImportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Host).WithMany(p => p.NegoPrompt)
                .HasForeignKey(d => d.HostID)
                .HasConstraintName("FK_NegoPrompt_NegoHost");

            entity.HasOne(d => d.Log).WithMany(p => p.NegoPrompt)
                .HasForeignKey(d => d.LogID)
                .HasConstraintName("FK_NegoPrompt_DataPortLog");
        });

        modelBuilder.Entity<NegoPromptRequestQueue>(entity =>
        {
            entity.HasKey(e => e.DataPortLogID);

            entity.Property(e => e.DataPortLogID).ValueGeneratedNever();

            entity.HasOne(d => d.DataPortLog).WithOne(p => p.NegoPromptRequestQueue)
                .HasForeignKey<NegoPromptRequestQueue>(d => d.DataPortLogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NegoPromptRequestQueue_DataPortLog");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.CompanyID)
                .HasName("PK12")
                .IsClustered(false);

            entity.Property(e => e.CompanyID).ValueGeneratedNever();
            entity.Property(e => e.Addr).HasMaxLength(256);
            entity.Property(e => e.CompanyName).HasMaxLength(128);
            entity.Property(e => e.ContactEmail).HasMaxLength(50);
            entity.Property(e => e.ContactFax).HasMaxLength(20);
            entity.Property(e => e.ContactMobilePhone).HasMaxLength(20);
            entity.Property(e => e.ContactName).HasMaxLength(50);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.EnglishAddr).HasMaxLength(256);
            entity.Property(e => e.EnglishName).HasMaxLength(50);
            entity.Property(e => e.EnglishRegAddr).HasMaxLength(256);
            entity.Property(e => e.Fax).HasMaxLength(50);
            entity.Property(e => e.InvoiceSignature).HasMaxLength(64);
            entity.Property(e => e.LogoURL).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ReceiptNo)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.RegAddr).HasMaxLength(256);
            entity.Property(e => e.UndertakerFax).HasMaxLength(20);
            entity.Property(e => e.UndertakerID).HasMaxLength(50);
            entity.Property(e => e.UndertakerMobilePhone).HasMaxLength(20);
            entity.Property(e => e.UndertakerName).HasMaxLength(50);
            entity.Property(e => e.UndertakerPhone).HasMaxLength(20);

            entity.HasMany(d => d.Company).WithMany(p => p.Enterprise)
                .UsingEntity<Dictionary<string, object>>(
                    "EnterpriseRelation",
                    r => r.HasOne<Organization>().WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EnterpriseRelation_Organization1"),
                    l => l.HasOne<Organization>().WithMany()
                        .HasForeignKey("EnterpriseID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EnterpriseRelation_Organization"),
                    j =>
                    {
                        j.HasKey("EnterpriseID", "CompanyID");
                    });

            entity.HasMany(d => d.Enterprise).WithMany(p => p.Company)
                .UsingEntity<Dictionary<string, object>>(
                    "EnterpriseRelation",
                    r => r.HasOne<Organization>().WithMany()
                        .HasForeignKey("EnterpriseID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EnterpriseRelation_Organization"),
                    l => l.HasOne<Organization>().WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EnterpriseRelation_Organization1"),
                    j =>
                    {
                        j.HasKey("EnterpriseID", "CompanyID");
                    });
        });

        modelBuilder.Entity<OrganizationBranchSettings>(entity =>
        {
            entity.HasKey(e => e.SettingID);

            entity.Property(e => e.BankCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.HandlingCharge).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StepCharge).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.BankCodeNavigation).WithMany(p => p.OrganizationBranchSettings)
                .HasForeignKey(d => d.BankCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationBranchSettings_BankData");

            entity.HasOne(d => d.Company).WithMany(p => p.OrganizationBranchSettings)
                .HasForeignKey(d => d.CompanyID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationBranchSettings_Organization");

            entity.HasOne(d => d.Log).WithMany(p => p.OrganizationBranchSettings)
                .HasForeignKey(d => d.LogID)
                .HasConstraintName("FK_OrganizationBranchSettings_RevisionLog");
        });

        modelBuilder.Entity<OrganizationExtension>(entity =>
        {
            entity.HasKey(e => e.CompanyID);

            entity.Property(e => e.CompanyID).ValueGeneratedNever();
            entity.Property(e => e.CA_NO).HasMaxLength(64);
            entity.Property(e => e.CP_ACCOUNT).HasMaxLength(32);
            entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");
            entity.Property(e => e.PID).HasMaxLength(50);
            entity.Property(e => e.SIGNET).HasMaxLength(32);
            entity.Property(e => e.STATUS_CODE).HasMaxLength(16);

            entity.HasOne(d => d.Company).WithOne(p => p.OrganizationExtension)
                .HasForeignKey<OrganizationExtension>(d => d.CompanyID)
                .HasConstraintName("FK_OrganizationExtension_Organization");

            entity.HasOne(d => d.CustomerOfBranchVersion).WithMany(p => p.OrganizationExtension)
                .HasForeignKey(d => d.CustomerOfBranchVersionID)
                .HasConstraintName("FK_OrganizationExtension_CustomerOfBranchVersion");
        });

        modelBuilder.Entity<OrganizationStatus>(entity =>
        {
            entity.HasKey(e => e.CompanyID);

            entity.Property(e => e.CompanyID).ValueGeneratedNever();
            entity.Property(e => e.CustomerID).HasMaxLength(16);
            entity.Property(e => e.HandlingCharge).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ReserveAccount).HasMaxLength(32);
            entity.Property(e => e.ReserveAccountName).HasMaxLength(64);
            entity.Property(e => e.ReserveAccountReceiptNo).HasMaxLength(10);
            entity.Property(e => e.StepCharge).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Company).WithOne(p => p.OrganizationStatus)
                .HasForeignKey<OrganizationStatus>(d => d.CompanyID)
                .HasConstraintName("FK_OrganizationStatus_Organization");

            entity.HasOne(d => d.Group).WithMany(p => p.OrganizationStatus)
                .HasForeignKey(d => d.GroupID)
                .HasConstraintName("FK_OrganizationStatus_BeneficiaryGroup");

            entity.HasOne(d => d.Log).WithMany(p => p.OrganizationStatus)
                .HasForeignKey(d => d.LogID)
                .HasConstraintName("FK_OrganizationStatus_RevisionLog");
        });

        modelBuilder.Entity<ReceivedDataQueue>(entity =>
        {
            entity.HasKey(e => e.DataPortLogID);

            entity.Property(e => e.DataPortLogID).ValueGeneratedNever();

            entity.HasOne(d => d.DataPortLog).WithOne(p => p.ReceivedDataQueue)
                .HasForeignKey<ReceivedDataQueue>(d => d.DataPortLogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivedDataQueue_DataPortLog");
        });

        modelBuilder.Entity<ResponseDataQueue>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.Property(e => e.LogID).ValueGeneratedNever();

            entity.HasOne(d => d.Log).WithOne(p => p.ResponseDataQueue)
                .HasForeignKey<ResponseDataQueue>(d => d.LogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResponseDataQueue_DataPortLog");

            entity.HasOne(d => d.Service).WithMany(p => p.ResponseDataQueue)
                .HasForeignKey(d => d.ServiceID)
                .HasConstraintName("FK_ResponseDataQueue_BeneficiaryServiceGroup");
        });

        modelBuilder.Entity<RevisionLog>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.Property(e => e.Actor)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.RevisionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ServiceDataQueue>(entity =>
        {
            entity.HasKey(e => e.DataPortLogID);

            entity.Property(e => e.DataPortLogID).ValueGeneratedNever();

            entity.HasOne(d => d.DataPortLog).WithOne(p => p.ServiceDataQueue)
                .HasForeignKey<ServiceDataQueue>(d => d.DataPortLogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceDataQueue_DataPortLog");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceDataQueue)
                .HasForeignKey(d => d.ServiceID)
                .HasConstraintName("FK_ServiceDataQueue_BeneficiaryServiceGroup");
        });

        modelBuilder.Entity<SpecificNotes>(entity =>
        {
            entity.HasKey(e => e.NoteID);

            entity.Property(e => e.CSCSalesDept).HasMaxLength(8);
            entity.Property(e => e.SpecialMessageForCS).HasMaxLength(16);
            entity.Property(e => e.其他).HasMaxLength(1024);
            entity.Property(e => e.分批交貨).HasDefaultValue(false);
            entity.Property(e => e.原留印鑑相符).HasDefaultValue(false);
            entity.Property(e => e.受益人單獨蓋章).HasDefaultValue(false);
            entity.Property(e => e.押匯發票起始日).HasColumnType("datetime");
            entity.Property(e => e.押匯起始日).HasColumnType("datetime");
            entity.Property(e => e.最後交貨日).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ROL_NAME)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.HasSequence<int>("OrgCompanyID");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
