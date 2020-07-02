using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util;
using IQB.Util.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserServices _userServices;
        private IConfiguration _configuration;
        public UserController(IUserServices userServices, IConfiguration configuration)
        {
            _configuration = configuration;
            _userServices = userServices;
        }
        [HttpPost]
        public ResultEntity<EUserInfo> Login(LoginUser loginUser)
        {
            ResultEntity<EUserInfo> result = new ResultEntity<EUserInfo>();
            try
            {
                if (string.IsNullOrEmpty(loginUser.Account))
                    throw new CCException(CCWebMsg.User_Account_Empty);
                result.Entity = _userServices.Login(loginUser);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultNormal Register(RegUser regUser)
        {
            ResultNormal result = new ResultNormal();
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
                        if (smsResult.Entity.SMSVerifyStatus == SMSVerifyStatus.Success)
                        {
                            result.ResultCode = Convert.ToInt32(_userServices.Register(regUser));
                            if (result.ResultCode == -1)
                            {
                                result.ErrorMsg = CCWebMsg.User_Reg_Exist;
                            }
                        }
                        else
                            result.ErrorMsg = smsResult.Entity.Msg;
                    }
                    else
                    {
                        result.ErrorMsg = CCWebMsg.SMS_Verify_Failure;
                    }

                 
                }
                else
                {
                    result.ErrorMsg = verifyMsg;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        #region 私有方法

        private string RequireVerifySMSCode(SMSRequire sms)
        {
            string url = _configuration["SiteUrls:Id4Url"]+ "sms/SubmitVerifyCode";
            string json = JsonConvert.SerializeObject(sms);
            string res = HttpUtil.Send(url, HttpUtil.HttpMethod.Post, json, HttpUtil.application_json);
           // HttpUtil.RequestUrlSendMsg()
            return "";
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