using Application.Common.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public IVillaRepository Villa { get; private set; }

        public IVillaNumberRepository VillaNumber { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db; 
            Villa = new VillaRepository(db);
            VillaNumber = new VillaNumberRepository(db);
        }
    }
}
