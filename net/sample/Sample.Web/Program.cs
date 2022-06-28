using Codeed.Framework.AspNet;
using Codeed.Framework.AspNet.RegisterServicesConfigurations;
using Codeed.Framework.AspNet.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sample.Web;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSerilogApi("Sample");
builder.Host.UseSerilog(Log.Logger);
builder.Services.RegisterCodeedFrameworkDependencies("Sample", "Sample", (opt) =>
{
    opt.ConfigureTenant<TenantService>();
    opt.ConfigureFirebaseAuthentication("codeedint", opt =>
    {
        opt.ConfigureCustomsAuthentications(auth =>
        {
            auth.AddJwtBearer("Custom", options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("72969c9f-ac13-4bb9-ac69-0307673b2c84")),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        });
    });

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