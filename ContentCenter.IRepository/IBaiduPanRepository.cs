using ContentCenter.Model.BaseEnum;
using ContentCenter.Model.ThirdPart.Baidu;
using ContentCenter.Model.ThirdPart.DouBan;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IRepository
{
    public interface IBaiduPanRepository : IBaseRepository<panAccessToken>
    {
        panAccessToken getAvaliableAccessToken();

        void expireAllAccessToken();

        void SaveToTempDb(List<panBookInfo> fileList);

        void SaveUpdateBook(List<panBookInfo> updateBookList);

        void DeleteTempDb(List<panBookInfo> fileList);

        //根据父Path 查询孩子所有Book
        List<panBookInfo> queryPanBookByPath(panQueryFile query);

        void updateTempFileName(List<panBookInfo> updateBookList);

        void bindDouBanCode(List<panBookInfo> updateBookList);

        List<RSearchOneBookResult> queryDouBanSearch(panQueryFile query);
    }
}
