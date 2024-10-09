using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DinoGarden;
using UnityEngine;
public class FarmStoreTabItem : TabbarItem
{
    [SerializeField] Image icon;

    override public void SetChoose(bool isChoose)
    {
        base.SetChoose(isChoose);
    }
}
