using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosperium.Management.EntityFrameworkCore.Seed.Host
{
    public class DefaultFlagsCreator
    {
        private readonly ManagementDbContext _context;

        public DefaultFlagsCreator(ManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            SeedFlags();
        }

        private void SeedFlags()
        {
            AddFlag("Visa", "visa.png");
            AddFlag("MasterCard", "mastercard.png");
            AddFlag("HiperCard", "hipercard.png");
            AddFlag("American Express", "american-express.png");
            AddFlag("Elo", "elo.png");
            AddFlag("Outra", "other-card.png");
        }

        private void AddFlag(string name, string image)
        {
            var flag = _context.Flags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.IconPath == image);
            if (flag == null) 
            { 
                flag = new OpenAPI.V1.Flags.FlagCard { Name = name, IconPath = image };
                _context.Flags.Add(flag);
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
