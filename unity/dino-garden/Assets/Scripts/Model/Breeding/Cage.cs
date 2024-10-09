using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnimalType
{
    none=0,
    chicken,
    cow,
    pig,    
    sheep
}

public enum CageStatus
{
    none, locked, broken, normal
}

[System.Serializable]
public class Upgrade
{
    public int level;
    public int currency;
    
    public CURRENCY_TYPE Currency
    {
        get
        {
            return (CURRENCY_TYPE)currency;
        }
    }
    public int amount;
}

[System.Serializable]
public class UpgradeDetail
{
    public int level;
    public int durability;
    public int capacity;
}

[System.Serializable]
public class Cage
{
    public int id;
    public int level;
    public int capacity;
    public int durability;
    public string type;
    public AnimalType Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return AnimalType.none;
            }
            AnimalType result;
            return System.Enum.TryParse<AnimalType>(type, true, out result) ? result : AnimalType.none;
        }
    }

    public string status;
    public CageStatus Status
    {
        get
        {
            if (string.IsNullOrEmpty(status))
            {
                return CageStatus.none;
            }
            CageStatus result;
            return System.Enum.TryParse<CageStatus>(status, true, out result) ? result : CageStatus.none;
        }
    }
    public int landId;

    public List<Upgrade> upgrade = new List<Upgrade>();
    public List<UpgradeDetail> upgradeDetail = new List<UpgradeDetail>();
    public List<Upgrade> fix = new List<Upgrade>();
    public List<Animal> animalList = new List<Animal>();
}