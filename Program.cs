using ChatApplication.Extensions;
using ChatApplication.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDatabase();
builder.Services.AddSignalR();
builder.Services.AddRazorComponents();
builder.Services.AddControllersWithViews();
builder.Services.AddChatServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHub<ChatHub>("hubs/chat");

app.Run();