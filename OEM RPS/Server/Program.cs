using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using OEM_RPS.Shared;
using OEM_RPS.Shared.Model;
using OEMRPS.Server.DataBaseContext;
using OEMRPS.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRockPaperScissors, RockPaperScissorsService>();

//Genric Repo layer

//RPSGame
//builder.Services.AddScoped<IGenericRepository<RPSGame>, GenericRepository<RPSGame>>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//RoundResult
//builder.Services.AddScoped(typeof(IGenericRepository<RoundResult>), typeof(GenericRepository<RoundResult>));

//Mangae DBContext
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

