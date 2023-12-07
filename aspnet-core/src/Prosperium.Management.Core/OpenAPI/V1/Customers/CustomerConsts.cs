using System.ComponentModel.DataAnnotations;

namespace Prosperium.Management.OpenAPI.V1.Customers
{
    public class CustomerConsts
    {
        public enum PhoneNumberType
        {
            [Display(Name = "Pessoal")]
            Personal,

            [Display(Name = "Trabalho")]
            Work,

            [Display(Name = "Casa")]
            Home,

            [Display(Name = "Celular")]
            Mobile,

            [Display(Name = "Outro")]
            Other
        }

        public enum EmailType
        {
            [Display(Name = "Pessoal")]
            Personal,

            [Display(Name = "Trabalho")]
            Work,

            [Display(Name = "Marketing")]
            Marketing,

            [Display(Name = "Suporte")]
            Support,

            [Display(Name = "Outro")]
            Other
        }

        public enum AddressType
        {
            [Display(Name = "Pessoal")]
            Personal,

            [Display(Name = "Trabalho")]
            Work,

            [Display(Name = "Entrega")]
            Shipping,

            [Display(Name = "Faturamento")]
            Billing,

            [Display(Name = "Outro")]
            Other
        }
    }
}
