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
            SeedBanks();
        }

        private void SeedBanks()
        {
            AddBanks("NU PAGAMENTOS S.A.", "NUBANK", "nubank.png", "18236120", "260");
            AddBanks("BANCO ITAUBANK S.A.", "ITAÚ", "itau.png", "60394079", "479");
            AddBanks("BANCO BRADESCO S.A.", "BRADESCO", "bradesco.png", "60746948", "237");
            AddBanks("CAIXA ECONOMICA FEDERAL", "CAIXA", "caixa.png", "360305", "104");
            AddBanks("BANCO SANTANDER S.A.", "SANTANDER", "santander.png", "90400888", "033");
            AddBanks("BANCO INTER S.A.", "INTER", "inter.png", "416968", "077");
            AddBanks("MERCADO PAGO IP LTDA.", "MERCADO PAGO", "mercado-pago.png", "10573521", "323");
            AddBanks("BANCO C6 S.A.", "C6", "csix.png", "61348538", "626");
            AddBanks("NEON PAGAMENTOS S.A.", "NEON", "neon.png", "20855875", "536");
            AddBanks("BMG S.A.", "BMG", "bmg.png", "61186680", "318");
            AddBanks("BANCO DO BRASIL S.A.", "BANCO DO BRASIL", "bb.png", "0", "001");
            AddBanks("BANRISUL S.A.", "BANRISUL", "banrisul.png", "92702067", "041");
            AddBanks("BANCO PAN S.A.", "BANCO PAN", "pan.png", "59285411", "623");
            AddBanks("PAGSEGURO INTERNET IP S.A.", "PAGBANK", "pagbank.png", "08561701", "290");
            AddBanks("PAYPAL", "PAYPAL", "paypal.png", "10878448", null);
            AddBanks("PICPAY BANK - BANCO MÚLTIPLO S.A.", "PICPAY", "picpay.png", "9516419", "079");
            AddBanks("BANCO SAFRA S.A.", "SAFRA", "safra.png", "58160789", "422");
            AddBanks("BANCO COOPERATIVO SICREDI S.A.", "SICREDI", "sicredi.png", "1181521", "748");

            SaveChanges();
        }

        private void AddBanks(string name, string tradeName, string image, string ispb, string compe)
        {
            var bank = _context.Banks.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.ImagePath == image && x.Compe == compe);
            if (bank == null)
            {
                bank = new Bank
                {
                    Name = name,
                    TradeName = tradeName,
                    Ispb = ispb,
                    Compe = compe,
                    ImagePath = image,
                    Origin = OpenAPI.V1.Accounts.AccountConsts.AccountOrigin.Manual
                };
                _context.Banks.Add(bank);
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
