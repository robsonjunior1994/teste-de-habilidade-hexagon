namespace UserCRUD.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string CivilState { get; set; }
        public string Cpf { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public User User { get; set; }
    }
}
