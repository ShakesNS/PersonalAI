var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/engines/davinci/completions");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ApiKey"]}");
});
builder.Services.AddHttpClient("Dall-E", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/images/generations");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ApiKey"]}");
});

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
