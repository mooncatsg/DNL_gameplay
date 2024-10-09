public enum CURRENCY_TYPE
{
    none = 0,
    WDNL = 1,
    WDNG = 2
}

[System.Serializable]
public class Buy
{
    public int id;
    public int warehouseId;
}

[System.Serializable]
public class BuySeed : Buy
{
    public string classification;
    public string type;
    public int quantity;
    public int status;
}


[System.Serializable]
public class BuyCrop : Buy
{
    public int level;
    public string status;
    public string rarity;
    public CropRarity Rarity
    {
        get
        {
            if (string.IsNullOrEmpty(rarity))
            {
                return CropRarity.none;
            }
            CropRarity result;
            return System.Enum.TryParse<CropRarity>(rarity, true, out result) ? result : CropRarity.none;
        }
    }
    public int capacity;
}


[System.Serializable]
public class BuyAnimal : Buy
{
    public int level;
    public string status;
    public string rarity;
    public int capacity;
}

[System.Serializable]
public class BuyAll : Buy
{
    public string classification;
    public string type;
    public STORE_ITEM_TYPE Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return STORE_ITEM_TYPE.none;
            }
            STORE_ITEM_TYPE result;
            return System.Enum.TryParse<STORE_ITEM_TYPE>(type, true, out result) ? result : STORE_ITEM_TYPE.none;
        }
    }
    public int quantity;

    public BuyAll (string _class,string _type,int _quantity)
    {
        this.classification = _class;
        this.type = _type;
        if (this.Type == STORE_ITEM_TYPE.crop_box)
            this.type = STORE_ITEM_TYPE.crop.ToString();
        this.quantity = _quantity;
    }
}