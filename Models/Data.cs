using myPet.Data;

namespace myPet.Models
{
    public class Data
    {
        public Persons Person { get; set; }
        public DateOnly[] Period { get; set; }
        public Data(Persons person, DateOnly[] period)
        {
            Person = person;
            Period = period;
        }
    }
}