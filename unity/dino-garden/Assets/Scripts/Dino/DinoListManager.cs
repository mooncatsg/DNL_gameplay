using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Linq;
public class DinoListManager : MonoBehaviour
{

	[SerializeField] ScrollRect scroll;
	[SerializeField] GameObject cardPrefab;
	[SerializeField] Button selectBtn;
	[SerializeField] Button closeBtn;
	List<DinoModel> dinoList;
	CardDinoController selectedDino;
	bool isCallingAPI = false;
    private void Start()
    {
		selectBtn.onClick.AddListener(OnSelectBtnClick);
		closeBtn.onClick.AddListener(OnCloseBtnClick);
	}

    public void OnSelectedDino(CardDinoController dino)
    {
		if (isCallingAPI)
        {
			dino?.DeselectCard();
        }
        else
        {
			selectedDino?.DeselectCard();
			selectedDino = dino;
			selectBtn.interactable = true;
        }
	}
	public void OnCloseBtnClick()
	{
		if (isCallingAPI)
			return;
		DinoCareManager.instance.ShowDinoCareList();
	}

	public void OnSelectBtnClick()
    {
		if (selectedDino == null)
			return;
		selectBtn.interactable = false;
		isCallingAPI = true;
		DinoCareManager.instance.OnSelectedDino(selectedDino.data);
    }

    public void OnEnable()
    {
		isCallingAPI = false;
		selectBtn.interactable = false;
		selectedDino = null;
		APIMng.Instance.APIDinoList((isSuccess, _dinoList, errMsg, code) => {
            if (isSuccess)
            {
				dinoList = (List<DinoModel>)_dinoList;
				dinoList = dinoList.OrderBy(x => x.rarity).ToList();
				SpawnDinoCardList();
			}
		});
    }

	public void SpawnDinoCardList()
    {
		scroll.content.ClearChild();
		foreach (var f in dinoList)
        {
            if (f.isEvolved)
            {
				GameObject go = GameObject.Instantiate(cardPrefab, scroll.content);
				go.name = f.nftId.ToString();
				go.GetComponent<CardDinoController>().Init(f, false, OnSelectedDino);
            }
		}
		StartCoroutine(DelayInitScrollRectOcclusion());
	}

	IEnumerator DelayInitScrollRectOcclusion()
    {

		yield return new WaitForEndOfFrame();
		scroll.content.GetComponent<GridLayoutGroup>().enabled = true;
		scroll.content.GetComponent<ContentSizeFitter>().enabled = true;
        yield return null;
        scroll.GetComponent<UI_ScrollRectOcclusion>().InitOrDirty();
        //scroll.GetComponent<UI_ScrollRectOcclusion>().SetDirty();
        yield return new WaitForEndOfFrame();
        scroll.GetComponent<UI_ScrollRectOcclusion>().OnScroll(Vector2.zero);


    }
}
