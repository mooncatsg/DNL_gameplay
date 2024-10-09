using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class PlantLifeTime
{
    public PlantType id;
    public GameObject[] prefabTime;
}

public class MapController : FastSingleton<MapController>
{    

    // Start is called before the first frame update
    public GameObject movePlace;
    int layerMask = -100;
    Ray rayc;
    [System.NonSerialized]
    List<ItemPlaceController> arrObjMove;

    public GameObject[] listBoard;
    [Space]
    [Header("DINO CARE==========================")]
    public GameObject dinoPrefab;
    [Space]
    [Header("FARM==========================")]
    public Transform listFarmParent;
    public Transform chikenCageTrans;
    public Transform cowCageTrans;
    public Transform pigCageTrans;
    public Transform sheepCageTrans;

    public List<Farm3DController> farmList = new List<Farm3DController>();
    public PlantLifeTime[] arrPlant;
    [Space]
    [Space]
    [Header("ANIMAL==========================")]
    public List<AnimalHouseCtrl> animalHouseList = new List<AnimalHouseCtrl>();
    [Space]
    public ItemPlaceController itemSelected;
    public ItemPlaceController itemChoosing;

    Vector3 INIT_POS_FARM = new Vector3(8.3f, 0f, -19f);
    public const int WIDTH_QUANTITY = 6;
    public const float WIDTH_HEIGHT_OFFSET = 10f;
    public int TEMP_CROPS_QUANTITY = 10;

    public Camera camTopDown;
    public Camera camPlayer;
    public GameObject playerNode;

    public UserInfor userInfor = null;
    public LandDetail currentlandDetail = null;
    public List<ShopItem> listShopAll = null;
    public int totalCrops;

    List<GameObject> listDinoCaring = new List<GameObject>();

    public List<StakingFarmWithDino> listStakingFarmWithDino = new List<StakingFarmWithDino>();
    void Start()
    {
        layerMask = LayerMask.GetMask("TouchMesh");
        if (layerMask < 0)
        {
            Debug.LogError("----layerMask=" + layerMask);
        }
        Ultils.CreateGrid();

        this.LoadInitialMap();

        playerNode.SetActive(false);
        camPlayer.enabled = false;
        camTopDown.enabled = true;
    }


    public void UpdateUserInfo()
    {
        APIMng.Instance.APIGetUserInfor((isSuccess, _userInfor, errMsg, code) =>
        {
            userInfor = (UserInfor)_userInfor;
            MainMenuUI.Instance.ShowUserInfo();
        });
    }    

    public void LoadInitialMap()
    {
        UpdateUserInfo();
        List<int> listCropsRest = new List<int>();
        List<int> listCropsDeposit = new List<int>();
        APIMng.Instance.APIGetShopListAll((isSuccess, _shopList, errMsg, code) =>
        {
            if(isSuccess)
                listShopAll = new List<ShopItem>((List<ShopItem>)_shopList);            
        });

        APIMng.Instance.APILandDetail((isSuccess, landDetail, errMsg, code) => {

            if(isSuccess)
            {
                int countPos = 0;
                currentlandDetail = (LandDetail)landDetail;
                var cropsInLand = currentlandDetail.crops.FindAll(x => x.Status == CropStatus.inland);
                for (int i = 0; i < cropsInLand.Count; i++)
                {
                    if(cropsInLand[i].position != 0)
                    { 
                        GameObject farmObject = GameObject.Instantiate(Resources.Load<GameObject>("Farm/Farm"), Vector3.zero, Quaternion.identity, this.listFarmParent);
                        int position = cropsInLand[i].position - 1;
                        farmObject.transform.localPosition = new Vector3(INIT_POS_FARM.x - (position % WIDTH_QUANTITY) * WIDTH_HEIGHT_OFFSET, 0f, (INIT_POS_FARM.z - (position / WIDTH_QUANTITY) * WIDTH_HEIGHT_OFFSET));
                        farmObject.transform.localScale = new Vector3(9, 5, 9);
                        farmObject.GetComponent<Farm3DController>().UpdateFarmData(cropsInLand[i]);
                    }
                    else
                    {
                        countPos++;
                    }
                }
                List<int> listPos = FreeListPositionInMap();
                int indexPos = 0;
                for (int i = 0; i < cropsInLand.Count; i++)
                {
                    if (cropsInLand[i].position == 0)
                    {
                        APIMng.Instance.APIUpdatePositon(cropsInLand[i].id, (int)listPos[indexPos], (isSuccess2, newCrop, errMsg2, code2) =>
                        {
                            if (isSuccess2)
                            {
                                UpdateNewCrops((Crop)newCrop);
                                
                            }
                            else
                            {
                                MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                            }
                        });
                        indexPos++;
                    }
                }
                foreach (AnimalHouseCtrl house in animalHouseList)
                {
                    house.InitData(currentlandDetail.cages.Find(x => x.Type == house.cageType));
                }
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
            }
        });
        UpdateDinoCaring();
    }


