using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.AOP
{
    public class CCLogAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"执行方法:{invocation.Method.Name}");
            invocation.Proceed();
         
            Console.WriteLine($"执行完成");
        }
    }
}
