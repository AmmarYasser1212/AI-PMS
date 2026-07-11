using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PMS.Infrastructre.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=PMSDB;Trusted_Connection=True;Encrypt=False");

        return new AppDbContext(optionsBuilder.Options);
    }
}
