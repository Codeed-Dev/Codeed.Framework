using Codeed.Framework.AspNet;
using Codeed.Framework.AspNet.Serilog;
using Microsoft.EntityFrameworkCore;
using Sample.Identity;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSerilogApi("Sample");
builder.Host.UseSerilog(Log.Logger);

builder.Services.RegisterCoddedFrameworkDependencies("Sample", "Sample", (opt) =>
{
    opt.ConfigureFirebaseAuthentication("sample-81d61");
    opt.ConfigureSwagger(swagger =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        swagger.XmlFile = Path.Combine(AppContext.BaseDirectory, xmlFile);
    });
    opt.ConfigureDatabase((options) =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
});

var app = builder.Build();

app.UseExceptionHandler("/error");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader());

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

app.Services.GetRequiredService<IdentityDbContext>().Database.Migrate();