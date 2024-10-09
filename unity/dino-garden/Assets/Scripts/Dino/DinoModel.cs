using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Linq;
using System;
[System.Serializable]
public class DinoExpressTraits
{
    public int Class;
    public int Teeth;
    public int Back;
    public int Horn;
    public int Texture;
    public int Eye;
    public int Wing;
}

public class DinoBodyDefine
{
    public static List<int> NovisBody = new List<int>(){ 5, 8, 9 };
    public static List<int> AquisBody = new List<int>() { 0, 2,6 };
    public static List<int> TerrosBody = new List<int>() { 1, 3,7 };
    public static List<int> DarkBody = new List<int>() { 4, 10 };
    public static List<int> LightBody = new List<int>() {11};
    public static List<List<int>> BodyArray = new List<List<int>>() { DinoBodyDefine.NovisBody, DinoBodyDefine.AquisBody, DinoBodyDefine.TerrosBody, DinoBodyDefine.DarkBody, DinoBodyDefine.LightBody };
}
[System.Serializable]
public class DinoModel 
{
    public int id;
    public int nftId;
    public int oldGenes;
    public string evolveGenes;
    public bool isEvolved;
    public int gender;
    public long bornAt;
    public string decodeGenes;
    public int rarity;
    //public int generations;
    //public string updatedAt;
    //public string createdAt;
    //public int breedCount;
    //public int fatherNftId;
    //public int motherNftId;
    //public string owner;
    //public string preOwner;
    //public string syncedAt;

    List<int> cacheTraits;
    public List<int> getTraits() 
    {
        if (cacheTraits == null)
        {
            if (!string.IsNullOrEmpty(decodeGenes))
            {
                cacheTraits = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(decodeGenes);
            }
        }
        if (cacheTraits == null)
        {
            BigInteger genes;
            BigInteger.TryParse(evolveGenes, out genes);
            string sourceBinary = genes.ToBinaryString();
            int prefixZeroAmount = 4 * 21 - sourceBinary.Length;
            string binaryGenes = "";
            for (int i = 0; i < prefixZeroAmount; i++)
            {
                Debug.Log(binaryGenes);
            }
            binaryGenes += sourceBinary;
            List<string> traits = new List<string>();
            int j = 1;
            while(j< binaryGenes.Length)
            {
                int c = 4;
                if (binaryGenes.Length - j < c)
                    c = binaryGenes.Length - j;
                traits.Add(binaryGenes.Substring(j, c));
                j += 4;
            }
            traits.Reverse();
            cacheTraits = traits.Select(trait => Convert.ToInt32(trait, 2)).ToList();
        }
        return cacheTraits;
    }

    public DinoExpressTraits getExpressingTraits() 
    {
        var traits = getTraits();
        var expressTraits = traits.Where((trait, index) => index % 3 == 0).ToList();
        var bodyParts = new DinoExpressTraits();
        bodyParts.Class = expressTraits[0];
        bodyParts.Teeth = expressTraits[1];
        bodyParts.Back = expressTraits[2];
        bodyParts.Horn = expressTraits[3];
        bodyParts.Eye = expressTraits[5];
        bodyParts.Wing = expressTraits[6];

        var bodyClass = DinoBodyDefine.BodyArray[bodyParts.Class];
        bodyParts.Texture = expressTraits[4];
        bodyParts.Texture = bodyClass[bodyParts.Texture % bodyClass.Count];
        if (nftId == 5813)
        {
            bodyParts.Wing = 7;
            bodyParts.Back = 7;
            bodyParts.Texture = 0;
            bodyParts.Eye = 6;
            bodyParts.Horn = 7;
            bodyParts.Teeth = 0;
        }
        return bodyParts;
    }

    public int calculateRarity() 
    {
        if (rarity > 0)
            return rarity;
        else
        {
            var traits = getTraits();

            int totalScore = 0;
            Dictionary<int, int> scoreByIndex = new Dictionary<int, int>() { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 2 }, { 5, 2 }, { 6, 2 }, { 7, 3 }, { 8, 3 }, { 9, 4 }, { 10, 5 } };
        
            foreach(var f in traits)
            {
                var index = traits.IndexOf(f);
                  if (index % 3 == 0) {
                    if (((float)index/3) == 1f || ((float)index / 3) == 2f|| ((float)index / 3) == 3f|| ((float)index / 3) == 6f) {
                      totalScore += scoreByIndex[f + 1];
                    }
                  }
            };

            if (totalScore <= 8)
            {
                return 1;
            }
            else if (totalScore <= 13)
            {
                return 2;
            }
            else if (totalScore <= 16)
            {
                return 3;
            }
            else if (totalScore <= 18)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }

    public int GetClass () 
    {
        var trait = getTraits();
        if (trait[0] == 0) return 1;
        if (trait[0] == 1) return 2;
        if (trait[0] == 2) return 3;
        if (trait[0] == 3) return 4;
        if (trait[0] == 4) return 5;
        return 1;
    }

    /**
     * GetRarityString
     */
    public string GetRarityString() {
        string rarityString = "Normal";
        switch (calculateRarity())
        {
            case 1:
                rarityString = "Normal";
                break;
            case 2:
                rarityString = "Rare";
                break;
            case 3:
                rarityString = "SuperRare";
                break;
            case 4:
                rarityString = "Legendary";
                break;
            case 5:
                rarityString = "Mystic";
                break;
        }
        return rarityString;
    }

    /**
     * getGenderString
     */
    public string GetGenderString() {
        return gender == 1 ? "Male" : "Female"; 
    }

    /**
     * GetClassString
     */
    public string GetClassString() {

        string classString = "Novis";
        switch (GetClass())
        {
            case 1:
                classString = "Novis";
                break;
            case 2:
                classString = "Aquis";
                break;
            case 3:
                classString = "Terros";
                break;
            case 4:
                classString = "Dark";
                break;
            case 5:
                classString = "Light";
                break;
        }
        return classString;
    }

    public Sprite GetClassIcon() {
        string classString = GetClassString();
        return Resources.Load<Sprite>("DinoCare/Class/" + classString);
    }

    public Sprite getGenderIcon() {
        string genderString = GetGenderString();
        return Resources.Load<Sprite>("DinoCare/Gender/" + genderString);
    }


    public Sprite getRarityCard() {
        string rarityString = GetRarityString();
        return Resources.Load<Sprite>("DinoCare/Card/" + rarityString );
    }

    public Sprite getDinoBGByRarity() {
        string rarityString = GetRarityString();
        return Resources.Load<Sprite>("DinoCare/DinoBG/" + rarityString); 
    }
}
