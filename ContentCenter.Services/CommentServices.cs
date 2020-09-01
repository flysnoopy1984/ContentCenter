using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class CommentServices: BaseServices<EComment_Res>,ICommentServices
    {
        private ICommentRepository _commentResRepository;
        private IPraizeRepository _praizeRepository;
        private ICommentReplyRepository _commentReplyResRepository;
   //     private ISqlSugarClient _transDb;


        public CommentServices(ICommentRepository commentResRepository,
                               ICommentReplyRepository commentReplyResRepository,
                               IPraizeRepository praizeRepository)
                            //   ISqlSugarClient transDb)
          : base(commentResRepository)
        {
            _commentResRepository = commentResRepository;
            _commentReplyResRepository = commentReplyResRepository;
            _praizeRepository = praizeRepository;
       //     _transDb = transDb;

        }


        #region  资源评论
        public void deleteComment_Res(long commentId)
        {
            try
            {
                _commentResRepository.Db.Ado.BeginTran();

                //删除评论
                _commentResRepository.DeleteByKey(commentId);

                //删除对应的所有回复[ccCommentReply_Res]
                _commentReplyResRepository.DeleteAllReplyByCommentId(commentId);

                //删除评论点赞
                _praizeRepository.DeletePraized_Comment_Res(commentId, null);
                //删除评论对应的所有回复 点赞
                 _praizeRepository.DeletePraized_AllReplyBelowComment(commentId);

                _commentResRepository.Db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                _commentResRepository.Db.Ado.RollbackTran();

                NLogUtil.cc_ErrorTxt("CommentServices deleteComment_Res:" + ex.Message);
                throw new Exception("删除评论失败");
            }
            
           
        }
        public long submitResComment(SubmitComment submitComment)
        {
            if (string.IsNullOrEmpty(submitComment.userId))
                throw new Exception("非法操作！");

            if (string.IsNullOrEmpty(submitComment.parentRefCode))
                throw new Exception("数据缺少[parentRefCode]");

            long result = -1;

            EComment_Res comment = new EComment_Res
            {
                authorId = submitComment.userId,
                content = submitComment.content,
                refCode = submitComment.refCode,
                parentRefCode = submitComment.parentRefCode,
            };

            result = _commentResRepository.Add(comment).Result;

            return result;
        }

        public ModelPager<VueCommentInfo> loadMoreComment_Res(QComment_Res query)
        {
            if (string.IsNullOrEmpty(query.reqUserId))
                throw new Exception("非法操作！");
            return _commentResRepository.GetCommentsByResCodes(query).Result;
        }

        /// <summary>
        /// 用户中心 用户评论
        /// </summary>
        /// <param name="qury"></param>
        /// <returns></returns>
        public ModelPager<VueUserComm> queryUserComm(QUserComm query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new Exception("非法操作！");
            return _commentResRepository.queryUserComm(query).Result;
        }
        #endregion

        #region 评论回复

        public long submitCommentReply(SubmitReply submitReply)
        {
            if (string.IsNullOrEmpty(submitReply.userId))
                throw new Exception("非法操作！");

            long result = -1;

            ECommentReply_Res reply = new ECommentReply_Res
            {
                authorId = submitReply.userId,
                content = submitReply.content,
                commentId = submitReply.commentId,
                replyType = ReplyType.Normal,
            };
            if (submitReply.replyId > 0)
            {
                reply.replyId = submitReply.replyId;
                reply.replyAuthorId = submitReply.replyAuthorId;
                reply.replyName = submitReply.replyAuthorName;
            }
            var transResult = _commentResRepository.Db.Ado.UseTran(() =>
            {   
                _commentResRepository.UpdateComment_ReplyNum(submitReply.commentId, OperationDirection.plus);
                result =_commentReplyResRepository.Add_Sync(reply);
            });
            if(!transResult.IsSuccess)  throw new Exception(transResult.ErrorMessage);
            return result;
        }


        public ModelPager<VueCommentReply> loadMoreComment_Reply(QComment_Reply query)
        {
            if (string.IsNullOrEmpty(query.reqUserId))
                throw new Exception("非法操作！");

            return _commentReplyResRepository.GetReplysByCommentId(query).Result;
        }

        /// <summary>
        /// 删除回复需要更新Comment上的回复总数，需要CommentId
        /// </summary>
        /// <param name="replyId"></param>
        /// <param name="commentId"></param>
        public void deleteCommentReply(long replyId,long commentId)
        {
            var r = _praizeRepository.Db.Ado.UseTran(() =>
            {
                //物理删除点赞
                _praizeRepository.DeletePraized_CommentReply_Res(replyId, null);

                //删除回复
                if (_commentReplyResRepository.DeleteByKey_Sync(replyId))
                {
                    _commentResRepository.UpdateComment_ReplyNum(commentId, OperationDirection.minus, 1);
                }             
                   
            });
            if (!r.IsSuccess)   throw new Exception(r.ErrorMessage);
        }

        public ModelPager<VueUserCommReply> queryUserCommReply(QUserCommReply query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new Exception("非法操作！");
            return _commentReplyResRepository.queryUserCommReply(query).Result;
        }
        #endregion

    }
}
