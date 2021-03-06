﻿using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentCenter.Services
{
 
    public class UserServices : BaseServices<EUserInfo>,IUserServices
    {
        private IUserRepository _userDb;
        private IUserBookRepository _userBookRepository;
        private IUserFinanceOverViewRepository _userFinanceOverViewRepository;
        private IUserFinanceRepository _userFinanceRepository;
        private IMsgInfoOverviewRepository _msgInfoOverviewRepository;
        private IMsgSystemRepository _msgSystemRepository;

        public UserServices(IUserRepository userRepository, 
            IUserBookRepository userBookRepository,
            IUserFinanceOverViewRepository userFinanceOverViewRepository,
            IUserFinanceRepository userFinanceRepository,
            IMsgInfoOverviewRepository msgInfoOverviewRepository,
            IMsgSystemRepository msgSystemRepository)
            :base(userRepository)
        {
            _msgInfoOverviewRepository = msgInfoOverviewRepository;
            _userBookRepository = userBookRepository;
            _userDb = userRepository;
            _userFinanceOverViewRepository = userFinanceOverViewRepository;
            _userFinanceRepository = userFinanceRepository;
            _msgSystemRepository = msgSystemRepository;
        }

        /* 用户书本收藏夹 Begin */
        public long AddFavBook(string bookCode,string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            return _userBookRepository.AddUserBook(bookCode, userId).Result;
        }

        public bool DelFavBook(string bookCode, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            return _userBookRepository.DelUserBook(bookCode, userId).Result;
        }

        public ModelPager<VueUserBook> queryUserbookList(QUserBook query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new Exception("非法操作！");
            return _userBookRepository.queryUserBook(query).Result;
        }

        /* 用户书本收藏夹 End */

        public bool HasRegistPhone(string phone)
        {
             var r = _userDb.GetCount(a => a.Phone == phone).Result;
            return r > 0;
        }

        public VueUserLogin Login(LoginUser loginUser)
        {
            VueUserLogin result = new VueUserLogin();
            var ui = _userDb.GetByExpSingle(a=>a.UserAccount == loginUser.Account && a.UserPwd == loginUser.Pwd).Result;
            if (ui == null)
                throw new CCException(CCWebMsg.User_Login_WrongUserPwd);

            result.MsgOverview = _msgInfoOverviewRepository.GetByUserId(ui.Id);

            result.UerInfo = ui.ToVueUser();
           
            return result;
        }

        public VueUerInfo GetAndVerifyUserForId4(string userAccount, string userPwd)
        {
            var ui = _userDb.GetByExpSingle(a => a.UserAccount == userAccount && a.UserPwd == userPwd).Result;
            if (ui == null)
                throw new CCException(CCWebMsg.User_Login_WrongUserPwd);
            return ui.ToVueUser();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="regUser"></param>
        /// <returns>-1 已存在,-2手机已使用</returns>
        public VueUserLogin Register(RegUser regUser)
        {
            int c = _userDb.GetCount(a=>a.UserAccount == regUser.Account).Result;
            int phone = _userDb.GetCount(a => a.Phone == regUser.Phone).Result;
            if (c > 0)
                throw new CCException(CCWebMsg.User_Reg_Exist_Account);
            if (phone > 0)
                throw new CCException(CCWebMsg.User_Reg_Exist_Phone);

            //用户基本信息
            EUserInfo ui = new EUserInfo
            {
                Id = CodeManager.UserCode(),  //Guid.NewGuid().ToString("N"),
                UserAccount = regUser.Account,
                UserPwd = regUser.Pwd,
                Phone = regUser.Phone,
                NickName = regUser.Account,
                Group_Notification = Model.BaseEnum.Group_Notification.normal,
            };
            //用户财务概览
            EUserFinanceOverview financeOverview = new EUserFinanceOverview
            {
                userId = ui.Id,
                pointEffectDate = DateTime.Now.AddDays(90),
                money = 0,
                chargePoint = 0,
                fixedPoint = 20,
            };
            //注册赠送积分
            EUserPointsTrans trans = new EUserPointsTrans
            {
                userId = ui.Id,
                createdDateTime = DateTime.Now,
                changeType = Model.BaseEnum.PointChangeType.newRegister,
                point = 20,
            };
            //消息通知（欢迎新用户）
            EMsgInfo_System msg = new EMsgInfo_System
            {
                ContentId = 10000,
                NotificationStatus = NotificationStatus.sent,
                ReceiveUserId = ui.Id,
            };
            DbResult<bool> transResult = null;
            transResult = _userDb.Db.Ado.UseTran(() =>
            {
                _userDb.AddNoIdentity_Sync(ui);
                _userFinanceOverViewRepository.AddNoIdentity_Sync(financeOverview);
                _userFinanceRepository.AddPointTrans_Sync(trans);
                //消息通知（欢迎新用户）
                _msgSystemRepository.AddNoIdentity_Sync(msg);
                //用户消息概览
                _msgInfoOverviewRepository.InitForNewUser_Sync(ui.Id);
               
            });
            if(!transResult.IsSuccess)
                throw new Exception("注册失败");

            VueUserLogin result = new VueUserLogin();
            result.UerInfo = ui.ToVueUser();
            result.MsgOverview = new VueMsgInfoOverview();
            result.MsgOverview.userId = ui.Id;
            return result;

        }

        public VueUC_UserInfo getUC_User(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            return _userDb.getUC_User(userId);
        }

        public void updateHeader(string userId, string headerUrl)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            _userDb.updateHeader(userId, headerUrl);
        }

        public void updateInfo(VueSubmitUserInfo submitData)
        {
            if (string.IsNullOrEmpty(submitData.userId))
                throw new Exception("非法操作！");
            if (string.IsNullOrEmpty(submitData.nickName))
                throw new Exception("昵称不能为空！");
            _userDb.updateInfo(submitData);
        
        }

        public List<UserSimple> queryNotificationGroup(Group_Notification group_Notification)
        {
            return _userDb.queryNotificationGroup(group_Notification);
        }
    }
}
