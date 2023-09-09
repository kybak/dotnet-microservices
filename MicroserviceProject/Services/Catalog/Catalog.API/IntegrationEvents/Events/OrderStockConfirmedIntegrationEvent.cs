using MicroserviceProject.BuildingBlocks.EventBus.Events;

namespace MicroserviceProject.Services.Catalog.Catalog.API.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
}
