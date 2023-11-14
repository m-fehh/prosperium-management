using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
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
            AddBanks("Nubank", "wwwroot/img/banks/nubank");
            AddBanks("Itaú", "wwwroot/img/banks/itau");
            AddBanks("Bradesco", "wwwroot/img/banks/bradesco");
            AddBanks("Caixa", "wwwroot/img/banks/caixa");
            AddBanks("Santander", "wwwroot/img/banks/santander");
            AddBanks("Inter", "wwwroot/img/banks/inter");
            AddBanks("Mercado Pago", "wwwroot/img/banks/mercado-pago");
            AddBanks("C6", "wwwroot/img/banks/csix");
            AddBanks("Neon", "wwwroot/img/banks/neon");
            AddBanks("BMG", "wwwroot/img/banks/bmg");
            AddBanks("Banco do Brasil", "wwwroot/img/banks/bb");
            AddBanks("Banrisul", "wwwroot/img/banks/banrisul");
            AddBanks("PAN", "wwwroot/img/banks/pan");
            AddBanks("PagBank", "wwwroot/img/banks/pagbank");
            AddBanks("PayPal", "wwwroot/img/banks/paypal");
            AddBanks("PicPay", "wwwroot/img/banks/picpay");
            AddBanks("Safra", "wwwroot/img/banks/safra");
            AddBanks("Sicredi", "wwwroot/img/banks/sicredi");

            SaveChanges();
        }

        private void AddBanks(string name, string image)
        {
            var bank = _context.Banks.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.ImagePath == image);
            if (bank == null)
            {
                bank = new Banks.Bank { Name = name, ImagePath = image };
                _context.Banks.Add(bank);
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
