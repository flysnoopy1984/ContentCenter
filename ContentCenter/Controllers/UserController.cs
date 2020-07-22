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
        public ResultEntity<VueUerInfo> Login(LoginUser loginUser)
        {
            ResultEntity<VueUerInfo> result = new ResultEntity<VueUerInfo>();
            try
            {
                if (string.IsNullOrEmpty(loginUser.Account))
                    throw new CCException(CCWebMsg.User_Account_Empty);
                var ui = _userServices.Login(loginUser);
                result.Entity = ui;
                var tokenResult = this.GetUserPwdToken(new requireUserPwdToken
                {
                    username = ui.UserAccount,
                    password = ui.TokenPwd
                });
                if (tokenResult.IsSuccess) result.Entity.Token = tokenResult.Entity;
                else result.ErrorMsg = "没有获取登陆令牌";

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultEntity<VueUerInfo> Register(RegUser regUser)
        {
            ResultEntity<VueUerInfo> result = new ResultEntity<VueUerInfo>();
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
                                username = ui.UserAccount,
                                password = ui.TokenPwd
                            });
                            if (tokenResult.IsSuccess) result.Entity.Token = tokenResult.Entity;
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