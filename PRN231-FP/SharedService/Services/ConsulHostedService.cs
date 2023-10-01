namespace SharedService.Services;

using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedService.Conf;
using SharedService.Helpers;

public class ConsulHostedService : IHostedService
{
    private IConsulClient  ConsulClient  { get; }
    private IConfiguration Configuration { get; }

    public ConsulHostedService(
        IConsulClient  consulClient,
        IConfiguration configuration
    )
    {
        this.ConsulClient  = consulClient;
        this.Configuration = configuration;
    }

    private ConsulConfiguration GetConfiguration()
    {
        var consulConf = this.Configuration.GetSection("Consul").Get<ConsulConfiguration>();
        EnvironmentAttribute.Populate(consulConf);
        return consulConf;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var consulConf = this.GetConfiguration();

        var check = new AgentServiceCheck
        {
            HTTP     = consulConf.HealthCheckUrl,
            Interval = TimeSpan.FromSeconds(consulConf.HealthCheckIntervalSeconds),
            Timeout  = TimeSpan.FromSeconds(consulConf.HealthCheckTimeoutSeconds),
        };

        var registration = new AgentServiceRegistration
        {
            ID      = consulConf.ServiceID,
            Name    = consulConf.Name,
            Address = consulConf.Address,
            Port    = consulConf.Port,
            Check   = check,
        };

        await this.ConsulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        await this.ConsulClient.Agent.ServiceRegister(registration, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var consulConf = this.GetConfiguration();

        await this.ConsulClient.Agent.ServiceDeregister(consulConf.ServiceID, cancellationToken);
    }
}

public static class ConsulServiceExtensions
{
    public static IServiceCollection AddConsul(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IConsulClient, ConsulClient>(provider =>
        {
            var configuration = provider.GetService<IConfiguration>();

            var consulConfiguration = configuration!.GetSection("Consul").Get<ConsulConfiguration>();
            EnvironmentAttribute.Populate(consulConfiguration);

            return new ConsulClient(consulConf =>
            {
                consulConf.Address = new Uri(consulConfiguration.ConsulBaseUrl);
            });
        });

        serviceCollection.AddSingleton<IHostedService, ConsulHostedService>();

        return serviceCollection;
    }
}