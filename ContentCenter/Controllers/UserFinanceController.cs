using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserFinanceController : CCBaseController
    {
        private IUserFinanceServices _userFinanceServices;
        public UserFinanceController(IUserFinanceServices userFinanceServices)
        {
            _userFinanceServices = userFinanceServices;
        }

        /// <summary>
        /// 用户概况
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultEntity<VueUserFinanceOverview> getOverview(string userId)
        {
            ResultEntity<VueUserFinanceOverview> result = new ResultEntity<VueUserFinanceOverview>();
            try
            {
                userId = this.getUserId();
                result.Entity = _userFinanceServices.getOverview(userId);
            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorMsg += "[钱包概况查询失败]";
                NLogUtil.cc_ErrorTxt("[UserFinanceController]getOverview:" + ex.Message);
            }
            return result;
        }
        
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultPager<VueUserChargeTrans> getChargeTrans(QUserTrans query)
        {
            ResultPager<VueUserChargeTrans> result = new ResultPager<VueUserChargeTrans>();
            try
            {
                query.userId = this.getUserId();
                result.PageData = _userFinanceServices.getChargeTrans(query);
            
            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorMsg += "[交易记录查询失败]";
                NLogUtil.cc_ErrorTxt("[UserFinanceController]getChargeTrans:" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 积分变动明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultPager<VueUserPointTrans> getPointTrans(QUserTrans query)
        {
            ResultPager<VueUserPointTrans> result = new ResultPager<VueUserPointTrans>();
            try
            {
                query.userId = this.getUserId();
                result.PageData = _userFinanceServices.getPointTrans(query);

            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorMsg += "[积分明细查询失败]";
                NLogUtil.cc_ErrorTxt("[UserFinanceController]getPointTrans:" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 佣金记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultPager<VueUserCommissionTrans> getCommissionTrans(QUserTrans query)
        {
            ResultPager<VueUserCommissionTrans> result = new ResultPager<VueUserCommissionTrans>();
            try
            {
                query.userId = this.getUserId();
                result.PageData = _userFinanceServices.getCommissionTrans(query);
            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorMsg += "[积分明细查询失败]";
                NLogUtil.cc_ErrorTxt("[UserFinanceController]getPointTrans:" + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 余额明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultPager<VueUserBalanceTrans> getBalanceTrans(QUserTrans query)
        {
            ResultPager<VueUserBalanceTrans> result = new ResultPager<VueUserBalanceTrans>();
            try
            {
                query.userId = this.getUserId();
                result.PageData = _userFinanceServices.getBalanceTrans(query);
            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorMsg += "[积分明细查询失败]";
                NLogUtil.cc_ErrorTxt("[UserFinanceController]getPointTrans:" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 用户充值
        /// </summary>
        /// <param name="submitData"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultNormal chargeMoney(VueSubmitUserCharge submitData)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                submitData.userId = this.getUserId();
                _userFinanceServices.submitUerChargeTrans(submitData);
            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                NLogUtil.cc_ErrorTxt("[UserFinanceController]chargeMoney:" + ex.Message);
                result.ErrorMsg += "[充值失败]";
            }
            return result;
        }
        
        [HttpPost]
        public ResultNormal addCommission(VueSubmitUserCommission submitData)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                submitData.userId = this.getUserId();
               // _userFinanceServices.add(submitData);
            }
            catch (CCException cex)
            {
                result.ErrorMsg = cex.Message;
            }
            catch (Exception ex)
            {
                NLogUtil.cc_ErrorTxt("[UserFinanceController]addCommission:" + ex.Message);
                result.ErrorMsg += "[添加佣金失败]";
            }
            return result;
        }
        //   public 
    }
}