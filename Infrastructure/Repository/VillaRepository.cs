using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext db;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }
        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Villa villa)
        {
            db.Update(villa);
        }
    }
}
