using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.Common;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : CCBaseController
    {
        private IUserServices _userServices;
        private IConfiguration _configuration;
        private IWebHostEnvironment _webHostEnvironment;
        private IResourceServices _resourceServices;
        public UserController(IUserServices userServices,
                IResourceServices resourceServices,
                IConfiguration configuration,
                IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _userServices = userServices;
            _resourceServices = resourceServices;


        }

        [HttpPost]
        public ResultEntity<ResponseToken> RefreshToken(string refToken)
        {
            ResultEntity<ResponseToken> result = new ResultEntity<ResponseToken>();
            string url = _configuration["Id4Config:Id4Url"] + "connect/token";
            requireUserPwdToken upToken = new requireUserPwdToken
            {
                grant_type = "refresh_token",
                client_id = _configuration["Id4Config:client_id"],
                client_secret = _configuration["Id4Config:client_secret"],
                refresh_token = refToken,
            };
         
            try
            {
                var data = $"grant_type={upToken.grant_type}&client_id={upToken.client_id}&client_secret={upToken.client_secret}&refresh_token={upToken.refresh_token}";
                var res = HttpUtil.Send(url, HttpUtil.HttpMethod.Post, data, HttpUtil.application_form);
                result.Entity = JsonConvert.DeserializeObject<ResponseToken>(res);

            }
            catch (Exception ex)
            {
                NLogUtil.cc_ErrorTxt($"【UserController】GetUserPwdToken:{ex.Message}");
                result.ErrorMsg = CCWebMsg.Token_GetUserPwdToken;
            }

            return result;
        } 

        [HttpPost]
        public ResultEntity<ResponseToken> GetUserPwdToken(requireUserPwdToken upToken)
        {
            ResultEntity<ResponseToken> result = new ResultEntity<ResponseToken>();
            string url = _configuration["Id4Config:Id4Url"] + "connect/token";
            upToken.grant_type = _configuration["Id4Config:grant_type"];
            upToken.client_id = _configuration["Id4Config:client_id"];
            upToken.client_secret = _configuration["Id4Config:client_secret"];

           // string json = JsonConvert.SerializeObject(upToken);
            try
            {
                var data = $"grant_type={upToken.grant_type}&client_id={upToken.client_id}&client_secret={upToken.client_secret}&username={upToken.username}&password={upToken.password}";
                var res = HttpUtil.Send(url, HttpUtil.HttpMethod.Post, data, HttpUtil.application_form);
                result.Entity = JsonConvert.DeserializeObject<ResponseToken>(res);

            }
            catch(Exception ex)
            {
                NLogUtil.cc_ErrorTxt($"【UserController】GetUserPwdToken:{ex.Message}");
                result.ErrorMsg = CCWebMsg.Token_GetUserPwdToken;
            }
            
            return result;
        }

        [HttpPost]
        public ResultEntity<VueUserLogin> Login(LoginUser loginUser)
        {
            ResultEntity<VueUserLogin> result = new ResultEntity<VueUserLogin>();
            try
            {
                if (string.IsNullOrEmpty(loginUser.Account))
                    throw new CCException(CCWebMsg.User_Account_Empty);
                var ui = _userServices.Login(loginUser);
                result.Entity = ui;
                var tokenResult = this.GetUserPwdToken(new requireUserPwdToken
                {
                    username = ui.UerInfo.UserAccount,
                    password = ui.UerInfo.TokenPwd
                });
                if (tokenResult.IsSuccess) result.Entity.UerInfo.Token = tokenResult.Entity;
                else result.ErrorMsg = "没有获取登陆令牌";

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        
        [HttpPost]
        public ResultEntity<VueUserLogin> Register(RegUser regUser)
        {
            ResultEntity<VueUserLogin> result = new ResultEntity<VueUserLogin>();
            try
            {
                string verifyMsg = VerifyUser(regUser);
                if (verifyMsg == "")
                {
                    //先校验验证码（短信接口在ID4服务上）
                    var smsJson = RequireVerifySMSCode(new SMSRequire
                    {
                        mobilePhone = regUser.Phone,
                        VerifyCode = regUser.VerifyCode
                    });
                    ResultEntity<OutSMS> smsResult =  JsonConvert.DeserializeObject<ResultEntity<OutSMS>>(smsJson);
                    if(smsResult.IsSuccess)
                    {
                        if (smsResult.Entity.SMSVerifyStatus == SMSVerifyStatus.Success){
                            //注册用户写入数据库
                            var ui =_userServices.Register(regUser);
                            result.Entity = ui;

                            //获取Token
                            var tokenResult = this.GetUserPwdToken(new requireUserPwdToken
                            {
                                username = ui.UerInfo.UserAccount,
                                password = ui.UerInfo.TokenPwd
                            });
                            if (tokenResult.IsSuccess) result.Entity.UerInfo.Token = tokenResult.Entity;
                            else result.ErrorMsg = "没有获取登陆令牌";
                        }
                        else result.ErrorMsg = smsResult.Entity.Msg;
                    }
                    else result.ErrorMsg = CCWebMsg.SMS_Verify_Failure;
                }
                else result.ErrorMsg = verifyMsg;              
            }
            catch(CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = CCWebMsg.User_Reg_Failure;
                NLogUtil.cc_ErrorTxt($"User Controller Register Error:{ex.Message}");
            }
            return result;
        }

        [HttpPost]
        public ResultNormal RegistRebot(string userAccount)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                if(string.IsNullOrEmpty(userAccount))
                    userAccount = DateTime.Now.ToString("yyyy-MM-dd") + StringUtil.GetRnd(2, true, false, false, false);
                var phone ="199"+ StringUtil.GetRnd(8, true, false, false, false);
                RegUser regUser = new RegUser
                {
                    Account = $"r{userAccount}",
                    Phone = phone,
                    Pwd = "111111",
                   
                };
                var ui = _userServices.Register(regUser);
                result.Message = ui.UerInfo.UserAccount;
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
          
        }
        /// <summary>
        ///收藏/取消喜爱的书
        /// </summary>
        /// <param name="userBook"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ResultNormal SwitchFavBook(reqUserBook userBook)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                userBook.userId = this.getUserId();
                if(userBook.direction == OperationDirection.plus)
                    _userServices.AddFavBook(userBook.bookCode, userBook.userId);
                else
                    _userServices.DelFavBook(userBook.bookCode, userBook.userId);
            }
            catch (Exception ex)
            {
                NLogUtil.cc_ErrorTxt("[UserController]-AddFavBook:" + ex.Message);
                result.ErrorMsg = "操作失败，请之后再尝试";
            }
            return result;
        }

        /// <summary>
        /// 收藏书的列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ResultPager<VueUserBook> FavBookList(QUserBook query)
        {
            ResultPager<VueUserBook> result = new ResultPager<VueUserBook>();
            try
            {
             //   query.userId = this.getUserId();
                result.PageData = _userServices.queryUserbookList(query);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Authorize]
        public ResultEntity<VueUC_UserInfo> GetUCInfo(string userId)
        {
            ResultEntity<VueUC_UserInfo> result = new ResultEntity<VueUC_UserInfo>();
            try
            {
                result.Entity = _userServices.getUC_User(userId);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(52428800)]
        public ResultNormal UploadHeander([FromForm] IFormFile file)
        {
            ResultNormal result = new ResultNormal();
            string filePath = null;
            try
            {
                if(file == null)
                {
                    result.ErrorMsg = "没有上传图片";
                    return result;
                }
                var userId = this.getUserId();
                var fn = userId + "_" + file.FileName;
               
                filePath = _webHostEnvironment.ContentRootPath + _configuration["BookSiteConfig:uploadTemp"] + fn;

                //写入到磁盘
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);//将上传的文件文件流，复制到fs中
                    fs.Flush();//清空文件流
                }
                var ossKey = OssKeyManager.UserAvatorKey(fn);
                var uploadResult = _resourceServices.uploadToOss(filePath, ossKey);
                if (uploadResult.IsSuccess)
                {
                    var url = _configuration["ossConfig:userHeaderRoot"] + fn;
                    url += $"?{StringUtil.GetRnd(5, true, false, false, false)}";
                    _userServices.updateHeader(userId, url);
                    result.Message = url;
                }
                else
                    result.IsSuccess = false;


            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            finally
            {
                if (filePath!=null && System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            return result;
        }
        [HttpPost]
        [Authorize]
        public ResultNormal UploadInfo(VueSubmitUserInfo submitData)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                submitData.userId = this.getUserId();
                _userServices.updateInfo(submitData);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        #region 私有方法

        private string RequireVerifySMSCode(SMSRequire sms)
        {

            string url = _configuration["Id4Config:Id4Url"] + "sms/SubmitVerifyCode";
            string json = JsonConvert.SerializeObject(sms);
            string res = "";
            try
            {
                 res = HttpUtil.Send(url, HttpUtil.HttpMethod.Post, json, HttpUtil.application_json);
            }
            catch(Exception ex)
            {
                NLogUtil.cc_ErrorTxt($"【UserController】RequireVerifySMSCode:{ex.Message}");
                throw new CCException(CCWebMsg.SMS_RequireVerifySMSCode);
            }
        
            return res;
        }

        private string VerifyUser(RegUser regUser)
        {
            if (string.IsNullOrEmpty(regUser.Account)) return CCWebMsg.User_Account_Empty;
            if (string.IsNullOrEmpty(regUser.Pwd)) return CCWebMsg.User_Pwd_Empty;
            if (string.IsNullOrEmpty(regUser.Phone)) return CCWebMsg.User_Phone_Empty;
            if (string.IsNullOrEmpty(regUser.VerifyCode)) return CCWebMsg.User_VC_Empty;
            return "";
        }
        #endregion
    }
}