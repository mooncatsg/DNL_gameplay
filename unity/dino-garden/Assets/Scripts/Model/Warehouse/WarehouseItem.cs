using System.Collections.Generic;
public enum ItemType
{
    none, water, fertilizer, pesticides, rice, corn, carrot, strawberry, crop
}
[System.Serializable]
public class WarehouseItem
{
    public int id;
    public int warehouseId;
    public string classification;                  
    public STORE_ITEM_TYPE Classification
    {
        get
        {
            if (string.IsNullOrEmpty(classification))
            {
                return STORE_ITEM_TYPE.none;
            }
            STORE_ITEM_TYPE result;
            return System.Enum.TryParse<STORE_ITEM_TYPE>(classification, true, out result) ? result : STORE_ITEM_TYPE.none;
        }
    }
    public string type;
    public ItemType Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return ItemType.none;
            }
            ItemType result;
            return System.Enum.TryParse<ItemType>(type, true, out result) ? result : ItemType.none;
        }
    }
    public string quantity;
    public float Quantity { get { return float.Parse(quantity); } }
    public int status;
    public float harvest;
}
