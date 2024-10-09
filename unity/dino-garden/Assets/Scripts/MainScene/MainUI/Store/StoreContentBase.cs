using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DinoExtensions;
public class StoreContentBase : MonoBehaviour
{
    [SerializeField] public GameObject storeIemPrefab;
    [SerializeField] public GameObject content;
    [SerializeField] public RectTransform scrollview;

    public List<StoreItemInfo> listStoreInfo = new List<StoreItemInfo>();
    float item_w;
    public virtual void SetupData() { }

    public void ShowContent()
    {
        this.SetupData();
        if (this.content.transform.childCount > 0)
        {
            // this.ResetPurchase();
            return;
        }

        var padding = this.content.GetComponent<HorizontalLayoutGroup>().padding;
        var space = this.content.GetComponent<HorizontalLayoutGroup>().spacing;
        padding.left = 20;
        for (int i = 0; i < this.listStoreInfo.Count; i++)
        {
            var item = Instantiate(this.storeIemPrefab, this.content.transform);
            item.GetComponent<StoreItemUI>().SetData(this.listStoreInfo[i]);
            item_w = item.GetComponent<RectTransform>().sizeDelta.x;

        }

        var check_w = this.scrollview.sizeDelta.x;
        Debug.Log("check_w = " + check_w.ToString());

        float width_content = this.listStoreInfo.Count * item_w + (this.listStoreInfo.Count - 1) * space + padding.left + padding.right;
        Debug.Log("width_content = " + width_content.ToString());

        if (width_content < check_w)
        {
            padding.left = padding.left + ((int)check_w - (int)width_content) / 2;
        }

        this.content.GetComponent<HorizontalLayoutGroup>().padding = padding;
    }

    public void ResetData()
    {
        this.listStoreInfo.Clear();
        this.content.DestroyAllChild();
    }

    public void ResetPurchase()
    {
        foreach (Transform child in this.content.transform)
        {
            child.GetComponent<StoreItemUI>().ResetData();
        }
    }

}
