using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderProduct : MonoBehaviour
{
    public Image orderIcon;    
    public Text orderTextCount;
    public GameObject statusFull;
    public GameObject statusNotFull;


    public ItemOrderData orderItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SetData(ItemOrderData _item)
    {
        orderItem = _item;
        List<Sprite> listIcons = MainMenuUI.Instance.GetListStoreItemIcon(_item.Classification);
        if (_item.Classification == STORE_ITEM_TYPE.animal)
            listIcons = MainMenuUI.Instance.listAnimalProductIcons;
        Sprite spr = listIcons.Find(x => x.name == _item.type);
        if (spr != null)
        {
            orderIcon.sprite = spr;
            orderIcon.SetNativeSize();
        }
        float realQuantity = 0;
        if(OrderUIManager.Instance.productList.Find(x => x.type == orderItem.type) != null)
            realQuantity = OrderUIManager.Instance.productList.Find(x => x.type == orderItem.type).Quantity;
        orderTextCount.text = realQuantity + "/" + orderItem.quantity;

        statusFull.SetActive(realQuantity >= orderItem.quantity);
        statusNotFull.SetActive(realQuantity < orderItem.quantity);
    }

    public bool IsFull()
    {
        float realQuantity = 0;
        if (OrderUIManager.Instance.productList.Find(x => x.type == orderItem.type) != null)
            realQuantity = OrderUIManager.Instance.productList.Find(x => x.type == orderItem.type).Quantity;
        return realQuantity >= orderItem.quantity;        
    }
}
