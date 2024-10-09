[System.Serializable]
public struct SeedInfo
{
    public int seedID;
    public string seedName;
    public int amount;
    public SeedInfo(int seedID, string seedName, int amount)
    {
        this.seedID = seedID;
        this.seedName = seedName;
        this.amount = amount;
    }
}

public class ItemInfoData
{
    public static string GetSeedNameBySeedId(int seedID)
    {
        string result = "";
        return result;
    }
}


[System.Serializable]
public class StoreItemInfo
{
    public STORE_ITEM_TYPE type;
    public int itemID;
    public string name;
    public string info;
    public int rate;
    public int time;
    public int currencyType;
    public int harvestTime;
    public StoreItemInfo(STORE_ITEM_TYPE type, int itemID, string name, int rate,int currencyType,int harvestTime)
    {
        this.type = type;
        this.itemID = itemID;
        this.name = name;
        this.rate = rate;
        this.info = "";
        this.time = 150;
        this.currencyType = currencyType;
        this.harvestTime = harvestTime;
    }
}

public enum STORE_TYPE
{
    none= 0,
    item,
    product,
}

public enum STORE_ITEM_TYPE
{
    none = 0,
    seed = 1,
    animal = 2,
    decor = 3,
    crop_box = 4,
    crop = 5,
    fertilizer = 6,
    plant = 7,
}

public class ItemPurchaseInfo
{
    public STORE_ITEM_TYPE type;
    public string name;
    public int itemID;
    public int rate;
    public int amount;
    public int currencyType;
    public ItemPurchaseInfo(StoreItemInfo itemInfo, int amount)
    {
        this.type = itemInfo.type;
        this.name = itemInfo.name;
        this.itemID = itemInfo.itemID;
        this.amount = amount;
        this.rate = itemInfo.rate;
        this.currencyType = itemInfo.currencyType;
    }
}


public class BuildingCropsInfo
{
    public int itemID;
    public int amount;
    public BuildingCropsInfo(int itemID, int amount)
    {
        this.itemID = itemID;
        this.amount = amount;
    }
}

public class ActionUIInfo
{
    public int acType;
    public ACTION_UI_TYPE AcType
    {
        get
        {
            return (ACTION_UI_TYPE)(acType);
        }
    }
    public int acId;
    public WarehouseItem itemInfo;
    public ActionUIInfo(int acType, int acId, WarehouseItem itemInfo)
    {
        this.acType = acType;
        this.acId = acId;
        this.itemInfo = itemInfo;
    }
}

public class StorageInfo
{
    public STORE_TYPE storeType;
    public STORE_ITEM_TYPE type;
    public string name;
    public int rate;
    public string quantity;
    public float Quantity { get { return float.Parse(quantity); } }
    public StorageInfo(STORE_TYPE storeType, STORE_ITEM_TYPE type, string name, int rate, string quantity)
    {
        this.storeType = storeType;
        this.type = type;
        this.name = name;
        this.rate = rate;
        this.quantity = quantity;
    }
}