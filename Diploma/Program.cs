using Diploma.DataAccess;
using Diploma.Repository;
using Diploma.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<DiplomDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPersonalityRepository, PersonalityRepository>();
builder.Services.AddScoped<IModalTypeRepository, ModalTypeRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Test}/{action=Index}/{id?}");

app.Run();
