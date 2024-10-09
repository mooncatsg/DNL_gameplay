using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DinoExtensions;
using System;
public class ActionUI : UIPopupBase
{
    [SerializeField] RectTransform scrollview;
    [SerializeField] RectTransform content;
    [SerializeField] List<Sprite> listIconTools;
    [SerializeField] List<Sprite> listAnimalTools;
    [SerializeField] GameObject acItemPrefab;
    [SerializeField] GameObject blockInput;
    [SerializeField] Image guiActionTimer;
    [SerializeField] Text guiActionTimerText;
    [SerializeField] Image timerSlider;
    [SerializeField] Text itemPlantSelectedName;
    public ACTION_UI_TYPE actionType = ACTION_UI_TYPE.SPAWN_TREE;
    int MAX_W = 824;
    int MIN_W = 380;
    float item_w;

    private void Update()
    {
        if (actionType == ACTION_UI_TYPE.TAKE_CARE_PLANT)
        {
            if (MapController.instance.itemChoosing != null && MapController.instance.itemChoosing.GetComponent<Farm3DController>() != null && MapController.instance.itemChoosing.GetComponent<Farm3DController>().IsHavePlant())
            {                
                int time = MapController.instance.itemChoosing.GetComponent<Farm3DController>().GetFarmTimer();
                if (time > 0)
                {
                    guiActionTimer.gameObject.SetActive(true);
                    //guiActionTimer.rectTransform.position = Camera.main.WorldToScreenPoint(MapController.instance.itemChoosing.transform.position);
                    //guiActionTimer.rectTransform.position = new Vector3(guiActionTimer.rectTransform.position.x, guiActionTimer.rectTransform.position.y + 50f, guiActionTimer.rectTransform.position.z);
                    timerSlider.fillAmount = MapController.instance.itemChoosing.GetComponent<Farm3DController>().SliderCountdown();

                    TimeSpan t = TimeSpan.FromSeconds(time);
                    guiActionTimerText.text = "" + string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
                }
                else
                {
                    guiActionTimer.gameObject.SetActive(false);

                    guiActionTimerText.text = "Need Harvest";
                }
            }
        }
        else
        {
            guiActionTimer.gameObject.SetActive(false);
            itemPlantSelectedName.gameObject.SetActive(false);
        }
    }
    public void ShowActionByType(ACTION_UI_TYPE acType)
    {
        actionType = acType;
        this.content.gameObject.DestroyAllChild();
        this.background.SetActive(false);
        List<ActionUIInfo> listActionInfo = new List<ActionUIInfo>();
        List<Sprite> listIcons = new List<Sprite>();
        switch (acType)
        {
            case ACTION_UI_TYPE.SPAWN_TREE:
                {
                    itemPlantSelectedName.gameObject.SetActive(false);

                    APIMng.Instance.APIItemList((isSuccess, seedInWarehouse, errMsg, code) =>
                    {
                        if(isSuccess)
                        {
                            Debug.LogError("ActionUI ==> APIItemList : " + ((List<WarehouseItem>)seedInWarehouse).Count);
                            List<WarehouseItem> listSeedInfo = ((List<WarehouseItem>)seedInWarehouse).FindAll(x => x.classification == "seed");
                            if (listSeedInfo.Count > 0)
                            {
                                int seedHave = 0;
                                for (int i = 0; i < listSeedInfo.Count; i++)
                                {
                                    var info = listSeedInfo[i];

                                    if (info.Quantity > 0)
                                    {
                                        seedHave++;
                                        PlantType seedType = Enum.TryParse<PlantType>(info.type, true, out seedType) ? seedType : PlantType.none;
                                        Debug.Log("ActionUI ==> " + info.id.ToString() + " | " + info.type.ToString() + " | " + info.Quantity.ToString() + " | " + ((int)seedType).ToString());
                                        listActionInfo.Add(new ActionUIInfo((int)acType, (int)seedType, info));
                                    }
                                }

                                listActionInfo.Add(new ActionUIInfo((int)ACTION_UI_TYPE.WITH_DRAW, (int)TakeCarePlantType.withdraw, null));
                                this.SetData(listActionInfo, listIcons);
                            }
                        }
                        else
                        {
                            listActionInfo.Add(new ActionUIInfo((int)ACTION_UI_TYPE.WITH_DRAW, (int)TakeCarePlantType.withdraw, null));
                            this.SetData(listActionInfo, listIcons);
                        }
                    });

                    
                }
                break;

            case ACTION_UI_TYPE.TAKE_CARE_PLANT:
                {
                    Farm3DController farmChoosing = MapController.instance.itemChoosing.GetComponent<Farm3DController>();
                    if (farmChoosing != null)
                    {
                        itemPlantSelectedName.gameObject.SetActive(true);
                        itemPlantSelectedName.text = farmChoosing.cropData.plant.type.ToUpper();

                        if (farmChoosing.CanHavest() )//&& !MapController.instance.IsDinoCaring(StakingClassification.plant, StakingType.garden))
                        {
                            listActionInfo.Add(new ActionUIInfo((int)acType, (int)TakeCarePlantType.havest, null));
                        }
                        else
                        {
                            if (farmChoosing.NeedToCatchWorm() && !MapController.instance.IsDinoCaring(StakingClassification.plant, StakingType.garden))
                                listActionInfo.Add(new ActionUIInfo((int)acType, (int)TakeCarePlantType.worm, null));

                            if (farmChoosing.NeedToWatering() && !MapController.instance.IsDinoCaring(StakingClassification.plant, StakingType.garden))
                                listActionInfo.Add(new ActionUIInfo((int)acType, (int)TakeCarePlantType.watering, null));
                        }
                        listActionInfo.Add(new ActionUIInfo((int)acType, (int)TakeCarePlantType.drop, null));                        
                        listIcons = listIconTools;
                        this.SetData(listActionInfo, listIcons);
                    }
                }
                break;

            case ACTION_UI_TYPE.TAKE_CARE_ANIMAL:
                {
                    AnimalController animalController = MapController.instance.itemChoosing.GetComponent<AnimalController>();
                    if (animalController.NeedToFeed())
                        listActionInfo.Add(new ActionUIInfo((int)acType, (int)TakeCareAnimalType.feeding, null));
                    if (animalController.NeedToHavest())
                        listActionInfo.Add(new ActionUIInfo((int)acType, (int)TakeCareAnimalType.havest, null));
                    listIcons = listAnimalTools;
                    this.SetData(listActionInfo, listIcons);
                }
                break;

            case ACTION_UI_TYPE.HARVEST:
                break;

            case ACTION_UI_TYPE.WITH_DRAW:
                break;
        }


    }

