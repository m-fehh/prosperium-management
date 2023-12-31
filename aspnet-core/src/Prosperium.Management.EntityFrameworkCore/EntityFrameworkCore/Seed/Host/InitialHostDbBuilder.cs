﻿namespace Prosperium.Management.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ManagementDbContext _context;

        public InitialHostDbBuilder(ManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            new DefaultCategoriesAndSubcategoriesCreator(_context).Create();
            new DefaultBanksCreator(_context).Create();
            new DefaultFlagsCreator(_context).Create();
            new DefaultPlansCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
