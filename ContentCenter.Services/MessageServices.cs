using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class MessageServices: BaseServices<EMsgInfo_Praize>, IMessageServices
    {
      
        private IMsgInfoOverviewRepository _msgInfoOverviewRepository;
        private IMsgPraizeRepository _msgPraizeRepository;
        private IMsgCommentResRepository _msgCommentResRepository;
        private IMsgReplyResRepository _msgReplyRepository;

        private IBookRepository _bookRepository;
        private IResourceReponsitory _resourceReponsitory;
        private ICommentRepository _commentRepository;
        private ICommentReplyRepository _commentReplyRepository;

        public MessageServices(IMsgPraizeRepository msgPraizeRepository,
            IMsgCommentResRepository msgCommentResRepository,
            IMsgReplyResRepository msgReplyRepository,

            IResourceReponsitory resourceReponsitory,
            ICommentRepository commentRepository,
            ICommentReplyRepository commentReplyRepository,
            IMsgInfoOverviewRepository msgInfoOverviewRepository,
            
            IBookRepository bookRepository)
         : base(msgPraizeRepository)
        {
            
            _commentReplyRepository = commentReplyRepository;
            _commentRepository = commentRepository;
            _resourceReponsitory = resourceReponsitory;

            _msgPraizeRepository = msgPraizeRepository;
            _msgCommentResRepository = msgCommentResRepository;
            _msgReplyRepository = msgReplyRepository;
            _msgInfoOverviewRepository = msgInfoOverviewRepository;

            _bookRepository = bookRepository;
        }

        /// <summary>
        /// 创建评论消息
        /// </summary>
        /// <param name="msgSubmitComment"></param>
        public void CreateNotification_Comment(MsgSubmitComment msgSubmitComment)
        { 
            var submit = msgSubmitComment.SubmitComment;
            if (msgSubmitComment.CommentId <= 0)
                return;

            if (string.IsNullOrEmpty(submit.userId))
                throw new CCException("非法人员 userId is null ");
            if (string.IsNullOrEmpty(submit.refCode))
                throw new CCException("资源不存在,refCode is null");

            //检查消息内容
            EMsgContent_CommentRes existContent = _msgCommentResRepository.GetContentCommentRes_Sync(submit.refCode);
            if (existContent == null) //不存在
            {
                //新内容
                existContent = commentToContent(msgSubmitComment);
                existContent.Id = _msgCommentResRepository.AddContentCommentRes_Sync(existContent);
            }
            //检查消息是否发送
            //bool IsExistMsg = _msgCommentResRepository.ExistMsgCommentRes_Sync(submit.refCode, submit.userId);
            //if (IsExistMsg) return; //已经发送过就退出
            //else
            //{

            //}
            //评论消息不用检查是否发送过...
            EMsgInfo_CommentRes msg = commentToMsg(msgSubmitComment);
            if (msg == null) return;
            var transResult = _msgCommentResRepository.Db.Ado.UseTran(() =>
            {
                //新消息
                _msgCommentResRepository.AddNoIdentity_Sync(msg);
                //总数
                _msgInfoOverviewRepository.UpdateNotificationNum(NotificationType.comment, msg.ReceiveUserId);
            });
            if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);
        }

        /// <summary>
        /// 创建点赞消息 
        /// </summary>
        /// <param name="msgSubmitPraize"></param>
        public void CreateNotification_Praize(MsgSubmitPraize msgSubmitPraize)
        {
            var submitPraize = msgSubmitPraize.SubmitPraize;
            //没有新的点赞需要通知的
            if (msgSubmitPraize.PraizeId <= 0)
                return;

            if (string.IsNullOrEmpty(submitPraize.userId))
                throw new CCException("非法人员 userId is null ");
            if (string.IsNullOrEmpty(submitPraize.refCode))
                throw new CCException("refCode is null");

            long refId = -1;
            if (submitPraize.praizeTarget == PraizeTarget.Resource)
                refId = submitPraize.resId;
            else
                refId = Convert.ToInt64(submitPraize.refCode); 

            if(refId == 0) throw new Exception("refId不存在");

            //检查消息内容
            EMsgContent_Praize existContent  = _msgPraizeRepository.GetContentPraize_Sync(refId, submitPraize.praizeTarget);
            if(existContent == null) //不存在
            {
                //新内容
                existContent = praizeToContent(msgSubmitPraize);
                existContent.Id = _msgPraizeRepository.AddContentPraize_Sync(existContent);
            }
            //检查消息是否发送
            bool IsExistMsg = _msgPraizeRepository.ExistMsgPraize_Sync(refId, submitPraize.praizeTarget, msgSubmitPraize.SubmitPraize.userId);
            if (IsExistMsg) return; //已经发送过就退出
            else
            {
                EMsgInfo_Praize msg = praizeToMsg(msgSubmitPraize);
                if (msg == null) return;
                var transResult = _msgPraizeRepository.Db.Ado.UseTran(() =>
                {
                    //新消息
                    msg.RefId = existContent.RefId;
                    _msgPraizeRepository.AddNoIdentity_Sync(msg);
                    //总数
                    _msgInfoOverviewRepository.UpdateNotificationNum(NotificationType.praize, msg.ReceiveUserId);
                });
                if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);
            }    
        }

        /// <summary>
        /// 创建回复消息
        /// </summary>
        /// <param name="msgSubmitReply"></param>
        public void CreateNotification_Reply(MsgSubmitReply msgSubmitReply)
        {
            var submit = msgSubmitReply.SubmitReply;
            //没有新的回复需要通知
            if (msgSubmitReply.ReplyId <= 0)
                return;

            if (string.IsNullOrEmpty(submit.userId))
                throw new CCException("非法人员 userId is null ");
            if (submit.commentId<0)
                throw new CCException("commentId is null");

            //检查消息内容
            EMsgContent_ReplyRes existContent = _msgReplyRepository.GetContentReplyRes_Sync(submit.commentId,submit.replyId);
            if (existContent == null) //不存在
            {
                //新内容
                existContent = replyToContent(msgSubmitReply);
                existContent.Id = _msgReplyRepository.AddContentReplyRes_Sync(existContent);
            }
            EMsgInfo_ReplyRes msg = replyToMsg(msgSubmitReply);
            if (msg == null) return;

            var transResult = _msgReplyRepository.Db.Ado.UseTran(() =>
            {
                //新消息
                _msgReplyRepository.AddNoIdentity_Sync(msg);
                //总数
                _msgInfoOverviewRepository.UpdateNotificationNum(NotificationType.reply, msg.ReceiveUserId);
            });
            if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);

        }
        public void SendNotification(SubmitNotification submitNotification)
        {
            //if (string.IsNullOrEmpty(submitNotification.sendId))
            //    throw new CCException("非法人员[通知]");
            //if (submitNotification.relatedId<=0)
            //    throw new CCException("通知-关键数据不完整");

            //switch (submitNotification.notificationType)
            //{
            //    case NotificationType.praize:
            //       // this.PraizeNotification(submitNotification);
            //        break;
            //    case NotificationType.reply:
            //        break;
            //    case NotificationType.comment:
            //        break;
            //}

        }


        #region 私有方法

        private EMsgInfo_Praize praizeToMsg(MsgSubmitPraize msgSubmitPraize)
        {
            var submitPraize = msgSubmitPraize.SubmitPraize;
            EUserInfo ownerInfo = null;
            switch (submitPraize.praizeTarget)
            {
                case PraizeTarget.Resource:
                    ownerInfo = _resourceReponsitory.getResoureOwnerId(submitPraize.refCode);
                    break;
                case PraizeTarget.Comment:
                    ownerInfo = _commentRepository.getCommentAutherId(Convert.ToInt64(submitPraize.refCode));
                    break;
                case PraizeTarget.CommentReply:
                    ownerInfo = _commentReplyRepository.getReplyAutherId(Convert.ToInt64(submitPraize.refCode));
                    break;
            }
            if (ownerInfo == null)
            {
                // NLogUtil.cc_ErrorTxt("[MessageServices]praizeToMsg:没有找到通知者用户Id");
                throw new Exception("[MessageServices]praizeToMsg:没有找到通知者用户Id");
            }

            EMsgInfo_Praize msg = new EMsgInfo_Praize
            {
                CreatedDateTime = DateTime.Now,

                PraizeId = msgSubmitPraize.PraizeId,
                //    RefId = submitPraize.refCode
                NotificationStatus = NotificationStatus.created,
                PraizeTarget = submitPraize.praizeTarget,
                SendUserId = submitPraize.userId,
                SendName = submitPraize.userName,
                SendHeaderUrl = submitPraize.userHeaderUrl,
                ReceiveUserId = ownerInfo.Id,
            };
            return msg;
        }

        /// <summary>
        /// [内容] -- 点赞
        /// </summary>

        private EMsgContent_Praize praizeToContent(MsgSubmitPraize msgSubmitPraize)
        {
            var submitPraize = msgSubmitPraize.SubmitPraize;
            var bi = _bookRepository.getBookSimple_ByCode(submitPraize.bookCode);
            if(bi == null) throw new Exception("消息服务:没有找到书本Code");

            EMsgContent_Praize msgContent = new EMsgContent_Praize
            {
                BookCode = bi.Code,
                BookName = bi.Title,
                BookUrl = bi.CoverUrl,
                PraizeTarget = submitPraize.praizeTarget,
            
            };
            string content ="";
            switch (msgContent.PraizeTarget)
            {
                case PraizeTarget.Resource:
                    EResourceInfo res = _resourceReponsitory.GetByKey(submitPraize.refCode).Result;
                    content = res.ResType == ResType.BookOss ?$"[{res.FileType}]{res.OrigFileName}" :$"[URL]-{res.Url}";
                    msgContent.RefId = res.Id;
                    break;
                case PraizeTarget.Comment:
                    EComment_Res comment = _commentRepository.GetByKey(Convert.ToInt64(submitPraize.refCode)).Result;
                    content = comment.content;
                    msgContent.RefId = comment.Id;
                    break;
                case PraizeTarget.CommentReply:
                    ECommentReply_Res reply = _commentReplyRepository.GetByKey(Convert.ToInt64(submitPraize.refCode)).Result;
                    content = reply.content;
                    msgContent.RefId = reply.Id;
                    break;
            }
            msgContent.OrigContent = content;
            return msgContent;
        }


        private EMsgInfo_CommentRes commentToMsg(MsgSubmitComment msgSubmitComment)
        {

            var submit = msgSubmitComment.SubmitComment;
            var ownerInfo = _resourceReponsitory.getResoureOwnerId(submit.refCode);
            if (ownerInfo == null)
                throw new Exception("[MessageServices]praizeToMsg:没有找到通知者用户Id");

            EMsgInfo_CommentRes msg = new EMsgInfo_CommentRes
            {
                CreatedDateTime = DateTime.Now,
                //   CommentId = msgSubmitComment.CommentId,
                NotificationStatus = NotificationStatus.created,
                CommentId = msgSubmitComment.CommentId,
                resCode = submit.refCode,

                SendUserId = submit.userId,
                SendName = submit.userName,
                SendHeaderUrl = submit.userHeaderUrl,
                ReceiveUserId = ownerInfo.Id,
                ReceiveContent = submit.content,
            };
            return msg;
        }


        /// [内容] --评论消息 
        private EMsgContent_CommentRes commentToContent(MsgSubmitComment msgSubmitComment)
        {
            var submit = msgSubmitComment.SubmitComment;

            var bi = _bookRepository.getBookSimple_ByCode(submit.parentRefCode);
            if (bi == null) throw new Exception("消息服务:没有找到书本Code");

            //Orig Content(针对哪个资源信息做的消息)
            EResourceInfo res = _resourceReponsitory.GetByKey(submit.refCode).Result;
            var origContent = res.ResType == ResType.BookOss ? $"[{res.FileType}]{res.OrigFileName}" : $"[URL]-{res.Url}";
            
            EMsgContent_CommentRes msgContent = new EMsgContent_CommentRes
            {
                BookCode = bi.Code,
                BookName = bi.Title,
                BookUrl = bi.CoverUrl,
                ResCode = res.Code,
                OrigContent = origContent,
               
                
            };

            return msgContent;

        }
        
        /// <summary>
        /// [内容] -- 回复信息
        /// </summary>
        private EMsgContent_ReplyRes replyToContent(MsgSubmitReply msgSubmitReply)
        {
            var submit = msgSubmitReply.SubmitReply;

            var bi = _bookRepository.getBookSimple_ByCode(submit.bookCode);
            if (bi == null) throw new Exception("消息服务:没有找到书本Code");

            EComment_Res comment = _commentRepository.GetByKey(submit.commentId).Result;
   
            EMsgContent_ReplyRes msgContent = new EMsgContent_ReplyRes
            {
                BookCode = bi.Code,
                BookName = bi.Title,
                BookUrl = bi.CoverUrl,
                CommentId = submit.commentId,
                OrigContent = comment.content,
               
                ReplyId = submit.replyId,    
            };
            if (submit.replyId > 0)
            {
                ECommentReply_Res reply = _commentReplyRepository.GetByKey(submit.replyId).Result;
                msgContent.OrigReplyContent = reply.content;
            }

            return msgContent;
        }

        private EMsgInfo_ReplyRes replyToMsg(MsgSubmitReply msgSubmitReply)
        {
            var submit = msgSubmitReply.SubmitReply;
            EUserInfo ownerInfo = null;
            if (submit.replyId <0) //接受者--回复评论的人
                ownerInfo = _commentRepository.getCommentAutherId(submit.commentId);
            else //接受者--回复回复的人
                ownerInfo = _commentReplyRepository.getReplyAutherId(submit.replyId);

            if (ownerInfo == null)
                throw new Exception("[MessageServices]replyToMsg:没有找到通知者用户Id");

            EMsgInfo_ReplyRes msg = new EMsgInfo_ReplyRes
            {
                CreatedDateTime = DateTime.Now,
                //   CommentId = msgSubmitComment.CommentId,
                NotificationStatus = NotificationStatus.created,
                CommentId = submit.commentId,
                ReplyId = msgSubmitReply.ReplyId,
                ReplyReplyId = submit.replyId,
                
                SendUserId = submit.userId,
                SendName = submit.userName,
                SendHeaderUrl = submit.userHeaderUrl,
                ReceiveUserId = ownerInfo.Id,
                ReceiveContent = submit.content,
            };
           
            return msg;
        }

     

        #endregion



    }
}
