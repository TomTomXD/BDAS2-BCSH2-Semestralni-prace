using InformacniSystemBanky.Model;

namespace FinancniInformacniSystemBanky.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DoB { get; set; }
        public string NationalIdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PersonType { get; set; }
        public Role Role{ get; set; }
        public Address Address { get; set; }
        public EmployeeDetails? EmployeeDetails { get; set; }
    }
}
