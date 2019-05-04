using System;
using System.Collections.Generic;
using System.Text;

namespace Gong.Common.Core.CQRS.Commands.Base
{
     abstract class BaseCommand
    {
        public BaseCommand()
        {

        }
        public abstract Result Invoke();
    }
}
