﻿using EBC.Core.Entities.Identity;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;

public class OrganizationAdressRepository : GenericRepository<OrganizationAdress>, IOrganizationAdressRepository
{
    public OrganizationAdressRepository(DbContext context) : base(context)
    {
    }
}
