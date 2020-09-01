using System;
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
    public class CommentController : CCBaseController
    {
        private ICommentServices _commentServices;
        public CommentController(ICommentServices commentServices)
        {
            _commentServices = commentServices;
        }
        #region 评论资源
        [HttpPost]
        public ResultNormal submitRes(SubmitComment commentRes)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                commentRes.userId = this.getUserId();
                result.ResultId = _commentServices.submitResComment(commentRes);
            
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultPager<VueCommentInfo> loadMore_Res(QComment_Res query)
        {
            ResultPager<VueCommentInfo> result = new ResultPager<VueCommentInfo>();
            try
            {
              query.reqUserId = this.getUserId();
              result.PageData = _commentServices.loadMoreComment_Res(query);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除资源评论
        /// </summary>
        /// <param name="delComment"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultNormal deleteRes(DelComment delComment)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                _commentServices.deleteComment_Res(delComment.commentId);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultPager<VueUserComm> UserComment(QUserComm query)
        {
            ResultPager<VueUserComm> result = new ResultPager<VueUserComm>();
            try
            {
              
                result.PageData = _commentServices.queryUserComm(query);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

      
        #endregion

        #region 回复评论
        [HttpPost]
        public ResultNormal submitReply(SubmitReply submitReply)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                submitReply.userId = this.getUserId();
                result.ResultId = _commentServices.submitCommentReply(submitReply);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultPager<VueCommentReply> loadMore_Reply(QComment_Reply query)
        {
            ResultPager<VueCommentReply> result = new ResultPager<VueCommentReply>();
            try
            {
                query.reqUserId = this.getUserId();
                result.PageData = _commentServices.loadMoreComment_Reply(query);
            

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除评论回复
        /// </summary>
        /// <param name="delComment"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultNormal deleteReply(DelComment delComment)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                _commentServices.deleteCommentReply(delComment.replyId,delComment.commentId);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultPager<VueUserCommReply> UserCommentReply(QUserCommReply query)
        {
            ResultPager<VueUserCommReply> result = new ResultPager<VueUserCommReply>();
            try
            {

                result.PageData = _commentServices.queryUserCommReply(query);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        #endregion
    }
}