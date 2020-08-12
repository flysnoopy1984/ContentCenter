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
        private ISqlSugarClient _transDb;


        public CommentServices(ICommentRepository commentResRepository,
                               ICommentReplyRepository commentReplyResRepository,
                               IPraizeRepository praizeRepository,
                               ISqlSugarClient transDb)
          : base(commentResRepository)
        {
            _commentResRepository = commentResRepository;
            _commentReplyResRepository = commentReplyResRepository;
            _praizeRepository = praizeRepository;
            _transDb = transDb;

        }


        #region  资源评论
        public void deleteComment_Res(long commentId)
        {
            try
            {
                _commentResRepository.Db.Ado.BeginTran();

                var comment = _commentResRepository.GetByKey(commentId).Result;
                var praize = _praizeRepository.GetPraize_Res(comment.refCode, comment.authorId).Result;
                //删除该评论对应的资源点赞
                if (praize != null)
                {
                    _praizeRepository.DeletePraized_Res(comment.refCode, comment.authorId);
                    _praizeRepository.UpdateResPraizedNum(comment.refCode, praize.PraizeType, OperationDirection.minus);
                }

                //删除评论
                _commentResRepository.DeleteByKey(commentId);
             //   _praizeRepository.DeletePraized_Comment_Res(commentId);

                //删除对应的所有回复
                _commentReplyResRepository.DeleteAllReplyByCommentId(commentId);
            //    _praizeRepository.DeletePraized_CommentReply_Res(commentId);

                _commentResRepository.Db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                _commentResRepository.Db.Ado.RollbackTran();

                NLogUtil.cc_ErrorTxt("CommentServices deleteComment_Res:" + ex.Message);
                throw new Exception("删除资源评论失败");
            }
            
           
        }
        public long submitResComment(SubmitComment submitComment)
        {
            if (string.IsNullOrEmpty(submitComment.userId))
                throw new Exception("非法操作！");

            long result = -1;

            EComment_Res comment = new EComment_Res
            {
                authorId = submitComment.userId,
                content = submitComment.content,
                refCode = submitComment.refCode,
            };

            result = _commentResRepository.Add(comment).Result;

            return result;
        }

        public ModelPager<VueCommentInfo> loadMoreComment_Res(QComment_Res query)
        {
            return _commentResRepository.GetCommentsByResCodes(query).Result;
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
            var r = _transDb.Ado.UseTranAsync(() =>
            {
              
                _commentResRepository.UpdateComment_ReplyNum(submitReply.commentId, OperationDirection.plus);
                result =_commentReplyResRepository.Add(reply).Result;
            });
          

            return result;
        }


        public ModelPager<VueCommentReply> loadMoreComment_Reply(QComment_Reply query)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
