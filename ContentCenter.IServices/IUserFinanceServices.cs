using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IUserFinanceServices: IBaseServices<EUserChargeTrans>
    {
        /// <summary>
        /// 提交充值记录
        /// </summary>
        /// <param name="submitData"></param>
        /// <returns></returns>
        bool submitUerChargeTrans(VueSubmitUserCharge submitData);

        /// <summary>
        /// 用户财务概况
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        VueUserFinanceOverview getOverview(string userId);

        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ModelPager<VueUserChargeTrans> getChargeTrans(QUserTrans query);

        ModelPager<VueUserPointTrans> getPointTrans(QUserTrans query);

        ModelPager<VueUserCommissionTrans> getCommissionTrans(QUserTrans query);

        ModelPager<VueUserBalanceTrans> getBalanceTrans(QUserTrans query);
    }
}
