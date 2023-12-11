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
            AddBanks("NU PAGAMENTOS S.A. - INSTITUIÇÃO DE PAGAMENTO", "NU PAGAMENTOS S.A.", "nubank.png", "18236120", "260");
            AddBanks("BANCO ITAUBANK S.A.", "ITAÚ UNIBANCO S.A.", "itau.png", "60394079", "479");
            AddBanks("BANCO BRADESCO S.A.", "BCO BRADESCO S.A.", "bradesco.png", "60746948", "237");
            AddBanks("CAIXA ECONOMICA FEDERAL", "CAIXA ECONOMICA FEDERAL", "caixa.png", "360305", "104");
            AddBanks("BANCO SANTANDER S.A.", "BCO SANTANDER (BRASIL) S.A.", "santander.png", "90400888", "033");
            AddBanks("BANCO INTER S.A.", "BANCO INTER S.A.", "inter.png", "416968", "077");
            AddBanks("MERCADO PAGO IP LTDA.", "MERCADO PAGO IP LTDA.", "mercado-pago.png", "10573521", "323");
            AddBanks("BANCO C6 S.A.", "BCO C6 S.A.", "csix.png", "61348538", "626");
            AddBanks("NEON PAGAMENTOS S.A.", "NEON PAGAMENTOS S.A.", "neon.png", "20855875", "536");
            AddBanks("BMG S.A.", "BCO BMG S.A.", "bmg.png", "61186680", "318");
            AddBanks("BANCO DO BRASIL S.A.", "BCO DO BRASIL S.A.", "bb.png", "0", "001");
            AddBanks("BANRISUL S.A.", "BCO DO ESTADO DO RS S.A.", "banrisul.png", "92702067", "041");
            AddBanks("BANCO PAN S.A.", "BCO PAN S.A.", "pan.png", "59285411", "623");
            AddBanks("PAGSEGURO INTERNET IP S.A.", "PAGSEGURO INTERNET IP S.A.", "pagbank.png", "08561701", "290");
            AddBanks("PAYPAL", "PAYPAL", "paypal.png", "10878448", null);
            AddBanks("PICPAY BANK - BANCO MÚLTIPLO S.A.", "PICPAY BANK - BANCO MÚLTIPLO S.A", "picpay.png", "9516419", "079");
            AddBanks("BANCO SAFRA S.A.", "BCO SAFRA S.A.", "safra.png", "58160789", "422");
            AddBanks("BANCO COOPERATIVO SICREDI S.A.", "BCO COOPERATIVO SICREDI S.A.", "sicredi.png", "1181521", "748");


            SaveChanges();
        }

        private void AddBanks(string name, string tradeName, string image, string ispb, string compe)
        {
            var bank = _context.Banks.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.ImagePath == image && x.Compe == compe);
            if (bank == null)
            {
                bank = new Bank { 
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
