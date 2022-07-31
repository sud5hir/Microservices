using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetryAndCircuitBreakerService
{
    public interface Interface1
    {
          Task<string> DoSomething(int i);

    }
}
