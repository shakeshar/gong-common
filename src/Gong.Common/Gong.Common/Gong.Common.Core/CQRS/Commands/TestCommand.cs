using Gong.Common.Core.CQRS.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gong.Common.Core.CQRS.Commands
{
    public class TestCommand : ICommand
    {
        public int Id { get; }        
        public TestCommand(int id)
        {
            this.Id = id;                
        }
    }
    public class TestHandler : ICommandHandler<TestCommand>
    {
        public TestHandler()
        {
            
        }

        public Result Handle(TestCommand command)
        {
            return new SuccessResult();
        }

       
    }
}
