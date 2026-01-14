using GymManagementBusinessLayer.Services.Classes;
using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.AnalyticsVM;
using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Data.DataSeeding;
using GymManagementDataAccessLayer.Repositories.Classes;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GymDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IMemberSessionService, MemberSessionService>();



builder.Services.AddMapster();

GymManagementBusinessLayer.Configurations.MappingConfiguration.RegisterMappings();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GymDBContext>();
    var webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
    var pendingMigrations = context.Database.GetPendingMigrations();
    if (pendingMigrations != null && pendingMigrations.Any())
    {
        context.Database.Migrate();
    }
    Seeder.SeedData(context, webHostEnvironment.WebRootPath);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
