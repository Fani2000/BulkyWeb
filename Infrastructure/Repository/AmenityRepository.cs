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
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext db;

        public AmenityRepository (ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public bool GetAny(Expression<Func<Amenity, bool>> expression)
        {
            return db.Amenities.Any(expression);
        }

        public void Save()
        {
            db.SaveChanges(); 
        }

        public void Update(Amenity amenity)
        {
            db.Amenities.Update(amenity);
        }
    }
}
