using HU.Inno.Teachers.Messaging;
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
    private IPublisher<TeacherCreated> _publisher;

    public TeacherController(DbSet<Teacher> teachers, IUnitOfWork uow, IPublisher<TeacherCreated> publisher)
    {
        _teachers = teachers;
        _uow = uow;
        _publisher = publisher;
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
        _publisher.Publish(TeacherCreated.Of(freshOne));
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