using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DinoExtensions;
using System;
public class StorePurchaseConfirmItem : MonoBehaviour
{
    [SerializeField] Text txtAmount;
    [SerializeField] Text txtPrice;
    [SerializeField] Image itemIcon;
    [SerializeField] Image coinIcon;

    [SerializeField] Sprite sprGCoin;
    [SerializeField] Sprite sprDnlCoin;

    public void SetData(ItemPurchaseInfo itemInfo)
    {
        this.txtAmount.text = String.Format("x{0}", itemInfo.amount.FormatMoney());
        this.txtPrice.text = (itemInfo.amount * itemInfo.rate).FormatMoney();
        if (itemInfo.currencyType == (int)CURRENCY_TYPE.WDNL)
        {
            this.coinIcon.sprite = sprDnlCoin;
        }
        else
        {
            this.coinIcon.sprite = sprGCoin;
        }
        Sprite spr = MainMenuUI.Instance.GetListStoreItemIcon(itemInfo.type).Find(x => x.name == itemInfo.name);
        if(spr != null)
        {
            itemIcon.sprite = spr;
            itemIcon.SetNativeSize();
        }

        //var listIcons = MainMenuUI.Instance.GetListStoreItemIcon(itemInfo.type);
        //if (listIcons.Count > itemInfo.itemID - 1)
        //{
        //    this.itemIcon.sprite = listIcons[itemInfo.itemID - 1];
        //    this.itemIcon.SetNativeSize();
        //}
    }
}
