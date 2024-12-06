namespace FinancniInformacniSystemBanky.Model
{
    public class Password
    {
        public int PasswordId { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