    public void UpdateLandetail(Action callback)
    {
        APIMng.Instance.APILandDetail((isSuccess, landDetail, errMsg, code) => {

            if (isSuccess)
            {
                currentlandDetail = (LandDetail)landDetail;
            }
            callback?.Invoke();
        });
    }
    public int totalCropInLand()
    {
        return (currentlandDetail.crops.FindAll(x => x.Status == CropStatus.inland)).Count;
    }
    public List<Crop> getCrop()
    {
        return currentlandDetail.crops.FindAll(x => x.Status == CropStatus.inland);
    }
    public int FreePositionInMap()
    {
        List<Farm3DController> listFarmObject = this.listFarmParent.GetComponentsInChildren<Farm3DController>().ToList();
        List<int> listPosition = new List<int>();
        foreach (Farm3DController farm in listFarmObject)
        {
            listPosition.Add(farm.cropData.position);
        }
        listPosition.Sort();
        for (int i = 1; i <= 100; i++)
        {
            if (!listPosition.Contains(i))
            {
                return i;
            }
        }
        return 0;
    }
    public List<int> FreeListPositionInMap() // tim vi tri nho nhat con trong.
    {
        List<Farm3DController> listFarmObject = this.listFarmParent.GetComponentsInChildren<Farm3DController>().ToList();
        List<int> listPosition = new List<int>();
        Debug.Log("call fun");
        foreach(Farm3DController farm in listFarmObject)
        {
            listPosition.Add(farm.cropData.position);
        }
        List<int> listPos = new List<int>();
        listPosition.Sort();
        for(int i=1;i <= 50; i++)
        {
            if(!listPosition.Contains(i) )
            {
                listPos.Add(i);
            }
        }
        return listPos;
    }

    /*public void ResetCropPosition()
    {
        this.ActionWaitForSeconds(0.5f, () => {
            List<Farm3DController> listFarm = this.listFarmParent.GetComponentsInChildren<Farm3DController>().ToList();
            for (int i = 0; i < listFarm.Count; i++)
            {
                int pos = listFarm[i].cropData.position;
                listFarm[i].transform.localPosition = new Vector3(INIT_POS_FARM.x - (pos % WIDTH_QUANTITY) * WIDTH_HEIGHT_OFFSET, 0f, (INIT_POS_FARM.z - (pos / WIDTH_QUANTITY) * WIDTH_HEIGHT_OFFSET));
            }
            Debug.Log(listFarm.Count);
            //CHECK FARM 
            if(listFarm.Count <= 0)
            {
                currentlandDetail.crops.Clear();
            }
        });
    }*/

    public void UpdateCropData()
    {
        APIMng.Instance.APILandDetail((isSuccess, landDetail, errMsg, code) => {
            currentlandDetail = (LandDetail)landDetail;
            var cropsInLand = currentlandDetail.crops.FindAll(x => x.Status == CropStatus.inland);
            List<Farm3DController> listFarm = this.listFarmParent.GetComponentsInChildren<Farm3DController>().ToList();
            if(listFarm.Count == cropsInLand.Count)
            {
                for (int i = 0; i < cropsInLand.Count; i++)
                {
                    listFarm[i].UpdateFarmData(cropsInLand[i]);
                }
            }
        });
    }    
    public void UpdateNewCrops(Crop newCrop)
    {
//        this.ActionWaitForSeconds(0.1f, () => {
            List<Farm3DController> listFarm = this.listFarmParent.GetComponentsInChildren<Farm3DController>().ToList();

            this.currentlandDetail.crops.Add(newCrop);
            GameObject farmObject = GameObject.Instantiate(Resources.Load<GameObject>("Farm/Farm"), Vector3.zero, Quaternion.identity, this.listFarmParent);
            int position = newCrop.position - 1; //listFarm.Count;
            farmObject.transform.localPosition = new Vector3(INIT_POS_FARM.x - (position % WIDTH_QUANTITY) * WIDTH_HEIGHT_OFFSET, 0f, (INIT_POS_FARM.z - (position / WIDTH_QUANTITY) * WIDTH_HEIGHT_OFFSET));
            farmObject.transform.localScale = new Vector3(9, 5, 9);
            farmObject.GetComponent<Farm3DController>().UpdateFarmData(newCrop);
        //});
    }

