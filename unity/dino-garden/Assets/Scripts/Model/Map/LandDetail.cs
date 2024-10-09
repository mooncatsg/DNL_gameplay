using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]


public class LandDetail : LandData
{
    public List<Crop> crops = new List<Crop>();
    public List<Cage> cages = new List<Cage>();
}