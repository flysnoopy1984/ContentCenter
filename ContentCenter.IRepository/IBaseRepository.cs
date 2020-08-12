
using ContentCenter.Model;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IBaseRepository<T> where T:class,new()
    {
        ISqlSugarClient Db { get; }

        Task<int> AddNoIdentity(T newEntity);
        Task<long> Add(T newEntity);

        Task<int> AddRange(List<T> listObj);

        Task<bool> DeleteByKey(long key);

        Task<bool> DeleteRangeByExp(Expression<Func<T, bool>> whereExp);

        Task<bool> Update(T updateEntity);

        /// <summary>
        /// 没有对象，根据where来部分更新
        /// </summary>
        /// <param name="colsExp"></param>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        Task<bool> UpdatePart_NoObj(Expression<Func<T, T>> colsExp, Expression<Func<T, bool>> whereExp);
        /// <summary>
        /// 有对象，根据主键来部分更新
        /// </summary>
        /// <param name="colsExp"></param>
        /// <returns></returns>
        Task<bool> UpdatePart_WithObj(T entity,Expression<Func<T, T>> colsExp);

        Task<T> GetByKey(object key);
        Task<T> GetByExpSingle(Expression<Func<T, bool>> whereExp);

        Task<int> GetCount(Expression<Func<T, bool>> whereExp);

        Task<List<T>> QueryList(Expression<Func<T,bool>> whereExp, Expression<Func<T, object>> orderByExp,bool desc = true);

        Task<ModelPager<T>> QueryPager(Expression<Func<T, bool>> whereExp, Expression<Func<T, object>> orderByExp, int pageIndex,int pageSize,bool desc = true);

        Task<int> SaveMasterData<M>(M saveObj) where M : BaseMasterTable, new();
    }
}
