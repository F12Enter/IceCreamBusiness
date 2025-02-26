using UnityEngine;

namespace Core.Economy
{
    public static class EconomyManager
    {
        public static int Money { get; private set; }

        public static void AddMoney(int amount) => Money += amount;
        
        public static void RemoveMoney(int amount) => Money -= amount;
        
        public static void SetMoney(int amount) => Money = amount;

    }

}
