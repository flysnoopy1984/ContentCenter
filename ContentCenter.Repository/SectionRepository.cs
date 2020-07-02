using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Repository
{
    public class SectionRepository : BaseRepository<ESection>, ISectionRepository
    {
        public SectionRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }
    }
}
