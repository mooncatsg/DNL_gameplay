using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameplayManager : FastSingleton<GameplayManager>
{
    public void TouchObject3D(ItemPlaceController obj)
    {
        string namesl = "";
        if (obj != null)
        {
            namesl = obj.gameObject.name;
        }
        GameObject textF = GameObject.Find("Canvas/TextTest");
        if (textF != null)
        {
            textF.GetComponent<Text>().text = namesl;
            //Debug.Log("----Select 3D Object:" + namesl);
        }
        if (obj == null)
        {
            return;
        }
        Farm3DController farm3d = obj.gameObject.GetComponent<Farm3DController>();
        if (farm3d != null)
        {
            //textF.GetComponent<Text>().text = " " + farm3d.GetFarmState().ToString();

            //if (farm3d.farmStt == FarmState.BLANK && MainMenuUI.Instance)
            //{
            //    MainMenuUI.Instance.ShowBuildStore();
            //    return;
            //}

            //if (farm3d.farmStt == FarmState.BLANK && MainMenuUI.Instance)
            //{
            //    MainMenuUI.Instance.ShowPlantTreeUI();
            //    return;
            //}
        }
        else
        {
            //AnimalHouseCtrl animalHouse = obj.gameObject.GetComponent<AnimalHouseCtrl>();
            //if (animalHouse != null)
            //{
            //    animalHouse.UpdateDisplayModel();
            //}
        }
    }

    public void OnStoreFarmDeposit(int amount)
    {
        Debug.Log("==> Gameplay => OnStoreFarmDeposit" + " | amount = " + amount);
        /*
         * Demo mình tạm truyền số farm vào đây, còn chạy thật sẽ là truyền List<LANDTYPE> 
         * -->list của kiểu đất
         */
        //MapController.instance.DepositListFarm(list)
        //MapController.instance.DepositListFarmByTest(amount);

        //TODO: Anh Son Chuyen Trang Thai Farm Here
    }

    public void OnPlantTreeBySeed(WarehouseItem seedInfo)
    {
        //Anh Sơn xử lý Farm 3D trồng cây ở đây
        Debug.Log("==> Anh Sơn xử lý Farm 3D trồng cây ở đây");
        //==>
        // 

        //Update Amount Seed To Data
    }

}
