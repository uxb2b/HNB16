using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class LetterOfCreditVersion
{
    public int LcID { get; set; }

    public int VersionNo { get; set; }

    public int? LcItemsID { get; set; }

    public int? SpecificNotesID { get; set; }

    public int? AttachableDocumentID { get; set; }

    public int VersionID { get; set; }

    public int? AmendingLcInformationID { get; set; }

    public string NotifyingBank { get; set; }

    public virtual ICollection<AmendingLcApplication> AmendingLcApplication { get; set; } = new List<AmendingLcApplication>();

    public virtual AmendingLcInformation AmendingLcInformation { get; set; }

    public virtual AttachableDocument AttachableDocument { get; set; }

    public virtual LetterOfCredit Lc { get; set; }

    public virtual LcItems LcItems { get; set; }

    public virtual ICollection<NegoDraft> NegoDraft { get; set; } = new List<NegoDraft>();

    public virtual SpecificNotes SpecificNotes { get; set; }
}
