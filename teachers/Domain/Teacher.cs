using HU.Inno.Teachers.Messaging;

namespace Teachers.Inno.HU.Domain;

public class Teacher
{
    public Teacher(string name, string email)
    {
        this.Name = name;
        this.Email = email;
    }

    public long Id { get; private set; }

    public string Name { get; private set; }
    public string Email { get; private set; }
}