using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class BuildingCropsUI : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject buildingCropItemPrefab;
    [SerializeField] GameObject guiBtnSelect;
    [SerializeField] GameObject guiBtnDeposit;
    [SerializeField] Button selectBtn;

    private List<BuildingCropsItemUI> listCropItems = new List<BuildingCropsItemUI>();
    private BuildingCropsItemUI selectedCropItems = null;
    public Action<ACTION_TYPE, List<int>> acCallback;

    public enum ACTION_TYPE
    {
        DEPOSIT,
        PURCHASE_CROP,
        BACK
    }

    private int _currentAmount = 0;

    public void ShowBuildingCrops()
    {
        this.guiBtnSelect.SetActive(true);
        this.guiBtnDeposit.SetActive(false);

        this.DestroyAllCropsItem();
        this.listCropItems.Clear();

        APIMng.Instance.APICropList((isSuccess, cropDetail, errMsg, code) =>
        {
            if(isSuccess)
            {
                List<Crop> listCropsInStore = ((List<Crop>)cropDetail).FindAll(x => x.Status == CropStatus.store);
                Debug.LogError("LIST CROPS IN STORE : " + listCropsInStore.Count);
                for (int i = 0; i < listCropsInStore.Count; i++)
                {
                    this.AddCropsItem(i, (int)listCropsInStore[i].Rarity, listCropsInStore[i].id);
                }
                selectBtn.interactable = listCropsInStore.Count > 0;
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
            }
        });


    }

    public void OnSelectCropItems(BuildingCropsItemUI item)
    {
        if(selectedCropItems != null) selectedCropItems.DeselectedStatus();
        selectedCropItems = item;
    }

    void AddCropsItem(int index, int cropsType,int _cropId)
    {
        var crop_item = Instantiate(buildingCropItemPrefab, this.content.transform);
        var comp = crop_item.GetComponent<BuildingCropsItemUI>();
        comp.SetData(index, cropsType, _cropId, OnSelectCropItems);
        this.listCropItems.Add(comp);
    }

    void DestroyAllCropsItem()
    {
        foreach (Transform child in this.content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnBtnSelectClicked()
    {
        this.guiBtnSelect.SetActive(false);
        this.guiBtnDeposit.SetActive(true);

        for (int i = 0; i < this.listCropItems.Count; i++)
        {
            this.listCropItems[i].SetSelectEnable(true);
        }
    }

    public void OnBtnPurchaseCropClicked()
    {
        if (this.acCallback != null)
        {
            this.acCallback(ACTION_TYPE.PURCHASE_CROP, null);
        }
    }

    public void OnBtnDepositClicked()
    {
        if (this.acCallback != null)
        {
            List<int> listCropsDeposit = new List<int>();
            List<int> listCropsRest = new List<int>();
            for (int i = 0; i < this.listCropItems.Count; i++)
            {
                var cropsItem = this.listCropItems[i];
                if (cropsItem.isSelect())
                {
                    listCropsDeposit.Add(cropsItem.cropId);
                }
                else
                {
                    listCropsRest.Add(cropsItem.cropId);
                }
            }

            Debug.LogError("OnBtnDepositClicked 1");
            //if(listCropsDeposit != null && listCropsDeposit.Count >= 1)
            if(selectedCropItems != null)
            {

                List<int> position = MapController.instance.FreeListPositionInMap();
                APIMng.Instance.APICropDeposit(selectedCropItems.cropId, position[0], (isSuccess2, cropDeposit, errMsg2, code2) =>
                {
                    if(isSuccess2)
                    {
                        MapController.instance.UpdateNewCrops((Crop)cropDeposit);

                        MainMenuUI.Instance.listCropsPurchased = listCropsRest;

                        this.acCallback(ACTION_TYPE.DEPOSIT, listCropsDeposit);
                    }
                    else
                    {
                        MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                    }
                });
            }
        }
    }

    public void OnBtnDepositAllClicked()
    {
        //if (this.acCallback != null)
        //{
        //    List<int> listCropsDeposit = new List<int>();
        //    for (int i = 0; i < this.listCropItems.Count; i++)
        //    {
        //        listCropsDeposit.Add(this.listCropItems[i].cropsType);

        //    }
        //    MainMenuUI.Instance.listCropsPurchased.Clear();
        //    this.acCallback(ACTION_TYPE.DEPOSIT, listCropsDeposit);
        //}
    }

    public void OnBtnBackClicked()
    {
        if (this.acCallback != null)
        {
            this.acCallback(ACTION_TYPE.BACK, null);
        }
    }
}
