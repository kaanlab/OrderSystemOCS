namespace OrderSystemOCS.Domain
{
    public enum Status
    {
        New = 0,
        AwaitingPayment,
        Paid,
        ToDelivery,
        Delivered,
        Completed
    }
}
