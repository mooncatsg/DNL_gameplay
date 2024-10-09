using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlantSeedItemUI : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] Text quantity;

    WarehouseItem info;
    string actionStr = "";

    public Action<WarehouseItem,string> acOnSelectClicked;

    public void SetInfo(WarehouseItem _info,string _actionStr)
    {
        info = _info;
        actionStr = _actionStr;
        if(MainMenuUI.Instance.GetPlanTreeUI().actionType == ACTION_TYPE.PLANT_TREE)
        {
            var sprite = Resources.Load<Sprite>("icon_Plant/" + info.type.ToString());
            this.imgIcon.rectTransform.sizeDelta = new Vector2(90f * sprite.rect.width / sprite.rect.height, 90f);
            this.imgIcon.sprite = sprite;            
            this.quantity.text = "x" + info.Quantity;
        }
        else if (MainMenuUI.Instance.GetPlanTreeUI().actionType == ACTION_TYPE.TAKE_CARE_TREE)
        {
            quantity.gameObject.SetActive(false);
            var sprite = Resources.Load<Sprite>("action/" + actionStr);
            this.imgIcon.rectTransform.sizeDelta = new Vector2(90f * sprite.rect.width / sprite.rect.height, 90f);
            this.imgIcon.sprite = sprite;
        }
    }

    public void OnBtnSelectClicked()
    {
        if (acOnSelectClicked != null)
        {
            acOnSelectClicked(this.info, actionStr);
        }
    }
}
