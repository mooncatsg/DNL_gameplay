using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDinoController : MonoBehaviour
{
    Action<CardDinoController> OnSelected;
    [SerializeField] DinoCharacterController dino;
    [SerializeField] Image classImg;
    [SerializeField] Image cardImg;
    [SerializeField] Image dinoBGImg;
    [SerializeField] Image genderImg;
    [SerializeField] Text nftIdText;
    [SerializeField] Image selectedImg;
    bool isSelected = false;
    public DinoModel data;
    public void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnCardClicked);
    }
    public void Init(DinoModel _data,bool _isSelected = false, Action<CardDinoController> _OnSelected = null)
    {
        data = _data;
        OnSelected = _OnSelected;
        dino.loadDinoData(_data.getExpressingTraits(), _data.calculateRarity(), _data.nftId);
        classImg.sprite = _data.GetClassIcon();
        cardImg.sprite = _data.getRarityCard();
        dinoBGImg.sprite = _data.getDinoBGByRarity();
        genderImg.sprite = _data.getGenderIcon(); 
        nftIdText.text = "#" + _data.nftId;
        isSelected = _isSelected;
        selectedImg.gameObject.SetActive(isSelected);
    }

    public void DeselectCard()
    {
        isSelected = false;
        selectedImg.gameObject.SetActive(isSelected);
    }

    public void OnCardClicked()
    {
        isSelected = true;
        selectedImg.gameObject.SetActive(isSelected);
        OnSelected?.Invoke(this);
    }
}
