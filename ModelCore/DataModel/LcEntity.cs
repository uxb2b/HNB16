using System.Data.Linq.Mapping;

namespace ModelCore.DataModel
{
    public partial class LcEntityDataContext
    {
        public LcEntityDataContext() :
            base(ModelCore.Properties.Settings.Default.ConnectionString, mappingSource)
        {
            OnCreated();
        }

        partial void OnCreated()
        {
            this.CommandTimeout = 86400;
        }
    }


}
