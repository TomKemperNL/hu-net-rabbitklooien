namespace Teachers.Inno.HU.Domain;

public class Teacher
{
    public Teacher(string name, string email)
    {
        this.Name = name;
        this.Email = email;
    }
    
    public long Id { get; private set; }

    public String Name { get; private set; }
    public String Email { get; private set; }
}