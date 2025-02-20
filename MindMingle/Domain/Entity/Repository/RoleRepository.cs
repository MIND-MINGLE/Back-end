using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
    public class RoleRespository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRespository(MMDbContext mMDbContext) : base(mMDbContext)
        {
        }
    }
}

