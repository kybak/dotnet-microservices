﻿using MicroserviceProject.BuildingBlocks.EventBus.Abstractions;
using MicroserviceProject.BuildingBlocks.EventBus.Events;
using MicroserviceProject.Services.Catalog.Catalog.API.Infrastructure;
using MicroserviceProject.Services.Catalog.Catalog.API.IntegrationEvents.Events;

namespace MicroserviceProject.Services.Catalog.Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler :
    IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    private readonly CatalogContext _catalogContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

    public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
        CatalogContext catalogContext,
        ICatalogIntegrationEventService catalogIntegrationEventService,
        ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
    {
        _catalogContext = catalogContext;
        _catalogIntegrationEventService = catalogIntegrationEventService;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
    {
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

            foreach (var orderStockItem in @event.OrderStockItems)
            {
                var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);
                var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }

            var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(@event.OrderId, confirmedOrderStockItems)
                : new OrderStockConfirmedIntegrationEvent(@event.OrderId);

            await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent);
            await _catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);

        }
    }
}
