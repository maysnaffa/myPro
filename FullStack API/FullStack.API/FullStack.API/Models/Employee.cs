namespace FullStack.API.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
       public  string name { get; set; }
       public string  Email { get; set; }

        public long  phone { get; set; }
       public long  salary { get; set; }
       public string  department { get; set; }

    }
}
