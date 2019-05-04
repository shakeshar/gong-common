using Gong.Common.Contract;
using Gong.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gong.Common.Infrastructure
{
    public class GenericEFRepository<TEntity> : BaseEFRepository<TEntity>, IDisposable, IRepository<TEntity>
  where TEntity : class
    {
        public GenericEFRepository(DbContext context) : base(context)
        {

        }
    }
}