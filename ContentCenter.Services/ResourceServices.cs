using Aliyun.OSS;
using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using IQB.Util.Models;
using IQB.Util.Models.Oss;
using SqlSugar;
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
        private ICommentRepository _commentRepository;
        private IPraizeRepository _praizeRepository;
        private IBookRepository _bookRepository;
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
        public ResourceServices(OssConfig ossConfig, 
                                IResourceReponsitory resourceReponsitory, 
                                ICommentRepository commentRepository,
                                IBookRepository bookRepository,
                                IPraizeRepository praizeRepository) : base(resourceReponsitory)
        {
            _ResourceReponsitory = resourceReponsitory;
            _commentRepository = commentRepository;
            _praizeRepository = praizeRepository;
            _bookRepository = bookRepository;
            _OssConfig = ossConfig;
        }

        public List<EResourceInfo> getFilesByOwner(string refCode, string owner, bool includeDelete = false)
        {
            var exp = Expressionable.Create<EResourceInfo>()
              .And(a => a.RefCode == refCode)
              .And(a => a.Owner == owner)
              .AndIF(!includeDelete, a => a.IsDelete == false).ToExpression();

         
            return _ResourceReponsitory.QueryList(exp, a=>a.UpdateDateTime).Result;
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


        public ResultEntity<EResourceInfo> saveResToDb(EResourceInfo resInfo)
        {
            ResultEntity<EResourceInfo> result = new ResultEntity<EResourceInfo>();
            try
            {
                var transResult = _ResourceReponsitory.Db.Ado.UseTran(()=>
                {
                    
                    _bookRepository.UpdateBookResNum(resInfo.RefCode, OperationDirection.plus);
                    _ResourceReponsitory.SaveMasterData_Sync(resInfo);
                });
                if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);
                result.Entity = resInfo;
                //var r = _ResourceReponsitory.SaveMasterData(resInfo).Result;

                //if (r<=0)  
                //    result.ErrorMsg = "没有保存任何数据";
                //result.Entity = resInfo;


            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        public ResultNormal uploadToOss(string localfilePath,string ossKey, bool isCover = true)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                var l = ossKey.Length;
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
            var transResult = _ResourceReponsitory.Db.Ado.UseTran(() =>
            {

                _bookRepository.UpdateBookResNum(deleteRes.bookCode, OperationDirection.minus);
                _ResourceReponsitory.LogicDelete(deleteRes.resCode);
            });
            if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);
            return 1;
          //  return transResult.IsSuccess
          //  return  _ResourceReponsitory.LogicDelete(deleteRes.resCode)?1:0;
        }

        public bool IsRepeatRes(string userId, string refCode, ResType resType, string fileType, bool includeDelete = false)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("用户信息获取错误");
            var c = _ResourceReponsitory.SameResCount(userId,refCode, resType,fileType, includeDelete).Result;
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
            if (string.IsNullOrEmpty(deleteRes.resCode) || string.IsNullOrEmpty(deleteRes.bookCode))
                throw new Exception("没有删除的资源");
            var resInfo = _ResourceReponsitory.GetByKey(deleteRes.resCode).Result;
            if (resInfo != null)
            {
                if(resInfo.ResType == ResType.BookOss)
                {
                    var toPath = OssKeyManager.BookDeletedKey(resInfo.OssPath);
                    result = ossMove(resInfo.OssPath, toPath);
                }
                if (result.IsSuccess)
                    deleteResInDb(deleteRes);
                
                  //  result.Message = _ResourceReponsitory.LogicDelete(deleteRes.resCode) ? "1" : "0";
            }
            else
                result.ErrorMsg = "没有找到删除资源";
          
            return result;
        }

        public EResourceInfo get(string pkCode, bool includeDelete = false)
        {
            var obj =  _ResourceReponsitory.GetByKey(pkCode).Result;
            if (!includeDelete && obj.IsDelete)
                return null;
            return obj;

        }

        public ModelPager<VueResInfo> getResByRefCode(QRes qRes)
        {
            if(string.IsNullOrEmpty(qRes.reqUserId))
                throw new Exception("非法操作！");

            var resList = _ResourceReponsitory.GetResByRefCode(qRes).Result;
            foreach(var res in resList.datas)
            {
                var commList = _commentRepository.GetCommentsByResCodes(new QComment_Res
                {
                    pageIndex = 1,
                    pageSize = qRes.withCommentNum,
                    resCode = res.resCode,
                    reqUserId = qRes.reqUserId
                }).Result;
                res.commList = commList;     
            }
            return resList;
        }

        public ResponseRes requireResOss(string ossPath)
        {
            ResponseRes oss = new ResponseRes();

            if ( string.IsNullOrEmpty(ossPath))
                throw new Exception("资源请求参数错误！");

            if (OssClient.DoesObjectExist(_OssConfig.bucketName, ossPath))
            {
                var req = new GeneratePresignedUriRequest(_OssConfig.bucketName, ossPath, SignHttpMethod.Get);
                var uri  = _ossClient.GeneratePresignedUri(req);
                oss.downloadUrl = uri.AbsoluteUri;
            }
            else
                throw new Exception("来晚了。资源失效！");
            return oss;
        }

        public void logRequireRes(string resCode,string requireUserId)
        {
            if (string.IsNullOrEmpty(resCode) || string.IsNullOrEmpty(requireUserId)){
                throw new Exception("资源请求参数错误！");
            }

            DbResult<bool> transResult = null;
            transResult = _ResourceReponsitory.Db.Ado.UseTran(() =>
            {
                _ResourceReponsitory.logRequireRes(resCode, requireUserId);
                _ResourceReponsitory.addRequireResNum(resCode);
            });

            if (transResult != null && !transResult.IsSuccess)
                throw new Exception(transResult.ErrorMessage);

        }

        public ModelPager<VueUserRes> queryUserRes(QUserRes query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new Exception("非法操作！");

            return _ResourceReponsitory.queryUserRes_GroupByBook(query).Result;
        }
    }
}
