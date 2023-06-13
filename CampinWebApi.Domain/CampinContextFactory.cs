using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CampinWebApi.Domain;

public class CampinContextFactory : IDesignTimeDbContextFactory<CampinDbContext>
{
    public CampinDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CampinDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=tempdb;User Id=sa;Password=18811938Aa.;MultipleActiveResultSets=false");

        return new CampinDbContext(optionsBuilder.Options);
    }
}