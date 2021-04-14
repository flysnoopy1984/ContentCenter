using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.BaseEnum
{
    /// <summary>
    /// 网盘转换成ccBook 状态。
    /// 2. 找到对应的DouBan Code
    /// 100.转换到数据库中。 
    /// </summary>
    public enum panConvertStatus
    {
            NotInDb = -1,
            All = 0,
            CreatePanBook = 1,
            FixName = 2,
            GetDouBanRelation = 5,
            ConvertToCCBook = 100,
    }

    public enum submitbookType
    {
         rootDir = 0, //提交父文件夹路径
         SingleFile = 1, //
         groupList = 10, //一个文件夹包含多个下载资源。
    }

    public enum panUpdateType
    {
        changeFileName = 0,
        bindDouBan = 10,

        changeNameAndBindDouBan = 100,
        deleteTempBook = -1,
    }

    public enum bookSearchResult
    {
        notFound = -1,
        Searched = 1,
        NotSearch =0,
    }
}
