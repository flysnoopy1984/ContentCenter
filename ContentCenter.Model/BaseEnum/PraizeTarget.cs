using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.BaseEnum
{
    public enum PraizeTarget
    {
        Resource = 1,

        Comment =2,

        CommentReply = 3,
    }

    public enum CommentTarget
    {
        Resource = 1,
    }

    public enum ReplyType
    {
        Normal =0,
        Author = 1,
    }

    public enum PraizeType
    {
        good = 1,
        noPraize = 0,
        bad = -1,
    }

  
    public enum OperationDirection
    {
        plus = 1, //从无到点赞,回复数+
        minus = 2,//从点赞到无，回复数-,
        update =3, //这种只有资源有用（从好评到差评）
        
    }
}
