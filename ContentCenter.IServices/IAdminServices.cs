using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IAdminServices: IBaseServices<ESectionTag>
    {
        /// <summary>
        /// 获取Section下有多少个Tag和多少个未使用的Tag
        /// </summary>
        /// <param name="tagNumber">获取Tag的数量</param>
        /// <returns></returns>
        RCSectionTagRelation GetSectionTagRelation(string secCode,int tagNumber);

        /// <summary>
        /// (事务方法)保存Section和Tag关系。(删除所有Section下的Tag,批量插入Tag)
        /// </summary>
        bool SaveSectionTag(ESection section, List<ETag> tagList);

       
    }
}
