using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Repository
{
    public class CommentReponsitory : BaseRepository<ECommentInfo>,ICommentRepository 
    { 
        public CommentReponsitory(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }
    }

}
