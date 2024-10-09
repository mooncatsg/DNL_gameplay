using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlantClassification
{
    none,
    plant,
    animal
}
public enum PlantActionType
{
    none,
    hungry,
    sick,
    normal,
    bug,
    weed,
    fertilize
}
public enum PlantStatus
{
    none, normal, warning, happening
}
[System.Serializable]
public class PlantFertilize
{
    public int code;
    public string message;
    public int id;
    public int din_land_animal_id;
    public int din_land_plant_id;
    public string classification;
    public PlantClassification Classification
    {
        get
        {
            if (string.IsNullOrEmpty(classification))
            {
                return PlantClassification.none;
            }
            PlantClassification result;
            return System.Enum.TryParse<PlantClassification>(classification, true, out result) ? result : PlantClassification.none;
        }
    }
    public string type;
    public PlantActionType Type
    {
        get
        {
            if (string.IsNullOrEmpty(type))
            {
                return PlantActionType.none;
            }
            PlantActionType result;
            return System.Enum.TryParse<PlantActionType>(type, true, out result) ? result : PlantActionType.none;
        }
    }
    public int severity;
    public string status;
    public PlantStatus Status
    {
        get
        {
            if (string.IsNullOrEmpty(status))
            {
                return PlantStatus.none;
            }
            PlantStatus result;
            return System.Enum.TryParse<PlantStatus>(status, true, out result) ? result : PlantStatus.none;
        }
    }
}
