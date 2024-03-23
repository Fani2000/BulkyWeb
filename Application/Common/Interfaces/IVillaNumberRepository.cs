using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        bool GetAny(Expression<Func<VillaNumber, bool>> expression);
        void Update(VillaNumber villa);
        void Save();
    }
}
