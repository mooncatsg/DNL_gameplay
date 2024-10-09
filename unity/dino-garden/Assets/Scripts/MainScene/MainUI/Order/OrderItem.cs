using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour
{
    [SerializeField] Image backgroundIcon;
    [SerializeField] Image statusIconFull;
    [SerializeField] Image statusIconNotFull;
    [SerializeField] Text moneyText;

    public Sprite normalSprite;
    public Sprite choosingSprite;

    public OrderData orderData = null;
    bool isFull = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(OrderData _orderData)
    {
        orderData = _orderData;
        moneyText.text = "" + orderData.totalPrice;

        isFull = true;
        for(int i=0;i<orderData.items.Count; i++)
        {
            float realQuantity = 0;
            WarehouseItem warehouseItem = OrderUIManager.Instance.productList.Find(x => x.type == orderData.items[i].type);
            if (warehouseItem != null)
            {
                realQuantity = warehouseItem.Quantity;
                if (realQuantity < orderData.items[i].quantity)
                {
                    isFull = false;
                    break;
                }
            }
            else
            {
                isFull = false;
                break;
            }
                
        }
        statusIconFull.gameObject.SetActive(isFull);
        statusIconNotFull.gameObject.SetActive(!isFull);
    }

    public void SetChoosing(bool choosing)
    {
        backgroundIcon.sprite = choosing ? choosingSprite : normalSprite;
        if(choosing)
            OrderUIManager.Instance.fullFillButton.interactable = isFull;
    }    

    public void OnClickThisButton()
    {
        OrderUIManager.Instance.OnClickOrderItem(this.orderData);
    }
}
