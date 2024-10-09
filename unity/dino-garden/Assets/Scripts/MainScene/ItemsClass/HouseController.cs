using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public enum HOUSE_TYPE
    {
        SHOP,
        STORAGE,
        PLAYER,
        FERTILIZER,
        SEED,
    }

    public HOUSE_TYPE houseType = HOUSE_TYPE.FERTILIZER;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectedHouse()
    {
        if(houseType == HOUSE_TYPE.SHOP)
        {
            MainMenuUI.Instance.ShowUIStore_Animal();
            //BienTempUI.instance.OpenAnimalBuyPanel();
        }
        else if(houseType == HOUSE_TYPE.STORAGE)
        {
            MainMenuUI.Instance.ShowStorageUI();
        }
        else if (houseType == HOUSE_TYPE.SHOP)
        {
            MainMenuUI.Instance.ShowUIStore();
        }
        else if (houseType == HOUSE_TYPE.SEED)
        {
            MainMenuUI.Instance.ShowUIStore_Seed();
        }
        else if (houseType == HOUSE_TYPE.FERTILIZER)
        {
            MainMenuUI.Instance.ShowUIStore_Fertilizer();
        }
    }
}
