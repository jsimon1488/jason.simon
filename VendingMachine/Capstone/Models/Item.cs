namespace Capstone.Models
{
    public class Item
    {
        public string SlotIdentifier { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string ProductType { get; }
        public int CountRemaining { get; set; }
        public bool IsSoldOut
        {
            get
            {
                if (CountRemaining == 0)
                {
                    return true;
                }
                return false;
            }
        }
        public override string ToString()
        {
            switch (ProductType)
            {
                case "Drink":
                    return "Glug Glug, Yum!";
                case "Candy":
                    return "Munch Munch, Yum!";
                case "Chip":
                    return "Crunch Crunch, Yum!";
                case "Gum":
                    return "Chew Chew, Yum!";
                default:
                    return "";
            }
        }
        public Item(string slotIdentifier, string name, decimal price, string productType)
        {
            this.SlotIdentifier = slotIdentifier;
            this.Name = name;
            this.Price = price;
            this.ProductType = productType;
            this.CountRemaining = 5;
        }
    }
}