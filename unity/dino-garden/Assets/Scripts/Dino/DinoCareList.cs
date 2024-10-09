using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoCareList : MonoBehaviour
{
    [SerializeField] DinoCareCard gardenCard;
    [SerializeField] DinoCareCard chickenCard;
    [SerializeField] DinoCareCard pigCard;
    [SerializeField] DinoCareCard cowCard;
    [SerializeField] DinoCareCard sheepCard;

    private void Start()
    {
        gardenCard.gameObject.SetActive(false);
        chickenCard.gameObject.SetActive(false);
        pigCard.gameObject.SetActive(false);
        cowCard.gameObject.SetActive(false);
        sheepCard.gameObject.SetActive(false);
        APIMng.Instance.APIDinoCaring((isSuccess, data, errMsg, code) => {
            if(isSuccess)
            {
                List<StakedFarm> stakedList = (List<StakedFarm>)data;
                bool hasStakedDino = isSuccess && stakedList != null && stakedList.Count > 0;

                StakedFarm tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.plant && x.dinoCare.Type == StakingType.garden)) : null;
                gardenCard.Initialize(tmp);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.chicken)) : null;
                chickenCard.Initialize(tmp);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.pig)) : null;
                pigCard.Initialize(tmp);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.cow)) : null;
                cowCard.Initialize(tmp);

                tmp = hasStakedDino ? stakedList.Find(x => (x.dinoCare != null && x.dinoCare.Classification == StakingClassification.animal && x.dinoCare.Type == StakingType.sheep)) : null;
                sheepCard.Initialize(tmp);
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
            }
        });
    }
    private void OnEnable()
    {

        if (MapController.instance.totalCropInLand() <= 0)
        {
            gardenCard.DisableAssignBtn();
        }
    }
    public void AssignDino(DinoModel dino, StakingClassification classification, StakingType type , Action callback)
    {
        System.Action<int, APICallback> callAPIAction = null;
        if (classification == StakingClassification.plant && type == StakingType.garden)
        {
            callAPIAction = APIMng.Instance.APIStakeDinoToGarden;
        }
        else if (classification == StakingClassification.animal && type == StakingType.chicken)
        {
            callAPIAction = APIMng.Instance.APIStakeDinoToChicken;
        }
        else if (classification == StakingClassification.animal && type == StakingType.cow)
        {
            callAPIAction = APIMng.Instance.APIStakeDinoToCow;
        }
        else if (classification == StakingClassification.animal && type == StakingType.pig)
        {
            callAPIAction = APIMng.Instance.APIStakeDinoToPig;
        }
        else if (classification == StakingClassification.animal && type == StakingType.sheep)
        {
            callAPIAction = APIMng.Instance.APIStakeDinoToSheep;
        }


        callAPIAction?.Invoke(dino.nftId, (isSuccess, data, errMsg, code) => {
            if (isSuccess)
            {
                StakingFarm stakingFarm = (StakingFarm)data;
                DinoCareCard updateCard = null;
                if (classification == StakingClassification.plant && type == StakingType.garden)
                {
                    updateCard = gardenCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.chicken)
                {
                    updateCard = chickenCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.cow)
                {
                    updateCard = cowCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.pig)
                {
                    updateCard = pigCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.sheep)
                {
                    updateCard = sheepCard;
                }
                updateCard?.UpdateStakingStatus(stakingFarm, dino);
                MapController.instance.UpdateDinoCaring();
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
                DinoCareManager.instance.Hide();
            }
            callback?.Invoke();
        });
    }

    public void AbortDino(int stakingFarmId, StakingClassification classification, StakingType type)
    {
        APIMng.Instance.APIUnstakeDino(stakingFarmId,(isSuccess, data, errMsg, code) => {
            if (isSuccess)
            {
                DinoCareCard updateCard = null; 
                if (classification == StakingClassification.plant && type == StakingType.garden)
                {
                    updateCard = gardenCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.chicken)
                {
                    updateCard = chickenCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.cow)
                {
                    updateCard = cowCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.pig)
                {
                    updateCard = pigCard;
                }
                else if (classification == StakingClassification.animal && type == StakingType.sheep)
                {
                    updateCard = sheepCard;
                }
                updateCard?.UpdateUnStaking();
                MapController.instance.UpdateDinoCaring();
            }
        });
    }

}
