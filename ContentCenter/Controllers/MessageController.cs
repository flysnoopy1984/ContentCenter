﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MessageController : CCBaseController
    {
        private IMessageServices _messageServices;

        public MessageController(IMessageServices messageServices)
        {
            _messageServices = messageServices;
           
        }
        /// <summary>
        /// 获取用户消息概况
        /// </summary>
        /// <returns></returns>
       [HttpPost]
       public ResultEntity<VueMsgInfoOverview> UserOverview()
       {
            ResultEntity<VueMsgInfoOverview> result = new ResultEntity<VueMsgInfoOverview>();
            try
            {
                string userId = this.getUserId();
                result.Entity = _messageServices.GetUserMsgOverview(userId);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public  ResultPager<VueMsgInfoNotification> QueryUserNotification(QMsgUser query)
        {
            ResultPager<VueMsgInfoNotification> result = new ResultPager<VueMsgInfoNotification>();
            try
            {
                query.userId= this.getUserId();
                result.PageData = _messageServices.QueryUserNotifictaion(query);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultPager<VueSystemNotification> QuerySystemNotification(QMsgUser query)
        {
            ResultPager<VueSystemNotification> result = new ResultPager<VueSystemNotification>();
            try
            {
               query.userId = this.getUserId();
               result.PageData = _messageServices.QuerySystemNotifictaion(query);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultNormal CreateSystemNotificationToGroup(MsgSubmitSystem submitData)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                var userId= this.getUserId();
                if (string.IsNullOrEmpty(userId)  || this.getUserAccount() != "jacky")
                {
                        result.ErrorMsg = "非法发送系统通知";
                }
                else
                    _messageServices.CreateNotification_System(submitData);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

    }
}