    public void UpdateNewAnimal(Action callback = null)
    {
        APIMng.Instance.APIListCage((isSuccess, _listCage, errMsg, code) => {    
            if(isSuccess)
            {
                currentlandDetail.cages = new List<Cage>((List<Cage>)_listCage);
                foreach (AnimalHouseCtrl house in animalHouseList)
                {
                    house.InitData(currentlandDetail.cages.Find(x => x.Type == house.cageType));
                }
                if (callback != null)
                    callback.Invoke();
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
            }
        });        
    }

    public bool IsDinoCaring(StakingClassification stakingClass, StakingType stakingType)
    {
        bool hasStakedDino = listStakingFarmWithDino != null && listStakingFarmWithDino.Count > 0;
        StakingFarmWithDino tmp = hasStakedDino ? listStakingFarmWithDino.Find(x => (x != null && x.Classification == stakingClass && x.Type == stakingType)) : null;
        return tmp != null;
    }

    public void UpdateDinoCaring()
    {
        ClearDinoCare();
        APIMng.Instance.APIDinoCaring((isSuccess, data, errMsg, code) => {
            if (isSuccess)
            {
                List<StakedFarm> stakedList = (List<StakedFarm>)data;
                bool hasStakedDino = isSuccess && stakedList != null && stakedList.Count > 0;

                foreach(StakedFarm stakedFarm in stakedList)
                {
                    listStakingFarmWithDino.Add(stakedFarm.dinoCare);
                }

                StakedFarm tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.plant && x.dinoCare.Type == StakingType.garden)) : null;
                if (tmp != null)
                    SpawnDino(tmp.dinoCare.dino, listFarmParent, false);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.chicken)) : null;
                if (tmp != null) SpawnDino(tmp.dinoCare.dino, chikenCageTrans);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.pig)) : null;
                if (tmp != null) SpawnDino(tmp.dinoCare.dino, pigCageTrans);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.cow)) : null;
                if (tmp != null) SpawnDino(tmp.dinoCare.dino, cowCageTrans);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.sheep)) : null;
                if (tmp != null) SpawnDino(tmp.dinoCare.dino, sheepCageTrans);
            }
        });
    }

    public void SpawnDino(DinoModel data, Transform parent, bool isAroundCage = true)
    {
        GameObject go = GameObject.Instantiate(dinoPrefab, parent) as GameObject;
        if (isAroundCage)
        {
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            go.transform.localPosition = new Vector3(1.1f, 0, 1.1f);
        }
        else
        {
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(15.3f, 0f, -12f);
        }
        listDinoCaring.Add(go);
        DinoCharacterController din = go.GetComponent<DinoCharacterController>();
        din.randomDino = false;
        din.autoRotate = false;
        din.autoMoveAroundCage = isAroundCage;
        din.autoMoveAroundCrop = !isAroundCage;
        din.loadDinoData(data.getExpressingTraits(), data.calculateRarity(), data.nftId);
    }
    public void ClearDinoCare()
    {
        foreach (var f in listDinoCaring)
        {
            GameObject.DestroyImmediate(f);
        }
        listDinoCaring.Clear();
        listStakingFarmWithDino.Clear();
    }
    public List<ItemPlaceController> GetAllObjectMove()
    {
        return arrObjMove;
    }

    public bool ValidTempLocation(ItemPlaceController item)
    {
        ItemPlaceController[] allitem = this.gameObject.GetComponentsInChildren<ItemPlaceController>();
        bool valid = true;




        return valid;
    }
    public Camera getActiveCamera()
    {
        Camera activeCam;
        if (camTopDown.enabled)
        {
            activeCam = camTopDown;
        }
        else
        {
            activeCam = camPlayer;
        }
        return activeCam;
    }
    public PLACE_TYPE CheckSelectItem(Vector2 posCanvas, bool highlight)
    {
        if (MainMenuUI.Instance.IsUIActive()) return PLACE_TYPE.NONE;
        Camera activeCam = this.getActiveCamera();
        rayc = activeCam.ScreenPointToRay(posCanvas);
        RaycastHit hit;
        ItemPlaceController itemSelecting = null;
        if (Physics.Raycast(rayc, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<ItemPlaceController>() != null)
                {
                    Debug.LogError("TOUCH : " + hit.collider.name);
                    itemSelecting = hit.collider.GetComponent<ItemPlaceController>();
                    itemSelecting.ShowClickActive();
                }
                else if (hit.collider.gameObject.name == NotiBillBoard.BOARDNAME)
                {
                    Debug.Log("---click board name---");
                    NotiBillBoard notiB = hit.collider.gameObject.GetComponent<NotiBillBoard>();
                    notiB.itemParent.ActionNotiClick(notiB);
                }
            }
        }

        //if (Physics.Raycast(rayc, out hit))
        //{
        //    if (hit.collider != null)
        //    {
        //        for (int ia = 0; ia < arrObjMove.Count; ia++)
        //        {
        //            GameObject tmpObj = arrObjMove[ia].gameObject;

        //            if ((hit.collider.gameObject.name == tmpObj.name))
        //            {
        //                itemSelecting = arrObjMove[ia];
        //                if (highlight)
        //                {
        //                    if (itemSelecting.placeType == PLACE_TYPE.MOVE)
        //                    {
        //                        itemSelecting.SelectedItem();

        //                    }
        //                }
        //                else
        //                {                            
        //                    itemSelecting.ShowClickActive();
        //                }
        //                break;
        //            }
        //            else if (hit.collider.gameObject.name == NotiBillBoard.BOARDNAME)
        //            {
        //                Debug.Log("---click board name---");
        //                NotiBillBoard notiB = hit.collider.gameObject.GetComponent<NotiBillBoard>();
        //                notiB.itemParent.ActionNotiClick(notiB);
        //                break;
        //            }
        //        }
        //    }
        //}
        //if(BienTempUI.instance.type != BienTempUI.UI_TYPE.UI_NONE)
        //{
        //    return PLACE_TYPE.NONE;
        //}    
        if (itemSelecting == null)
        {
            itemSelected = null;
            if (itemChoosing != null)
            {
                itemChoosing.OutlineEnable(false);
            }
            itemChoosing = null;
            return PLACE_TYPE.NONE;
        }
        itemSelected = itemSelecting;
        return
            itemSelecting.placeType;
    }

    public void DeSelectAllItem()
    {
        return;
        if (itemSelected != null)
        {
            Debug.Log("----Finish Drag-----:" + itemSelected.gameObject.name);
            itemSelected.FinishDragItem();
            LoadMapDataCtr.ins.SaveMapData();
            itemSelected = null;
        }
        for (int ia = 0; ia < arrObjMove.Count; ia++)
        {
            arrObjMove[ia].DeSelectItem();
        }

    }

    public void MoveItemSelected(Vector2 pos)
    {
        if (itemSelected == null)
        {
            return;
        }
        return;
        rayc = Camera.main.ScreenPointToRay(pos);

        RaycastHit hit;
        if (Physics.Raycast(rayc, out hit, 1000, layerMask))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "Ground")
                {
                    //tgMove.UpdatePosTG(hit.point);
                    Vector3 vtpos = hit.point;
                    //vtpos.y = itemSelected.gameObject.transform.position.y;
                    //itemSelected.gameObject.transform.position = vtpos;
                    itemSelected.UpdateTouchPos(hit.point);
                }

            }

        }
    }




    public void SwitchCamView()
    {
        Debug.Log("----switch camera view----");
        if (camPlayer.enabled)
        {
            camPlayer.enabled = false;

            // playerNode.GetComponent<PlayerViewCtrl>().DisableTouch();
            playerNode.SetActive(false);

            camTopDown.enabled = true;
            camTopDown.gameObject.SetActive(true);
        }
        else
        {
            DeSelectAllItem();
            playerNode.SetActive(true);
            camPlayer.enabled = true;
            // playerNode.GetComponent<PlayerViewCtrl>().WatingEnableTouch();
            camTopDown.gameObject.SetActive(false);
            camTopDown.enabled = false;

        }
    }
























    //public void testInsert()
    //{
    //    DepositListFarmByTest(3);
    //}
    //public void DepositListFarmByTest(int cf)
    //{
    //    List<LANDTYPE> listLand = new List<LANDTYPE>();

    //    for(int a = 0; a < cf; a++)
    //    {
    //        LANDTYPE landtype = LANDTYPE.LAND1;
    //        listLand.Add(landtype);

    //    }
    //    this.DepositListFarm(listLand);


    //}
    //public void DepositListFarm(List<LANDTYPE> list_land)
    //{
    //    return;
    //    int cf = list_land.Count;
    //    int idu = 0;
    //    for(int a = 0; a < this.arrFarm.Length; a++)
    //    {
    //        Farm3DController tmpFrm = this.arrFarm[a];
    //        if (tmpFrm.farmStt == FarmState.NOT_YET)
    //        {
    //            tmpFrm.UpdateLand(list_land[idu]);
    //            idu++;
    //        }
    //        if (idu >= cf)
    //        {
    //            break;
    //        }
    //    }

    //    LoadMapDataCtr.ins.SaveFrameData();
    //}
}

