namespace Demo.Models.Repositories
{
    public class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>()
        {
        new Shirt(){ShirtId = 1, Brand = "Brand 1", Color = "Blue", Gender = "Men", Price = 10, Size = 10},
        new Shirt(){ShirtId = 2, Brand = "Brand 2", Color = "Black ", Gender = "Women", Price = 32, Size = 12},
        new Shirt(){ShirtId = 3, Brand = "Brand 3", Color = "Pink", Gender = "Women", Price = 13, Size = 11},
        new Shirt(){ShirtId = 4, Brand = "Brand 4", Color = "Yellow", Gender = "Men", Price = 300, Size = 10},

        };
        public static bool ShirtExist(int id)
        {
            return shirts.Any(x => x.ShirtId == id);
        }
        public static Shirt GetShirtById(int id)
        {
            return shirts.FirstOrDefault(x => x.ShirtId == id);
        }

        public static Shirt? GetShirtByProperties(string? brand, string? gender, string? color, int? size)
        {
            return shirts.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(brand) && !string.IsNullOrWhiteSpace(x.Brand) && x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(gender) && !string.IsNullOrWhiteSpace(x.Gender) && x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(color) && !string.IsNullOrWhiteSpace(x.Color) && x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
            (size.HasValue && x.Size.HasValue && size.HasValue == x.Size.HasValue));
        }

        public static List<Shirt> GetShirts()
        {
            return shirts;
        }

        public static void AddShirt(Shirt shirt)
        {
            int maxId = shirts.Max(x => x.ShirtId);
            shirt.ShirtId = maxId + 1;
            shirts.Add(shirt);
        }

        public static void UpdateShirt(Shirt shirt)
        {
            var shirtToUpdate = shirts.First(x => x.ShirtId == shirt.ShirtId);
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Gender = shirt.Gender;
            shirtToUpdate.Size = shirt.Size;
        }

        public static void DeleteShirt(int shirtId)
        {
            var shirt = GetShirtById(shirtId);
            if(shirt != null)
            {
                shirts.Remove(shirt);
            }
        }

    }
}
