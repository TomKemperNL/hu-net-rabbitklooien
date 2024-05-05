using Microsoft.AspNetCore.Mvc;
using Teachers.Inno.HU.Domain;

namespace Teachers.Inno.HU.Controllers;

[ApiController]
[Route("/teachers")]
public class TeacherController
{
    private TeacherContext teachers = new TeacherContext();

    [HttpGet]
    public ActionResult<List<Teacher>> AllTeachers()
    {
        // return new List<Teacher>(){ new Teacher("Bob", "bob@hu.nl")};
        return teachers.Teachers.AsQueryable().ToList();
    }
    
    
}