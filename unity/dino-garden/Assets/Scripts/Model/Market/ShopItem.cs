

public enum ShopItemClassification
{
    none, crop, animal, seed
}

[System.Serializable]
public class ShopItem
{
    public int id;
    public string classification;
    public string type;
    public int price;
    public int quantity;
    public int sold;
    public string status;
    public string createdAt;
    public string updatedAt;
    public int priceType;
    public int harvestTime;
}
