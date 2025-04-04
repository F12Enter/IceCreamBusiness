using UnityEngine;
using System.Collections.Generic;

namespace Core.OrderSystem
{
    public class OrderData
    {
        public string OrderID;
        public string LocalizedKey;
        
        public List<Flavour> RequiredFlavours;
        public int MinimumScoops;

        public string PrintOrder()
        {
            string flavours = string.Join(", ", RequiredFlavours);
            return $"I need {MinimumScoops} scoops of {flavours} flavour.";
        }

        public OrderData(List<Flavour> flavours, int minimumScoops, string orderID)
        {
            RequiredFlavours = flavours;
            MinimumScoops = minimumScoops;
            OrderID = orderID;
        }

    }
}