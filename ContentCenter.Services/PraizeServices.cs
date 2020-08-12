﻿using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class PraizeServices: BaseServices<EPraize_Res>, IPraizeServices
    {
        private IPraizeRepository _praizeRepository;
        private ISqlSugarClient _transDb;

        public PraizeServices(IPraizeRepository praizeRepository, ISqlSugarClient transDb)
         : base(praizeRepository)
        {
          
            _praizeRepository = praizeRepository;
            _transDb = transDb;
        }

        public long handlePraize(SubmitPraize submitPraize)
        {
            if (string.IsNullOrEmpty(submitPraize.userId))
                throw new Exception("非法操作！");

            switch (submitPraize.praizeTarget)
            {
                case PraizeTarget.Resource:
                    return this.handlePraize_Res(submitPraize);
                case PraizeTarget.Comment:
                    return handPraize_Comment(submitPraize);
                case PraizeTarget.CommentReply:
                    return handPraize_CommentReply(submitPraize);
            }
            return 0;
          
        }

        private long handPraize_CommentReply(SubmitPraize submitPraize)
        {
            if (submitPraize.praizeDirection == OperationDirection.plus)
            {
                if (_praizeRepository.HasPraized_CommentReply_Res(Convert.ToInt64(submitPraize.refCode), submitPraize.userId).Result > 0)
                {
                    return 0;
                }

                EPraize_CommentReply praize = new EPraize_CommentReply
                {
                    praizeDate = DateTime.Now,
                    PraizeType = submitPraize.praizeType,
                    commentReplyId = Convert.ToInt64(submitPraize.refCode),
                    userId = submitPraize.userId,
                    commentId =  Convert.ToInt64(submitPraize.parentRefCode),
                };

                _transDb.Ado.UseTranAsync(() =>
                {
                    _praizeRepository.AddPraize_CommentReply(praize);
                    _praizeRepository.UpdateCommentReplyPraized_GoodNum(Convert.ToInt64(submitPraize.refCode), OperationDirection.plus);
                });
            }
            return 0;
        }

        private long handPraize_Comment(SubmitPraize submitPraize)
        {
            if (submitPraize.praizeDirection == OperationDirection.plus)
            {
                if (_praizeRepository.HasPraized_Comment_Res(Convert.ToInt64(submitPraize.refCode), submitPraize.userId).Result > 0)
                {
                    return 0;
                }

                EPraize_Comment praize = new EPraize_Comment
                {
                    praizeDate = DateTime.Now,
                    PraizeType = PraizeType.good,
                    commentId = Convert.ToInt64(submitPraize.refCode),
                    userId = submitPraize.userId,
                    RefCode = submitPraize.parentRefCode,
                };

                _transDb.Ado.UseTranAsync(() =>
                {
                    _praizeRepository.AddPraize_Comment(praize);
                    _praizeRepository.UpdateCommentPraized_GoodNum(Convert.ToInt64(submitPraize.refCode),OperationDirection.plus);
                });
            }
            else
            {
                _transDb.Ado.UseTranAsync(() =>
                {
                    _praizeRepository.DeletePraized_Comment_Res(Convert.ToInt64(submitPraize.refCode), submitPraize.userId);
                    _praizeRepository.UpdateCommentPraized_GoodNum(Convert.ToInt64(submitPraize.refCode), OperationDirection.minus);
                });
            }
            return 0;
        }

        /// <summary>
        /// 处理点赞，新增，取消，切换
        /// </summary>
        /// <param name="submitPraize"></param>
        /// <returns></returns>
        private long handlePraize_Res(SubmitPraize submitPraize)
        {
            /*！！可能的漏洞
             * 没有对更新的赞和取消的赞做原始赞检查
             */
            if (submitPraize.praizeDirection == OperationDirection.plus){
                if (_praizeRepository.HasPraized_Res(submitPraize.refCode, submitPraize.userId).Result > 0){
                    return 0;
                }       
            }
            EPraize_Res praize = new EPraize_Res
            {
                praizeDate = DateTime.Now,
                PraizeType = submitPraize.praizeType,
                ResCode = submitPraize.refCode,
                userId = submitPraize.userId,
                RefCode = submitPraize.parentRefCode,
               
            };
            switch (submitPraize.praizeDirection){
                case OperationDirection.plus:
                    _transDb.Ado.UseTranAsync(() =>
                    {
                         _praizeRepository.Add(praize);
                        _praizeRepository.UpdateResPraizedNum(submitPraize.refCode, submitPraize.praizeType, OperationDirection.plus);
                    });
                    break;
                case OperationDirection.minus:
                    _transDb.Ado.UseTranAsync(() =>
                    {
                        _praizeRepository.DeletePraized_Res(submitPraize.refCode, submitPraize.userId);
                        _praizeRepository.UpdateResPraizedNum(submitPraize.refCode, submitPraize.praizeType, OperationDirection.minus);
                    });
                    break;
                case OperationDirection.update:
                    _transDb.Ado.UseTranAsync(() =>
                    {
                        _praizeRepository.UpdatePraized_Res(submitPraize.praizeType,submitPraize.refCode, submitPraize.userId);
                        _praizeRepository.UpdateResPraizedNum(submitPraize.refCode, submitPraize.praizeType, OperationDirection.update);
                    });
                    break;
            }
         
            return 0;
        }

       
    }
}