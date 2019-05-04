using System;
using System.Collections.Generic;
using System.Text;

namespace Gong.Common.Core.CQRS
{
    public interface ICommand
    {
    }
   
    public interface ICommandHandler<T>
       where T : ICommand
    {
        Result Handle(T command);
    }
}
