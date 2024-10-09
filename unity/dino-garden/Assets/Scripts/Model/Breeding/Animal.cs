using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalLifecycle
{
    public int no;
    public string name;
    public long interval;
    public string endTime;
}
[System.Serializable]
public class fixEventPrice
{
    public string type;
    public float price;
    public float amount;
    public int typeCurrency;
}

[System.Serializable]
public class Animal
{
    public int id;    
    public int havestCnt;
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

    public StakingType StakeType
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return StakingType.none;
            }
            StakingType result;
            return System.Enum.TryParse<StakingType>(type, true, out result) ? result : StakingType.none;
        }
    }
    public string status;
    public int level;
    public int cageId;
    public string createdAt;
    public List<AnimalLifecycle> lifecycles;
    public List<EventCycle> events;
    public int hp;
    public int remain_hp;
    public int maxHarvestCount;
    public int harvest;
    public List<fixEventPrice> fixEventPrice;
}
