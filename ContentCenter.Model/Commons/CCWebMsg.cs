using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class CCWebMsg
    {
        public const string User_Account_Empty = "用户账户不能为空";
        public const string User_Pwd_Empty = "用户密码不能为空";
        public const string User_Phone_Empty = "用户手机号不能为空";
        public const string User_VC_Empty = "校验码不能为空";

        public const string User_Reg_Exist_Account = "用户账户已存在";
        public const string User_Reg_Exist_Phone = "手机号已注册";
        public const string User_Reg_Failure = "暂时无法注册，注册失败，如需帮助，请联系客服";

        public const string User_Login_WrongUserPwd = "用户名或密码错误!";

        public const string SMS_Verify_Failure = "短信校验失败";
        public const string SMS_RequireVerifySMSCode = "短信服务暂时不可用，请等待或联系客服";

        public const string Token_GetUserPwdToken = "获取登陆令牌服务不可用，请等待或联系客服";


    }
}
