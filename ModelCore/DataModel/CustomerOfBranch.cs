using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class CustomerOfBranch
{
    public string BankCode { get; set; }

    public int OrganizationID { get; set; }

    public string PayableAccount { get; set; }

    public string Addr { get; set; }

    public string Phone { get; set; }

    public string ContactEmail { get; set; }

    public string Undertaker { get; set; }

    public string CompanyName { get; set; }

    public int? Status { get; set; }

    public int? CustomerOfBranchVersionID { get; set; }

    public int? CurrentLevel { get; set; }

    public int? PostponeMonths { get; set; }

    public int? UsancelimitedDays { get; set; }

    public virtual BankData BankCodeNavigation { get; set; }

    public virtual ICollection<CreditApplicationDocumentary> CreditApplicationDocumentary { get; set; } = new List<CreditApplicationDocumentary>();

    public virtual LevelExpression CurrentLevelNavigation { get; set; }

    public virtual CustomerOfBranchVersion CustomerOfBranchVersion { get; set; }

    public virtual ICollection<FpgLcMonthlyReport> FpgLcMonthlyReport { get; set; } = new List<FpgLcMonthlyReport>();

    public virtual ICollection<NegoLC> NegoLC { get; set; } = new List<NegoLC>();

    public virtual Organization Organization { get; set; }
}
