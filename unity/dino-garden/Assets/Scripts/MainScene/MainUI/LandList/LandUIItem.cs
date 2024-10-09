using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LandUIItem : MonoBehaviour
{
    public List<Sprite> iconSpriteList;
    public Image iconImage;
    public Text idTxt;
    public Text aboutTxt;
    public Text cropTxt;
    public Text crop1Txt;
    public Text crop2Txt;
    public Text otherTxt;
    public Button enterBtn;

    public LandDataDetail landData = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(LandDataDetail data)
    {
        landData = data;

        // VALIDATE UI
        iconImage.sprite = iconSpriteList[(int)(landData.Rarity)-1];
        enterBtn.interactable = landData.id != APIMng.Instance.landid;
        idTxt.text = "#" + landData.id;
        
        if (Int32.TryParse(landData.mintAt, out int j))
        {

            DateTime t = UnixTimeStampToDateTime((double)j);

            string rarityStr = landData.rarity.ToUpper();
            if (landData.Rarity == LAND_RARITY.mysthical)
                rarityStr = "MYSTIC";
            aboutTxt.text = "" + rarityStr + "\n" + t.ToString("dd/MM/yyyy");
        }
        else
        {
            Console.WriteLine("String could not be parsed.");
        }

        cropTxt.text = "MAXIMUM CROP: " + landData.cropLimit + "\nCROP AVAILABLE: " + landData.crop.Count;
        crop1Txt.text = "NORMAL: " + GetCropQuantityByName("normal") + "\nRARE: " + GetCropQuantityByName("rare") + "\nSUPER RARE: " + GetCropQuantityByName("superRare");
        crop2Txt.text = "LEGENDARY: " + GetCropQuantityByName("legendary") + "\nMYSTIC: " + GetCropQuantityByName("mystic");

        string otherText = landData.warehouse.used + "/" + landData.warehouse.capacity;
        if (landData.animal != null && landData.animal.Count == 4)
        {
            otherText += "\n" + data.animal.Find(x => x.type == "chicken").count;
            otherText += "\n" + data.animal.Find(x => x.type == "cow").count;
            otherText += "\n" + data.animal.Find(x => x.type == "sheep").count;
            otherText += "\n" + data.animal.Find(x => x.type == "pig").count;
        }
        otherTxt.text = otherText;
    }


    public int GetCropQuantityByName(string name)
    {
        LandDateCrop crop = landData.crop.Find(x=> x.rarity == name);
        if (crop != null)
            return Int32.Parse(crop.count);
        return 0;
    }    

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    public void OnEnterButtonClicked()
    {
        APIMng.Instance.landid = landData.id;
        SceneManager.LoadScene("MainScene");
    }
}
