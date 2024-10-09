using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoGarden;
using DinoExtensions;
using UnityEngine.UI;
using System;
public class StorageSellUI : UIPopupBase
{
    [SerializeField] Button btnMinus;
    [SerializeField] Button btnPlus;
    [SerializeField] Button btnMax;
    [SerializeField] Text txtAmount;
    [SerializeField] Text txtPrice;
    [SerializeField] Text txtName;
    [SerializeField] Image iconItem;
    [SerializeField] GameObject blockInput;
    [SerializeField] GameObject contentBg;

    float _amount = 1;
    StorageInfo _info;

    public void ShowSellPopup(StorageInfo info, Vector3 point)
    {
        this._amount = 1;
        this._info = info;
        this.updateAmountAndPrice();

        this.blockInput.SetActive(false);
        this.background.transform.position = point;
        var local_point = this.background.transform.localPosition;
        point = new Vector3(point.x, point.y, 0);
        // Debug.Log("Point = " + local_point);
        if (local_point.x < 0)
        {
            this.contentBg.transform.SetLocalScaleX(1);
            local_point.y = local_point.y + 145;
            local_point.x = local_point.x + 110;
        }
        else
        {
            this.contentBg.transform.SetLocalScaleX(-1);
            local_point.y = local_point.y + 145;
            local_point.x = local_point.x - 110;
        }
        this.background.transform.localPosition = local_point;
        Sprite spr = MainMenuUI.Instance.GetListStoreItemIcon(info.type).Find(x => x.name == info.name);

        if (spr != null)
        {
            this.iconItem.sprite = spr;
            this.iconItem.SetNativeSize();
        }

        this.txtName.text = info.name.ToUpper();

        this.Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, () =>
        {
            this.blockInput.SetActive(true);
        });
    }

    public void OnPlus()
    {
        if (this._amount >= this._info.Quantity)
            return;

        this._amount++;
        this.updateAmountAndPrice();
    }

    public void OnMinus()
    {
        if (this._amount <= 1)
            return;

        this._amount--;
        this.updateAmountAndPrice();
    }

    public void OnMax()
    {
        if (this._amount >= this._info.Quantity)
            return;

        this._amount = this._info.Quantity;
        this.updateAmountAndPrice();
    }

    void updateAmountAndPrice()
    {
        this.txtAmount.text = System.String.Format("x{0}", this._amount);
        this.txtPrice.text = (this._amount * this._info.rate).FormatMoney();

        this.btnMinus.interactable = this._amount > 1;
        this.btnPlus.interactable = this._amount < this._info.Quantity;
        this.btnMax.interactable = this._amount < this._info.Quantity;
    }

    public void OnSell()
    {
        //TODO: BienVT ==> SELL : this._amount
        Debug.LogError("TODO: BienVT ==> OnSell ==> " + this._amount.FormatMoney() + "  |  Price = " + (this._amount * this._info.rate).FormatMoney());
        OnBtnCloseClicked(null);
    }

    public override void OnBtnCloseClicked(Button btn)
    {
        this.blockInput.SetActive(false);
        base.OnBtnCloseClicked(btn);
    }
}
