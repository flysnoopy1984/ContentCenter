using ContentCenter.IRepository;
using ContentCenter.Model;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class UserFinanceRepository: BaseRepository<EUserChargeTrans>, IUserFinanceRepository
    {
        public UserFinanceRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public bool AddPointTrans_Sync(EUserPointsTrans newEntity)
        {
            var insertable = base.Db.Insertable(newEntity);
            return insertable.ExecuteCommand()>0;
        
        }

        public async Task<ModelPager<VueUserBalanceTrans>> getBalanceTrans(QUserTrans query)
        {
            ModelPager<VueUserBalanceTrans> result = new ModelPager<VueUserBalanceTrans>(query.pageIndex, query.pageSize);

            var mainSql = Db.Queryable<EUserBalanceTrans>()
                .Where(a => a.userId == query.userId
                && a.createdDateTime.Date >= query.startDate.Date
                && a.createdDateTime.Date <= query.endDate.Date)
                .OrderBy(a => a.createdDateTime, OrderByType.Desc)
                .Select(a => new VueUserBalanceTrans
                {
                    dbDateTime = a.createdDateTime,
                    money= a.money,
                    changeType = a.changeType
                });
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }

        public async Task<ModelPager<VueUserChargeTrans>> getChargeTrans(QUserTrans query)
        {
            ModelPager<VueUserChargeTrans> result = new ModelPager<VueUserChargeTrans>(query.pageIndex, query.pageSize);

            var mainSql = Db.Queryable<EUserChargeTrans>()
                .Where(a => a.userId == query.userId
                && a.createdDateTime.Date >= query.startDate.Date
                && a.createdDateTime.Date <= query.endDate.Date)
                .OrderBy(a=>a.createdDateTime,OrderByType.Desc)
                .Select(a => new VueUserChargeTrans
                {
                    dbDateTime = a.createdDateTime,
                    amount = a.money,
                    rate = a.rate,
                    point = a.point
                });
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }

        public async Task<ModelPager<VueUserCommissionTrans>> getCommissionTrans(QUserTrans query)
        {
            ModelPager<VueUserCommissionTrans> result = new ModelPager<VueUserCommissionTrans>(query.pageIndex, query.pageSize);

            var mainSql = Db.Queryable<EUserCommissionTrans>()
                .Where(a => a.userId == query.userId
                && a.createdDateTime.Date >= query.startDate.Date
                && a.createdDateTime.Date <= query.endDate.Date)
                .OrderBy(a => a.createdDateTime, OrderByType.Desc)
                .Select(a => new VueUserCommissionTrans
                {
                    dbDateTime = a.createdDateTime,
                    commission =a.commission,
                    changeType = a.changeType
                });
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }

        public async Task<ModelPager<VueUserPointTrans>> getPointTrans(QUserTrans query)
        {
            ModelPager<VueUserPointTrans> result = new ModelPager<VueUserPointTrans>(query.pageIndex, query.pageSize);

            var mainSql = Db.Queryable<EUserPointsTrans>()
                .Where(a => a.userId == query.userId
                && a.createdDateTime.Date >= query.startDate.Date
                && a.createdDateTime.Date <= query.endDate.Date)
                 .OrderBy(a => a.createdDateTime, OrderByType.Desc)
                .Select(a => new VueUserPointTrans
                {
                    dbDateTime = a.createdDateTime,
                    changeType = a.changeType,
                    point = a.point
                });
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }
    }
}
