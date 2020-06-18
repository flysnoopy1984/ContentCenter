using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class ResultPager<T> :ResultNormal where T : class
    {
        private ModelPager<T> _pageData;
        public ModelPager<T> PageData
        {
            get
            {
                if (_pageData == null)
                    _pageData = new ModelPager<T>();
                return _pageData;
            }
        }
    }
}
