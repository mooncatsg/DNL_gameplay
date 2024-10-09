using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;
public class FarmStoreUI : UIPopupBase
{
    [SerializeField] FarmGachaUI guiFarmGacha;
    public Action<Action> acBackCallback;

    private void Awake()
    {
        this.guiFarmGacha.acCallback = this.OnFarmGachaCallback;
    }

    public void OnUpdateLayoutWhenActive()
    {
        this.guiFarmGacha.gameObject.SetActive(true);
        this.guiFarmGacha.ShowGacha();
    }

    public void OnBtnBackClose()
    {
        this.OnBackCallback(null);
    }

    private void OnBackCallback(Action callback)
    {
        if (acBackCallback != null)
        {
            acBackCallback(callback);
        }
    }

    public void OnFarmGachaCallback(FarmGachaUI.ACTION_TYPE acType, int amount)
    {
        Debug.LogError("==> OnFarmGacha: " + acType + "| " + amount);
        switch (acType)
        {
            case FarmGachaUI.ACTION_TYPE.PURCHASE:
                {
                    MainMenuUI.Instance.ShowAlertMessage("DO YOU WANT TO BUY A RANDOM CROP ?","BUY CROP",true,null,()=> {
                        APIMng.Instance.APIBuyBox(1, (isSuccess, cropDetail, errMsg, code) =>
                        {
                            if (isSuccess && cropDetail != null)
                            {
                                MapController.instance.UpdateUserInfo();

                                StoreUI.Instance.ShowCropsResultPurchase(1, (List<BuyCrop>)cropDetail);
                            }
                            else
                            {
                                MainMenuUI.Instance.ShowAlertMessage(errMsg);
                            }
                        });
                    });


                }
                break;

            case FarmGachaUI.ACTION_TYPE.BACK:
                this.OnBtnBackClose();
                break;

            default:
                break;
        }
    }

    public void ResetData()
    {
        this.guiFarmGacha.ResetData();
    }

}
