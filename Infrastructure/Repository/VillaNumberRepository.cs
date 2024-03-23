using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public bool GetAny(Expression<Func<VillaNumber, bool>> expression)
        {
            return db.villaNumbers.Any(expression);
        }

        public void Save()
        {
            db.SaveChanges(); 
        }

        public void Update(VillaNumber villa)
        {
            db.villaNumbers.Update(villa);
        }
    }
}
