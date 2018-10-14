using FW.Base.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace FW.Base.BaseDal
{
    /// <summary>
    /// IQueryable扩展
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 将查询语句转换为查询结果
        /// </summary>
        /// <typeparam name="T">查询结果的类型</typeparam>
        /// <param name="source">查询语句</param>
        /// <param name="queryParam">查询参数</param>
        /// <returns></returns>
        public static ListByPages<T> ToListByPages<T>(this IQueryable<T> source, BaseSearchParam queryParam, params Object[] objects) where T : new()
        {
            if (queryParam == null)
            {                
                //return null;
                throw new InvalidOperationException("未设置分页查询基本参数");
            }

            if (queryParam.page < 0)
            {
                throw new InvalidOperationException("起始记录数不能小于1");
            }
            if (queryParam.limit < 0)
            {
                throw new InvalidOperationException("每页记录数不能小于1");
            }

            //总记录数
            var totalCount = source.Count();

            //总页数
            var totalPageCount = 0;
            List<T> data = new List<T>();
            if (queryParam.IsExport && queryParam.IsAllPage)
            {
                //不分页，用于导出时查询数据
                data = source.OrderBy(queryParam.orderString).ToList();
            }
            else
            {
                //分页数据
                totalPageCount = (totalCount + queryParam.limit - 1) / queryParam.limit;
                data = source.OrderBy(queryParam.orderString).Skip(queryParam.startIndex).Take(queryParam.limit).ToList();
            }

            //构建查询结果
            var listByPages = new ListByPages<T>
            {
                rows = data,
                total = totalCount,
                totalpagecount = totalPageCount,
                page = queryParam.page,
                limit = queryParam.limit
            };
            return listByPages;
        }

        //还需要合计方法
        //还需要自定义数据列查询/导出方法
    }
}
