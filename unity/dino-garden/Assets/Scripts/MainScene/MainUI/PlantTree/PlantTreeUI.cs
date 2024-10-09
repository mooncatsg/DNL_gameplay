using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ACTION_TYPE
{
    PLANT_TREE = 1,
    TAKE_CARE_TREE = 2,
    TAKE_CARE_ANIMAL = 3,
    HARVEST = 4,
    WITH_DRAW = 5,
}
public class PlantTreeUI : UIPopupBase
{
    [Header("CHILD")]
    [SerializeField] GameObject scrollContent;
    [SerializeField] ScrollRect scrollView;
    [SerializeField] GameObject plantSeedItemPrefab;
    [SerializeField] Image bgImage;
    [SerializeField] Image bgTimer;
    [SerializeField] public Text labelTimer;

    public ACTION_TYPE actionType = ACTION_TYPE.PLANT_TREE;

    public void Show(TRANSITION_TYPE showType = TRANSITION_TYPE.ZOOM, TRANSITION_TYPE hideType = TRANSITION_TYPE.ZOOM, Action callback = null)
    {
        // supere
        base.Show(showType, hideType, () =>
        {
            Debug.LogWarning("2");
            this.scrollView.movementType = ScrollRect.MovementType.Elastic;
            callback();
        });
    }

    public void SetData(ACTION_TYPE _actionType)
    {
        actionType = _actionType;

        foreach (Transform child in this.scrollContent.transform)
        {
            Destroy(child.gameObject);
        }
        //var listSeedInfo = LoadMapDataCtr.ins.GetListSeedInfo();

        if (actionType == ACTION_TYPE.PLANT_TREE)
        {
            bgTimer.gameObject.SetActive(false);

            // Load seed in store
            APIMng.Instance.APIItemList((isSuccess, seedInWarehouse, errMsg, code) =>
            {
                Debug.LogError("APIItemList : " + ((List<WarehouseItem>)seedInWarehouse).Count);
                List<WarehouseItem> listSeedInfo = ((List<WarehouseItem>)seedInWarehouse).FindAll(x => x.classification == "seed");
                int count = 0;

                if (listSeedInfo.Count > 0)
                {
                    for (int i = 0; i < listSeedInfo.Count; i++)
                    {
                        if (listSeedInfo[i].Quantity > 0)
                        {
                            count++;
                            var seed_item = Instantiate(plantSeedItemPrefab, this.scrollContent.transform);
                            var comp = seed_item.GetComponent<PlantSeedItemUI>();
                            comp.SetInfo(listSeedInfo[i], "");
                            comp.acOnSelectClicked = this.OnSeedSelectClicked;
                        }
                    }
                }

                bgImage.rectTransform.sizeDelta = new Vector2(120f + count * 120f, 120f);

            });
        }
        else if (actionType == ACTION_TYPE.TAKE_CARE_TREE)
        {
            Farm3DController farmChoosing = MapController.instance.itemChoosing.GetComponent<Farm3DController>();
            if (farmChoosing != null)
            {
                bgTimer.gameObject.SetActive(true);
                bgTimer.rectTransform.position = Camera.main.WorldToScreenPoint(farmChoosing.transform.position);
                bgTimer.rectTransform.position = new Vector3(bgTimer.rectTransform.position.x, bgTimer.rectTransform.position.y + 35f, bgTimer.rectTransform.position.z);
                labelTimer.text = "" + farmChoosing.GetFarmTimer();
            }
            int count = 2;
            // Fertilizer
            {
                var seed_item = Instantiate(plantSeedItemPrefab, this.scrollContent.transform);
                var comp = seed_item.GetComponent<PlantSeedItemUI>();
                comp.SetInfo(null, "fertilizer");
                comp.acOnSelectClicked = this.OnSeedSelectClicked;
            }

            // Drop
            {
                var seed_item = Instantiate(plantSeedItemPrefab, this.scrollContent.transform);
                var comp = seed_item.GetComponent<PlantSeedItemUI>();
                comp.SetInfo(null, "drop");
                comp.acOnSelectClicked = this.OnSeedSelectClicked;
            }

            // Havest
            if (farmChoosing.CanHavest())
            {
                count++;
                var seed_item = Instantiate(plantSeedItemPrefab, this.scrollContent.transform);
                var comp = seed_item.GetComponent<PlantSeedItemUI>();
                comp.SetInfo(null, "havest");
                comp.acOnSelectClicked = this.OnSeedSelectClicked;
            }

            bgImage.rectTransform.sizeDelta = new Vector2(120f + count * 120f, 120f);

        }

        this.scrollView.movementType = ScrollRect.MovementType.Unrestricted;
    }

    public void OnSeedSelectClicked(WarehouseItem info, string actionStr)
    {
        this.OnClose(null, () =>
        {
            ItemPlaceController itemChoosing = MapController.instance.itemChoosing;

            if (itemChoosing != null && itemChoosing.GetComponent<Farm3DController>() != null)
            {
                int idx = MapController.instance.currentlandDetail.crops.FindIndex(x => x.id == itemChoosing.GetComponent<Farm3DController>().cropData.id);
                PlantType seedType = Enum.TryParse<PlantType>(info.type, true, out seedType) ? seedType : PlantType.none;
                APIMng.Instance.APIPlantSeeding(info.id, itemChoosing.GetComponent<Farm3DController>().cropData.id, seedType, (isSuccess2, plantSeeding, errMsg2, code2) =>
                {
                    if(isSuccess2 )
                    {
                        MapController.instance.currentlandDetail.crops[idx].plant = (Plant)plantSeeding;
                        itemChoosing.GetComponent<Farm3DController>().ActionSeed((Plant)plantSeeding);
                    }    
                    else
                    {
                        MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                    }    
                });
            }
        });
    }

}
