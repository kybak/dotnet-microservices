using MicroserviceProject.BuildingBlocks.EventBus.Events;

namespace MicroserviceProject.Services.Catalog.Catalog.API.IntegrationEvents
{
    public interface ICatalogIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
