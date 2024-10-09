using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

[System.Serializable]
public class LifeCycle
{
    public int no;
    public string name;
    public long interval;
    public string endTime;
}

public enum EventCycleType
{
    none = 0,
    worm,
    arid,
    hungry,
    sick
}
// worm, arid, hungry,sick
[System.Serializable]
public class EventCycle
{
    public int id;
    public string type;
    public EventCycleType Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return EventCycleType.none;
            }
            EventCycleType result;
            return System.Enum.TryParse<EventCycleType>(type, true, out result) ? result : EventCycleType.none;
        }
    }
    public string status;
    public string classification;
    public int serverity;
    public string startTime;
    public int StartTime
    {
        get
        {
            if (string.IsNullOrEmpty(startTime))
            {
                return 0;
            }
            return (int)((DateTime.ParseExact(startTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture) - DateTime.UtcNow).TotalSeconds);
        }
    }

    public string fixTime;
    public int animalId;
    public int plantId;
    public bool happened;
}

[System.Serializable]
public class PlantSeedRequest
{
    public string type;
}
public enum PlantType
{
    none = 0,
    carrot,
    rice,
    corn,
    strawberry,
    chili,
    grape,
    potato,
    tomato,
}
[System.Serializable]
public class Plant
{
    public int id;
    public string type;
    public PlantType Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return PlantType.none;
            }
            PlantType result;
            return System.Enum.TryParse<PlantType>(type, true, out result) ? result : PlantType.none;
        }
    }
    public int level;
    public string status;
    public string createdAt;
    public int cropId;
    public List<LifeCycle> lifecycles;
    public List<EventCycle> events;
    public List<fixEventPrice> fixEventPrice;
}

[System.Serializable]
public class PlantWatering
{
    public EventCycle eventCycle;
    public string classification;
    public string status;
    public string type;
    public PlantType Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return PlantType.none;
            }
            PlantType result;
            return System.Enum.TryParse<PlantType>(type, true, out result) ? result : PlantType.none;
        }
    }
}
