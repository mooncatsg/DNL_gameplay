using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DinoGarden;
using System;
using System.Linq;
public class StoreUI : UIPopupBase
{
    [Header("STORE CHILD")]
    [SerializeField] FarmStoreUI farmStore;
    [SerializeField] StoreSeedsUI seedsStore;
    [SerializeField] StoreAnimalUI animalStore;
    [SerializeField] StoreDecorUI decorStore;
    [SerializeField] Tabbar tabbar;
    [SerializeField] StorePurchaseConfirm purchaseConfirm;
    [SerializeField] FarmYourCropUI guiCropsResult;

    public List<StoreItemInfo> listBuy = new List<StoreItemInfo>();

    public static StoreUI Instance;
    private bool _isShowed = false;
    private void Awake()
    {
        Instance = this;
        this.tabbar.acTabClick = this.OnTabbarClicked;
        this.farmStore.acBackCallback = this.OnBackCallback;
        this.guiCropsResult.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (this._isShowed)
        {
            this.tabbar.clearDataAndActive();
        }
    }

    public void SetDataWithTabbar(int index)
    {
        this.tabbar.setTabIndex(index);
    }

    public void OnTabbarClicked(int index)
    {
        this._isShowed = true;
        listBuy.Clear();
        this.farmStore.gameObject.SetActive(false);
        this.seedsStore.gameObject.SetActive(false);
        this.animalStore.gameObject.SetActive(false);
        this.decorStore.gameObject.SetActive(false);

        switch (index)
        {
            case 0:
                this.seedsStore.gameObject.SetActive(true);
                this.seedsStore.ResetPurchase();
                this.seedsStore.ShowContent();
                break;

            case 1:
                this.animalStore.gameObject.SetActive(true);
                this.animalStore.ResetPurchase();
                this.animalStore.ShowContent();
                break;

            case 2:
                this.decorStore.gameObject.SetActive(true);
                this.decorStore.ResetPurchase();
                this.decorStore.ShowContent();
                break;

            case 3:
                this.farmStore.gameObject.SetActive(true);
                this.farmStore.ResetData();
                this.farmStore.OnUpdateLayoutWhenActive();
                break;

            default:
                break;
        }

        //Reset data purchase when switch tab
        if (StorePurchaseController.Instance)
        {
            StorePurchaseController.Instance.ResetData();
        }
    }

    private void OnBackCallback(Action callback)
    {
        this.OnClose(null, callback);
    }

    public void ShowPurchaseConfirm(Dictionary<int, ItemPurchaseInfo> dataPurchase, int dnlCoin, int gCoin)
    {
        if (this.purchaseConfirm != null)
        {
            this.purchaseConfirm.gameObject.SetActive(true);
            this.purchaseConfirm.ShowStorePurchaseConfirm(dataPurchase, dnlCoin, gCoin);
        }
    }

    public void ShowCropsResultPurchase(int amountBox, List<BuyCrop> listBuyCropReturn)
    {
        List<int> listCropsType = new List<int>();
        for (int i = 0; i < listBuyCropReturn.Count; i++)
        {
            listCropsType.Add((int)listBuyCropReturn[i].Rarity);
        }

        this.guiCropsResult.gameObject.SetActive(true);
        MainMenuUI.Instance.listCropsPurchased.AddRange(listCropsType);
        this.guiCropsResult.ShowYourCrop(listCropsType);
    }

    public void ShowAlertPurchaseSuccess(int dnlCoin, int gCoin, List<BuyAll> listBuyReturn)
    {
        var alert = MainMenuUI.Instance.AddAlertMessageConfirm();
        var token = gCoin > 0 ? gCoin : dnlCoin;
        string content = String.Format("SUCCESSFUL SPEND {0} TOKENS TO BUY : ", token);
        for (int i = 0; i < listBuyReturn.Count; i++)
        {
            string lower = "";
            if(listBuyReturn[i].quantity > 1)
            {
                lower = "S";
                if (listBuyReturn[i].type == PlantType.tomato.ToString() || listBuyReturn[i].type == PlantType.potato.ToString() || listBuyReturn[i].type == PlantType.chili.ToString())
                {
                    lower = "ES";
                }
            }    
            if (i == 0)
                content += listBuyReturn[i].quantity + " " + listBuyReturn[i].type.ToUpper() + lower;
            else content += " , " + listBuyReturn[i].quantity + " " + listBuyReturn[i].type.ToUpper() + lower;
        }

        alert.showAlert("PURCHASE SUCCESSFUL", content);
        alert.SetRightCallback((amount) =>
        {
            Debug.Log("ALERT allback CONFIRM");
            this.CloseStore();
        });
    }

    public void ShowAlertPurchaseFail(string errMss = "")
    {
        var alert = MainMenuUI.Instance.AddAlertMessageConfirm();
        if (string.Equals(errMss, ""))
            alert.showAlert("PURCHASE FAILED", String.Format("THERE'S SOMETHING WRONG WITH YOUR ORDER, PLEASE TRY AGAIN."));
        else alert.showAlert("PURCHASE FAILED", errMss);
        alert.SetRightCallback((amount) =>
        {
            // Debug.Log("");
        });
    }

    public void CloseStore()
    {
        this.OnBtnCloseClicked(null);
    }

    public override void OnBtnCloseClicked(Button btn)
    {
        Debug.Log("==> Popup Base: OnBtnCloseClicked");
        this.ResetData();
        this.OnClose(btn, null);
    }

    void ResetData()
    {
        this.farmStore.ResetData();
        this.seedsStore.ResetData();
        this.animalStore.ResetData();
        this.decorStore.ResetData();
        StorePurchaseController.Instance.ResetData();
    }

    void ResetPurchase()
    {

    }

    public void AddBuyItemList(StoreItemInfo item)
    {
        listBuy.Add(item);
    }

    public void RemoveBuyItemList(StoreItemInfo item)
    {
        listBuy.Remove(item);
    }
}
