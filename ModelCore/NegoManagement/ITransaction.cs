using System.Collections;

namespace ModelCore.NegoManagement
{
    public interface INegoDraftTransaction
    {
        bool DoTransaction(IList draftNo, string hostUrl);
    }

    public interface ITransaction
    {
        bool DoTransaction(object paramValue, string hostUrl);
    }

    public interface IMarkedDraft
    {
        int? DraftID
        {
            get;
        }
        string BranchNo
        {
            get;
        }
    }
}