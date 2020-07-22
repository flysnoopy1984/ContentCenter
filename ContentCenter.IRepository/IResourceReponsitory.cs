using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IResourceReponsitory: IBaseRepository<EResourceInfo>
    {

        Task<bool> LogicDelete(string resCode);


    }
}
