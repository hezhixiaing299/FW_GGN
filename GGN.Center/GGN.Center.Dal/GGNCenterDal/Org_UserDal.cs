using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseCommon;
using FW.Base.BaseDal;
using FW.Base.BaseEntity;
using FW.Tool;
using GGN.Center.Entities;

namespace GGN.Center.Dal
{
    public class Org_UserDal : BaseDal<Org_User>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        View_FeaturePositionDal vfoDal = new View_FeaturePositionDal();
        View_MenuSyDal vmsDal = new View_MenuSyDal();
        View_UserOrgInfoDal vuoiDal = new View_UserOrgInfoDal();
        View_DataRolePositionDal vdrpDal = new View_DataRolePositionDal();
        Auth_PositionSpecialMenuConfigDal apsmcDal = new Auth_PositionSpecialMenuConfigDal();

        public Org_UserDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Org_User> DbSet
        {
            get
            {
                return activeContext.Org_User;
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
        public  vOrg_User GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vOrg_User>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Org_User model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Org_User
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

        public OperateStatus CheckLogin(BaseUser User)
        {
            OperateStatus op = new OperateStatus { IsSuccessful = false,Message="初始异常!" };
            var aa = 999;
            var bb = "111111";
            var cc = DEncrypt.Get32_MD5Lower(bb, null);
            var dd = DEncrypt.Get32_MD5Lower(cc+ aa, null);

            try
            {
                //取得静态数据--登录失效时间
                int LoginStateTime = int.Parse(GlobalStaticParam.GetByCode("LoginStateTime").ToString());

                #region 用户名密码验证
                //根据用户名查询数据
                var queryuser = activeContext.Org_User.FirstOrDefault(p => p.Id == User.Id || p.LoginName == User.LoginName
                                                        || (!string.IsNullOrEmpty(p.Phone) && p.Phone == User.Phone)
                                                        || (!string.IsNullOrEmpty(p.Email) && p.Email == User.Email));
                if (queryuser == null) //如果用户名不存在
                {
                    op.Message = "用户不存在,请检查用户名!";
                    return op;
                }
                //用户被冻结
                if (queryuser.IsFreeze)
                {
                    op.Message = "登录失败!用户已被冻结!";
                    return op;
                }

                //如果用户名存在
                //1.取得提交的密码明文
                //明文加密为初始密码(第一次加密)  
                //因为前台已经加密,所以这里就用提交的密文作为第一次加密内容,不再不加密了.注意:前台加密要和后台加密算法相同
                string pwtext = User.PassWord; //前台提交的时候请使用此属性                
                
                //取得用户随机数
                var userRandom = queryuser.Random;

                //用提交的密文+用户随机数,再次加密,生成提交的最后密码
                var lastPassWord = DEncrypt.Get32_MD5Lower(pwtext + userRandom, null);

                if (pwtext != queryuser.SourcePW || lastPassWord != queryuser.PassWord)
                {
                    op.Message = "密码不正确!";
                    return op;
                }
                #endregion

                #region  查看是否有用户登录状态
                var LoginState = (from a in activeContext.Sys_UserLoginState
                                  where (a.LoginName.Equals(queryuser.LoginName))
                                  select a).FirstOrDefault();                
                if (LoginState == null)
                {
                    Sys_UserLoginState UsersLoginState = new Sys_UserLoginState();
                    UsersLoginState.Id = Guid.NewGuid();
                    UsersLoginState.SessionId = Guid.NewGuid().ToString().Replace("-", "");
                    UsersLoginState.UserId = queryuser.Id;
                    UsersLoginState.LoginName = queryuser.LoginName;
                    UsersLoginState.LastTime = DateTime.Now;
                    UsersLoginState.EqpMark = User.EquipmentMark;                    
                    UsersLoginState.PeriodTime = UsersLoginState.LastTime.AddHours(LoginStateTime); //延长时间
                    activeContext.Sys_UserLoginState.Add(UsersLoginState);
                    activeContext.SaveChanges();
                }
                else
                {
                    LoginState.EqpMark = User.EquipmentMark;
                    LoginState.LastTime = DateTime.Now;
                    LoginState.PeriodTime = LoginState.LastTime.AddHours(LoginStateTime); //延长时间
                    activeContext.SaveChanges();
                }
                #endregion


                op.IsSuccessful = true;
                op.Message = "";
                //op.Data = returnUser;
            }
            catch (Exception ex)
            {
                op.IsSuccessful = false;
                op.Message = ex.Message;
            }
            return op;
        }

        public ListByPages<vOrg_User> QuickQuery(Org_UserQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Org_User
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.LoginName.Contains(queryParam.KeyWords)
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.UserName.Contains(queryParam.KeyWords)
                          || temp.ShortName.Contains(queryParam.KeyWords)
                          || temp.SourcePW.Contains(queryParam.KeyWords)
                          || temp.Random.Contains(queryParam.KeyWords)
                          || temp.PassWord.Contains(queryParam.KeyWords)
                          || temp.IdCard.Contains(queryParam.KeyWords)
                          || temp.Email.Contains(queryParam.KeyWords)
                          || temp.Telephone.Contains(queryParam.KeyWords)
                          || temp.Phone.Contains(queryParam.KeyWords)
                          || temp.Address.Contains(queryParam.KeyWords)
                          || temp.QQ.Contains(queryParam.KeyWords)
                          || temp.BankName.Contains(queryParam.KeyWords)
                          || temp.BankAccount.Contains(queryParam.KeyWords)
                          || temp.FreezeReason.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vOrg_User
                        {
                             Id = temp.Id,
                             LoginName = temp.LoginName,
                             Code = temp.Code,
                             UserName = temp.UserName,
                             ShortName = temp.ShortName,
                             SourcePW = temp.SourcePW,
                             Random = temp.Random,
                             PassWord = temp.PassWord,
                             Gender = temp.Gender,
                             IdCard = temp.IdCard,
                             Email = temp.Email,
                             Telephone = temp.Telephone,
                             Phone = temp.Phone,
                             Address = temp.Address,
                             QQ = temp.QQ,
                             BankName = temp.BankName,
                             BankAccount = temp.BankAccount,
                             CreateTime = temp.CreateTime,
                             IsFreeze = temp.IsFreeze,
                             FreezeReason = temp.FreezeReason,
                             Remark = temp.Remark,
                             IsOutSide = temp.IsOutSide,
                             IsSuperMgr = temp.IsSuperMgr,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vOrg_User> Query(Org_UserQueryParam queryParam)
        {
            var query = from temp in activeContext.Org_User
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.LoginName.Contains(queryParam.KeyWords)
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.UserName.Contains(queryParam.KeyWords)
                          || temp.ShortName.Contains(queryParam.KeyWords)
                          || temp.SourcePW.Contains(queryParam.KeyWords)
                          || temp.Random.Contains(queryParam.KeyWords)
                          || temp.PassWord.Contains(queryParam.KeyWords)
                          || temp.IdCard.Contains(queryParam.KeyWords)
                          || temp.Email.Contains(queryParam.KeyWords)
                          || temp.Telephone.Contains(queryParam.KeyWords)
                          || temp.Phone.Contains(queryParam.KeyWords)
                          || temp.Address.Contains(queryParam.KeyWords)
                          || temp.QQ.Contains(queryParam.KeyWords)
                          || temp.BankName.Contains(queryParam.KeyWords)
                          || temp.BankAccount.Contains(queryParam.KeyWords)
                          || temp.FreezeReason.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vOrg_User
                        {
                             Id = temp.Id,
                            LoginName = temp.LoginName,
                             Code = temp.Code,
                             UserName = temp.UserName,
                             ShortName = temp.ShortName,
                             SourcePW = temp.SourcePW,
                             Random = temp.Random,
                             PassWord = temp.PassWord,
                             Gender = temp.Gender,
                             IdCard = temp.IdCard,
                             Email = temp.Email,
                             Telephone = temp.Telephone,
                             Phone = temp.Phone,
                             Address = temp.Address,
                             QQ = temp.QQ,
                             BankName = temp.BankName,
                             BankAccount = temp.BankAccount,
                             CreateTime = temp.CreateTime,
                             IsFreeze = temp.IsFreeze,
                             FreezeReason = temp.FreezeReason,
                             Remark = temp.Remark,
                             IsOutSide = temp.IsOutSide,
                             IsSuperMgr = temp.IsSuperMgr,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }

        /// <summary>
        /// 根据登录名获取用户
        /// </summary>
        /// <param name="loionName">登录名</param>
        /// <returns></returns>
        public Org_User GetByLoginName(string loginName)
        {
            //从数据库中获取
            Org_User user = activeContext.Org_User.Where(u => u.LoginName == loginName && u.IsFreeze == false).SingleOrDefault();
            return user;
        }
        
        /// <summary>
        /// 获取用户全部信息
        /// </summary>
        /// <param name="loginname"></param>
        public UserBackFullInfo GetUserFullInfo(Org_UserQueryParam queryParam)
        {
            UserBackFullInfo result = new UserBackFullInfo();
            //全都为空,查个毛啊,这里是精确查询1条,不是模糊查多条
            if (queryParam.Id == Guid.Empty && string.IsNullOrEmpty(queryParam.LoginName)
                && string.IsNullOrEmpty(queryParam.Phone) && string.IsNullOrEmpty(queryParam.Email)
                && string.IsNullOrEmpty(queryParam.IdCard))
            {
                return null;  //返回空,让调用处报异常去
            }
            result.BaseInfo = this.GetUserBaseInfo(queryParam);

            //获取用户组织机构
            if (result.BaseInfo.IsSuperMgr && result.BaseInfo.LoginName == "sysadmin") //超级管理员
            {
                result.UserOrgInfoList = (new Org_OrganizationDal()).GetAllUserOrgInfo();
                result.UserFeatureInfoList = vfoDal.GetSystemManagerFeatures().ToList();
                result.UserMenuInfoList = vmsDal.GetAll().ToList();
                //数据权限暂时不考虑
            }
            else
            {
                if (result.BaseInfo.IsSuperMgr)
                {
                    result.UserOrgInfoList = this.GetUserOrgInfos(queryParam);
                    result.UserFeatureInfoList = vfoDal.GetSystemManagerFeatures().ToList();
                    //var aa = result.UserFeatureInfoList.Where(p => p.FeatureCode == "DCBI-CRE-Head" || p.FeatureCode == "DCBI-CRE-Send").ToList();                    
                    result.UserMenuInfoList = vmsDal.GetAll().ToList();
                    //var bb = result.UserMenuInfoList.Where(p => p.MenuCode == "DCBI-CRE-Draft" || p.MenuCode == "DCBI-CRE-Finance").ToList();
                }
                else
                {
                    result.UserOrgInfoList = this.GetUserOrgInfos(queryParam);

                    //用户岗位
                    var PositionIds = result.UserOrgInfoList.Where(k => k.PositionId.HasValue).Select(p => p.PositionId.Value).ToList();

                    #region 获取用户功能权限
                    View_FeaturePositionQueryParam featurepqueryParam = new View_FeaturePositionQueryParam();
                    //取出用户的多个岗位,查询多岗位下的功能项
                    //featurepqueryParam.PositionIds = result.UserOrgInfoList.Where(k => k.PositionId.HasValue).Select(p => p.PositionId.Value).ToList();
                    featurepqueryParam.PositionIds = PositionIds;
                    result.UserFeatureInfoList = this.GetUserFeatureInfos(featurepqueryParam);
                    #endregion

                    #region 获取用户菜单
                    //获取用户的功能项,查询对应的菜单(IsMenu是菜单的功能项)
                    View_MenuSysQueryParam menusyqueryParam = new View_MenuSysQueryParam();
                    menusyqueryParam.FeatureIds = result.UserFeatureInfoList.Where(k => k.FeatureId.HasValue && k.FeatureIsMenu.HasValue && k.FeatureIsMenu.Value == true).Select(p => p.FeatureId.Value).Distinct().ToList();
                    result.UserMenuInfoList = this.GetUserMenuInfoList(menusyqueryParam);

                    //加上特别配置的菜单
                    Auth_PositionSpecialMenuConfigQueryParam apsmcqueryParam = new Auth_PositionSpecialMenuConfigQueryParam();
                    apsmcqueryParam.PositionIds = PositionIds;
                    var specialMenuIds = this.GetMenuByPositions(apsmcqueryParam); //根据岗位ids查找岗位特别配置的菜单ids
                    menusyqueryParam.Ids = specialMenuIds;
                    var specialMenuInfos = this.GetMenuByIds(menusyqueryParam); //根据菜单ids查找菜单信息
                    result.UserMenuInfoList = result.UserMenuInfoList.Concat(specialMenuInfos).DistinctBy(p => p.Id).ToList();
                    #endregion

                    //取得用户数据权限
                    View_DataRolePositionQueryParam udataquery = new View_DataRolePositionQueryParam();
                    //取得用户所有岗位
                    //udataquery.PositionIds = result.UserOrgInfoList.Where(k => k.PositionId.HasValue).Select(p => p.PositionId.Value).Distinct().ToList();
                    udataquery.PositionIds = PositionIds;
                    result.UserDataInfoList = this.GetUserDataInfos(udataquery);
                }
                #region 获取当前登录用户默认组织机构、公司、区域信息
                var OrgAll = (new Org_OrganizationDal()).GetAllOrg();

                //当前登录组织机构
                var notNullOrg = result.UserOrgInfoList.Where(f => !string.IsNullOrEmpty(f.OrganizationCode)).ToList();
                var firstOrg = notNullOrg.OrderBy(f => f.OrganizationCode).FirstOrDefault();
                if (firstOrg != null)
                {
                    //result.CurrentOrganizationId = firstOrg.OrganizationId.Value;
                    //result.CurrentOrganizationCode = firstOrg.OrganizationCode;
                    //result.CurrentOrganizationName = firstOrg.OrganizationName;
                }
                List<string> parentCode = new List<string>();
                //获取当前登录用户的所有父级组织机构
                foreach (var get in notNullOrg)
                {
                    var OriCode = get.OrganizationRelationShipCode.Replace("[" + get.OrganizationCode + "]", "");
                    var Ary = OriCode.Split(']');
                    foreach (var str in Ary)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            parentCode.Add(str.Replace("[", "").Replace("]", ""));
                        }
                    }

                }

                var getCompany = OrgAll.Where(f => f.OrganizationCategoryCode == "Company" && parentCode.Contains(f.Code)).OrderBy(f => f.Code).FirstOrDefault();
                var getArea = OrgAll.Where(f => f.OrganizationCategoryCode == "Region" && parentCode.Contains(f.Code)).OrderBy(f => f.Code).FirstOrDefault();
                if (getCompany != null)
                {
                    //result.CurrentCompanyId = getCompany.OrganizationId;
                    //result.CurrentCompanyCode = getCompany.Code;
                    //result.CurrentCompanyName = getCompany.Name;
                }
                if (getArea != null)
                {
                    //result.CurrentAreaId = getArea.OrganizationId;
                    //result.CurrentAreaCode = getArea.Code;
                    //result.CurrentAreaName = getArea.Name;
                }
                else
                {
                    //未找到区域公司时查找是否存在总公司
                    getArea = OrgAll.Where(f => f.OrganizationCategoryCode == "MainCompany" && parentCode.Contains(f.Code)).OrderBy(f => f.Code).FirstOrDefault();
                    if (getArea != null)
                    {
                        //result.CurrentAreaId = getArea.OrganizationId;
                        //result.CurrentAreaCode = getArea.Code;
                        //result.CurrentAreaName = getArea.Name;
                    }
                }

                #endregion

            }
            return result;
        }

        /// <summary>
        /// 获取用基础信息
        /// 这个方法是==,如果要模糊查询,请另写方法
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public UserBaseInfo GetUserBaseInfo(Org_UserQueryParam queryParam)
        {
            var query = from temp in activeContext.Org_User
                        where (queryParam.Id == Guid.Empty || temp.Id == queryParam.Id)
                           && (string.IsNullOrEmpty(queryParam.LoginName) || temp.LoginName == queryParam.LoginName)
                           && (string.IsNullOrEmpty(queryParam.Phone) || temp.Phone == queryParam.Phone)
                           && (string.IsNullOrEmpty(queryParam.Email) || temp.Email == queryParam.Email)
                           && (string.IsNullOrEmpty(queryParam.IdCard) || temp.IdCard == queryParam.IdCard)
                        select new UserBaseInfo
                        {
                            Id = temp.Id,
                            LoginName = temp.LoginName,
                            Code = temp.Code,
                            UserName = temp.UserName,
                            ShortName = temp.ShortName,
                            Gender = temp.Gender,
                            IdCard = temp.IdCard,
                            Email = temp.Email,
                            Telephone = temp.Telephone,
                            Phone = temp.Phone,
                            Address = temp.Address,
                            QQ = temp.QQ,
                            CreateTime = temp.CreateTime,
                            IsFreeze = temp.IsFreeze,
                            FreezeReason = temp.FreezeReason,
                            Remark = temp.Remark,
                            IsSuperMgr = temp.IsSuperMgr,
                            IsOutSide = temp.IsOutSide
                        };
            var result = query.FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 获取用户组织机构信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_UserOrgInfo> GetUserOrgInfos(Org_UserQueryParam queryParam)
        {
            var result = vuoiDal.GetUserOrgInfos(queryParam);
            return result;
        }

        /// <summary>
        /// 获取用户功能权限信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_FeaturePosition> GetUserFeatureInfos(View_FeaturePositionQueryParam queryParam)
        {
            var result = vfoDal.GetUserFeatureInfos(queryParam);
            return result;
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_MenuSys> GetUserMenuInfoList(View_MenuSysQueryParam queryParam)
        {
            var result = vmsDal.GetUserMenuInfoList(queryParam);
            return result;
        }

        /// <summary>
        /// 根据岗位查找菜单(排除重复的)
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<Guid> GetMenuByPositions(Auth_PositionSpecialMenuConfigQueryParam queryParam)
        {
            var result = apsmcDal.GetMenuByPositions(queryParam);
            return result;
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_MenuSys> GetMenuByIds(View_MenuSysQueryParam queryParam)
        {
            var result = vmsDal.GetMenuByIds(queryParam);
            return result;
        }

        /// <summary>
        /// 获取用户数据权限信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public Hashtable GetUserDataInfos(View_DataRolePositionQueryParam queryParam)
        {
            Hashtable result = vdrpDal.GetUserDataInfos(queryParam);
            return result;
        }

        /// <summary>
        /// 获取所有组织机构员工信息(在职员工)
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_UserOrgInfo> GetAllUserInfos()
        {
            var query = from temp in activeContext.View_UserOrgInfo
                        where temp.IsFreeze == false
                        select temp;
            var result = query.ToList();
            return result;
        }

        #endregion

    }
}

