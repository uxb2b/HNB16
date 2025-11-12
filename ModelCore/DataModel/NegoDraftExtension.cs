using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class NegoDraftExtension
{
    public int NegoDraftID { get; set; }

    public DateTime? DueDate { get; set; }

    public string NegoBranch { get; set; }

    public string BeneficiaryAccountNo { get; set; }

    public int? DraftType { get; set; }

    public string LcBranch { get; set; }

    public virtual BankData LcBranchNavigation { get; set; }

    public virtual BankData NegoBranchNavigation { get; set; }

    public virtual NegoDraft NegoDraft { get; set; }
}
