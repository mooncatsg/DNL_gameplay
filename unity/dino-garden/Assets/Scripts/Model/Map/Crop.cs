using System.Collections.Generic;
public enum CropStatus
{
    none = 0,
    inland,
    store
}

public enum CropRarity
{
    none = 0,
    normal,
    rare,
    super_rare,
    legendary,
    mystic
}

[System.Serializable]

public class Crop
{
    public int id;
    public int din_warehouse_id;
    public int level;
    public int capacity;
    public int season;
    public int maxSeason;
    public int renewPrice;
    public int position;
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

    public string status;
    public CropStatus Status
    {
        get
        {
            if (string.IsNullOrEmpty(status))
            {
                return CropStatus.none;
            }
            CropStatus result;
            return System.Enum.TryParse<CropStatus>(status, true, out result) ? result : CropStatus.none;
        }
    }

    public Plant plant;
}
