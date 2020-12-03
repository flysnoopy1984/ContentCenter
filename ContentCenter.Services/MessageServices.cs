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
using System.Threading;
using System.Threading.Tasks;

namespace ContentCenter.Services
{
    public class MessageServices: BaseServices<EMsgInfo_Praize>, IMessageServices
    {
      
        private IMsgInfoOverviewRepository _msgInfoOverviewRepository;
        private IMsgPraizeRepository _msgPraizeRepository;
        private IMsgCommentResRepository _msgCommentResRepository;
        private IMsgReplyResRepository _msgReplyRepository;
        private IMsgSystemRepository _msgSystemRepository;

        private IBookRepository _bookRepository;
        private IResourceReponsitory _resourceReponsitory;
        private ICommentRepository _commentRepository;
        private ICommentReplyRepository _commentReplyRepository;
        private IUserRepository _userRepository;

        public MessageServices(IMsgPraizeRepository msgPraizeRepository,
            IMsgCommentResRepository msgCommentResRepository,
            IMsgReplyResRepository msgReplyRepository,
            IMsgSystemRepository msgSystemRepository,

            IResourceReponsitory resourceReponsitory,
            ICommentRepository commentRepository,
            ICommentReplyRepository commentReplyRepository,
            IMsgInfoOverviewRepository msgInfoOverviewRepository,

            IUserRepository userRepository,
            IBookRepository bookRepository)
         : base(msgPraizeRepository)
        {
            
            _commentReplyRepository = commentReplyRepository;
            _commentRepository = commentRepository;
            _resourceReponsitory = resourceReponsitory;

            _msgSystemRepository = msgSystemRepository;
            _msgPraizeRepository = msgPraizeRepository;
            _msgCommentResRepository = msgCommentResRepository;
            _msgReplyRepository = msgReplyRepository;
            _msgInfoOverviewRepository = msgInfoOverviewRepository;

            _bookRepository = bookRepository;
            _userRepository = userRepository;
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
           
            //评论消息不用检查是否发送过...
            EMsgInfo_CommentRes msg = commentToMsg(msgSubmitComment);
            if (msg == null) return;
            // //检查发送者和接受者是否同一人
            if (msg.ReceiveUserId == msg.SendUserId)
                return;

            var transResult =  _msgCommentResRepository.Db.Ado.UseTran(() =>
            {
                //新消息
                _msgCommentResRepository.AddNoIdentity_Sync(msg);
                //总数
                _msgInfoOverviewRepository.UpdateNotificateToUnRead(NotificationType.comment, msg.ReceiveUserId);
            });
            if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);
        }

        /// <summary>
        /// 创建点赞消息 
        /// </summary>
        /// <param name="msgSubmitPraize"></param>
        public void CreateNotification_Praize(MsgSubmitPraize msgSubmitPraize)
        {
         //  Console.WriteLine("start CreateNotification_Praize");
          
            var submitPraize = msgSubmitPraize.SubmitPraize;
            //没有新的点赞需要通知的
            if (msgSubmitPraize.PraizeId <= 0)
                return;

            if (string.IsNullOrEmpty(submitPraize.userId))
                throw new CCException("非法人员 userId is null ");
            if (string.IsNullOrEmpty(submitPraize.refCode))
                throw new CCException("refCode is null");

         

            //检查消息内容
            EMsgContent_Praize existContent  = _msgPraizeRepository.GetContentPraize_Sync(submitPraize.refCode, submitPraize.praizeTarget);
            if(existContent == null) //不存在
            {
                //新内容
                existContent = praizeToContent(msgSubmitPraize);
                existContent.Id = _msgPraizeRepository.AddContentPraize_Sync(existContent);
            }
            //检查消息是否发送
            bool IsExistMsg = _msgPraizeRepository.ExistMsgPraize_Sync(submitPraize.refCode, submitPraize.praizeTarget, msgSubmitPraize.SubmitPraize.userId);
            if (IsExistMsg) return; //已经发送过就退出
            else
            {
                EMsgInfo_Praize msg = praizeToMsg(msgSubmitPraize);
                if (msg == null) return;
                // //检查发送者和接受者是否同一人
                if (msg.ReceiveUserId == msg.SendUserId)
                    return;

                var transResult =   _msgPraizeRepository.Db.Ado.UseTran(() =>
                {
                    //新消息
                    msg.RefId = existContent.RefId;
                    _msgPraizeRepository.AddNoIdentity_Sync(msg);
                    //总数
                    _msgInfoOverviewRepository.UpdateNotificateToUnRead(NotificationType.praize, msg.ReceiveUserId);
                });
                if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);
            }
         //   Console.WriteLine("end CreateNotification_Praize");
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
            var res = _resourceReponsitory.getSimpleByCommentId(submit.commentId);
            if (res == null) throw new Exception("消息服务[CreateNotification_Reply]:没有找到资源");

