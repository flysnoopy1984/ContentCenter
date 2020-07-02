﻿using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class,new()
    {
        private readonly ISqlSugarClient _db;
      

        public ISqlSugarClient Db
        {
            get { return _db; }
        }
        public BaseRepository(ISqlSugarClient sqlSugarClient)
        {
            _db = sqlSugarClient;

            //_db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            //{
            //    NLogUtil.cc_InfoTxt($"Before Sql:{sql}");
            //};
            //_db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            //{
            //    Console.WriteLine(sql);
            //};
            _db.Aop.OnError = (exp) =>//执行SQL 错误事件
            {
                NLogUtil.cc_ErrorTxt($"Sql:{exp.Sql}");
              //  Console.WriteLine(exp.Sql);
                //exp.sql exp.parameters 可以拿到参数和错误Sql             
            };
          
        }

        public async Task<int> AddNoIdentity(T newEntity)
        {
            var insertable = _db.Insertable<T>(newEntity);
            return await insertable.ExecuteCommandAsync();
        }
        public async Task<long> Add(T newEntity)
        {
            var insertable = _db.Insertable<T>(newEntity);
            return await insertable.ExecuteReturnBigIdentityAsync();
        }
        public async Task<int> AddRange(List<T> listObj)
        {
            if (listObj == null || listObj.Count == 0) return -1;

            var insertable = _db.Insertable<T>(listObj);
            return await insertable.ExecuteCommandAsync();
        }

        public async Task<bool> DeleteByKey(long key)
        {
            var op = _db.Deleteable<T>(key);
            return await op.ExecuteCommandHasChangeAsync(); 
            
        }

        public async Task<bool> DeleteRangeByExp(Expression<Func<T, bool>> whereExp)
        {
            var op = _db.Deleteable<T>(whereExp);
            return await op.ExecuteCommandHasChangeAsync();
        }

        public async Task<T> GetByKey(object key)
        {
            var op = _db.Queryable<T>().InSingleAsync(key);
            return await op;
        }

        public async Task<T> GetByExpSingle(Expression<Func<T, bool>> whereExp)
        {
            var op = _db.Queryable<T>().Where(whereExp).FirstAsync();
            return await op;
        }

        public async Task<int> GetCount(object key)
        {
           return await _db.Queryable<T>().CountAsync();
        }

        public async Task<List<T>> QueryList(Expression<Func<T, bool>> whereExp, Expression<Func<T, object>> orderByExp, bool desc = true)
        {
            var op = _db.Queryable<T>().
                WhereIF(whereExp != null, whereExp).
                OrderByIF(orderByExp != null, orderByExp,desc?OrderByType.Desc:OrderByType.Asc).
                ToListAsync();

            return await op;
        }

        public async Task<ModelPager<T>> QueryPager(Expression<Func<T, bool>> whereExp, Expression<Func<T, object>> orderByExp, int pageIndex, int pageSize, bool desc = true)
        {
            RefAsync<int> totalCount = 0;
            var op = await _db.Queryable<T>().
                WhereIF(whereExp != null, whereExp).
                OrderByIF(orderByExp != null, orderByExp, desc ? OrderByType.Desc : OrderByType.Asc).
                ToPageListAsync(pageIndex, pageSize,totalCount);

            int totalapage = Math.Ceiling(totalCount > 0 ? totalCount.ObjToDecimal() / pageSize.ObjToDecimal() : 0).ObjToInt();

            return new ModelPager<T>()
            {
                datas = op,
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPage = totalapage
            };
        }

        public async Task<bool> Update(T updateEntity)
        {
            var op = _db.Updateable<T>(updateEntity);
            return await op.ExecuteCommandHasChangeAsync();
        }
    }
}
