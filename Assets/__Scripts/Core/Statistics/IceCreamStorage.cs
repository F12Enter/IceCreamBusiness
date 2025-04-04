using Core.OrderSystem;

namespace Core.Statistics
{
    public static class IceCreamStorage
    {
        public static int VanillaCount { get; private set; }
        public static int ChocolateCount { get; private set; }
        public static int StrawberryCount { get; private set; }

        public static int GetCountByFlavour(Flavour flavour)
        {
            switch (flavour)
            {
                case Flavour.Vanilla:
                    return VanillaCount;
                case Flavour.Chocolate:
                    return ChocolateCount;
                case Flavour.Strawberry:
                    return StrawberryCount;
                default:
                    return 0;
            }
        }

        public static void Add(Flavour flavour, int amount)
        {
            switch (flavour)
            {
                case Flavour.Vanilla:
                    VanillaCount += amount;
                    break;
                case Flavour.Chocolate:
                    ChocolateCount += amount;
                    break;
                case Flavour.Strawberry:
                    StrawberryCount += amount;
                    break;
            }
        }

        public static void Remove(Flavour flavour, int amount)
        {
            switch (flavour)
            {
                case Flavour.Vanilla:
                    VanillaCount -= amount;
                    break;
                case Flavour.Chocolate:
                    ChocolateCount -= amount;
                    break;
                case Flavour.Strawberry:
                    StrawberryCount -= amount;
                    break;
            }
        }

        public static void Set(Flavour flavour, int amount)
        {
            switch (flavour)
            {
                case Flavour.Vanilla:
                    VanillaCount = amount;
                    break;
                case Flavour.Chocolate:
                    ChocolateCount = amount;
                    break;
                case Flavour.Strawberry:
                    StrawberryCount = amount;
                    break;
            }
        }

    }
}

