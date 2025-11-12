using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Core.DataWork;
using ModelCore.Locale;


namespace ModelCore.DataModel
{
    public partial class ModelSource<TEntity> : ModelSource<LcEntityDbContext, TEntity>
        where TEntity : class,new()
    {
        public ModelSource() : base() { }
        public ModelSource(GenericManager<LcEntityDbContext> manager) : base(manager) { }

        //public Naming.DataResultMode ResultModel
        //{
        //    get;
        //    set;
        //}

    }

    public partial class ModelSourceInquiry<TEntity> : ModelSourceInquiry<LcEntityDbContext, TEntity>
        where TEntity : class,new()
    {

    }

    public class ModelSource : GenericManager<LcEntityDbContext>
    {

        public ModelSource() : base() { }
        public ModelSource(GenericManager<LcEntityDbContext> manager) : base(manager) { }

    }

}
