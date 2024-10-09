using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PositionUnit
{
    public int ux;
    public int uy;
    [System.NonSerialized]
    public float x;
    [System.NonSerialized]
    public float z;
    [System.NonSerialized]
    public bool select;
    [System.NonSerialized]
    public bool active;
}


public static class Ultils
{
    
    // tim den object 3DMap/Ground xem scale x,z bang bao nhieu v1 x=12,z=12;
    static int MAPSZIE = 12;
    static int GSIZE = 10;
    static List<PositionUnit> listCell;
    //static bool isCM;
    public static void CreateGrid()
    {
        listCell = new List<PositionUnit>();
        int HC = 6;
        for (int ix = 0; ix < MAPSZIE; ix++)
        {
            for (int iz = 0; iz < MAPSZIE; iz++)
            {
                PositionUnit tmpCell = new PositionUnit();
                tmpCell.ux = ix;// - HC;
                tmpCell.uy = iz;// - HC;
                tmpCell.x = (tmpCell.ux-HC) * GSIZE + GSIZE/2.0f;
                tmpCell.z = (tmpCell.uy-HC) * GSIZE + GSIZE / 2.0f;
                tmpCell.select = false;
                tmpCell.active = true;
                listCell.Add(tmpCell);
            }
        }
    }
    public static  void CreateCacheGrid(PositionUnit bPos)
    {
        
        int LS = listCell.Count;
        for (int i = 0; i < LS; i++)
        {
            PositionUnit tmP = listCell[i];
            tmP.active = true;
            tmP.select = false;
            listCell[i] = tmP;
        }
        List<ItemPlaceController> listMove = MapController.instance.GetAllObjectMove();
        int ML = listMove.Count;
        //Debug.Log("------ML=" + ML);
        for(int m = 0; m < ML; m++)
        {
            ItemPlaceController itemP = listMove[m];
            int xs = itemP.posUnit.ux;
            int ys = itemP.posUnit.uy;
            if ((xs == bPos.ux) && (ys == bPos.uy))
            {
               // Debug.Log(itemP.gameObject.name+" ----bPos= " + bPos.ux + " uy=" + bPos.uy);
               // Debug.Log("----xs= " + xs + " ys=" + ys);
                continue;
            }
           // Debug.Log("-------------=" + itemP.gameObject.name);
            SetGridDisable(xs, ys);
            if (itemP.USize == 2)
            {
                SetGridDisable(xs+1, ys);
                SetGridDisable(xs, ys+1);
                SetGridDisable(xs+1, ys+1);
            }
        }
    }
    public static bool CheckAvaiable(int ux,int uy,int sz)
    {
        if (sz == 1)
        {
            if (CheckBock(ux, uy)==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else// sz==2
        {
            int tt = CheckBock(ux, uy)+ CheckBock(ux+1, uy) + CheckBock(ux, uy+1) + CheckBock(ux+1, uy+1);
            if (tt == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    static int CheckBock(int ux, int uy)// kt moi o,1 la khoa , 0 la mo
    {
        // 0 , 6
        // 1 , 6
        // 2 , 7
        // 3 ,8 
        // 4 , 9
        if (ux < 5)
        {
            int mY = 12;
            if (ux == 4)
            {
                mY = 9;
            }else if (ux == 3)
            {
                mY = 8;
            }
            else if (ux == 2)
            {
                mY = 7;
            }
            else if ((ux == 1)||(ux==0))
            {
                mY = 6;
            }
            if (uy > mY)
            {
                return 1;
            }
        }

        int LSC = listCell.Count;
        int block = 1;
        for (int i = 0; i < LSC; i++)
        {
            PositionUnit tmP = listCell[i];
            if ((ux == tmP.ux) && (uy == tmP.uy))
            {

                if (!tmP.select)
                {
                    block = 0;
                }
                break;
            }

        }

        return block;
    }
    static void SetGridDisable(int ux,int uy)
    {
        int LSC = listCell.Count;
        for (int i = 0; i < LSC; i++)
        {
            PositionUnit tmP = listCell[i];
            if ((ux==tmP.ux) && (uy==tmP.uy))
            {
                if (tmP.select)
                {
                    Debug.LogError("-----WTF--------FUCKKKKK----");
                    break;
                }
                tmP.select = true;
                listCell[i] = tmP;
                //Debug.Log("-----SetGridDisable="+ux+","+uy);
                break;
            }
            
        }
    }
    public static void FinishMove()
    {
        //isCM = false;
    }
    public static float GetDistance(PositionUnit pU, Vector3 vtPos)
    {
     
        float dtx = pU.x - vtPos.x;
        float dtz = pU.z - vtPos.z;
        Vector2 vt2 = new Vector2(dtx, dtz);
        //float dt = Mathf.Sqrt(dtx * dtx + dtz * dtz) ;
        float dt = vt2.magnitude;
        return dt;
    }
    public static PositionUnit GetRoundCell(Vector3 vtPos)
    {
        PositionUnit rtpos = new PositionUnit();

        rtpos.active = false;
        int LS = listCell.Count;
        float min_dt = 1000;
        for (int i = 0; i < LS; i++)
        {
            PositionUnit tmP = listCell[i];
            float dt = Ultils.GetDistance(tmP, vtPos);
            if (min_dt > dt)
            {
                min_dt = dt;

                rtpos.ux = tmP.ux;
                rtpos.uy = tmP.uy;
                rtpos.x = tmP.x;
                rtpos.z = tmP.z;
                rtpos.active = true;
            }
        }
        return rtpos;
    }
    public static PositionUnit GetUnitPos(int ux,int uy, int sz)
    {
        PositionUnit rtpos=new PositionUnit();
        rtpos.active = false;
        if((ux<0)|| (uy<0))
        {
            return rtpos;
        }
        int max_s = MAPSZIE - sz;
        if ((ux > max_s) || (uy > max_s))
        {
            return rtpos;
        }
        int LS = listCell.Count;
        for(int i = 0; i <LS; i++)
        {
            PositionUnit tmP = listCell[i];
            if ((tmP.ux == ux) && (tmP.uy == uy)){
                rtpos.ux = tmP.ux;
                rtpos.uy = tmP.uy;
                rtpos.x = tmP.x;
                rtpos.z = tmP.z;
                if (sz == 2)
                {
                    rtpos.x = rtpos.x + GSIZE / 2.0f;
                    rtpos.z = rtpos.z + GSIZE / 2.0f;
                }
                rtpos.active = true;

                break;
            }
        }
        


        return rtpos;
    }
 
    public static PositionUnit ConvertVec3ToUnit(Vector3 pos, int sz)
    {
        if (sz == 2)
        {
            pos.x = pos.x - GSIZE / 2.0f;
            pos.z = pos.z - GSIZE / 2.0f;
        }
        PositionUnit pu = Ultils.GetRoundCell(pos);
        if (sz == 2)
        {
            pu.x = pu.x + GSIZE / 2.0f;
            pu.z = pu.z + GSIZE / 2.0f;
        }

        return pu;


    }
}
