using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebBirthdayMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// + Строка соединения
//string connection = builder.Configuration.GetConnectionString("DefaultConnection");
string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb2;Trusted_Connection=True;";

// + добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//builder.Services.AddScoped<Service>(sp => new Service(sp.GetRequiredService<IOptions<ApiConfiguration>>().Value));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Api}/{action=Test}");

// вынес в контроллер
//app.MapControllerRoute(name: "id_get", pattern: "{controller=api}/{action=users}/{id?:int}");

app.Run();

//public class AppContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
//{
//    public ApplicationContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
//        //optionsBuilder.UseSqlite("Data Source=blog.db");

//        return new ApplicationContext(optionsBuilder.Options);
//    }
//}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    //public string img = "";
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Аня", Birthday = Convert.ToDateTime("2005-08-04"), Type = true },
                new User { Id = 2, Name = "Светлана Ивановна", Birthday = Convert.ToDateTime("1974-12-31"), Type = false },
                new User { Id = 3, Name = "дедушка", Birthday = Convert.ToDateTime("1937-01-15"), Type = true }
        );
    }*/
}