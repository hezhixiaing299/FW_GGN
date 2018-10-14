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
    public class Org_OrganizationDal : BaseDal<Org_Organization>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Org_OrganizationDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Org_Organization> DbSet
        {
            get
            {
                return activeContext.Org_Organization;
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
        public  vOrg_Organization GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vOrg_Organization>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Org_Organization model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Org_Organization
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
        #endregion

        #region 方法
        
        public ListByPages<vOrg_Organization> QuickQuery(Org_OrganizationQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Org_Organization
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords)
                          || temp.ParentCode.Contains(queryParam.KeyWords)
                          || temp.ParentName.Contains(queryParam.KeyWords)
                          || temp.BankName.Contains(queryParam.KeyWords)
                          || temp.BankAccount.Contains(queryParam.KeyWords)
                          || temp.RelationShipCode.Contains(queryParam.KeyWords))
                        select new vOrg_Organization
                        {
                             Id = temp.Id,
                             GroupId = temp.GroupId,
                             OrganizationCategoryId = temp.OrganizationCategoryId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Remark = temp.Remark,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             IsUsing = temp.IsUsing,
                             ParentOrganizationId = temp.ParentOrganizationId,
                             ParentCode = temp.ParentCode,
                             ParentName = temp.ParentName,
                             BankName = temp.BankName,
                             BankAccount = temp.BankAccount,
                             RelationShipCode = temp.RelationShipCode,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vOrg_Organization> Query(Org_OrganizationQueryParam queryParam)
        {
            var query = from temp in activeContext.Org_Organization
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords)
                          || temp.ParentCode.Contains(queryParam.KeyWords)
                          || temp.ParentName.Contains(queryParam.KeyWords)
                          || temp.BankName.Contains(queryParam.KeyWords)
                          || temp.BankAccount.Contains(queryParam.KeyWords)
                          || temp.RelationShipCode.Contains(queryParam.KeyWords))
                        select new vOrg_Organization
                        {
                             Id = temp.Id,
                             GroupId = temp.GroupId,
                             OrganizationCategoryId = temp.OrganizationCategoryId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Remark = temp.Remark,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             IsUsing = temp.IsUsing,
                             ParentOrganizationId = temp.ParentOrganizationId,
                             ParentCode = temp.ParentCode,
                             ParentName = temp.ParentName,
                             BankName = temp.BankName,
                             BankAccount = temp.BankAccount,
                             RelationShipCode = temp.RelationShipCode,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }

        /// <summary>
        /// 获取全部的组织机构和附加信息
        /// </summary>
        /// <returns></returns>
        public List<vOrg_Organization> GetAllOrg()
        {
            var query = (from temp in activeContext.Org_Organization
                         join organ in activeContext.Org_Organization on temp.ParentOrganizationId equals organ.Id into organtemp
                         from neworgantemp in organtemp.DefaultIfEmpty()
                         join orggroup in activeContext.Org_Group on temp.GroupId equals orggroup.Id into orggrouptemp
                         from neworggroup in orggrouptemp.DefaultIfEmpty()
                         join ooc in activeContext.Org_OrganizationCategory on temp.OrganizationCategoryId equals ooc.Id into ooctemp
                         from newooc in ooctemp.DefaultIfEmpty()
                         select new vOrg_Organization
                         {
                             Id = temp.Id, //关系id
                             GroupId = temp.GroupId,
                             OrganizationCategoryId = temp.OrganizationCategoryId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Remark = temp.Remark,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             IsUsing = temp.IsUsing,
                             ParentOrganizationId = temp.ParentOrganizationId,
                             ParentCode = temp.ParentCode,
                             ParentName = temp.ParentName,
                             BankName = temp.BankName,
                             BankAccount = temp.BankAccount,
                             RelationShipCode = temp.RelationShipCode,
                             OrganizationCategoryCode = newooc.Code,
                             OrganizationCategoryName = newooc.Name,
                             OrganizationCategorySort = newooc.Sort,
                             GroupName = neworggroup.Name,

                         });
            var resutls = query.ToList();
            return resutls;
        }


        public List<View_UserOrgInfo> GetAllUserOrgInfo()
        {
            var orgList = GetAllOrg().Select(p =>
                            new View_UserOrgInfo
                            {
                                Id = p.Id, 
                                OrganizationCode = p.Code,
                                OrganizationName = p.Name,
                                ParentOrganizationId = p.ParentOrganizationId,
                                ParentOrganizationCode = p.ParentCode,
                                ParentOrganizationName = p.ParentName,
                                OrganizationLevel = p.Level,
                                OrganizationRelationShipCode = p.RelationShipCode,
                                OrganizationSort = p.Sort,
                                OrganizationRemark = p.Remark,
                                OrganizationIsUsing = p.IsUsing,
                                OrganizationCategoryId = p.OrganizationCategoryId,
                                OrganizationCategoryCode = p.OrganizationCategoryCode,
                                OrganizationCategoryName = p.OrganizationCategoryName,
                                OrganizationCategorySort = p.OrganizationCategorySort,
                                GroupId = p.GroupId,
                                GroupName = p.GroupName,
                            }
                            );
            return orgList.ToList();
        }

        #endregion
    }
}

