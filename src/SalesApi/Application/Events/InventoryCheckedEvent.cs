namespace SalesApi.Application.Events;

public record InventoryCheckedEvent(int OrderId, bool IsValid, List<string> Errors);