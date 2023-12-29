using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    public class TransactionConsts
    {
        public enum TransactionType
        {
            [Display(Name = "Despesas")]
            Gastos = 1,

            [Display(Name = "Receitas")]
            Ganhos = 2,

            [Display(Name = "Transferência")]
            Transferência = 3,

            [Display(Name = "Saldo")]
            Saldo = 4
        }

        public enum PaymentType
        {
            [Display(Name = "Pagamento com Cartão de Crédito")]
            Crédito = 1,

            [Display(Name = "Pagamento com Cartão de Débito")]
            Débito = 2,

            [Display(Name = "Pagamento com Saldo")]
            Saldo = 3
        }

        public enum PaymentTerms
        {
            [Display(Name = "Pagamento à Vista")]
            Imediatamente = 1,

            [Display(Name = "Pagamento Parcelado")]
            Parcelado = 2,

            [Display(Name = "Pagamento Recorrente")]
            Recorrente = 3,

            [Display(Name = "Saldo")]
            Saldo = 4
        }
    }
}
