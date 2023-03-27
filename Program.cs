using CapstoneProject.Models;
using CapstoneProject.Models.Account_Models;
using Microsoft.EntityFrameworkCore;
//using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultConnectionStringValue"));
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMvc();
builder.Services.AddDbContext<Capstone_DBContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("CapstoneDatabaseConnectionString")));
//builder.Services.AddDbContext<Capstone_DBContext>(opt => opt.UseSqlServer(builder.Configuration["ConnectionStrings:MBSConnStr"]));
builder.Services.AddScoped<IAppointmentRepository, CapstoneRepository>();
builder.Services.AddScoped<IAccountRepository, CapstoneRepository>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
