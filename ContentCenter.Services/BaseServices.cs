using ContentCenter.IRepository;
using ContentCenter.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class BaseServices<T>:IBaseServices<T> where T:class,new()
    {
        private IBaseRepository<T> _baseRepository;
        public BaseServices(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
    }
}
