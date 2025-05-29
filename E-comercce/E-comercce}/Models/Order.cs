namespace E_comercce_.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Recibido";
        public List<OrderItem> Items { get; set; } = new();
    }
}
