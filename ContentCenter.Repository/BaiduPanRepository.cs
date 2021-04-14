using ContentCenter.IRepository;
using ContentCenter.Model.BaseEnum;
using ContentCenter.Model.ThirdPart.Baidu;
using ContentCenter.Model.ThirdPart.DouBan;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Repository
{
    public class BaiduPanRepository: BaseRepository<panAccessToken>, IBaiduPanRepository
    {
        public BaiduPanRepository(ISqlSugarClient[] sugarClient)
         : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public void SaveToTempDb(List<panBookInfo> fileList)
        {
          
            Db.Saveable(fileList).ExecuteCommand();
        }

        public void expireAllAccessToken()
        {
            var op = Db.Updateable<panAccessToken>()
                .SetColumns(a => new panAccessToken() { IsExpired = true })
                .Where(a=>a.IsExpired == false);
            op.ExecuteCommand();
        }

        public panAccessToken getAvaliableAccessToken()
        {
            panAccessToken panAccessToken = Db.Queryable<panAccessToken>()
                .Where(a => a.IsExpired == false)
                .First();
      
            return panAccessToken;
        }
     

        public List<panBookInfo> queryPanBookByPath(panQueryFile query)
        {
   
            var path = query.rootPath;
            var convertStatus = query.convertStatus;

            var q = Db.Queryable<panBookInfo>();
            q = q.WhereIF(!string.IsNullOrEmpty(path) && path!="/", a => a.parentPath == path);
            q = q.WhereIF(convertStatus != panConvertStatus.All, a => a.ConvertStatus == convertStatus);
            
            var panList =  q.ToPageList(query.pageIndex,query.pageSize);
      
            return panList;
        }

        //书本对应的豆瓣搜索结果
        public List<RSearchOneBookResult> queryDouBanSearch(panQueryFile query)
        {
            var convertStatus = query.convertStatus;
            var path = query.rootPath;
            var sql = Db.Queryable<panBookInfo, ESearchOneBookResult>((b, s) => new object[]
             {
                 JoinType.Inner,b.fsId == s.fsId
             })
            .WhereIF(!string.IsNullOrEmpty(path) && path != "/", b => b.parentPath == path)
            .WhereIF(convertStatus != panConvertStatus.All, b => b.ConvertStatus == convertStatus)
            .Select((b, s) => new RSearchOneBookResult
            {
                Code = s.Code,
                CoverUrl = s.CoverUrl,
                fsId = s.fsId,
                Id = s.Id,
                keyWord = s.keyWord,
                Name = s.Name
            });

            return sql.ToList();
        }

        //更新临时文件名
        public void updateTempFileName(List<panBookInfo> updateBookList)
        {
            Db.Updateable(updateBookList).UpdateColumns(b => new { b.filename }).ExecuteCommand();
        }

        //绑定DouBanCode
        public void bindDouBanCode(List<panBookInfo> updateBookList)
        {
            Db.Updateable(updateBookList).UpdateColumns(
                b => new { b.DoubanCode,b.ConvertStatus }
            ).ExecuteCommand();
        }

        /*更新临时文件名 
         *绑定DouBanCode
         */
        public void SaveUpdateBook(List<panBookInfo> updateBookList)
        {
            Db.Updateable(updateBookList).UpdateColumns(b => new { 
                b.filename, 
                b.DoubanCode, 
                b.ConvertStatus 
            }).ExecuteCommand();
        }

        //删除临时Book
        public void DeleteTempDb(List<panBookInfo> fileList)
        {
            int r = Db.Deleteable(fileList).ExecuteCommand();
        }

    }
}
