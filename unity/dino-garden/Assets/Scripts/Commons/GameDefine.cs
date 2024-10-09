using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TRANSITION_TYPE
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT,
    ZOOM
}

public static class GameDefine
{
    public static List<float> PLANT_TIME_GROWTH = new List<float> {120f/720f,60f/480f,120f/360f,30f/240f,120f/600f,360f/1440f,240f/1080f,60f/960f };
    public static List<float> ANIMAL_TIME_GROWTH = new List<float> { 60f / 360f, 240f / 720f, 360f / 960f,720f/1440f};
    public static int MAX_ANIMAL_IN_CAGE = 4;
    public static List<string> LIST_TREE_DESCRIPTION = new List<string>() {
        "Dinos have their shiny eyes like marble might relate to this vegetable",
        "The most common plant that being grown in the Dinoland",
    "Most Dinos think it was Wheat in the beginning, turn out it tastes much better",    
    "It's soft, It's sweet. Dinos enjoy it till the last bit",
    "Novis Dino has to take a mouthful of these in their adult ceremony...",
    "Dionysius sounds pretty familiar to Dino, right?",
    "Imagine a Terros Dino stands beside these...",
    "Some elder Dinos have a secret sauce recipe with this juicy fruit that stole by a country shaped like a boot",   
    
    };

    public static List<string> LIST_ANIMAL_DESCRIPTION = new List<string>() {
        "They're biological related to Dino but somehow still being raised in ranches",
    "There's a religion consider cow as sacred symbol of life, but they're still able to consume dairy products like our Dinos tho",    
    "Scientist said they're even smarter than dog. But do you wonder how're they comparing to Dinos?!",
    "Do Dinos really need any product from these kind since they're not wearing any cloth?",};

    public static string ParseProduct(string animalStr)
    {
        switch(animalStr)
        {
            case "chicken":
                return "EGG";
            case "cow":
                return "MILK";
            case "sheep":
                return "WOOL";
            case "pig":
                return "PORK";    

        }
        return animalStr;
    }
    public static string getRarity(int rarity)
    {
        switch (rarity)
        {
            case 1:
                return "NORMAL";                
            case 2:
                return "RARE";                
            case 3:
                return "SUPER RARE";
            case 4:
                return "LEGENDARY";                
            case 5:
                return "MYSTIC";
                
        }
        return rarity.ToString();
    }
}















#region LoadJSON

[System.Serializable]
public struct ItemPos
{

    public int px;
    public int py;
    public int usize;
    public int itemID;

}

public class CameraData
{
    public float px;
    public float py;
    public float pz;
    public float rx;
    public float ry;
    public float rz;
    public string ConvertFloatToString(float fnb)
    {
        double f100 = Math.Round(fnb * 100);
        f100 = f100 / 100.0f;
        return (f100 + "");
    }
    public Vector3[] LoadFromSystem()
    {
        string strsave = PlayerPrefs.GetString("CAMERADATA", "");
        if (strsave.Length < 10)
        {
            return null;
        }
        Debug.Log("---strsave=" + strsave);
        string[] arrstrf = strsave.Split('|');
        if (arrstrf.Length != 6)
        {
            return null;
        }
        Vector3[] vtrt = new Vector3[2];
        float[] arrF = new float[6];
        for(int a = 0; a < 6; a++)
        {
            arrF[a] = float.Parse(arrstrf[a]);
        }
        vtrt[0].x = arrF[0];
        vtrt[0].y = arrF[1];
        vtrt[0].z = arrF[2];

        vtrt[1].x = arrF[3];
        vtrt[1].y = arrF[4];
        vtrt[1].z = arrF[5];
        return vtrt;
    }
    //--spD=16.93|43.18|-89.22|0.31|0|0
    public void SaveData(Transform tranf)
    {
        string spD = this.ConvertFloatToString(tranf.position.x);
        spD = spD + "|" + this.ConvertFloatToString(tranf.position.y);
        spD = spD + "|" + this.ConvertFloatToString(tranf.position.z);


        spD = spD + "|" + this.ConvertFloatToString(tranf.eulerAngles.x);
        spD = spD + "|" + this.ConvertFloatToString(tranf.eulerAngles.y);
        spD = spD + "|" + this.ConvertFloatToString(tranf.eulerAngles.z);
        PlayerPrefs.SetString("CAMERADATA", spD);
        
    }
}
[System.Serializable]
public struct ItemFarmData
{
    public int frmStt;
    public int landType;
    public int farmID;
    public int farmTime;

    public int Space;//= 100;
    public int compression;//= 0;
    public int validateNumber()
    {
        return (int)Math.Pow(Space, 3);
    }
    public int CompressionData()
    {
       
        double cp = frmStt * Math.Pow(Space, 3) + landType * Math.Pow(Space, 2) + farmID * Math.Pow(Space, 1) + farmTime * Math.Pow(Space, 0);
        compression = (int)cp;

        return compression;
    }
    public void ExtractData()
    {
        
        int tempC = compression;
        farmTime = tempC % ((int)Math.Pow(Space, 1));
        tempC = tempC - farmTime;
        farmID = (tempC % ((int)Math.Pow(Space, 2))) / ((int)Math.Pow(Space, 1));
        tempC = tempC - farmID;
        landType = (tempC % ((int)Math.Pow(Space, 3))) / ((int)Math.Pow(Space, 2));
        tempC = tempC - landType;
        frmStt = (tempC % ((int)Math.Pow(Space, 4))) / ((int)Math.Pow(Space, 3));
    }
}

#endregion


#region --Config ItemData
public enum FarmState
{
    seed = 0,
    chit = 1,
    growth = 2,
    canHarvest = 3,
}

[System.Serializable]
public class FarmPrefab
{
    public Mesh farmMesh;
    public Material farmMat;
    public int landID;
}
public enum LANDTYPE
{

    LAND1=1,
    LAND2=2,
    LAND3=3,
    LAND4=4,
    LAND5=5,
    LAND6=6
      
}
public enum PLANTING_TIMELINE {

    TIME1,
    TIME2,
    TIME3,
    TIME4,
    TIME5
}
#endregion



#region CameraDefineTouch
public enum INPUTSTATE
{
    NONE,
    BEGIN,
    MOVE,
    HOLD_AND_MOVE,
    ENDED,
}

#endregion
