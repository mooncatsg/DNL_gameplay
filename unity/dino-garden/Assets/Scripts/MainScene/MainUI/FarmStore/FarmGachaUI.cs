using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DinoExtensions;
public class FarmGachaUI : MonoBehaviour
{
    public Action<ACTION_TYPE, int> acCallback;
    [SerializeField] UIAmountController amountController;
    [SerializeField] Text txtQuickPrice;
    [SerializeField] Image textQuickImg;
    private bool isShowed = false;
    private int rate;
    public enum ACTION_TYPE
    {
        PURCHASE,
        BACK
    }

    private void Start()
    {
        this.amountController.acSetAmountCalback = this.OnAcSetAmountCallback;

        if (MapController.instance.listShopAll.Find(x => x.classification == "crop") != null)
        {
            txtQuickPrice.text = MapController.instance.listShopAll.Find(x => x.classification == "crop").price.ToString();
            textQuickImg.sprite = MainMenuUI.Instance.GetCurrencySprite((CURRENCY_TYPE)MapController.instance.listShopAll.Find(x => x.classification == "crop").priceType);
        }
    }

    public void ShowGacha()
    {
        if (this.isShowed) return;
        this.isShowed = true;
        this.amountController.SetStartAmountAndRate(0, rate);
    }

    public void OnBtnPurchaseClicked()
    {
        this.acCallback(ACTION_TYPE.PURCHASE, 1);

        // var alert = MainMenuUI.Instance.AddAlertFarmPurchase();
        // float rate = 500;  
        // alert.showAlert(1, rate);
        // alert.SetLeftCallback((amount) =>
        // { 
        //     Debug.Log("LeftCallback = " + amount.ToString());
        // });

        // alert.SetRightCallback((amount) =>
        // {
        //     Debug.Log("RightCallback = " + amount.ToString());
        //     if (this.acCallback != null)
        //     {
        //         this.acCallback(ACTION_TYPE.PURCHASE, 3 * amount);
        //     }
        // });
    }

    public void OnAcSetAmountCallback(int amount)
    {
        Debug.Log("OnAcSetAmountCallback = " + amount.ToString());
        if (amount < 0) 
            amount = 0;
        if(MapController.instance.listShopAll.Find(x => x.classification == "crop") != null)
        {
            int price = MapController.instance.listShopAll.Find(x => x.classification == "crop").price;
            int priceType = MapController.instance.listShopAll.Find(x => x.classification == "crop").priceType;
            StorePurchaseController.Instance.SetOrderItem(new StoreItemInfo(STORE_ITEM_TYPE.crop_box, 1, "gacha", price, priceType,0), amount);
        }
    }

    public void OnBtnBackClicked()
    {
        if (this.acCallback != null)
        {
            this.acCallback(ACTION_TYPE.BACK, 0);
        }
    }

    public void ResetData()
    {
        this.amountController.SetStartAmountAndRate(0, rate);
    }
}
