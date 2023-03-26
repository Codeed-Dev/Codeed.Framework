using Codeed.Framework.AspNet;
using Codeed.Framework.AspNet.EventBus;
using Codeed.Framework.AspNet.RegisterServicesConfigurations;
using Codeed.Framework.AspNet.Serilog;
using Codeed.Framework.Tenant;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddSerilogApi("Godia");
builder.Host.UseSerilog(Log.Logger);

builder.Services.RegisterCodeedFrameworkDependencies(builder.Configuration, "Godia", "Godia", (opt) =>
{
    opt.ConfigureTenant<FixedTenantService>();

    opt.ConfigureFirebaseAuthentication("codeedmeta");

    opt.ConfigureSwagger(c =>
    {
        c.Version = "v1";
        c.Title = "Godia";
        c.Description = "Godia API's";
    });
    opt.ConfigureDatabase((options) =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
    });
    opt.ConfigureEventBus();
    opt.ConfigureMongoDb(builder.Configuration.GetSection("MongoDb"));
    opt.ConfigureBackgroundTasks();
});


var app = builder.Build();
app.SubscribeEventHandlers();
app.UseExceptionHandler("/error");
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("__allOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();