using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using ContentCenter.Model.Commons;
using ContentCenter.Model.ThirdPart.Baidu;
using ContentCenter.Model.ThirdPart.DouBan;
using IQB.Util;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Services
{
    public class BaiduPanService : BaseServices<panAccessToken>, IBaiduPanService
    {
        private IBaiduPanRepository _baiduPanRepository;
        private ISystemRepository _systemRepository;
        private IMemoryCache _memoryCache;
        public BaiduPanService(
            IMemoryCache memoryCache,
            ISystemRepository systemRepository,
           IBaiduPanRepository baiduPanRepository)
         : base(baiduPanRepository)
        {
            _baiduPanRepository = baiduPanRepository;
            _memoryCache = memoryCache;
            _systemRepository = systemRepository;

        }


        public void SaveToTempDb(submitSaveBooks submitData)
        {
            switch (submitData.submitbookType)
            {
                case submitbookType.rootDir:
                    this.SaveToTempDbByRootDir(submitData);break;
                case submitbookType.SingleFile:
                    this.SaveToTempDbBySingle(submitData);break;
                case submitbookType.groupList:
                    this.SaveToTempDbByGroup(submitData);break;
                default:
                    return;    
            }

        }

        public panFileList requireFileList(string rootPath, string accessToken)
        {
            string url = $"https://pan.baidu.com/rest/2.0/xpan/multimedia?method=listall&path={rootPath}&access_token={accessToken}";
            return  HttpUtil.Get<panFileList>(url, "pan.baidu.com");
        }

       public panFilemetaList requireFileMateList(string rootPath,string accessToken,List<string> fsIds)
        {
            if (fsIds.Count <= 0) throw new CCException("没有合适的文件");

            string fsids = string.Join(",", fsIds);
            if (string.IsNullOrEmpty(rootPath))
                rootPath = "/";
            string url = @$"https://pan.baidu.com/rest/2.0/xpan/multimedia?method=filemetas&path={rootPath}&fsids=[{fsids}]&dlink=1&access_token={accessToken}";
           var result =    HttpUtil.Get<panFilemetaList>(url, "pan.baidu.com");
            return result;
        }

        public RpanAccessToken getAvaliableAccessToken()
        {
            var sysConfig = _memoryCache.GetOrCreate(CConstants.MemoryKey_SysConfig, a =>
            {
                a.SetAbsoluteExpiration(TimeSpan.FromDays(90));
                return _systemRepository.GetByKey("ContentCenter").Result;
            });
            RpanAccessToken result = new RpanAccessToken();

            panAccessToken panAccessToken =_memoryCache.Get<panAccessToken>(CConstants.MemoryKey_BaiduPanAccessToken);
          
        
            if (panAccessToken == null)
            {
                panAccessToken = _baiduPanRepository.getAvaliableAccessToken();
                if(panAccessToken == null) {
                    return result;
                }
                _memoryCache.Set(CConstants.MemoryKey_BaiduPanAccessToken, panAccessToken, TimeSpan.FromHours(1));
            }
            //现在时间和创建AccessToken时间差
            int consumDay = ToolUtil.DateDiff(DateTime.Now, panAccessToken.createDateTime);
            int remainDay = sysConfig.baiduPanTokenExpiredDay - consumDay;
            if (remainDay <= 0)
            {
               
                _baiduPanRepository.expireAllAccessToken();
                _memoryCache.Remove(CConstants.MemoryKey_BaiduPanAccessToken);
            }
            else
            {
                result.remainDay = remainDay;
                result.panAccessToken = panAccessToken;
            }
          
            return result;
        }

        public int SaveAccessToken(panAccessToken panAccessToken)
        {
            if (panAccessToken.Id <= 0)
            {
                panAccessToken.createDateTime = DateTime.Now;
                _baiduPanRepository.expireAllAccessToken();
                return _baiduPanRepository.AddNoIdentity_Sync(panAccessToken);
            }
            return 0;

        }


        public List<RpanBookInfo> queryPanBookByPath(panQueryFile query)
        {
            List<RpanBookInfo> result = new List<RpanBookInfo>();

            var panList =  _baiduPanRepository.queryPanBookByPath(query);
            List<RSearchOneBookResult> searchList = null;
            if (query.querySearchResultNum > 0)
            {
                searchList = _baiduPanRepository.queryDouBanSearch(query);
            }
            foreach(var pan in panList)
            {
                RpanBookInfo rObj = new RpanBookInfo();
                if (searchList !=null && searchList.Count > 0)
                {
                    rObj.searchList = searchList.FindAll(s => s.fsId == pan.fsId).Take(query.querySearchResultNum).ToList();
                }
                rObj.panBookInfo = pan;
                result.Add(rObj);
            }
            return result;


        }

        public void updateTempFile(submitUpdateBook submitUpdate)
        {
            switch(submitUpdate.panUpdateType)
            {
                case panUpdateType.changeFileName:
                    _baiduPanRepository.updateTempFileName(submitUpdate.updateBookList);
                    break;
                case panUpdateType.bindDouBan:
                    _baiduPanRepository.bindDouBanCode(submitUpdate.updateBookList);
                    break;
                case panUpdateType.deleteTempBook:
                    _baiduPanRepository.DeleteTempDb(submitUpdate.updateBookList);
                    break;
                case panUpdateType.changeNameAndBindDouBan:
                    _baiduPanRepository.SaveUpdateBook(submitUpdate.updateBookList);
                    break;

            }

        }

     
        #region 私有逻辑方法


        private void SaveToTempDbBySingle(submitSaveBooks submitData)
        {
            List<panBookInfo> panBookList = submitData.panBookList;
            if(panBookList == null || panBookList.Count ==0)
                throw new CCException("没有书本数据传入");
            List<string> fsIds = new List<string>();
            foreach (var panBook in panBookList)
            {
                if (panBook.isdir == 1) return;
                panBook.InitBackFields(submitData.sectionCode);

                if (panBookInfo.IsSupportFileType(panBook.FileType))
                    fsIds.Add(panBook.fsId);
            }
            var downList = requireFileMateList(null,submitData.accessToken, fsIds);
            if (downList.list.Count == 0) throw new CCException("接口返回空下载地址");

            foreach (var downInfo in downList.list)
            {
                var panBook = panBookList.Find(b => b.fsId == downInfo.fs_id);
                if (panBook == null) throw new CCException("没有找到下载地址");
                panBook.dlink = downInfo.dlink;
            }
            panBookList.RemoveAll(b => b.dlink == null || b.dlink == "");
            _baiduPanRepository.SaveToTempDb(panBookList);
        }
        private void SaveToTempDbByRootDir(submitSaveBooks submitData)
        {
            panFileList panList = requireFileList(submitData.startPath, submitData.accessToken);

            List<panBookInfo> panBookList = new List<panBookInfo>();
            List<string> fsIds = new List<string>();
            foreach (var panFile in panList.list)
            {
                if (panFile.isdir == 1) continue;
                var panBook = panFile.toPanBook(submitData.startPath);
                if (panBookInfo.IsSupportFileType(panBook.FileType))
                {
                    panBookList.Add(panBook);
                    fsIds.Add(panFile.fs_id);
                }
            }

            var downList = requireFileMateList(submitData.startPath, submitData.accessToken, fsIds);
            if (downList.list.Count == 0) throw new CCException("接口返回空下载地址");
            foreach (var downInfo in downList.list)
            {
                var panBook = panBookList.Find(b => b.fsId == downInfo.fs_id);
                if (panBook == null)
                    throw new CCException("没有找到下载地址");

                panBook.ConvertStatus = panConvertStatus.CreatePanBook;
                panBook.dlink = downInfo.dlink;
            }
            panBookList.RemoveAll(b => b.dlink == null || b.dlink == "");

            _baiduPanRepository.SaveToTempDb(panBookList);
        }
        private void SaveToTempDbByGroup(submitSaveBooks submitData)
        {
            List<string> fsIds = new List<string>();
            var panBookList = submitData.panBookList;
            foreach (var panBook in panBookList)
            {
                if (panBook.isdir == 1) continue;
                panBook.InitBackFields(submitData.sectionCode);

                if (panBookInfo.IsSupportFileType(panBook.FileType))
                    fsIds.Add(panBook.fsId);    
            }
          
            var downList = requireFileMateList(submitData.startPath, submitData.accessToken, fsIds);
            if (downList.list.Count == 0) throw new CCException("接口返回空下载地址");
            foreach (var downInfo in downList.list)
            {
                var panBook = panBookList.Find(b => b.fsId == downInfo.fs_id);
                if (panBook == null) throw new CCException("没有找到下载地址");

                if(string.IsNullOrEmpty(panBook.GroupName)) throw new CCException("书本没有组名称");
                panBook.dlink = downInfo.dlink;

            }
            panBookList.RemoveAll(b => b.dlink == null || b.dlink == "");
            _baiduPanRepository.SaveToTempDb(panBookList);
        }

       
        #endregion
    }

}
