using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.OpenAPI.V1.Accounts
{
    public class AccountConsts
    {
        public enum AccountType
        {
            [Display(Name = "Conta Corrente ")]
            Corrente = 1,

            [Display(Name = "Conta Poupança")]
            Poupança = 2,

            [Display(Name = "Investimento")]
            Investimento = 3,

            [Display(Name = "Outros")]
            Outros = 4,

            [Display(Name = "Indefinido")]
            Indefinido = 5,

            [Display(Name = "Benefícios")]
            Benefícios = 6,

            [Display(Name = "Cartão de crédito")]
            Crédito = 7
        }

        public enum AccountOrigin
        {
            Manual = 1,
            Pluggy = 2
        }
    }
}
