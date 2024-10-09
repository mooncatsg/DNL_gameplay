using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
//using UnityEngine.InputSystem.EnhancedTouch;
public class LoadMapDataCtr : MonoBehaviour
{
    

    private string KEY_STOREMAP = "STOREMAP";
    private string KEY_STOREFARM = "STOREFARM";

    private string KEY_STOREMAP_JSON = "STOREMAP_JSON";
    private string KEY_STOREFARM_JSON = "STOREFARM_JSON";

    public GameObject mainCamera;
    public static LoadMapDataCtr ins { get; protected set; }

    void Start()
    {
        //TouchSimulation.Enable();
        ins = this;


       // LoadMapStoreJSON();
       // LoadSeedStore();
        //Invoke("LoadMapStoreJSON", 0.2f);

    }
    private void LoadMapStoreJSON()
    {
        string strsave = PlayerPrefs.GetString(KEY_STOREMAP_JSON, "");
        Debug.Log("----strsave=" + strsave);
        if (strsave.Length < 10)
        {
            return;
        }

        
        List<ItemPlaceController> listObjConfig = MapController.instance.GetAllObjectMove();

        ItemPos[] arrItem = JsonHelper.FromJson<ItemPos>(strsave);
        foreach (ItemPos itemp in arrItem)
        {
            foreach (ItemPlaceController tmpObj in listObjConfig)
            {
                if (tmpObj.item_id == itemp.itemID)
                {
                    tmpObj.UpdatePosFromConfig(itemp);
                    break;
                }
            }
        }

    }

    


   
    
    public void SaveMapData()
    {

        ItemPlaceController[] arritem = this.gameObject.GetComponentsInChildren<ItemPlaceController>();
        List<ItemPos> listPos = new List<ItemPos>();

        for (int i = 0; i < arritem.Length; i++)
        {
            ItemPlaceController itp = arritem[i];
            if (itp.placeType == PLACE_TYPE.MOVE)
            {
                ItemPos itemp = new ItemPos();
                itemp.itemID = itp.item_id;
                itemp.usize = itp.USize;
                itemp.px = itp.posUnit.ux;
                itemp.py = itp.posUnit.uy;
                listPos.Add(itemp);
            }
        }
        string save_str = JsonHelper.ToJson<ItemPos>(listPos.ToArray()); ;
        PlayerPrefs.SetString(KEY_STOREMAP_JSON, save_str);
        //Debug.Log("---save=" + save_str);


    }







    string arrConvertToString(List<int> listPos)
    {
        string save_str = "";
        for (int i = 0; i < listPos.Count; i++)
        {
            if (save_str.Length < 3)
            {
                save_str = listPos[i] + "";
            }
            else
            {
                save_str = save_str + "," + listPos[i];
            }


        }

        return save_str;
    }

    public void SaveFrameData()
    {
        return;
        /*
        List<int> listFarm = new List<int>();
        ItemFarmData itemp = new ItemFarmData();
        Farm3DController[] arrfarm = this.transform.GetComponentsInChildren<Farm3DController>();
        for (int b = 0; b < arrfarm.Length; b++)
        {
            Farm3DController tmpF = arrfarm[b];
            itemp.frmStt = (int)tmpF.farmStt;
            itemp.landType = (int)tmpF.landTpe;
            itemp.farmID = tmpF.FarmID;
            itemp.farmTime = 1;
            itemp.CompressionData();
            listFarm.Add(itemp.compression);
        }
        string save_str = this.arrConvertToString(listFarm);
        PlayerPrefs.SetString(KEY_STOREFARM, save_str);
        Debug.Log("---save=" + save_str);
        */
    }


    private String KEY_SEED_STORE = "KEY_SEED_STORE_MOCKUP_1";
    private List<SeedInfo> _listSeedInfo = new List<SeedInfo>();
    public void LoadSeedStore()
    {
        var data_str = PlayerPrefs.GetString(KEY_SEED_STORE, "");
        Debug.Log("SEED DATA = " + data_str);
        if (data_str.Length < 10)
        {
            this._listSeedInfo.Add(new SeedInfo(seedID: 1, seedName: "pineapple", amount: 2));
            this._listSeedInfo.Add(new SeedInfo(seedID: 2, seedName: "grape", amount: 0));
            this._listSeedInfo.Add(new SeedInfo(seedID: 3, seedName: "grapefruit", amount: 1));
            this._listSeedInfo.Add(new SeedInfo(seedID: 4, seedName: "strawberry", amount: 2));
            this._listSeedInfo.Add(new SeedInfo(seedID: 5, seedName: "watermelon", amount: 1));
            this._listSeedInfo.Add(new SeedInfo(seedID: 6, seedName: "apple", amount: 2));
            this._listSeedInfo.Add(new SeedInfo(seedID: 7, seedName: "cherry", amount: 1));
            this._listSeedInfo.Add(new SeedInfo(seedID: 8, seedName: "orange", amount: 3));
            this._listSeedInfo.Add(new SeedInfo(seedID: 9, seedName: "cantaloupe", amount: 0));

            PlayerPrefs.SetString(KEY_SEED_STORE, JsonHelper.ToJson<SeedInfo>(this._listSeedInfo.ToArray()));
        }
        else
        {
            SeedInfo[] dataArr = JsonHelper.FromJson<SeedInfo>(data_str);
            this._listSeedInfo = dataArr.ToList();

            //For Test:
            var totalAmount = 0;
            for (int i = 0; i < this._listSeedInfo.Count; i++)
            {
                totalAmount += this._listSeedInfo[i].amount;
            }

            if (totalAmount <= 0)
            {
                PlayerPrefs.SetString(KEY_SEED_STORE, "");
                this.LoadSeedStore();
            }
        }
    }

    public List<SeedInfo> GetListSeedInfo()
    {
        return this._listSeedInfo;
    }

    public void SaveSeedInfo(int seedId, string seedName, int amount)
    {
        bool isHaveSeed = false;
        for (int i = 0; i < this._listSeedInfo.Count; i++)
        {
            var seedInfo = this._listSeedInfo[i];
            if (seedInfo.seedID == seedId)
            {
                seedInfo.amount = amount;
                isHaveSeed = true;
                this._listSeedInfo[i] = seedInfo;
                break;
            }
        }

        if (!isHaveSeed)
        {
            this._listSeedInfo.Add(new SeedInfo(seedID: seedId, seedName: seedName, amount: amount));
        }

        var data_str = JsonHelper.ToJson<SeedInfo>(this._listSeedInfo.ToArray());
        Debug.Log("SEED DATA = " + data_str);
        PlayerPrefs.SetString(KEY_SEED_STORE, data_str);
    }
}
