using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentCenter.Common;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using ContentCenter.Model.Commons;
using IQB.Util;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace ContentCenter.Controllers
{
   
    [Route("[controller]/[action]")]
    [ApiController]
    public class BookController : CCBaseController
    {
        private  IBookServices _bookServices;
        private IResourceServices _resourceServices;
        private IWebHostEnvironment _webHostEnvironment;
        public BookController(IBookServices bookServices, IResourceServices resourceServices, IWebHostEnvironment webHostEnvironment)
        {
            _resourceServices = resourceServices;
            _bookServices = bookServices;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public ResultPager<RBookList> getBookListPager(QBookList query)
        {
            ResultPager<RBookList> result = new ResultPager<RBookList>();
            try
            {
               
                var pd = _bookServices.GetBookListPager(query);
                result.PageData = pd;
                result.PageData.pageIndex = query.pageIndex;
                result.PageData.pageSize = query.pageSize;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
            
        }

        [HttpGet]
        [Authorize]
        public ResultNormal getNoAuth()
        {
            var a = HttpContext.User;
            ResultNormal r = new ResultNormal();
            r.Message = "获取Token.Pass Auth";
            return r;
        }

        [HttpGet]
        [Authorize]
        public ResultEntity<EBookInfo> Info(string code)
        {
            ResultEntity<EBookInfo> result = new ResultEntity<EBookInfo>();
            try
            {
                result.Entity = _bookServices.Info(code);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取网站的Section
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultEntity<Dictionary<string,List<ESection>>> GetSection(SectionType sectionType=  SectionType.All)
        {
            ResultEntity<Dictionary<string, List<ESection>>> result = new ResultEntity<Dictionary<string, List<ESection>>>();

            try
            {
                result.Entity = _bookServices.GetWebSection(sectionType);
            }
            catch (Exception ex)
            {
                result.ErrorMsg =$"GetSection{ ex.Message}";
            }
            return result;
        }

        /// <summary>
        /// 获取Tag列表
        /// </summary>
        /// <param name="number"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultList<RTag> GetTags(int number,OrderByType orderByType)
        {
            ResultList<RTag> result = new ResultList<RTag>();

            try
            {
                result.List = _bookServices.GetTagList(number, orderByType);
               // result.Entity = _bookServices.GetWebSection(sectionType);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = $"GetTags{ ex.Message}";
            }
            return result;
        }


        #region 资源相关

        /// <summary>
        /// 获取此书本所有可用资源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ResultPager<VueResInfo> GetAllResourcesByRefCode(QRes qRes)
        {
            ResultPager<VueResInfo> result = new ResultPager<VueResInfo>();
            try
            {
                qRes.reqUserId = this.getUserId();
                result.PageData = _resourceServices.getResByRefCode(qRes);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取当前用户对于此书的所有资源
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ResultList<EResourceInfo> GetResourceByOwner(string refCode,bool ignoreDelete = false)
        {
            ResultList<EResourceInfo> result = new ResultList<EResourceInfo>();
            try
            {
                var userId = base.getUserId();
                result.List =  _resourceServices.getFilesByOwner(refCode, userId);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
          
           
            
        }
        /// <summary>
        /// 针对外部Url资源
        /// </summary>
        /// <param name="uploadRes"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ResultEntity<EResourceInfo> SaveResourceInfo(UploadRes uploadRes)
        {
            ResultEntity<EResourceInfo> result = new ResultEntity<EResourceInfo>();
            try
            {
                var verifyMsg = VerifyUpload(uploadRes);
                if (verifyMsg != null)
                {
                    result.ErrorMsg = verifyMsg;
                    return result;
                }
                result = _resourceServices.saveResToDb(GenerateResource(uploadRes)); 

            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// 删除资源，将oss移到Deleted文件夹下
        /// </summary>
        /// <param name="deleteRes"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ResultNormal DeleteResource(DeleteRes deleteRes)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                result = _resourceServices.deleteResource(deleteRes);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 上传书本
        /// 支持重传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [RequestSizeLimit(52428800)]
        public ResultEntity<EResourceInfo> Upload([FromForm] IFormFile file)//,
        {
            ResultEntity<EResourceInfo> result = new ResultEntity<EResourceInfo>();
            EResourceInfo origRes = null;
            var filePath = _webHostEnvironment.ContentRootPath + "\\UploadFiles\\temp\\" + file.FileName;
          
            try
            {
                if (file != null)
                {
                    var uploadRes = this.RequestToUploadRes();

                    var verifyMsg = VerifyUpload(uploadRes);

                    if (verifyMsg != null)
                    {
                        result.ErrorMsg = verifyMsg;
                        return result;
                    }
                   
                    //写入到磁盘
                    using (FileStream fs = System.IO.File.Create(filePath)){
                        file.CopyTo(fs);//将上传的文件文件流，复制到fs中
                        fs.Flush();//清空文件流
                    }
                    //上传到Oss   
                   
                    var ossKey = OssKeyManager.BookKey(filePath, uploadRes.refCode, getUserId());
                    //如果是重新提交，则需要删除Oss资源
                    if (uploadRes.isReset){
                        origRes = _resourceServices.get(uploadRes.resCode);
                        if(origRes.OssPath != ossKey)
                            _resourceServices.ossDelete(origRes.OssPath);
                    }
                    var uploadResult = _resourceServices.uploadBookToOss(filePath, ossKey);
                    if (uploadResult.IsSuccess)
                    {
                        var resourceInfo = GenerateResource(uploadRes, ossKey);
                        resourceInfo.OrigFileName = file.FileName;
                        //上传信息写入到数据库
                        result = _resourceServices.saveResToDb(resourceInfo);
                       
                    }
                    else
                    {
                        result.ErrorMsg = "上传失败";
                    }
                } 
            }
            catch (Exception ex)
            {
                result.ErrorMsg = "上传失败";
                NLogUtil.cc_ErrorTxt("BookController-Upload:" + ex.Message);
            }
            finally
            {
                if(System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            return result;
        }

       
        private UploadRes RequestToUploadRes()
        {
            ResType resType = ResType.BookOss;
            if (!string.IsNullOrEmpty(Request.Form["resType"]))
                resType = Enum.Parse<ResType>(Request.Form["resType"]);
            UploadRes uploadRes = new UploadRes
            {
                resType = resType,
                fileType = Request.Form["fileType"],
                refCode = Convert.ToString(Request.Form["refCode"]),
                owner = getUserId(),
                outerUrl = Convert.ToString(Request.Form["outerUrl"]),
                remark = Convert.ToString(Request.Form["remark"]),
                isReset = Convert.ToBoolean(Request.Form["isReset"]),

            };
            if (uploadRes.isReset) 
                uploadRes.resCode = Convert.ToString(Request.Form["resCode"]);
            return uploadRes;
        }

        private EResourceInfo GenerateResource(UploadRes uploadRes,string ossPath)
        {
            var res = GenerateResource(uploadRes);
            res.OssPath = ossPath;
            return res;
        }

        private EResourceInfo GenerateResource(UploadRes uploadRes)
        {
            EResourceInfo resourceInfo = new EResourceInfo
            {
                Owner = uploadRes.owner,
                RefCode = uploadRes.refCode,
                Remark = uploadRes.remark,
                ResType = uploadRes.resType,
                Url = uploadRes.outerUrl,
                FileType = uploadRes.fileType,
            };
            if (uploadRes.isReset) 
                resourceInfo.Code = uploadRes.resCode;
            else
            resourceInfo.Code = CodeManager.ResCode(
                resourceInfo.FileType,
                (int)resourceInfo.ResType,
                resourceInfo.RefCode,
                resourceInfo.Owner);
            return resourceInfo;

        }

       
        /// <summary>
        /// 校验上传的资源
        /// </summary>
        /// <returns></returns>
        private string VerifyUpload(UploadRes uploadRes )
        {
            //校验用户
            var userCode = base.getUserInfo(CConstants.Id4Claim_UserAccount);
            if (userCode == null){
                return "没有获取用户信息，请重新登陆";
            }
            if (string.IsNullOrEmpty(uploadRes.refCode)){
                return "没有对应的资源Code";
            }
            if (Request.HasFormContentType && string.IsNullOrEmpty(Request.Form["resType"])){
                return "没有对应的资源类型";
            }
            if (string.IsNullOrEmpty(uploadRes.fileType))
            {
                return "没有对应的文件类型";
            }

            if(uploadRes.resType == ResType.Book_Url){
                var url = uploadRes.outerUrl.ToLower();
                if (string.IsNullOrEmpty(uploadRes.outerUrl)) return "资源缺少Url";
                if (!VerifyUtil.VerifyUrl(url)) return "不是有效的http/https Url";
                if (!VerifyUtil.VerifyHttp(url)) return "url地址无法访问，请检查是否已失效";
            }
            if (!uploadRes.isReset){
                if (_resourceServices.IsRepeatRes(uploadRes.refCode, uploadRes.resType, uploadRes.fileType)){
                    return uploadRes.resType == ResType.BookOss ? "此书已上传同类型文件，请查看页面下方列表" :
                        $"已上传[{uploadRes.fileType}]文件类型的外部资源，请查看页面下方列表";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(uploadRes.resCode))
                {
                    return "没有找到重传的资源";
                }
            }
           
            return null;
        }

        #endregion
    }
}