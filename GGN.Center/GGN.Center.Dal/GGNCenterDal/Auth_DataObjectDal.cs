using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseDal;
using FW.Base.BaseEntity;
using FW.Tool;
using GGN.Center.Entities;

namespace GGN.Center.Dal
{
    public class Auth_DataObjectDal : BaseDal<Auth_DataObject>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;
        private readonly IQueryable<Auth_DataObject> ThisQuery;

        public Auth_DataObjectDal()
        {
            activeContext = new GGNCenterEntities();
            ThisQuery = activeContext.Auth_DataObject;
        }

        /// <summary>
        /// 行数据权限
        /// </summary>
        /// <param name="Ids"></param>
        public Auth_DataObjectDal(IList<Guid> Ids)
        {
            activeContext = new GGNCenterEntities();
            ThisQuery = activeContext.Auth_DataObject.Where(p => Ids.Contains(p.Id));
        }

        protected override DbSet<Auth_DataObject> DbSet
        {
            get
            {
                return activeContext.Auth_DataObject;
            }
        }
        /// <summary>
        /// 实体对象上下文
        /// </summary>
        protected override DbContext ActiveContext
        {
            get
            {
                return activeContext;
            }
        }
        #endregion
        
        #region 根据Id获取扩展对象
        /// <summary>
        /// 根据Id获取扩展对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  vAuth_DataObject GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vAuth_DataObject>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Auth_DataObject model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Auth_DataObject
                            //where
                            //temp..Contains(model.)
                            select temp;
                var Count = query.Count();
                if (model.Id == Guid.Empty || model.Id == null)
                {
                    if (Count == 0)
                    {
                        op.IsSuccessful = true;
                    }
                    else
                    {
                        op.IsSuccessful = false;
                        op.Message = "标示不能重复";
                    }
                }
                else
                {
                    Count = query.Where(f => f.Id != model.Id).Count();
                    if (Count == 0)
                    {
                        op.IsSuccessful = true;
                    }
                    else
                    {
                        op.IsSuccessful = false;
                        op.Message = "标示不能重复";
                    }
                }
            }
            catch (Exception ex)
            {
                op.IsSuccessful = false;
                op.Message = ex.Message;
            }
            return op;
        }

        /// <summary>
        /// 检查是否合法(code是否已存在)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True:合法;False:不合法</returns>
        public OperateStatus CheckValidate(Auth_DataObject entity)
        {
            OperateStatus result = new OperateStatus { Message = "编码Code已经存在!", IsSuccessful = false };
            var templist = activeContext.Auth_DataObject.Where(p => p.Code == entity.Code).FirstOrDefault();
            //如果没有记录,通过
            if (templist == null)
            {
                result.Message = "检查通过!";
                result.IsSuccessful = true;
                return result;
            }
            //如果有记录,Id相同,说明是编辑,通过
            if (entity.Id == templist.Id)
            {
                result.Message = "检查通过!";
                result.IsSuccessful = true;
                return result;
            }

            return result;
        }
        #endregion

        #region 方法

        public ListByPages<vAuth_DataObject> QuickQuery(Auth_DataObjectQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_DataObject
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Assembly.Contains(queryParam.KeyWords)
                          || temp.ClassFullName.Contains(queryParam.KeyWords)
                          || temp.DataBase.Contains(queryParam.KeyWords)
                          || temp.TableName.Contains(queryParam.KeyWords)
                          || temp.ShowType.Contains(queryParam.KeyWords)
                          || temp.TreeColomnBase.Contains(queryParam.KeyWords)
                          || temp.TreeColomnParent.Contains(queryParam.KeyWords))
                        select new vAuth_DataObject
                        {
                             Id = temp.Id,
                             ApplicationId = temp.ApplicationId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Sort = temp.Sort,
                             Assembly = temp.Assembly,
                             ClassFullName = temp.ClassFullName,
                             DataBase = temp.DataBase,
                             TableName = temp.TableName,
                             ShowType = temp.ShowType,
                             TreeColomnBase = temp.TreeColomnBase,
                             TreeColomnParent = temp.TreeColomnParent,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vAuth_DataObject> Query(Auth_DataObjectQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_DataObject
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Assembly.Contains(queryParam.KeyWords)
                          || temp.ClassFullName.Contains(queryParam.KeyWords)
                          || temp.DataBase.Contains(queryParam.KeyWords)
                          || temp.TableName.Contains(queryParam.KeyWords)
                          || temp.ShowType.Contains(queryParam.KeyWords)
                          || temp.TreeColomnBase.Contains(queryParam.KeyWords)
                          || temp.TreeColomnParent.Contains(queryParam.KeyWords))
                        select new vAuth_DataObject
                        {
                             Id = temp.Id,
                             ApplicationId = temp.ApplicationId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Sort = temp.Sort,
                             Assembly = temp.Assembly,
                             ClassFullName = temp.ClassFullName,
                             DataBase = temp.DataBase,
                             TableName = temp.TableName,
                             ShowType = temp.ShowType,
                             TreeColomnBase = temp.TreeColomnBase,
                             TreeColomnParent = temp.TreeColomnParent,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

