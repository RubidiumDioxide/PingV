var builder = WebApplication.CreateBuilder(args);

var routes = new[] {
    new RouteConfig()
    {
        RouteId = "all", 
        ClusterId = "schedule",
        Match = new RouteMatch
        {
            Path = "{**catch-all}"
        }
    }
}; 

var clusters = new[] {
    new ClusterConfig()
    {
        ClusterId = "schedule",
        HealthCheck = new HealthCheckConfig
        {
            Active = new ActiveHealthCheckConfig
            {
                Enabled = true,
                Interval = TimeSpan.FromMinutes(1),
                Timeout = TimeSpan.FromSeconds(10),
                Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures,
                Path = "/health",
                Query = "?foo=bar",
            }
        },
        Metadata = new Dictionary<string, string> { { ConsecutiveFailuresHealthPolicyOptions.ThresholdMetadataName, "5" } }, 
        Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
        {
            { "destination1", new DestinationConfig() { Address = "http://schedule:8080" } } 
        }
    }
};

builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);  

var app = builder.Build();

app.MapReverseProxy(); 

app.Run();
