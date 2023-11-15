using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using System.Linq;
using static Prosperium.Management.OpenAPI.V1.Transactions.TransactionConsts;

namespace Prosperium.Management.EntityFrameworkCore.Seed.Host
{
    public class DefaultCategoriesAndSubcategoriesCreator
    {
        private readonly ManagementDbContext _context;

        public DefaultCategoriesAndSubcategoriesCreator(ManagementDbContext context)
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

            SeedCategoriesAndSubcategories();
        }

        private void SeedCategoriesAndSubcategories()
        {
            #region Gastos 
            AddCategoryAndSubcategory(true, "fas fa-utensils", "Alimentação", "Açougue", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-utensils", "Alimentação", "Feira", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-utensils", "Alimentação", "Padaria", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-utensils", "Alimentação", "Restaurante", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-utensils", "Alimentação", "Supermercado", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-utensils", "Alimentação", "Cafeteria/Lanchonete", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "far fa-grin-squint", "Assinaturas", "Aplicativo", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-grin-squint", "Assinaturas", "Game", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Água", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Aluguel/Prestação", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Condomínio", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Energia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Funcionários", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Gás", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Internet", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "IPTU", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Lavanderia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Manutenção", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Seguro", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Telefone/Celular", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-home", "Casa", "Objetos de casa", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-wallet", "Dívidas", "Cartão de crédito em atraso", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-wallet", "Dívidas", "Cheque especial", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-school", "Educação", "Cursos", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-school", "Educação", "Escola", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-school", "Educação", "Faculdade", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-wine-glass", "Lazer", "Bares", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-wine-glass", "Lazer", "Boates", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-wine-glass", "Lazer", "Festas", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-medal", "Objetivos de vida", "Aposentadoria", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-medal", "Objetivos de vida", "Metas pessoais", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "far fa-smile-beam", "Pessoal", "Cabeleireiro", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-smile-beam", "Pessoal", "Higiene pessoal", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-smile-beam", "Pessoal", "Maquiagem", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-dog", "Pet", "Alimentação", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-dog", "Pet", "Banho e tosa", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Academia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Dentista", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Esportes", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Exames", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Farmácia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Médico", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart","Saúde", "Plano de saúde", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "far fa-heart", "Saúde", "Terapias", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "Anuidade cartão", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "Carnê", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "CONFINS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "Gorjetas", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "Imposto de renda", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "INSS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "ISS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "PIS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-money-bill-alt", "Taxas", "Tarifa bancária", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Combustível", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Estacionamento", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Limpeza", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Manutenção", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Multas", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Pedágio", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Prestação", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Seguro", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Táxis e aplicativos", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Transporte público", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "IPVA", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-truck-pickup", "Transporte", "Licenciamento", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-tshirt", "Vestuário", "Calçados", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-tshirt", "Vestuário", "Roupas", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "fas fa-plane", "Viagem", "Hospedagem", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "fas fa-plane", "Viagem", "Passagem", TransactionType.Gastos);


            #endregion

            #region Ganhos:

            AddCategoryAndSubcategory(false, "fas fa-cash-register", "Saldo da Conta", "Saldo da Conta", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-exchange-alt", "Cashback", "Cashback", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-grin-beam", "Investimento", "Rendimentos", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-grin-beam", "Investimento", "Proventos", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-glass-cheers", "Prêmio", "Distribuição de lucros", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-glass-cheers", "Prêmio", "Bônus", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-gift", "Presente", "Presente", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-hand-holding-usd", "Receita variável", "Comissões", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-hand-holding-usd", "Receita variável", "Reembolso", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-hand-holding-usd", "Receita variável", "Seguro", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-hand-holding-usd", "Receita variável", "Férias", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-hand-holding-usd", "Receita variável", "13º Salário", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-hand-holding-usd", "Receita variável", "Restituição de impostos", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-handshake", "Salário", "Salário", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-handshake", "Salário", "Pró-Labore", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-helicopter", "Transferência", "Resgate", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "fas fa-helicopter", "Transferência", "Lançamento entre contas", TransactionType.Ganhos);


            #endregion

            SaveChanges();
        }

        private void AddCategoryAndSubcategory(bool isVisible, string iconPath, string categoryName, string subcategoryName, TransactionType transactionType)
        {
            var category = _context.Categories.IgnoreQueryFilters().FirstOrDefault(c => c.Name == categoryName && c.TransactionType == transactionType);

            if (category == null)
            {
                category = new Category { Name = categoryName, TransactionType = transactionType, IconPath = iconPath, IsVisible = isVisible };
                _context.Categories.Add(category);
                SaveChanges(); 
            }

            var subcategory = _context.Subcategories.FirstOrDefault(s => s.Name == subcategoryName && s.CategoryId == category.Id);

            if (subcategory == null)
            {
                subcategory = new Subcategory { Name = subcategoryName, CategoryId = category.Id };
                _context.Subcategories.Add(subcategory);
            }
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

