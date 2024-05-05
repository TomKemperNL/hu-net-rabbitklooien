using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teachers.Inno.HU.Domain;

namespace Teachers.Inno.HU.Controllers;

[ApiController]
[Route("/teachers")]
public class TeacherController
{
    private DbSet<Teacher> _teachers;
    private IUnitOfWork _uow;
    
    public TeacherController(DbSet<Teacher> teachers, IUnitOfWork uow)
    {
        _teachers = teachers;
        _uow = uow;
    }

    [HttpGet]
    public ActionResult<List<Teacher>> AllTeachers()
    {
        // return new List<Teacher>(){ new Teacher("Bob", "bob@hu.nl")};
        return _teachers.AsQueryable().ToList();
    }
    
    [HttpPost]
    public ActionResult<Teacher> AddTeacher([FromBody] NewTeacher newTeacher)
    {
        Teacher freshOne = new Teacher(newTeacher.name, newTeacher.email);
        _teachers.Add(freshOne);
        _uow.Flush();
        return freshOne;
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTeacher(long id)
    {
        Teacher t = _teachers.Find(id);
        if (t == null)
        {
            return new NotFoundResult();
        }
        else
        {
            _teachers.Remove(t);
            _uow.Flush();
            return new NoContentResult();
        }
    }
    
    public record NewTeacher(string name, string email);

}