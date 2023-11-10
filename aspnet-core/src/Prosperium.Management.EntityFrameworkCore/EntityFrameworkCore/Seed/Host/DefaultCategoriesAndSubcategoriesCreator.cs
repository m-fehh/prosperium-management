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
            AddCategoryAndSubcategory("Alimentação", "Açougue", TransactionType.Gastos);
            AddCategoryAndSubcategory("Alimentação", "Feira", TransactionType.Gastos);
            AddCategoryAndSubcategory("Alimentação", "Padaria", TransactionType.Gastos);
            AddCategoryAndSubcategory("Alimentação", "Restaurante", TransactionType.Gastos);
            AddCategoryAndSubcategory("Alimentação", "Supermercado", TransactionType.Gastos);
            AddCategoryAndSubcategory("Alimentação", "Cafeteria/Lanchonete", TransactionType.Gastos);

            AddCategoryAndSubcategory("Assinaturas", "Aplicativo", TransactionType.Gastos);
            AddCategoryAndSubcategory("Assinaturas", "Game", TransactionType.Gastos);

            AddCategoryAndSubcategory("Casa", "Água", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Aluguel/Prestação", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Condominio", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Energia", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Funcionários", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Gás", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Internet", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "IPTU", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Lavanderia", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Manutenção", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Seguro", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Telefone/Celular", TransactionType.Gastos);
            AddCategoryAndSubcategory("Casa", "Objetos de casa", TransactionType.Gastos);

            AddCategoryAndSubcategory("Dívidas", "Cartão de crédito em atraso", TransactionType.Gastos);
            AddCategoryAndSubcategory("Dívidas", "Cheque especial", TransactionType.Gastos);

            AddCategoryAndSubcategory("Educação", "Cursos", TransactionType.Gastos);
            AddCategoryAndSubcategory("Educação", "Escola", TransactionType.Gastos);
            AddCategoryAndSubcategory("Educação", "Faculdade", TransactionType.Gastos);

            AddCategoryAndSubcategory("Lazer", "Bares", TransactionType.Gastos);
            AddCategoryAndSubcategory("Lazer", "Boates", TransactionType.Gastos);
            AddCategoryAndSubcategory("Lazer", "Festas", TransactionType.Gastos);

            AddCategoryAndSubcategory("Objetivos de vida", "Aposentadoria", TransactionType.Gastos);
            AddCategoryAndSubcategory("Objetivos de vida", "Metas pessoais", TransactionType.Gastos);

            AddCategoryAndSubcategory("Pessoal", "Cabelereiro", TransactionType.Gastos);
            AddCategoryAndSubcategory("Pessoal", "Higiene pessoal", TransactionType.Gastos);
            AddCategoryAndSubcategory("Pessoal", "Maquiagem", TransactionType.Gastos);

            AddCategoryAndSubcategory("Pet", "Alimentação", TransactionType.Gastos);
            AddCategoryAndSubcategory("Pet", "Banho e tosa", TransactionType.Gastos);

            AddCategoryAndSubcategory("Saúde", "Academia", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Dentista", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Esportes", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Exames", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Farmácia", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Médico", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Plano de saúde", TransactionType.Gastos);
            AddCategoryAndSubcategory("Saúde", "Terapias", TransactionType.Gastos);

            AddCategoryAndSubcategory("Taxas", "Anuídade cartão", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "Carnê", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "CONFINS", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "Gorjetas", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "Imposto de renda", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "INSS", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "ISS", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "PIS", TransactionType.Gastos);
            AddCategoryAndSubcategory("Taxas", "Tárifa bancária", TransactionType.Gastos);

            AddCategoryAndSubcategory("Transporte", "Combustível", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Estacionamento", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Limpeza", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Manutenção", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Multas", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Pedágio", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Prestação", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Seguro", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Táxis e aplicativos", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Transporte público", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "IPVA", TransactionType.Gastos);
            AddCategoryAndSubcategory("Transporte", "Licenciamento", TransactionType.Gastos);

            AddCategoryAndSubcategory("Vestuário", "Calçados", TransactionType.Gastos);
            AddCategoryAndSubcategory("Vestuário", "Roupas", TransactionType.Gastos);

            AddCategoryAndSubcategory("Viagem", "Hospedagem", TransactionType.Gastos);
            AddCategoryAndSubcategory("Viagem", "Passagem", TransactionType.Gastos);

            #endregion

            #region Ganhos:

            AddCategoryAndSubcategory("Cashback", "Cashback", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Investimento", "Rendimentos", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Investimento", "Proventos", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Premio", "Distribuição de lucros", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Premio", "Bônus", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Presente", "Presente", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Receita variável", "Comissões", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Receita variável", "Reembolso", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Receita variável", "Seguro", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Receita variável", "Férias", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Receita variável", "13º Salário", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Receita variável", "Restituição de impostos", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Salário", "Salário", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Salário", "Pró-Labore", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Transferência", "Resgate", TransactionType.Ganhos);
            AddCategoryAndSubcategory("Transferência", "Lançamento entre contas", TransactionType.Ganhos);

            #endregion

            SaveChanges();
        }

        private void AddCategoryAndSubcategory(string categoryName, string subcategoryName, TransactionType transactionType)
        {
            var category = _context.Categories.IgnoreQueryFilters().FirstOrDefault(c => c.Name == categoryName && c.TransactionType == transactionType);

            if (category == null)
            {
                category = new Category { Name = categoryName, TransactionType = transactionType };
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

