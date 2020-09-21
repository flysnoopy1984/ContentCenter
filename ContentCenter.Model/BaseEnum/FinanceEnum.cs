using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.BaseEnum
{
    public enum PointChangeType
    {
        charge = 1,  //充值所得

        newRegister = 10, // 新注册用户所得

        consume_Download = -1, //下载消耗
    }

    public enum CommissionChangeType
    {
        ad = 1,//推广所得

        settle = -1,//结算
    }

    public enum BalanceChangeType
    {
        charge=1, //余额充值

        drawMoney = -1 //取款
    }
}
