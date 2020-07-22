using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class ResourceReponsitory:BaseRepository<EResourceInfo>, IResourceReponsitory
    {
        public ResourceReponsitory(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

      

        public Task<bool> LogicDelete(string resCode)
        {
            return base.UpdatePart(a => new EResourceInfo() { IsDelete = true }, a => a.Code == resCode);
        }




    }
}
