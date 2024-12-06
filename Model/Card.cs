namespace FinancniInformacniSystemBanky.Model
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
        public CardType CardType { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
    }
}
