using EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{ 
    [System.Serializable]
    public class ItemStock : Stock<ItemModel,Item�arrierHolder>
    {
        public ItemStock (List<Item�arrierHolder> models)
        {
            _holdersInStock = new List<Item�arrierHolder>();
            for (int i =0;i<models.Count;i++)

            _holdersInStock.Add (new Item�arrierHolder( models[i]));
        }
        public ItemStock (ItemStock itStock)
        {
            _holdersInStock = new List<Item�arrierHolder>();
            for (int i = 0; i < itStock.HoldersInStock.Count; i++)
            { 
                _holdersInStock.Add(new Item�arrierHolder(itStock.HoldersInStock[i]));
            }
        }
    }
}
