using Codeed.Framework.AspNet;
using Codeed.Framework.AspNet.Serilog;
using Codeed.Framework.AspNet.Tenant;
using Codeed.Framework.Tenant;
using Microsoft.EntityFrameworkCore;
using Sample.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSerilogApi("Sample");
builder.Host.UseSerilog(Log.Logger);
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.RegisterCoddedFrameworkDependencies("Sample", "Sample", (opt) =>
{
    opt.ConfigureFirebaseAuthentication("codeedint");
    opt.ConfigureSwagger(c =>
    {
        c.Version = "v1";
        c.Title = "Sample";
        c.Description = "Sample Application";
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