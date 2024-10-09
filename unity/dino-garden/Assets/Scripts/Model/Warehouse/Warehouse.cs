using System.Collections.Generic;

[System.Serializable]
public class UpgradePrice
{
    public int amount;
    public int typeCurrency;
}

[System.Serializable]
public class UpgradeWarehouse
{
    public int level;
    public int capacity;
    public UpgradePrice upgrade_price;
}

[System.Serializable]
public class Warehouse
{
    public int id;
    public int land_id;
    public int level;
    public int capacity;
    public int used;
    public int status;
    public List<UpgradeWarehouse> upgradeDetail;
    public List<WarehouseItem> items;
    public List<WarehouseItem> products;
}
