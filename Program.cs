using ToDo_List_with_additions.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IToDosService, ToDosService>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
