namespace FinancniInformacniSystemBanky.Model.Helpers
{
    public class NormalAccount
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ComboBoxOption => $"{FirstName} {LastName} ({AccountNumber.Trim()})";
        public string AccountNumber { get; set; }
    }
}