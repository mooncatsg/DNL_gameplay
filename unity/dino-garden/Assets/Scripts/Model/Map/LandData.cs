using System;
using System.Collections.Generic;

[System.Serializable]
public class LandData
{
    public int id;
    public int nftId;
    public int status;
    public string rarity;
    public int water;
    public int rock;
    public int mountain;
    public int tree;
    public int longitude;
    public int latitude;
    public string prevOwner;
    public string mintAt;
    public string syncedAt;
    public string updatedAt;
    public int ownerId;
}

// FOR LIST LAND
[System.Serializable]
public class LandDataDetail
{
    public int id;
    public int nftId;
    public int status;
    public string rarity;
    public LAND_RARITY Rarity
    {
        get
        {
            if (string.IsNullOrEmpty(rarity))
            {
                return LAND_RARITY.none;
            }
            LAND_RARITY result;
            return System.Enum.TryParse<LAND_RARITY>(rarity, true, out result) ? result : LAND_RARITY.none;
        }
    }
    public int water;
    public int rock;
    public int mountain;
    public int tree;
    public int longitude;
    public int latitude;
    public string prevOwner;
    public string mintAt;
    public string syncedAt;
    public string updatedAt;
    public int ownerId;
    public List<LandDateCrop> crop;
    public LandDateWarehouse warehouse;
    public List<LandDateAnimal> animal;
    public int cropLimit;
}

[System.Serializable]
public class LandDateCrop
{
    public string rarity;
    public string count;
}

[System.Serializable]
public class LandDateWarehouse
{
    public int id;
    public int capacity;
    public int used;
}

[System.Serializable]
public class LandDateAnimal
{
    public string type;
    public int count;
}

public enum LAND_RARITY
{
    none = 0,
    normal,
    rare,
    super_rare,
    legendary,
    mysthical,
}