using Teachers.Inno.HU.Controllers;

namespace Teachers.Inno.HU;

public class DbContextUnitOfWork : IUnitOfWork
{
    private TeacherDBContext dbContext;
    
    public DbContextUnitOfWork(TeacherDBContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public void Flush()
    {
        this.dbContext.SaveChanges();
    }
}