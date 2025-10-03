namespace ElectronicStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class OrderModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
    }
}