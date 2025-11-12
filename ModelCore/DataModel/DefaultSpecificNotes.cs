using System;
using System.Collections.Generic;

namespace ModelCore.DataModel;

public partial class DefaultSpecificNotes
{
    public int CompanyID { get; set; }

    public int NoteID { get; set; }

    public virtual Organization Company { get; set; }

    public virtual SpecificNotes Note { get; set; }
}
