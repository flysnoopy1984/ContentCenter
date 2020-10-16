using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IRepository
{
    public interface IMsgCommentResRepository:IBaseRepository<EMsgInfo_CommentRes>
    {
        EMsgContent_CommentRes GetContentCommentRes_Sync(string recCode);

      //  bool ExistMsgCommentRes_Sync(string resCode, string sendUserId);

        long AddContentCommentRes_Sync(EMsgContent_CommentRes content);
    }
}
