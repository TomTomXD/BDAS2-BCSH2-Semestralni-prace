namespace FinancniInformacniSystemBanky.Model
{
    public class StandingOrder
    {
        public int StandingOrderId { get; set; }
        public decimal Amount { get; set; }
        public int SendersAccountId { get; set; }
    }
}
