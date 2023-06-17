using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CampinWebApi.Domain;

public class CampinContextFactory : IDesignTimeDbContextFactory<CampinDbContext>
{
    public CampinDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CampinDbContext>();
        optionsBuilder.UseSqlServer("Server=tcp:campin.database.windows.net,1433;Initial Catalog=campindatabase;Persist Security Info=False;User ID=aycan;Password=18811938Aa.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        return new CampinDbContext(optionsBuilder.Options);
    }
}