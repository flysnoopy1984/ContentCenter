using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class UserFinanceOverViewRepository: BaseRepository<EUserFinanceOverview>,IUserFinanceOverViewRepository
    {
        public UserFinanceOverViewRepository(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public Task<VueUserFinanceOverview> getOverview(string userId)
        {
            return Db.Queryable<EUserFinanceOverview>().Where(a => a.userId == userId)
                .Select(a => new VueUserFinanceOverview
                {
                    commission = a.commission,
                    fixPoint = a.fixedPoint,
                    money = a.money,
                    point = a.chargePoint,
                    dbDateTime = a.pointEffectDate,
                   
                  //  effectDate = a.pointEffectDate.ToString("yyyy-")

                })
                .FirstAsync();
        }

        public bool updateChargeMoney(updateFinOverview updateFinOverview)
        {
            if (updateFinOverview.direction == Model.BaseEnum.OperationDirection.minus)
                updateFinOverview.money = -updateFinOverview.money;

            return base.UpdatePart_NoObj_Sync(a => new EUserFinanceOverview() 
            { money = a.money+ updateFinOverview.money },
                a => a.userId == updateFinOverview.userId
            );
           
        }

        public bool updateChargePoint(updateFinOverview updateFinOverview)
        {
            if (updateFinOverview.direction == Model.BaseEnum.OperationDirection.minus)
            {
                return base.UpdatePart_NoObj_Sync(a => new EUserFinanceOverview()
                { 
                    chargePoint = a.chargePoint - updateFinOverview.point 
                },
                a => a.userId == updateFinOverview.userId);
            }
            else
            {
                return base.UpdatePart_NoObj_Sync(a => new EUserFinanceOverview()
                { 
                   chargePoint = a.chargePoint + updateFinOverview.point ,
                   pointEffectDate = updateFinOverview.pointEffectDate
                },
               a => a.userId == updateFinOverview.userId);
            }
            
        }

        public bool updateCommission(updateFinOverview updateFinOverview)
        {
            if (updateFinOverview.direction == Model.BaseEnum.OperationDirection.minus)
                updateFinOverview.commission = -updateFinOverview.commission;

            return base.UpdatePart_NoObj_Sync(a => new EUserFinanceOverview()
            { money = a.commission + updateFinOverview.commission },
                a => a.userId == updateFinOverview.userId
            );
        }

        public bool updateFixPoint(updateFinOverview updateFinOverview)
        {
            if (updateFinOverview.direction == Model.BaseEnum.OperationDirection.minus)
                updateFinOverview.fixPoint = -updateFinOverview.fixPoint;

            return base.UpdatePart_NoObj_Sync(a => new EUserFinanceOverview()
            { fixedPoint = a.fixedPoint + updateFinOverview.fixPoint },
                a => a.userId == updateFinOverview.userId
            );
        }
    }
}
