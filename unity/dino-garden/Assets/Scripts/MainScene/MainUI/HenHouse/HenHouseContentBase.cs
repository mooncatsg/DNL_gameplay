using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DinoExtensions;
using System;
public class HenHouseContentBase : MonoBehaviour
{
    [SerializeField] Button btnConfirm;
    [SerializeField] Text txtPrice;
    [SerializeField] Image currencyIcon;
    [SerializeField] GameObject material;
    Action<int> _acConfirmCallback;
    int _price = 0;

    public void Show(int price,CURRENCY_TYPE currencyType, bool isEnable, Action<int> acConfirmCallback)
    {
        this._price = price;
        this.txtPrice.text = price.FormatMoney();
        this.btnConfirm.interactable = isEnable;
        this.txtPrice.gameObject.SetActive(isEnable);
        this.btnConfirm.gameObject.ForceRebuildLayoutImmediate();
        this._acConfirmCallback = acConfirmCallback;
        this.currencyIcon.sprite = MainMenuUI.Instance.GetCurrencySprite(currencyType);
    }

    public void OnBtnConfirmClicked(Button btn)
    {
        if (this._acConfirmCallback != null)
        {
            this._acConfirmCallback(this._price);
        }
    }
}
