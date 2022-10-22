namespace ECommerce.Domain.Entities;

public enum OrderStatus
{
    Created,
    Paid,
    InProcess,
    ReadyForDispatch,
    Dispatched,
    Delivered,
    Returned,
    Rejected,
    Canceled,
    Closed
}