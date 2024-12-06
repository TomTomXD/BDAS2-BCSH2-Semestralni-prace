using FinancniInformacniSystemBanky.Model.Helpers;

namespace FinancniInformacniSystemBanky.Model
{
    public class CardType : ILookupEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}