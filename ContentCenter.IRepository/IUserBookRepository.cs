using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IUserBookRepository: IBaseRepository<EUserBook>
    {
        public Task<int> HasFavBook(string bookCode, string userId); 
        public Task<long> AddUserBook(string bookCode,string userId);
        public Task<bool> DelUserBook(string bookCode, string userId);

        public Task<ModelPager<VueUserBook>> queryUserBook(QUserBook query);
    }
}
