﻿using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IResourceServices : IBaseServices<EResourceInfo>
    {
 
        /// <summary>
        /// 上传OSS
        /// </summary>
        /// <param name="localfilePath"></param>
        /// <param name="owner"></param>
        /// <param name="isCover">是否覆盖原有文件</param>
        /// <returns></returns>
        ResultNormal uploadToOss(string localfilePath,string ossKey,bool isCover = true);

        /// <summary>
        /// 请求Oss资源 ，生成临时Url
        /// </summary>
        /// <returns></returns>
        ResponseRes requireResOss(string ossPath);

        /// <summary>
        /// 记录下载请求
        /// </summary>
        void logRequireRes(string resCode, string requireUserId);

        bool IsRepeatRes(string userId, string refCode, ResType resType, string fileType, bool includeDelete = false);

        /// <summary>
        /// 检查OSS
        /// </summary>
        /// <param name="ossPath"></param>
        /// <returns></returns>
        bool ossExist(string ossPath);

        /// <summary>
        /// 删除OSS
        /// </summary>
        /// <param name="ossPath"></param>
        /// <returns></returns>
        ResultNormal ossDelete(string ossPath);

        ResultNormal ossMove(string fromPath,string toPath);
        /// <summary>
        /// 删除数据库的资源,删除时资源对应的评论，点赞都需要被删除
        /// </summary>
        /// <param name="deleteRes"></param>
        /// <returns></returns>
        int deleteResInDb(DeleteRes deleteRes);

        ResultNormal deleteResource(DeleteRes deleteRes);

        /// <summary>
        /// 保存Res到数据库
        /// </summary>
        /// <param name="ossRes"></param>
        /// <returns></returns>
        ResultEntity<EResourceInfo> saveResToDb(EResourceInfo resInfo);

        List<EResourceInfo> getFilesByOwner(string refCode, string owner, bool includeDelete = false);

        ModelPager<VueUserRes> queryUserRes(QUserRes query);
        /// <summary>
        /// 获取某本书的所有资源
        /// </summary>
        /// <param name="qRes"></param>
        /// <returns></returns>
        ModelPager<VueResInfo> getResByRefCode(QRes qRes);

        EResourceInfo get(string pkCode, bool includeDelete = false);
    }
}
