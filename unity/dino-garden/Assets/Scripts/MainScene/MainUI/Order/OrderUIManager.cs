using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OrderUIManager : UIPopupBase
{
    public Transform orderParentTransform;
    public GameObject orderItemPrefab;

    public Transform productParentTransform;
    public GameObject productItemPrefab;

    public Button fullFillButton;

    public List<OrderData> listOrderData = new List<OrderData>();
    public OrderData choosingOrderItem = null;
    public List<WarehouseItem> productList = new List<WarehouseItem>();

    public static OrderUIManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        fullFillButton.onClick.AddListener(OnClickFullfillButton);
    }

    void OnClickFullfillButton()
    {
        if(choosingOrderItem != null)
        {
            APIMng.Instance.APISellOrderList(choosingOrderItem.id, (isSuccess, sellOrderData, errMsg, code) =>
            {
            if(isSuccess && ((ItemOrderData)sellOrderData) != null)
            {
                MapController.instance.UpdateUserInfo();
                    int priceType = ((ItemOrderData)sellOrderData).priceType;
                    MainMenuUI.Instance.ShowAlertMessage("SUCCESS TO SELL PRODUCT FOR : "+((ItemOrderData)sellOrderData).price + (priceType == 2 ? " W-DNG" : " W-DNL"),"MESSAGE",false,()=> {
                        ShowOrderList();
                    }, () => {
                        ShowOrderList();
                    });

                }
                else
                {
                    MainMenuUI.Instance.ShowAlertMessage(errMsg);
                }
            });
        }
    }

    public void ShowOrderList()
    {
        this.Show();

        // REMOVE OLD ITEM
        List<OrderItem> listOldOrderItem = orderParentTransform.GetComponentsInChildren<OrderItem>().ToList();
        for(int i= listOldOrderItem.Count-1;i>=0;i--)
        {
            GameObject.Destroy(listOldOrderItem[i].gameObject);
        }

        APIMng.Instance.APIProductsList((isSuccess2, _productList, errMsg2, code2) =>
        {
            if (isSuccess2)
            {
                productList = (List<WarehouseItem>)_productList;
                APIMng.Instance.APIGetOrderList((isSuccess, _orderList, errMsg, code) =>
                {
                    if (isSuccess)
                    {
                        listOrderData = new List<OrderData>((List<OrderData>)_orderList);
                        for (int i = 0; i < listOrderData.Count; i++)
                        {
                            GameObject obj = GameObject.Instantiate(orderItemPrefab, orderParentTransform);
                            obj.GetComponent<OrderItem>().SetData(listOrderData[i]);
                        }

                        if (listOrderData.Count > 0)
                        {
                            OnClickOrderItem(listOrderData[0]);
                        }
                        else
                        {
                            for (int i = productParentTransform.childCount; i>0; i--)
                            {
                                GameObject.Destroy(productParentTransform.GetChild(i-1).gameObject);
                            }
                            fullFillButton.interactable = false;
                        }
                    }
                    else
                    {
                        MainMenuUI.Instance.ShowAlertMessage(errMsg);
                    }
                });
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg2);
            }
        });
    }    
    public void OnClickOrderItem(OrderData clickedItem)
    {
        choosingOrderItem = clickedItem;
        List<OrderItem> listOrderItem = orderParentTransform.GetComponentsInChildren<OrderItem>().ToList();
        foreach(OrderItem orderItem in listOrderItem)
        {
            orderItem.SetChoosing(clickedItem == orderItem.orderData);
        }

        for (int i= productParentTransform.childCount-1;i>= 0;i--)
        {
            GameObject.Destroy(productParentTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < clickedItem.items.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(productItemPrefab, productParentTransform);
            obj.GetComponent<OrderProduct>().SetData(clickedItem.items[i]);
        }

    }
}
