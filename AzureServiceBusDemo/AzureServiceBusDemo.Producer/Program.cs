using AzureServiceBusDemo.Common;
using AzureServiceBusDemo.Producer.Services;
using Microsoft.Azure.ServiceBus.Management;

internal class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<AzureService>();
        builder.Services.AddSingleton<ManagementClient>(isp => new ManagementClient(Constants.ConnectionString));

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.Run();
    }
}