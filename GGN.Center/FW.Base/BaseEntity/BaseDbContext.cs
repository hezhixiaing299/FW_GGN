using System.Data.Entity;
using System.Threading.Tasks;

namespace FW.Base.BaseEntity
{
    //[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        /// <summary>
        /// 根据表示判断用重写的SaveChanges方法，还是普通的上下文SaveChanges方法
        /// </summary>
        public bool LogChangesDuringSave { get; set; }

        /// <summary>
        /// 可以重写异步保存,加快存储效率,目前可以忽略,建议使用Nuget上的第三方批量保存
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 可以重写异步保存,加快存储效率,目前可以忽略,建议使用Nuget上的第三方批量保存
        /// </summary>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// 实现批量保存的方法
        /// </summary>
        public void SaveChangesBulk()
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            #region 可以自定义分类处理
            //var entries = from e in this.ChangeTracker.Entries()
            //              where e.State != EntityState.Unchanged
            //              select e;   //过滤所有修改了的实体，包括：增加 / 修改 / 删除
            //List<object> addList = new List<object>();
            //List<object> deleteList = new List<object>();
            //List<object> modifiedList = new List<object>();
            //foreach (var entry in entries)
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            addList.Add(entry.Entity);
            //            break;
            //        case EntityState.Deleted:
            //            deleteList.Add(entry.Entity);
            //            break;
            //        case EntityState.Modified:
            //            modifiedList.Add(entry.Entity);
            //            break;
            //    }
            //}
            //if (addList.Count > 0)
            //{
            //    this.BulkInsert(addList);  //新增的数据如何处理
            //}
            //if (deleteList.Count > 0)
            //{
            //    this.BulkDelete(deleteList);  //删除的数据如何处理
            //}
            //if (modifiedList.Count > 0)
            //{
            //    this.BulkUpdate(modifiedList);  //修改的数据如何处理
            //}
            #endregion
            this.SaveChanges();   //暂没实现,占位用.后面有时间了用实际批量保存的Nuget包方法来实现
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        /// <summary>
        /// 重写SaveChanges,为了加入一些自己的东西
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            if (LogChangesDuringSave)  //根据表示判断用重写的SaveChanges方法，还是普通的上下文SaveChanges方法
            {
                //这里可以在保存前,统一做一些事,比如写日志,发送消息等
            }
            var returnId = base.SaveChanges();  //返回普通的上下文SaveChanges方法
            this.Configuration.AutoDetectChangesEnabled = false;
            return returnId;
        }

    }
}
