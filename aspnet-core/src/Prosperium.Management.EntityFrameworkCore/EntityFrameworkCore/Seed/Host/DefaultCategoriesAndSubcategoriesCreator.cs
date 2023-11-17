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
            AddCategoryAndSubcategory(true, "alimentacao.png", "Alimentação", "Açougue", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "alimentacao.png", "Alimentação", "Feira", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "alimentacao.png", "Alimentação", "Padaria", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "alimentacao.png", "Alimentação", "Restaurante", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "alimentacao.png", "Alimentação", "Supermercado", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "alimentacao.png", "Alimentação", "Cafeteria/Lanchonete", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "assinatura.png", "Assinaturas", "Aplicativo", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "assinatura.png", "Assinaturas", "Game", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Água", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Aluguel/Prestação", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Condomínio", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Energia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Funcionários", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Gás", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Internet", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "IPTU", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Lavanderia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Manutenção", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Seguro", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Telefone/Celular", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "casa.png", "Casa", "Objetos de casa", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "dividas.png", "Dívidas", "Cartão de crédito em atraso", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "dividas.png", "Dívidas", "Cheque especial", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "educacao.png", "Educação", "Cursos", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "educacao.png", "Educação", "Escola", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "educacao.png", "Educação", "Faculdade", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "lazer.png", "Lazer", "Bares", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "lazer.png", "Lazer", "Boates", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "lazer.png", "Lazer", "Festas", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "objetivos.png", "Objetivos de vida", "Aposentadoria", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "objetivos.png", "Objetivos de vida", "Metas pessoais", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "pessoal.png", "Pessoal", "Cabeleireiro", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "pessoal.png", "Pessoal", "Higiene pessoal", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "pessoal.png", "Pessoal", "Maquiagem", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "pet.png", "Pet", "Alimentação", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "pet.png", "Pet", "Banho e tosa", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Academia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Dentista", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Esportes", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Exames", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Farmácia", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Médico", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png","Saúde", "Plano de saúde", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "saude.png", "Saúde", "Terapias", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "Anuidade cartão", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "Carnê", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "CONFINS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "Gorjetas", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "Imposto de renda", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "INSS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "ISS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "PIS", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "taxas.png", "Taxas", "Tarifa bancária", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Combustível", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Estacionamento", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Limpeza", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Manutenção", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Multas", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Pedágio", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Prestação", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Seguro", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Táxis e aplicativos", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Transporte público", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "IPVA", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "transporte.png", "Transporte", "Licenciamento", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "vestuario.png", "Vestuário", "Calçados", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "vestuario.png", "Vestuário", "Roupas", TransactionType.Gastos);

            AddCategoryAndSubcategory(true, "viagem.png", "Viagem", "Hospedagem", TransactionType.Gastos);
            AddCategoryAndSubcategory(true, "viagem.png", "Viagem", "Passagem", TransactionType.Gastos);


            #endregion

            #region Ganhos:

            AddCategoryAndSubcategory(false, "saldo.png", "Saldo da Conta", "Saldo da Conta", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "cashback.png", "Cashback", "Cashback", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "investimento.png", "Investimento", "Rendimentos", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "investimento.png", "Investimento", "Proventos", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "premio.png", "Prêmio", "Distribuição de lucros", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "premio.png", "Prêmio", "Bônus", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "presente.png", "Presente", "Presente", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "receitas.png", "Receita variável", "Comissões", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "receitas.png", "Receita variável", "Reembolso", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "receitas.png", "Receita variável", "Seguro", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "receitas.png", "Receita variável", "Férias", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "receitas.png", "Receita variável", "13º Salário", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "receitas.png", "Receita variável", "Restituição de impostos", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "salario.png", "Salário", "Salário", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "salario.png", "Salário", "Pró-Labore", TransactionType.Ganhos);

            AddCategoryAndSubcategory(true, "transferencia.png", "Transferência", "Resgate", TransactionType.Ganhos);
            AddCategoryAndSubcategory(true, "transferencia.png", "Transferência", "Lançamento entre contas", TransactionType.Ganhos);


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

