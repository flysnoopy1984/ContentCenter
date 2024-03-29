﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.Commons
{
    public class CConstants
    {
        #region JWT 配置常量
        /// <summary>
        /// JWT发布者
        /// </summary>
        public const string JWTIssuer = "sfw";

        /// <summary>
        /// JWT消费者
        /// </summary>
        public const string JWTAud = "cus";

        /// <summary>
        /// JWT证书Key 
        /// </summary>
        public const string JWTIssSerKey = "shanghaishanghaishanghai";
        #endregion

        public const string Id4Claim_UserAccount = "UserAccount";
        public const string Id4Claim_UserId = "UserId";
        public const string Id4Claim_UserNickName = "UserNickName";

        public const string MemoryKey_SysConfig = "mk-sysConfig";

        public const string MemoryKey_BaiduPanAccessToken = "mk-baiduPanAccessToken";





    }
}
