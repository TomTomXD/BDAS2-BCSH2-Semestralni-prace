namespace FinancniInformacniSystemBanky.Model
{
    public enum Role
    {
        OverenyKlient,
        NeoverenyKlient,
        Zamestnanec,
        Admin
    }

    public class PersonDetails
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DoB { get; set; }
        public string NationalIdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
