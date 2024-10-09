using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DinoExtensions;
using System;

public class StoreItemUI : MonoBehaviour
{
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtTime;
    [SerializeField] Text txtInfo;
    [SerializeField] Image icon;
    [SerializeField] Image priceBg;
    [SerializeField] Image currencyImage;
    [SerializeField] Text txtPrice;
    [SerializeField] UIAmountController amountController;
    [SerializeField] Sprite sprPrice0;
    [SerializeField] Sprite sprPrice1;

    public StoreItemInfo _itemInfo;

    private void Start()
    {
        this.amountController.acSetAmountCalback = this.OnAcSetAmountCallback;
    }

    public void SetData(StoreItemInfo info)
    {
        this._itemInfo = info;
        List<Sprite> listIcons = MainMenuUI.Instance.GetListStoreItemIcon(info.type);
        this.icon.sprite = listIcons.Find(x => x.name == info.name);
        //if (listIcons.Count > info.itemID - 1)
        //{
        //    this.icon.sprite = listIcons[info.itemID - 1];
        //}

        this.icon.SetNativeSize();
        this.txtTitle.text = info.name.ToUpper();

        TimeSpan t = TimeSpan.FromSeconds(info.harvestTime);
        if (t.Days > 0) 
        {
            this.txtTime.text = "FARMING TIME: " + string.Format("{0:D2}d:{1:D2}h:{2:D2}m", t.Days, t.Hours, t.Minutes); 
        }
        else {this.txtTime.text = "FARMING TIME: " + string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds); }

        this.amountController.SetStartAmountAndRate(0, this._itemInfo.rate);

        this.currencyImage.sprite = MainMenuUI.Instance.GetCurrencySprite((CURRENCY_TYPE)info.currencyType);
        if (info.type == STORE_ITEM_TYPE.seed)
        {
            if (Enum.TryParse(info.name, out PlantType myStatus) )
            {
                this.txtInfo.text = GameDefine.LIST_TREE_DESCRIPTION[((int)myStatus)-1];
            }    
        }
        else if (info.type == STORE_ITEM_TYPE.animal)
        {
            if (Enum.TryParse(info.name, out AnimalType myStatus))
            {
                this.txtInfo.text = GameDefine.LIST_ANIMAL_DESCRIPTION[((int)myStatus) - 1];
            }
        }

        //if(_itemInfo.type == STORE_ITEM_TYPE.animal)
        //{
        //    Cage cage = MapController.instance.currentlandDetail.cages.Find(x => x.type == _itemInfo.name);
        //    if(cage.status == "locked")
        //    {

        //    }    
        //}    
    }

    public void ResetData()
    {
        this.amountController.SetStartAmountAndRate(0, this._itemInfo.rate);
    }

    public void OnAcSetAmountCallback(int amount)
    {
        Debug.Log("OnAcSetAmountCallback = " + amount.ToString());
        if (amount > 0)
        {
            this.priceBg.sprite = sprPrice1;
            this.txtPrice.GetComponent<UnityEngine.UI.Outline>().enabled = true;
            this.txtPrice.color = new Color(159.0f / 255.0f, 63.0f / 255.0f, 59.0f / 255.0f);
        }
        else
        {
            amount = 0;
            this.priceBg.sprite = sprPrice0;
            this.txtPrice.GetComponent<UnityEngine.UI.Outline>().enabled = false;
            this.txtPrice.color = new Color(128.0f / 255.0f, 84.0f / 255.0f, 67.0f / 255.0f);
        }

        if (this._itemInfo != null)
        {
            StorePurchaseController.Instance.SetOrderItem(this._itemInfo, amount);

        }
    }
}
