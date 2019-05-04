using System;
using System.Threading.Tasks;

namespace Gong.Common.Core.CQRS.Dispatcher
{
    public class Dispatcher
    {
        private readonly IServiceProvider _provider;
        public Dispatcher(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public async Task<Result> Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            try
            {
                Type handlerType = type.MakeGenericType(typeArgs);
                dynamic handler = _provider.GetService(handlerType);
                
                Result Result = handler.Handle((dynamic)command);
                return Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<T> Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            try
            {
                Type handlerType = type.MakeGenericType(typeArgs);
                dynamic handler = _provider.GetService(handlerType);

                T Result = await handler.Handle((dynamic)query);
                return Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<T> Dispatch<T>(IQuery<T> query, Type result)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), result };
            try
            {
                Type handlerType = type.MakeGenericType(typeArgs);
                dynamic handler = _provider.GetService(handlerType);

                T Result = await handler.Handle((dynamic)query);
                return Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
