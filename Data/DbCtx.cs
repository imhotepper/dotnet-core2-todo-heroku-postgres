using Microsoft.EntityFrameworkCore;

public class DbCtx : DbContext
{
    // When used with ASP.net core, add these lines to Startup.cs
    //   var connectionString = Configuration.GetConnectionString("BlogContext");
    //   services.AddEntityFrameworkNpgsql().AddDbContext<BlogContext>(options => options.UseNpgsql(connectionString));
    // and add this to appSettings.json
    // "ConnectionStrings": { "BlogContext": "Server=localhost;Database=blog" }

    public DbCtx(DbContextOptions<DbCtx> options) : base(options) =>
        Database.Migrate();

    public DbSet<Todo> Todos { get; set; }
}