var builder = WebApplication.CreateBuilder(args);

var routes = new[] {
    new RouteConfig()
    {
        RouteId = "schedule_route", 
        ClusterId = "schedule_cluster",
        Match = new RouteMatch
        {
            Path = "{**catch-all}"
        }
    }, 
    new RouteConfig()
    {
        RouteId = "wishlist_route",
        ClusterId = "wishlist_cluster",
        Match = new RouteMatch
        {
            Path = "{**catch-all}"
        }
    }
}; 

var clusters = new[] {
    new ClusterConfig()
    {
        ClusterId = "schedule_cluster",
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
            { "schedule_destination1", new DestinationConfig() { Address = "http://schedule:8080" } } 
        }
    }, 
    new ClusterConfig()
    {
        ClusterId = "wishlist_cluster",
        HealthCheck = new HealthCheckConfig
        {
            Active = new ActiveHealthCheckConfig
            {
                Enabled = true,
                Interval = TimeSpan.FromMinutes(1),
                Timeout = TimeSpan.FromSeconds(10),
                Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures,
                Path = "/actuator/health",
                Query = "?foo=bar",
            }
        },
        Metadata = new Dictionary<string, string> { { ConsecutiveFailuresHealthPolicyOptions.ThresholdMetadataName, "5" } },
        Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
        {
            { "wishlist_destination1", new DestinationConfig() { Address = "http://wishlist:8080" } }
        }
    }
};

builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);  

var app = builder.Build();

app.MapReverseProxy(); 

app.Run();
