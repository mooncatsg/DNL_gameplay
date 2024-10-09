using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FarmYourCropUI : UIPopupBase
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject cropsItemPrefab;
    private int _currentBoxAmount = 0;
    public Action<int> acCallback;

    public void ShowYourCrop(List<int> listCropsType)
    {
        listCropsType.Sort((a, b) => b.CompareTo(a));
        foreach (Transform child in this.content.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < listCropsType.Count; i++)
        {
            this.AddFarmItem(listCropsType[i]);
        }

        this.Show();
    }

    public void AddFarmItem(int cropsType)
    {
        var item = Instantiate(cropsItemPrefab, this.content.transform);
        item.GetComponent<StoreCropsItem>().SetData(cropsType);
    }

    public void AddCropsItem()
    {

    }

    public void OnBtnBackClicked()
    {
        this.OnBtnCloseClicked(null);
        if (this.acCallback != null)
        {
            this.acCallback(this._currentBoxAmount);
        }
        StoreUI.Instance.CloseStore();
    }

}
