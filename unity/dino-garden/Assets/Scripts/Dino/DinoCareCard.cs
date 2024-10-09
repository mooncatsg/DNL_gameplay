using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoCareCard : MonoBehaviour
{
	[SerializeField] CardDinoController dino;
	[SerializeField] GameObject icon;
	[SerializeField] Button assignBtn;
	[SerializeField] Button abortBtn;
	[SerializeField] StakingType type;
	[SerializeField] StakingClassification classification;
    int stakedFarmId;
    private void Start()
    {
        assignBtn.onClick.AddListener(AssginDino);
        abortBtn.onClick.AddListener(AbortDino);
    }
    public void DisableAssignBtn()
    {
        assignBtn.interactable = false;
    }

    public void Initialize(StakedFarm data)
    {
        bool hasDinoStaked = data != null && data.dinoCare != null && data.dinoCare.dino != null;
        stakedFarmId = hasDinoStaked ? data.dinoCare.id : -1;
        dino.gameObject.SetActive(hasDinoStaked);
        icon.gameObject.SetActive(!hasDinoStaked);
        abortBtn.gameObject.SetActive(hasDinoStaked);
        assignBtn.gameObject.SetActive(!hasDinoStaked);
        if (hasDinoStaked)
        {
            dino.Init(data.dinoCare.dino);
        }
        this.gameObject.SetActive(true);
    }

    public void UpdateStakingStatus(StakingFarm data, DinoModel dinoData)
    {
        bool hasDinoStaked = true;
        stakedFarmId = data.id;
        dino.gameObject.SetActive(hasDinoStaked);
        icon.gameObject.SetActive(!hasDinoStaked);
        abortBtn.gameObject.SetActive(hasDinoStaked);
        assignBtn.gameObject.SetActive(!hasDinoStaked);
        if (hasDinoStaked)
        {
            dino.Init(dinoData);
        }
    }
    public void UpdateUnStaking()
    {
        bool hasDinoStaked = false;
        stakedFarmId = -1;
        dino.gameObject.SetActive(hasDinoStaked);
        icon.gameObject.SetActive(!hasDinoStaked);
        abortBtn.gameObject.SetActive(hasDinoStaked);
        assignBtn.gameObject.SetActive(!hasDinoStaked);
    }
    public void AssginDino()
    {
        DinoCareManager.instance.ShowDinoList(classification, type);
    }

    public void AbortDino()
    {
        DinoCareManager.instance.dinoCareList.AbortDino(stakedFarmId, classification, type);
    }

}
