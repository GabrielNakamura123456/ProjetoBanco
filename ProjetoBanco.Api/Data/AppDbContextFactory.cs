using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjetoBanco.Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseOracle(
            "User Id=RM560671;Password=250905;Data Source=oracle.fiap.com.br:1521/ORCL");

        return new AppDbContext(optionsBuilder.Options);
    }
}
