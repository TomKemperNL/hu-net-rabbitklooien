namespace Teachers.Inno.HU.Domain;

public record TeacherCreated(string Email)
{
    public static TeacherCreated Of(Teacher newTeacher)
    {
        return new TeacherCreated(newTeacher.Email);
    }
}