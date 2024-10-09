using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimalHouseCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    public AnimalType cageType = AnimalType.chicken;

    [SerializeField] GameObject babyAnimal;
    [SerializeField] List<GameObject> babyAnimalList;

    [SerializeField] GameObject adultAnimal;
    [SerializeField] List<GameObject> adultAnimalList;

    [SerializeField] GameObject lockObj;
    [SerializeField] GameObject openObj;

    public List<GameObject> listAnimals;
    public Cage cageData = null;


    [SerializeField]
    GameObject prefabEffectEat;

    void Start()
    { 
    }

    public void InitData(Cage _data)
    {
        cageData = _data;
        UpdateDisplayModel();
    }
    public void UpdateDisplayModel()
    {
        lockObj.SetActive(cageData.status == "locked");
        openObj.SetActive(cageData.status != "locked");
        this.UpdateAnimalDisplay();
    }
    public void UpdateAnimalDisplay()
    {
        if (cageData.status == "locked")
        {
            for (int i = 0; i < listAnimals.Count; i++)
            {
                listAnimals[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < listAnimals.Count; i++)
            {
                if(i<cageData.animalList.Count)
                {
                    listAnimals[i].GetComponent<AnimalController>().SetData(cageData.animalList[i]);
                }                        
                listAnimals[i].SetActive(i < cageData.animalList.Count);
            }
        }             
    }

    public void SelectedAnimalHouse()
    {
        if(cageData != null)
        {
            Debug.Log(cageData.type);
            if (cageData.Type == AnimalType.chicken)
            {
                SoundManager.instance.PlaySound(SoundType.CHICKEN);
            }
            else if (cageData.Type == AnimalType.cow)
            {
                SoundManager.instance.PlaySound(SoundType.COW);
            }
            else if (cageData.Type == AnimalType.sheep)
            {
                SoundManager.instance.PlaySound(SoundType.SHEEP);
            }
            else if (cageData.Type == AnimalType.pig)
            {
                SoundManager.instance.PlaySound(SoundType.PIG);
            }
            MainMenuUI.Instance.ShowUIHenHouse(cageData);
        }
           
    }


    public void PlayAnimationFinish()
    {
        //playAnimationName("jump");
    }
    public void PlayAnimationEat()
    {
        Instantiate(this.prefabEffectEat, this.transform);
        //this.mainAni.SetTrigger("go_eat");
        //playAnimationName("eat");
    }




    //private void playAnimationName(string nameani)
    //{
    //    Animator[] anictrl = animalObj.GetComponentsInChildren<Animator>();
    //    foreach (Animator anic in anictrl)
    //    {
    //        if (anic.gameObject.name == "ANIMALS")
    //        {
    //            continue;
    //        }
    //        anic.SetTrigger(nameani);
    //    }
    //}
    //public void AnimalGrowUp()
    //{
    //    foreach (Transform eachChild in animalObj.transform)
    //    {
    //        foreach (Transform child2 in eachChild.transform)
    //        {
                
    //            if (child2.name == this.aniData.NameObject)
    //            {
    //                child2.GetComponent<SkinnedMeshRenderer>().sharedMesh = this.aniData.listMesh[1];
    //                //Debug.Log("Child found. Mame: " + child2.name);
    //            }
    //        }
    //    }
    //}

}
