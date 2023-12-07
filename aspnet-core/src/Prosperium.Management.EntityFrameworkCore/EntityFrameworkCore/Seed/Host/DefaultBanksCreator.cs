using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Banks;
using System.Linq;

namespace Prosperium.Management.EntityFrameworkCore.Seed.Host
{
    public class DefaultBanksCreator
    {
        private readonly ManagementDbContext _context;

        public DefaultBanksCreator(ManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (ManagementConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            SeedBanks();
        }

        private void SeedBanks()
        {
            AddBanks("Nubank", "nubank.png");
            AddBanks("Itaú", "itau.png");
            AddBanks("Bradesco", "bradesco.png");
            AddBanks("Caixa", "caixa.png");
            AddBanks("Santander", "santander.png");
            AddBanks("Inter", "inter.png");
            AddBanks("Mercado Pago", "mercado-pago.png");
            AddBanks("C6", "csix.png");
            AddBanks("Neon", "neon.png");
            AddBanks("BMG", "bmg.png");
            AddBanks("Banco do Brasil", "bb.png");
            AddBanks("Banrisul", "banrisul.png");
            AddBanks("PAN", "pan.png");
            AddBanks("PagBank", "pagbank.png");
            AddBanks("PayPal", "paypal.png");
            AddBanks("PicPay", "picpay.png");
            AddBanks("Safra", "safra.png");
            AddBanks("Sicredi", "sicredi.png");

            SaveChanges();
        }

        private void AddBanks(string name, string image)
        {
            var bank = _context.Banks.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.ImagePath == image);
            if (bank == null)
            {
                bank = new Bank { Name = name, ImagePath = image, Origin = OpenAPI.V1.Accounts.AccountConsts.AccountOrigin.Manual };
                _context.Banks.Add(bank);
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
