namespace SharedService.Conf;

using SharedService.Helpers;

public class ConsulConfiguration
{
    [Environment("CONSUL_CONF_BASE_URL")]
    public string ConsulBaseUrl { get; set; } = null!;

    [Environment("CONSUL_CONF_ID")]
    public string ServiceID { get; set; } = null!;

    [Environment("CONSUL_CONF_NAME")]
    public string Name { get; set; } = null!;

    [Environment("CONSUL_CONF_ADDRESS")]
    public string Address { get; set; } = null!;

    [Environment("CONSUL_CONF_PORT")]
    public int Port { get; set; }

    [Environment("CONSUL_CONF_HC_URL")]
    public string HealthCheckUrl { get; set; } = null!;

    [Environment("CONSUL_CONF_HC_INTERVAL")]
    public int HealthCheckIntervalSeconds { get; set; }

    [Environment("CONSUL_CONF_HC_TIMEOUT")]
    public int HealthCheckTimeoutSeconds { get; set; }
}