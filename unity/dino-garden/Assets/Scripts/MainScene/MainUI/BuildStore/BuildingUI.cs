using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoGarden;
using System;
public class BuildingUI : UIPopupBase
{
    [SerializeField] BuildingCropsUI guiCrops;
    [SerializeField] public Tabbar tabbar;
    private bool _isShowed = false;

    public static BuildingUI Instance;

    private void Awake() 
    {
        Instance = this;
        this.tabbar.acTabClick = this.OnTabbarClicked;
        this.guiCrops.acCallback = this.OnBuildingCropsCallback;
    }

    private void OnEnable()
    {
        //this.ActionWaitForEndOfFrame(()=> {
        //    if (this._isShowed)
        //    {
        //        this.tabbar.clearDataAndActive();
        //    }
        //});
    }

    private void OnTabbarClicked(int index)
    {
        this._isShowed = true;
        this.guiCrops.gameObject.SetActive(false);
        Debug.Log("==> OnTabbarClicked: " + index);
        switch (index)
        {
            case 0:
                this.guiCrops.gameObject.SetActive(true);
                MainMenuUI.Instance.listCropsPurchased.Sort((a, b) => b.CompareTo(a));
                this.guiCrops.ShowBuildingCrops();
                break;

            case 1:
                break;

            case 2:
                break;

            default:
                break;
        }
    }

    void OnBuildingCropsCallback(BuildingCropsUI.ACTION_TYPE acType, List<int> listCropsDeposit)
    {
        Debug.Log("==> OnBuildingCropsCallback ACTION TYPE: " + acType);
        switch (acType)
        {
            case BuildingCropsUI.ACTION_TYPE.DEPOSIT:
                {
                    this.OnBackCallback(() =>
                    {
                        Debug.LogError("==> DEPOSITED: " + listCropsDeposit.Count);
                        //TODO: Crops Deposit BienVT implement here ==> listCropsDeposit
                        if (listCropsDeposit != null && listCropsDeposit.Count >= 1)
                        {

                        }
                    });
                }
                break;

            case BuildingCropsUI.ACTION_TYPE.PURCHASE_CROP:
                {
                    this.OnBackCallback(() =>
                    {
                        MainMenuUI.Instance.ShowUIStore();
                    });
                }
                break;
            default:
                break;
        }
    }

    private void OnBackCallback(Action callback)
    {
        this.OnClose(null, callback);
    }
}