            EMsgContent_ReplyRes existContent = _msgReplyRepository.GetContentReplyRes_Sync(res.Code);
            if (existContent == null) //不存在
            {
                //新内容
                existContent = replyToContent(msgSubmitReply,res);
                existContent.Id = _msgReplyRepository.AddContentReplyRes_Sync(existContent);
            }
            EMsgInfo_ReplyRes msg = replyToMsg(msgSubmitReply,res);
            if (msg == null) return;

            // //检查发送者和接受者是否同一人
            if (msg.ReceiveUserId == msg.SendUserId)
                return;

            var transResult =  _msgReplyRepository.Db.Ado.UseTran(() =>
            {
                //新消息
                _msgReplyRepository.AddNoIdentity_Sync(msg);
                //总数
                _msgInfoOverviewRepository.UpdateNotificateToUnRead(NotificationType.reply, msg.ReceiveUserId);
            });
            if (!transResult.IsSuccess) throw new Exception(transResult.ErrorMessage);

        }

        public void CreateNotification_System(MsgSubmitSystem submitSystem)
        {
            EMsgContent_System existContent = _msgSystemRepository.GetContentSystem_Sync(submitSystem.contentId);
            if (existContent ==null)
            {
                existContent = new EMsgContent_System()
                {
                    htmlContent = submitSystem.htmlContent,
                    htmlTitle = submitSystem.htmlTitle,
                    Id = submitSystem.contentId
                };
                _msgSystemRepository.AddContentSystem(existContent);
            }
            this.CreateSystemNoteMessage(submitSystem);
            //EMsgInfo_System msg = new EMsgInfo_System(); 
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

        public VueMsgInfoOverview GetUserMsgOverview(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new CCException("非法操作");

            return  _msgInfoOverviewRepository.GetByUserId(userId);
        }

        public  ModelPager<VueMsgInfoNotification> QueryUserNotifictaion(QMsgUser query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new CCException("非法请求");
            ModelPager<VueMsgInfoNotification> result = null;
            switch (query.notificationType)
            {
                case NotificationType.praize:
                    result = _msgPraizeRepository.queryUserPraize(query).Result;
                    break;
                case NotificationType.comment:
                    result = _msgCommentResRepository.queryUserComment(query).Result;
                    break;
                case NotificationType.reply:
                    result= _msgReplyRepository.queryUserReply(query).Result;
                    break;
            }
            //异步更新消息数据
            if (query.updateMsgToRead){
               Async_MsgToReadAfterQuery(query, result.datas);  
            }
          
            return result == null? new ModelPager<VueMsgInfoNotification>():result;

        }
        public ModelPager<VueSystemNotification> QuerySystemNotifictaion(QMsgUser query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new CCException("非法请求");
            ModelPager<VueSystemNotification> result = _msgSystemRepository.querySystemNotification(query);
            if (query.updateMsgToRead)
            {
                Async_MsgToReadAfterQuery(query,null,result.datas);
            }
            return result;
        }
        //异步方法 查询后获取未读消息转成已读
        public void Async_MsgToReadAfterQuery(QMsgUser query,
            List<VueMsgInfoNotification> queryResult =null,
            List<VueSystemNotification> systemResult = null  //由于系统消息对象不同，暂时只能这样写。。。
            )
        {
            Task.Run(() =>
            {
                try
                {
                    SubmitUnReadMsgIdList unReadList = new SubmitUnReadMsgIdList();
                    if(query.notificationType == NotificationType.system)
                    {
                        foreach (var msg in systemResult)
                        {
                            if (msg.NotificationStatus != NotificationStatus.read)
                                unReadList.msgIdList.Add(msg.msgId);
                        }
                    }
                    else
                    {
                        foreach (var msg in queryResult)
                        {
                            if (msg.NotificationStatus != NotificationStatus.read)
                                unReadList.msgIdList.Add(msg.msgId);
                        }
                    }
                   
                    if (unReadList.msgIdList.Count > 0)
                    {
                        unReadList.notificationType = query.notificationType;
                        unReadList.targetStatus = NotificationStatus.read;
                        unReadList.userId = query.userId;


                        this.updateMsgToRead(unReadList);
                    }

                }
                catch (Exception ex)
                {
                    NLogUtil.cc_ErrorTxt("[MessageService]MsgToReadAfterQuery_Async:" + ex.Message);
                }
            });

        }

        #region 私有方法

        //更新消息到已读
        private void updateMsgToRead(SubmitUnReadMsgIdList submitData)
        {
            //   ResultNormal result = new ResultNormal();
            if (string.IsNullOrEmpty(submitData.userId))
                throw new CCException("非法请求");

            var msgList = submitData.msgIdList;
            if (msgList == null || msgList.Count <= 0)
                throw new Exception("[MessgeServices] updateMsgToRead:入参没有信息更新");

            var transResult = _msgReplyRepository.Db.Ado.UseTran(() =>
            {
                int num = 0;
                //更新已读消息
                switch (submitData.notificationType)
                {
                    case NotificationType.praize:
                        num = _msgPraizeRepository.UpdateMsgStatus(submitData);
                        break;
                    case NotificationType.comment:
                        num = _msgCommentResRepository.UpdateMsgStatus(submitData);
                        break;
                    case NotificationType.reply:
                        num = _msgReplyRepository.UpdateMsgStatus(submitData);
                        break;
                    case NotificationType.system:
                        num = _msgSystemRepository.UpdateMsgStatus(submitData);
                        break;

                }
                if (num > 0)
                    //概况更新为已读
                    _msgInfoOverviewRepository.UpdateNotificateToRead(submitData.notificationType, submitData.userId, num);
            });

            if (!transResult.IsSuccess)
            {
                throw new Exception(transResult.ErrorMessage);
                // result.ErrorMsg = transResult.ErrorMessage;
            }

        }


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
                //NotificationStatus = NotificationStatus.created,
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
            if(bi == null) throw new Exception("消息服务[praizeToContent]:没有找到书本Code");

            EMsgContent_Praize msgContent = new EMsgContent_Praize
            {
                BookCode = bi.Code,
                BookName = bi.Title,
                BookUrl = bi.CoverUrl,
                PraizeTarget = submitPraize.praizeTarget,
            };
            string content ="";
            ResSimple res = null;
            switch (msgContent.PraizeTarget)
            {
                case PraizeTarget.Resource:
                    res = _resourceReponsitory.getSimpleByCode(submitPraize.refCode);
                 //   content = res.ResType == ResType.BookOss ?$"[文件]-{res.OrigFileName}" :$"[URL]-{res.Url}";
                    msgContent.RefId = res.Code;
                    break;
                case PraizeTarget.Comment:
                    EComment_Res comment = _commentRepository.GetByKey(Convert.ToInt64(submitPraize.refCode)).Result;
                    content = comment.content;
                    msgContent.RefId = comment.Id.ToString();
                    msgContent.CommentId = comment.Id;
                    msgContent.OrigContent = content;
                    res = _resourceReponsitory.getSimpleByCode(submitPraize.parentRefCode);
                    break;
                case PraizeTarget.CommentReply:
                    ECommentReply_Res reply = _commentReplyRepository.GetByKey(Convert.ToInt64(submitPraize.refCode)).Result;
                    content = reply.content;
                    msgContent.CommentId = reply.commentId;
                    msgContent.ReplyId = reply.Id;
                    msgContent.RefId = reply.Id.ToString();
                    msgContent.OrigContent = content;
                    res = _resourceReponsitory.getSimpleByCommentId(Convert.ToInt64(submitPraize.parentRefCode));
                    break;
            }
            if(res == null)
                throw new Exception("消息服务[praizeToContent]:没有找到资源信息");
            msgContent.ResCode = res.Code;
            msgContent.ResName = res.ShowName;
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
                //NotificationStatus = NotificationStatus.created,
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
            if (bi == null) throw new Exception("消息服务[commentToContent]:没有找到书本Code");

            //Orig Content(针对哪个资源信息做的消息)
            var res = _resourceReponsitory.getSimpleByCode(submit.refCode);
            if (res == null) throw new Exception("消息服务[commentToContent]:没有找资源");
            //EResourceInfo res = _resourceReponsitory.GetByKey(submit.refCode).Result;
            //var origContent = res.ResType == ResType.BookOss ? $"[{res.FileType}]{res.OrigFileName}" : $"[URL]-{res.Url}";

            EMsgContent_CommentRes msgContent = new EMsgContent_CommentRes
            {
                BookCode = bi.Code,
                BookName = bi.Title,
                BookUrl = bi.CoverUrl,
                ResCode = res.Code,
                ResName = res.ShowName,
                
             //   OrigContent = submit.content,  
            };


            return msgContent;

        }
        
        /// <summary>
        /// [内容] -- 回复信息
        /// </summary>
        private EMsgContent_ReplyRes replyToContent(MsgSubmitReply msgSubmitReply,ResSimple resSimple)
        {
            var submit = msgSubmitReply.SubmitReply;

            var bi = _bookRepository.getBookSimple_ByCode(submit.bookCode);
            if (bi == null) throw new Exception("消息服务[replyToContent]:没有找到书本Code");
           

            //评论原始内容
            //   EComment_Res comment = _commentRepository.GetByKey(submit.commentId).Result;
            EMsgContent_ReplyRes msgContent = new EMsgContent_ReplyRes
            {
                BookCode = bi.Code,
                BookName = bi.Title,
                BookUrl = bi.CoverUrl,
                ResCode = resSimple.Code,
                ResName = resSimple.ShowName
            };
            //资源信息
            //var res = _resourceReponsitory.getSimpleByCommentId(submit.commentId);
            //msgContent.ResCode = res.Code;
            //msgContent.ResName = res.ShowName;

            //if (submit.replyId > 0)
            //{
            //    //回复信息
            //    ECommentReply_Res reply = _commentReplyRepository.GetByKey(submit.replyId).Result;
            //    msgContent.OrigReplyContent = reply.content
            //}

            return msgContent;
        }

        private EMsgInfo_ReplyRes replyToMsg(MsgSubmitReply msgSubmitReply, ResSimple resSimple)
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
               // NotificationStatus = NotificationStatus.created,
                CommentId = submit.commentId,
                ReplyId = msgSubmitReply.ReplyId,
                ReplyReplyId = submit.replyId,
                resCode = resSimple.Code,
                SendUserId = submit.userId,
                SendName = submit.userName,
                SendHeaderUrl = submit.userHeaderUrl,
                ReceiveUserId = ownerInfo.Id,
                ReceiveContent = submit.content,
            };
           
            return msg;
        }

        private void CreateSystemNoteMessage(MsgSubmitSystem submitSystem)
        {
            DbResult<bool> transResult = null;

             if (submitSystem.systemNoteTarget == SystemNoteTarget.Single)
            {
                EMsgInfo_System msg = new EMsgInfo_System
                {
                    CreatedDateTime = DateTime.Now,
                    ContentId = submitSystem.contentId,
                    ReceiveUserId = submitSystem.receiveUserId,
                };
                transResult = _msgReplyRepository.Db.Ado.UseTran(() =>
                {
                    //新消息
                    _msgSystemRepository.AddNoIdentity_Sync(msg);
                    //总数
                    _msgInfoOverviewRepository.UpdateNotificateToUnRead(NotificationType.system, msg.ReceiveUserId);
                });
            }
            else
            {
                List<UserSimple> userList = _userRepository.queryNotificationGroup(submitSystem.receiveGroupId);
                List<EMsgInfo_System> msgList = new List<EMsgInfo_System>();
               
                foreach (var u in userList)
                {
                    EMsgInfo_System msg = new EMsgInfo_System
                    {
                        CreatedDateTime = DateTime.Now,
                        ContentId = submitSystem.contentId,
                        ReceiveUserId = u.UserId,
                    };
                    msgList.Add(msg);


                }
                transResult = _msgReplyRepository.Db.Ado.UseTran(() =>
                {
                    //新消息
                    _msgSystemRepository.AddRange(msgList);
                    //总数
                    _msgInfoOverviewRepository.UpdateGroupToUnRead(submitSystem.receiveGroupId);
                });
            }

            if (transResult !=null && !transResult.IsSuccess) 
                throw new Exception(transResult.ErrorMessage);


        }

       

        #endregion



    }
}
