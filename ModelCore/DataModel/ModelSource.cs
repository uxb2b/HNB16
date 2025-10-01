using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.DataAccess;
using ModelCore.Locale;


namespace ModelCore.DataModel
{
    public partial class ModelSource<TEntity> : ModelSource<LcEntityDataContext, TEntity>
        where TEntity : class,new()
    {
        public ModelSource() : base() { }
        public ModelSource(GenericManager<LcEntityDataContext> manager) : base(manager) { }

        //public Naming.DataResultMode ResultModel
        //{
        //    get;
        //    set;
        //}

    }

    public partial class ModelSourceInquiry<TEntity> : ModelSourceInquiry<LcEntityDataContext, TEntity>
        where TEntity : class,new()
    {

    }

    public class ModelSource : GenericManager<LcEntityDataContext>
    {

        public ModelSource() : base() { }
        public ModelSource(GenericManager<LcEntityDataContext> manager) : base(manager) { }

    }

}
