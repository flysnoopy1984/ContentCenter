using Aliyun.OSS;
using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using IQB.Util.Models;
using IQB.Util.Models.Oss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace ContentCenter.Services
{
    public class ResourceServices: BaseServices<EResourceInfo>, IResourceServices
    {
        private OssConfig _OssConfig;
        private IResourceReponsitory _ResourceReponsitory;
        private OssClient _ossClient;

        protected OssClient OssClient
        {
            get
            {
                if(_ossClient == null)
                    _ossClient = new OssClient(_OssConfig.endpoint, _OssConfig.accessKeyId, _OssConfig.accessKeySecret);
                return _ossClient;
            }
        }
        public ResourceServices(OssConfig ossConfig, IResourceReponsitory resourceReponsitory) : base(resourceReponsitory)
        {
            _ResourceReponsitory = resourceReponsitory;
            _OssConfig = ossConfig;
        }

        public List<EResourceInfo> getFilesByOwner(string refCode, string owner, bool ignoreDelete = false)
        {
            Expression<Func<EResourceInfo, bool>> whereExp = a => a.RefCode == refCode && a.Owner == owner;
            if (!ignoreDelete)
                whereExp = a => a.RefCode == refCode && a.Owner == owner && a.IsDelete == false;

            return _ResourceReponsitory.QueryList(whereExp, null).Result;
        }

        public bool ossExist(string ossPath)
        {
            try
            {
                return OssClient.DoesObjectExist(_OssConfig.bucketName, ossPath);
            }
            catch(Exception ex)
            {
                throw ex;
            }  
        }


        public ResultNormal saveResToDb(EResourceInfo resInfo)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                var r = _ResourceReponsitory.SaveMasterData<EResourceInfo>(resInfo).Result;
                //_ResourceReponsitory.LogicDelete(resInfo.Code);

                // = _ResourceReponsitory.Add(resInfo).Result;
              
                if (r<=0)  result.ErrorMsg = "没有保存任何数据";
  
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        public ResultNormal uploadBookToOss(string localfilePath,string ossKey, bool isCover = true)
        {
            ResultNormal result = new ResultNormal();
            try
            {

                if(isCover || (!isCover && !ossExist(ossKey)))
                {
                    OssClient.PutObject(_OssConfig.bucketName, ossKey, localfilePath);
                }

            }
            catch (Exception ex)
            {
                
                NLogUtil.cc_ErrorTxt("Oss Services-uploadFile:" + ex.Message);
                result.ErrorMsg = ex.Message;

            }
            return result;

        }

        public ResultNormal ossDelete(string ossPath)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                if (OssClient.DoesObjectExist(_OssConfig.bucketName, ossPath))
                {
                    OssClient.DeleteObject(_OssConfig.bucketName, ossPath);
                }
              
            }
            catch (Exception ex)
            {
                NLogUtil.cc_ErrorTxt("Oss Services-deleteOss:" + ex.Message);
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        ///
        public int deleteResInDb(DeleteRes deleteRes)
        {
            return  _ResourceReponsitory.LogicDelete(deleteRes.resCode).Result?1:0;
        }

        public bool IsRepeatRes(string refCode, ResType resType, string fileType, bool ignoreDelete = true)
        {
            var c = _ResourceReponsitory.GetCount(a => a.RefCode == refCode && 
            a.ResType == resType && a.FileType == fileType && (!ignoreDelete || (ignoreDelete && a.IsDelete == true))).Result;
            return c > 0;

        }

        public ResultNormal ossMove(string fromPath, string toPath)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                if (OssClient.DoesObjectExist(_OssConfig.bucketName, fromPath))
                {
                    var metadata = new ObjectMetadata();
                    //metadata.AddHeader("mk1", "mv1");
                    //metadata.AddHeader("mk2", "mv2");
                    var req = new CopyObjectRequest(_OssConfig.bucketName, fromPath, _OssConfig.bucketName, toPath)
                    {
                        NewObjectMetadata = metadata
                    };
                    // 拷贝文件。
                    OssClient.CopyObject(req);
                    OssClient.DeleteObject(_OssConfig.bucketName, fromPath);
                }
            
            }
            catch (Exception ex)
            {

                NLogUtil.cc_ErrorTxt("Oss Services-moveOss:" + ex.Message);
                result.ErrorMsg = ex.Message;

            }
            return result;
        }

        public ResultNormal deleteResource(DeleteRes deleteRes)
        {
            ResultNormal result = new ResultNormal();
            var resInfo = _ResourceReponsitory.GetByKey(deleteRes.resCode).Result;
            if (resInfo != null)
            {
                if(resInfo.ResType == ResType.BookOss)
                {
                    var toPath = OssKeyManager.BookDeletedKey(resInfo.OssPath);
                    result = ossMove(resInfo.OssPath, toPath);
                }
                if(result.IsSuccess)
                    result.Message = _ResourceReponsitory.LogicDelete(deleteRes.resCode).Result ? "1" : "0";
            }
            else
                result.ErrorMsg = "没有找到删除资源";
          
            return result;
        }

    }
}
