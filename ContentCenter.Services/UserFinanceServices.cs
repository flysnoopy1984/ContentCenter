using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class UserFinanceServices: BaseServices<EUserChargeTrans>, IUserFinanceServices
    {
        private IUserFinanceRepository _userFinanceRepository;
        private IUserFinanceOverViewRepository _userFinanceOverViewRepository;
      
        public UserFinanceServices(IUserFinanceRepository userFinanceRepository,
            IUserFinanceOverViewRepository userFinanceOverViewRepository
           )
          : base(userFinanceRepository)
        {
            _userFinanceRepository = userFinanceRepository;
            _userFinanceOverViewRepository = userFinanceOverViewRepository;
            
        }

        public ModelPager<VueUserBalanceTrans> getBalanceTrans(QUserTrans query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new CCException("余额变动-非法进入");
            return _userFinanceRepository.getBalanceTrans(query).Result;
        }

        public ModelPager<VueUserChargeTrans> getChargeTrans(QUserTrans query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new CCException("充值记录-非法进入");
            return _userFinanceRepository.getChargeTrans(query).Result;
        }

        public ModelPager<VueUserCommissionTrans> getCommissionTrans(QUserTrans query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new CCException("佣金明细-非法进入");
            return _userFinanceRepository.getCommissionTrans(query).Result;
        }

        public VueUserFinanceOverview getOverview(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new CCException("钱包概况-非法进入");
            return _userFinanceOverViewRepository.getOverview(userId).Result;
        }

        public ModelPager<VueUserPointTrans> getPointTrans(QUserTrans query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new CCException("积分明细-非法进入");
            return _userFinanceRepository.getPointTrans(query).Result;
        }

        public bool submitUerChargeTrans(VueSubmitUserCharge submitData)
        {
            DbResult<bool> transResult = null;

            if (string.IsNullOrEmpty(submitData.userId))
                throw new CCException("【提交充值】非法进入");
            if (submitData.amount < 1)
                throw new CCException("【提交充值】金额不能小于1");
            if (submitData.points < 10)
                throw new CCException("【提交充值】积分不能小于10");
           // if(submitData.pointEffectDate)

            transResult = _userFinanceRepository.Db.Ado.UseTran(() =>
            {
                //充值交易记录
                _userFinanceRepository.Add_Sync(new EUserChargeTrans
                {
                    userId = submitData.userId,
                    createdDateTime = DateTime.Now,
                    money = submitData.amount,
                    point = submitData.points,
                    rate = submitData.rate
                }) ;

                //积分变动
                _userFinanceRepository.AddPointTrans_Sync(new EUserPointsTrans
                {
                    userId = submitData.userId,
                    createdDateTime = DateTime.Now,
                    point = submitData.points,
                    changeType = Model.BaseEnum.PointChangeType.charge

                });

                //用户财务概况表
                _userFinanceOverViewRepository.updateChargePoint(new updateFinOverview
                {
                    userId = submitData.userId,
                    direction = Model.BaseEnum.OperationDirection.plus,
                    point = submitData.points,
                    pointEffectDate = submitData.pointEffectDate
                });
            });
            if (!transResult.IsSuccess)
                throw new Exception(transResult.ErrorMessage);
            return true;
            
        }

        
    }
}
