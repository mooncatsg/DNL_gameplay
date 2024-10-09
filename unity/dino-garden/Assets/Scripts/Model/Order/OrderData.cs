using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemOrderData
{
    public int id;
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
    public int price;
    public int priceType;
    public int quantity;
    public string note;
    public int marketId;
}

[System.Serializable]
public class OrderData
{
    public int id;
    public string name;
    public int discount;
    public int quantity;
    public int fulfilled;
    public string validFrom;
    public string validTo;
    public string status;
    public string createdAt;
    public string updatedAt;
    public int totalPrice;

    public List<ItemOrderData> items;
}
