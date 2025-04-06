using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Core.Localization;

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
            var translatedFlavours = RequiredFlavours.Select(flavour =>
                TranslationManager.Instance.GetTranslation($"Flavour.{flavour}")
            );
            string flavours = string.Join(", ", translatedFlavours);
            
            return TranslationManager.Instance.GetTranslation("NPC.Order", MinimumScoops, flavours);
           
        }

        public OrderData(List<Flavour> flavours, int minimumScoops, string orderID)
        {
            RequiredFlavours = flavours;
            MinimumScoops = minimumScoops;
            OrderID = orderID;
        }

    }
}