    void SetData(List<ActionUIInfo> listActionInfo, List<Sprite> _listIcons)
    {
        Debug.LogError(listActionInfo.Count + " - "+ _listIcons.Count);

        var padding = this.content.GetComponent<HorizontalLayoutGroup>().padding;
        var space = this.content.GetComponent<HorizontalLayoutGroup>().spacing;
        padding.left = 50;

        int totalItem = listActionInfo.Count;
        for (int i = 0; i < totalItem; i++)
        {
            if (listActionInfo[i].AcType == ACTION_UI_TYPE.SPAWN_TREE)
            {
                this.AddActionItemUI(listActionInfo[i], MainMenuUI.Instance.GetListStoreItemIcon(STORE_ITEM_TYPE.seed)[listActionInfo[i].acId - 1]);
            }
            else
            {
                this.AddActionItemUI(listActionInfo[i], listIconTools[listActionInfo[i].acId - 1]);
            }
        }

        float width = totalItem * item_w + (totalItem - 1) * space + padding.left + padding.right;
        Debug.Log("width = " + width.ToString());

        var size_width = Mathf.Min(width, MAX_W);
        size_width = Mathf.Max(size_width, MIN_W);
        Debug.Log("size_width = " + size_width.ToString());

        var size = new Vector2(size_width, this.scrollview.sizeDelta.y);
        this.scrollview.sizeDelta = size;

        if (width < MIN_W)
        {
            padding.left = padding.left + (MIN_W - (int)width) / 2;
        }

        this.content.GetComponent<HorizontalLayoutGroup>().padding = padding;

        this.content.ForceUpdateRectTransforms();
        this.scrollview.gameObject.ForceRebuildLayoutImmediate();

        this.blockInput.SetActive(false);
        this.background.SetActive(true);
        this.Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, () =>
        {
            this.blockInput.SetActive(true);
        });
    }

    void AddActionItemUI(ActionUIInfo info, Sprite spIcon)
    {
        var item = Instantiate(this.acItemPrefab, this.content.transform);
        var comp = item.GetComponent<ActionItemUI>();
        comp.SetData(info, spIcon);
        comp.acItemClicked = this.OnItemClicked;
        item_w = item.GetComponent<RectTransform>().sizeDelta.x;
    }

    void OnItemClicked(ActionUIInfo info)
    {
        Debug.Log("Action ==> OnItemClicked | Type = " + info.acType.ToString() + " | Index = " + info.acId.ToString());
        float price = 0; int typeCurrency = 0;
        switch ((ACTION_UI_TYPE)info.acType)
        {
            case ACTION_UI_TYPE.SPAWN_TREE:
                //TODO: click seeds
                {
                    Debug.LogError("ActionUI ==> CLICK ==> " + info.acId.ToString());
                    if (MapController.instance.itemChoosing == null)
                        return;
                    Farm3DController farmChoosing = MapController.instance.itemChoosing.GetComponent<Farm3DController>();

                    if (farmChoosing != null)
                    {
                        int idx = MapController.instance.currentlandDetail.crops.FindIndex(x => x.id == farmChoosing.cropData.id);
                        PlantType seedType = Enum.TryParse<PlantType>(info.itemInfo.type, true, out seedType) ? seedType : PlantType.none;
                        APIMng.Instance.APIPlantSeeding(info.itemInfo.id, farmChoosing.cropData.id, seedType, (isSuccess2, plantSeeding, errMsg2, code2) =>
                        {
                            if(isSuccess2)
                            {
                                //MapController.instance.currentlandDetail.crops[idx].plant = (Plant)plantSeeding;
                                MapController.instance.UpdateLandetail(()=> { 
                                    farmChoosing.ActionSeed((Plant)plantSeeding);
                                });
                            }
                            else
                            {
                                MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                            }
                        });
                    }
                }
                break;

            case ACTION_UI_TYPE.TAKE_CARE_PLANT:
                List<Crop> listCrop = MapController.instance.getCrop();
                switch ((TakeCarePlantType)info.acId)
                {
                    case TakeCarePlantType.watering:
                        //TODO: watering
                        {
                            ItemPlaceController itemChoosing = MapController.instance.itemChoosing;
                            if (itemChoosing != null && itemChoosing.GetComponent<Farm3DController>() != null)
                            {
                                for (int i = 0; i < listCrop.Count; i++)
                                    if (listCrop[i].id == itemChoosing.GetComponent<Farm3DController>().cropData.id)
                                    {
                                        List<fixEventPrice> fixEventPrices = listCrop[i].plant.fixEventPrice;
                                        for (int j = 0; j < fixEventPrices.Count; j++)
                                        {
                                            Debug.Log(fixEventPrices[j].type);
                                            if (fixEventPrices[j].type == "arid")
                                            {
                                                price = fixEventPrices[j].amount;
                                                typeCurrency = fixEventPrices[j].typeCurrency;
                                            }
                                        }
                                    }
                                MainMenuUI.Instance.ShowAlertMessage("DO YOU WANT TO WATERING WITH "+ price+ (typeCurrency == 2 ? " W-DNG" : " W-DNL"), "WATERING",true,null,()=> {

                                    APIMng.Instance.APIPlantWatering(itemChoosing.GetComponent<Farm3DController>().cropData.plant.id, (isSuccess2, _plantWatering, errMsg2, code2) =>
                                    {
                                        if (isSuccess2)
                                        {
                                            PlantWatering plantWatering = (PlantWatering)_plantWatering;
                                            if (plantWatering != null)
                                            {
                                                MainMenuUI.Instance.ShowAlertMessage("WATERING YOUR PLANT SUCCESSFUL", "SUCCESSFUL");
                                                MapController.instance.UpdateCropData();
                                                MapController.instance.UpdateUserInfo();
                                            }
                                        }
                                        else
                                        {
                                            MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                                        }
                                    });
                                });
                            }
                        }
                        break;
                    case TakeCarePlantType.worm:
                        //TODO: worm
                        {
                            ItemPlaceController itemChoosing = MapController.instance.itemChoosing;
                            if (MapController.instance.itemChoosing != null && MapController.instance.itemChoosing.GetComponent<Farm3DController>() != null)
                            {
                                for (int i = 0; i < listCrop.Count; i++)
                                    if (listCrop[i].id == itemChoosing.GetComponent<Farm3DController>().cropData.id)
                                    {
                                        List<fixEventPrice> fixEventPrices = listCrop[i].plant.fixEventPrice;
                                        for (int j = 0; j < fixEventPrices.Count; j++)
                                            if (fixEventPrices[j].type == "worm")
                                            {
                                                price = fixEventPrices[j].amount;
                                                typeCurrency = fixEventPrices[j].typeCurrency;
                                            }
                                    }
                                MainMenuUI.Instance.ShowAlertMessage("DO YOU WANT TO CATCH WORM WITH" + price + (typeCurrency == 2 ? " W-DNG" : " W-DNL"), "CATCH WORM", true, null, () =>
                                {
                                    APIMng.Instance.APIPlantKillBug(MapController.instance.itemChoosing.GetComponent<Farm3DController>().cropData.plant.id, (isSuccess2, _plantWorm, errMsg2, code2) =>
                                    {
                                        if (isSuccess2)
                                        {
                                            PlantWatering plantWorm = (PlantWatering)_plantWorm;
                                            if (plantWorm != null)
                                            {
                                                MainMenuUI.Instance.ShowAlertMessage("CATCH WORM SUCCESSFUL", "SUCCESSFUL");
                                                MapController.instance.UpdateCropData();
                                                MapController.instance.UpdateUserInfo();
                                            }
                                        }
                                        else
                                        {
                                            MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                                        }
                                    });
                                });                                
                            }
                        }
                        break;
                    case TakeCarePlantType.fertilizer:
                        //TODO: fertilizer
                        {
                            //if (MapController.instance.itemChoosing != null && MapController.instance.itemChoosing.GetComponent<Farm3DController>() != null)
                            //{
                            //    APIMng.Instance.APIPlantFertilize(MapController.instance.itemChoosing.GetComponent<Farm3DController>().cropData.plant.id,1, (isSuccess2, _plantFertilize, errMsg2, code2) =>
                            //    {
                            //        PlantFertilize plantFertilize = (PlantFertilize)_plantFertilize;
                            //        if(plantFertilize != null)
                            //        {
                            //            var alert = MainMenuUI.Instance.AddAlertMessageConfirm();
                            //            alert.showAlert(" SUCCESSFUL", String.Format("YOUR SUCCESSFUL FERTILIZE YOUR PLANT"));
                            //            alert.SetRightCallback((amount) =>
                            //            {
                            //            });
                            //        }
                            //    });
                            //}
                        }
                        break;

                    case TakeCarePlantType.drop:
                        //TODO: drop
                        {
                            Debug.LogError("PLANT ==> drop");
                            if (MapController.instance.itemChoosing == null)
                                return;

                            MainMenuUI.Instance.ShowAlertMessage("ARE YOU SURE DROP PLANT ?", "DROP PLANT",true, null, () => 
                            {
                                Farm3DController farmChoosing = MapController.instance.itemChoosing.GetComponent<Farm3DController>();
                                if (farmChoosing != null && farmChoosing.cropData.plant != null)
                                {
                                    APIMng.Instance.APIPlantRemove(farmChoosing.cropData.plant.id, (isSuccess2, plantHavest, errMsg2, code2) =>
                                    {
                                        if (isSuccess2)
                                        {
                                            farmChoosing.DropFarm();
                                            MainMenuUI.Instance.ShowAlertMessage("SUCCESS DROP PLANT !!!");
                                        }
                                        else
                                        {
                                            MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                                        }
                                    });
                                }
                            });
                            
                        }
                        break;

                    case TakeCarePlantType.havest:
                        //TODO: havest
                        {
                            Debug.LogError("PLANT ==> havest");
                            ItemPlaceController itemChoosing = MapController.instance.itemChoosing;
                            if (itemChoosing != null && itemChoosing.GetComponent<Farm3DController>() != null)
                            {
                                int idx = MapController.instance.currentlandDetail.crops.FindIndex(x => x.id == itemChoosing.GetComponent<Farm3DController>().cropData.id);
                                APIMng.Instance.APIPlantHarvest(itemChoosing.GetComponent<Farm3DController>().cropData.plant.id, (isSuccess2, plantHavest, errMsg2, code2) =>
                                {
                                    if(isSuccess2)
                                    {
                                        MapController.instance.currentlandDetail.crops[idx].plant = null;
                                        itemChoosing.GetComponent<Farm3DController>().HavestFarm();
                                        MainMenuUI.Instance.ShowAlertMessage("FINISH HARVEST : " + ((WarehouseItem)plantHavest).type.ToUpper() + "\nQUANTITY : " + ((WarehouseItem)plantHavest).harvest);
                                    }
                                    else
                                    {
                                        MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                                    }
                                });
                            }
                        }
                        break;
                }
                break;

            case ACTION_UI_TYPE.TAKE_CARE_ANIMAL:
                if (MapController.instance.itemChoosing == null || MapController.instance.itemChoosing.GetComponent<AnimalController>() == null)
                    return;
                AnimalController animalChoosing = MapController.instance.itemChoosing.GetComponent<AnimalController>();

                switch ((TakeCareAnimalType)info.acId)
                {
                    case TakeCareAnimalType.feeding:
                        //TODO: feeding
                        {
                            Debug.LogError("ANIMAL ==> feeding");
                            APIMng.Instance.APIAnimalFeed(animalChoosing.animalData.id, (isSuccess2, animalFeeding, errMsg2, code2) =>
                            {
                                if (isSuccess2)
                                {
                                    MapController.instance.UpdateNewAnimal();
                                    MapController.instance.UpdateUserInfo();
                                }
                                else
                                    MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                            });
                        }
                        break;

                    case TakeCareAnimalType.havest:
                        //TODO: havest
                        {
                            Debug.LogError("ANIMAL ==> havest");
                            APIMng.Instance.APIAnimalHarvest(animalChoosing.animalData.id, (isSuccess2, animalHarvest, errMsg2, code2) =>
                            {
                                if (isSuccess2)
                                {
                                    MainMenuUI.Instance.ShowAlertMessage("Success Harvest.\nHarvest Count : " + (animalChoosing.animalData.havestCnt + 1));
                                    MapController.instance.UpdateNewAnimal();
                                }
                                else
                                    MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                            });
                        }
                        break;
                }
                break;

            case ACTION_UI_TYPE.WITH_DRAW:
                {
                    Debug.LogError("PLANT ==> withdraw");
                    if (MapController.instance.itemChoosing == null)
                        return;

                    MainMenuUI.Instance.ShowAlertMessage("ARE YOU SURE WITH DRAW PLANT ?", "WITH DRAW", true, null, () =>
                    {
                        Farm3DController farmChoosing = MapController.instance.itemChoosing.GetComponent<Farm3DController>();
                        if (farmChoosing != null && farmChoosing.cropData != null)
                        {
                            APIMng.Instance.APICropWithdraw(farmChoosing.cropData.id, (isSuccess2, crop, errMsg2, code2) =>
                            {
                                if (isSuccess2)
                                {
                                    MainMenuUI.Instance.ShowAlertMessage("SUCCESS WITH DRAW !!!");
                                    farmChoosing.WithdrawCrop();
                                }
                                else
                                {
                                    MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                                }
                            });
                        }
                    });
                }
                break;

        }


        this.OnBtnCloseClicked(null);
    }

    public override void OnBtnCloseClicked(Button btn)
    {
        this.blockInput.SetActive(false);
        base.OnBtnCloseClicked(btn);
    }
}


public enum ACTION_UI_TYPE
{
    SPAWN_TREE = 1,
    TAKE_CARE_PLANT = 2,
    TAKE_CARE_ANIMAL = 3,
    HARVEST = 4,
    WITH_DRAW = 5
}

public enum TakeCarePlantType
{
    none, fertilizer, drop, havest, watering, worm,withdraw
}

public enum TakeCareAnimalType
{
    none, feeding, havest
}