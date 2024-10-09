using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//CLASIFICATION:
//{
//      CROP: 'crop',
//      SEED: 'seed',
//      PLANT: 'plant',
//      ANIMAL: 'animal',
//      FOOD: 'food',
//      FERTILIZE: 'fertilize',
//      PESTICIDE: 'pesticide',
//      TOOL: 'tool'
//}
//CROP:
//{
//    RANDOM: 0,
//        NORMAL: 1,
//        RARE: 2,
//        SUPER_RARE: 3,
//        LEGENDARY: 4,
//        MYSTHICAL: 5
//},
//ANIMAL:
//{
//    COW: 'cow',
//        PIG: 'pig',
//        SHEEP: 'sheep',
//        CHICKEN: 'chicken',
//},
//PLANT:
//{
//    RICE: 'rice',
//        CORN: 'corn',
//        CARROT: 'carrot',
//        STRAWBERRY: 'strawberry',
//        TOMATO: 'tomato',
//        POTATO: 'potato',
//        GRAPE: 'grape',
//        PEPPER: 'pepper',
//}
public enum StakingClassification
{
    None = 0,
    crop,
    seed,
    plant,
    animal,
    food,
    fertilize,
    pesticide,
    tool
}
public enum StakingType
{
    none = 0,
    garden,
    chicken,
    cow,
    pig,
    sheep
}

[Serializable]
public class StakeDinoRequest
{
    public int nftId;
    public string classification;
    public string type;
}

[Serializable]
public class UnstakeDinoRequest
{
    public int stakingFarmId;
}



[Serializable]
public class StakingFarm 
{
    public int id;
    public string classification;
    public string type;
    public int status;
    public int landId;
    public int nftId;

    public StakingClassification Classification 
    { 
        get
        {
            if (string.IsNullOrEmpty(classification))
            {
                return StakingClassification.None;
            }
            StakingClassification result;
            return System.Enum.TryParse<StakingClassification>(classification, true, out result) ? result : StakingClassification.None;
        }
    }
    public StakingType Type
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
}

[Serializable]
public class StakingFarmWithDino : StakingFarm
{
    public DinoModel dino;
}  

[Serializable]
public class StakedFarm
{
    public int landId;
    public string type;
    public StakingFarmWithDino dinoCare;
}
