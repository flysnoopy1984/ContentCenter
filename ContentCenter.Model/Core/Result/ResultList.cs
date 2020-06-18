using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class ResultList<T> : ResultNormal where T : class
    {
        public List<T> List { get; set; }
    }
  
}
