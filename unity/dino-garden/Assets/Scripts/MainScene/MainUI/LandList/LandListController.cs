using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandListController : UIPopupBase
{
    // Start is called before the first frame update
    public Transform listLand;
    public GameObject landItemPrefab;
    public List<LandDataDetail> listLandDataDetail = new List<LandDataDetail>();
    void Start()
    {
        for(int i= listLand.childCount-1;i>=0; i--)
        {
            GameObject.Destroy(listLand.GetChild(i).gameObject);
        }    

        APIMng.Instance.APILandList((isSuccess, landList, errMsg, code) => { 
            if(isSuccess)
            {
                listLandDataDetail = (List<LandDataDetail>)landList;
                if(listLandDataDetail != null && listLandDataDetail.Count > 0)
                {
                    for (int i = 0; i < listLandDataDetail.Count; i++)
                    {
                        GameObject obj = GameObject.Instantiate(landItemPrefab, listLand);
                        obj.GetComponent<LandUIItem>().SetData(listLandDataDetail[i]);
                    }
                }
            }   
            else
            {

            }    
        });
    }
}
