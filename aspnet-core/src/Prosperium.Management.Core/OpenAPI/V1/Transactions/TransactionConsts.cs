namespace Prosperium.Management.OpenAPI.V1.Transactions
{
    public class TransactionConsts
    {
        public enum TransactionType
        {
            Gastos = 1,
            Ganhos = 2,
            Transferência = 3
        }

        public enum PaymentType
        {
            Crédito = 1,
            Débito = 2
        }

        public enum PaymentTerms
        {
            Imediatamente = 1,
            Parcelado = 2,
            Recorrente = 3,
        }
    }
}
