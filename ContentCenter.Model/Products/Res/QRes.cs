using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QRes: ReqMasterData
    {
        public string refCode { get; set; }

        private QueryPager _pager;
        public QueryPager pager {
            get { 
                if (_pager == null)
                    _pager = new QueryPager(1,3);
                return _pager;
            }
            set { _pager = value; }
        }

        /// <summary>
        /// 如果是固定资源，query时不会筛选出，单独查询这条记录
        /// </summary>
        public string fixedResCode { get; set; }

        public long fixedCommentId { get; set; } = -1;

        /// <summary>
        /// 带出评论数量
        /// </summary>
        public int withCommentNum { get; set; } = 2;

        public string reqUserId { get; set; }
       
    }
}
