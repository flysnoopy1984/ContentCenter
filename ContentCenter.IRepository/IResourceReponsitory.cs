using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IResourceReponsitory: IBaseRepository<EResourceInfo>
    {

        Task<bool> LogicDelete(string resCode);

        Task<int> SameResCount(string refCode, ResType resType, string fileType, bool includeDelete = false);

        Task<ModelPager<VueResInfo>> GetResByRefCode(QRes qRes);
       
    }
}
