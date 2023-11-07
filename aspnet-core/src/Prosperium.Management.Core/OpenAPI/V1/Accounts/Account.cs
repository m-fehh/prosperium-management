using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    [Table("P_Accounts")]
    public class Account : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string AccountNickname { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BalanceAvailable { get; set; } // Saldo disponível na conta
        public AccountType AccountType { get; set; }
        public Account FinancialInstitution { get; set; }
        public bool MainAccount { get; set; }
        public string Image { get; set; }
    }

    public enum AccountType
    {
        ContaCorrente = 1,
        Poupança = 2,
        Investimento = 3,
        Outros = 4,
        Indefinido = 5,
        Benefícios = 6
    }
}
