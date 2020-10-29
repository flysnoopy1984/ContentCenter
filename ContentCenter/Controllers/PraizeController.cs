using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PraizeController : CCBaseController
    {
        private IPraizeServices _praizeServices;
        private IMessageServices _messageServices;

        public PraizeController(IPraizeServices praizeServices, IMessageServices messageServices)
        {
            _praizeServices = praizeServices;
            _messageServices = messageServices;
        }

        /// <summary>
        /// 提交点赞
        /// </summary>
        /// <param name="submitPraize"></param>
        /// <returns></returns>
        [HttpPost]
        public  ResultNormal submit(SubmitPraize submitPraize)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                if(string.IsNullOrEmpty(submitPraize.userId)) submitPraize.userId = this.getUserId();
                else
                {
                    if(submitPraize.userId != this.getUserId())
                        throw new CCException("身份不明确，请登录后再尝试");
                }
                result.ResultId = _praizeServices.handlePraize(submitPraize);

                //创建通知消息
                asyncCreateMessage(submitPraize, result.ResultId);
               // Console.WriteLine("controller return");

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取用户点过的赞的资源，评论，回复。
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultPager<VueUserPraize> UserPraize(QUserPraize query)
        {
            ResultPager<VueUserPraize> result = new ResultPager<VueUserPraize>();
            try
            {
                result.PageData = _praizeServices.queryUserPraize(query);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        private void asyncCreateMessage(SubmitPraize submitPraize,long praizeId)
        {
            Task.Run(() =>
            {
                try
                {
                    _messageServices.CreateNotification_Praize(new MsgSubmitPraize
                    {
                        SubmitPraize = submitPraize,
                        PraizeId = praizeId,
                    });


                }
                catch (Exception msgEx)
                {
                    NLogUtil.cc_ErrorTxt("【点赞通知】错误:" + msgEx.Message);
                }
            });
        }
    }
}