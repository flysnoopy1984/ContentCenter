using ContentCenter.IServices;
using ContentCenter.IRepository;
using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using ContentCenter.Model.Commons;

namespace ContentCenter.Services
{
    public class SystemServices : BaseServices<ESysConfig>, ISystemServices
    {
        private ISystemRepository _systemRepository;
        private IMemoryCache _memoryCache;

        public SystemServices(ISystemRepository systemRepository, IMemoryCache memoryCache)
         : base(systemRepository)
        {
            _systemRepository = systemRepository;
            _memoryCache = memoryCache;
        }

        public ESysConfig GetSysConfig()
        {
            return _memoryCache.GetOrCreate<ESysConfig>(CConstants.MemoryKey_SysConfig, a =>
            {
                a.SetAbsoluteExpiration(TimeSpan.FromDays(90));
                return _systemRepository.GetByKey("ContentCenter").Result;
            });
           
        }
    }
}
