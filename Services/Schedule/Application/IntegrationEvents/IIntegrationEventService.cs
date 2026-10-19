namespace Schedule.Application.IntegrationEvents; 

public interface IIntegrationEventService
{
    Task SaveEventAndContextChangesAsync(IntegrationEvent e);
    Task PublishThroughEventBusAsync(IntegrationEvent e);
}
