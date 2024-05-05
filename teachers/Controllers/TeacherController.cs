using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teachers.Inno.HU.Domain;

namespace Teachers.Inno.HU.Controllers;

[ApiController]
[Route("/teachers")]
public class TeacherController
{
    private DbSet<Teacher> Teachers;

    public TeacherController(DbSet<Teacher> teachers)
    {
        this.Teachers = teachers;
    }

    [HttpGet]
    public ActionResult<List<Teacher>> AllTeachers()
    {
        // return new List<Teacher>(){ new Teacher("Bob", "bob@hu.nl")};
        return this.Teachers.AsQueryable().ToList();
    }
    
    
}