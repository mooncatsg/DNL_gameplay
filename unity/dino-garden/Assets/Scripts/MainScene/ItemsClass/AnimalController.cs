using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class AnimalController : MonoBehaviour
{
    public AnimalType animalType = AnimalType.chicken;
    public Animal animalData = null;
    public Transform notifyIcon;


    private void Update()
    {
        if( !MapController.instance.IsDinoCaring(StakingClassification.animal, animalData.StakeType) &&(NeedToFeed() || NeedToHavest()))
        {
            notifyIcon.gameObject.SetActive(true);
            notifyIcon.LookAt(Camera.main.transform);
        }
        else
        {
            notifyIcon.gameObject.SetActive(false);
        }
    }

    public bool NeedToFeed()
    {
        if(animalData != null && animalData.events.Count > 0)
        {
            if (animalData.events[0].Type != EventCycleType.hungry || animalData.events[0].fixTime != null)
                return false;
            DateTime dt = DateTime.ParseExact(animalData.events[0].startTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            int remainTime = (int)((dt.ToLocalTime() - DateTime.Now).TotalSeconds);
            return remainTime <= 0;
        }
        return false;
    }

    public bool NeedToHavest()
    {
        if (animalData != null && animalData.lifecycles != null && animalData.lifecycles.Count > 0)
        {
            DateTime dt = DateTime.ParseExact(animalData.lifecycles[animalData.lifecycles.Count-1].endTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            int remainTime = (int)((dt.ToLocalTime() - DateTime.Now).TotalSeconds);
            return remainTime <= 0;
        }
        return false;
    }

    public void OnFinishHarvest()
    {

    }

    public void SetData(Animal _animalData)
    {
        animalData = _animalData;
        notifyIcon.gameObject.SetActive(false);
    }

    public void SelectedAnimal()
    {
        //Debug.LogError("SelectedAnimal : " + animalType.ToString());
        //if(NeedToFeed() || NeedToHavest())
        //    MainMenuUI.Instance.ShowActionUI(ACTION_UI_TYPE.TAKE_CARE_ANIMAL);
    }
}
