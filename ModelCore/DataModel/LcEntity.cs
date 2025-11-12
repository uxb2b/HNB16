using CommonLib.Core.DataWork;
using CommonLib.DataAccess;
using Microsoft.EntityFrameworkCore;
using ModelCore.Properties;
using System.Data.Linq.Mapping;

namespace ModelCore.DataModel
{
    //public partial class LcEntityDbContext
    //{
    //    public LcEntityDbContext() :
    //        base(ModelCore.Properties.Settings.Default.ConnectionString, mappingSource)
    //    {
    //        OnCreated();
    //    }

    //    partial void OnCreated()
    //    {
    //        this.CommandTimeout = 86400;
    //    }
    //}

    public partial class LcEntityDbContext
    {
        private readonly SqlLogger? _logWriter;
        public LcEntityDbContext()
        {
            if (CommonLib.Core.Properties.AppSettings.Default.SqlLog)
            {
                _logWriter = new SqlLogger { IgnoreSelect = CommonLib.Core.Properties.AppSettings.Default.SqlLogIgnoreSelect };
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(AppSettings.Default.ConnectionString)
                .LogTo((sql) =>
                {
                    _logWriter?.WriteLine(sql);
                })
                .UseLazyLoadingProxies(); // << ¶}±Ò Lazy Loading Proxy

        public override void Dispose()
        {
            base.Dispose();
            _logWriter?.Dispose();
        }

    }



}
