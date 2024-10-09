using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoCareManager : MonoBehaviour
{
	#region Singleton
	/// <summary>
	/// The public static reference to the instance
	/// Call if you sure the reference is available
	/// </summary>
	private static DinoCareManager _instance;
	public static bool IsInstantiated { get; private set; } //checking bool is faster than checking null
	/// <summary>
	/// This is safe reference
	/// Call to get reference in Awake Function
	/// </summary>
	public static DinoCareManager instance
	{
		get
		{
			if (IsInstantiated)
			{
				return _instance;
			}
			_instance = (DinoCareManager)FindObjectOfType(typeof(DinoCareManager));
			if (!_instance)
			{
				GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/" + typeof(DinoCareManager).ToString()) as GameObject);
				return gameObject.GetComponent<DinoCareManager>();
			}
			if (_instance)
			{
				IsInstantiated = true;
			}
			return _instance;
		}
	}

	protected void Awake()
	{
		// Make instance in Awake to make reference performance uniformly.
		if (!_instance)
		{
			_instance = (DinoCareManager)this;
			IsInstantiated = true;
		}
		// If there is an instance already in the same scene, destroy this script.
		else if (_instance != this)
		{
			Debug.LogWarning("Singleton " + typeof(DinoCareManager) + " is already exists.");
			Destroy(this);
		}
	}
	protected void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
			IsInstantiated = false;
		}
	}
	#endregion

	[SerializeField] DinoListManager dinoList;
	[SerializeField] public DinoCareList dinoCareList;
	[SerializeField] Button instructionBtn;
	[SerializeField] Button hideInstructionBtn;
	[SerializeField] Button close;
	[SerializeField] GameObject instruction;
	StakingClassification classification;
	StakingType type;
	private void Start()
    {
		instructionBtn.onClick.AddListener(ShowInstruction);
		hideInstructionBtn.onClick.AddListener(ShowDinoCareList);
		close.onClick.AddListener(Hide);
		ShowDinoCareList();
	}

	public void ShowInstruction()
	{
		dinoList.gameObject.SetActive(false);
		instruction.gameObject.SetActive(true);
		dinoCareList.gameObject.SetActive(false);
	}

	public void ShowDinoCareList()
	{
		dinoList.gameObject.SetActive(false);
		instruction.gameObject.SetActive(false);
		dinoCareList.gameObject.SetActive(true);
	}

	public void ShowDinoList()
	{
		dinoList.gameObject.SetActive(true);
		instruction.gameObject.SetActive(false);
		dinoCareList.gameObject.SetActive(false);
	}

	public void Show()
	{
	}
	public void Hide()
	{
		GameObject.DestroyImmediate(gameObject);
	}

	public void ShowDinoList(StakingClassification _classification, StakingType _type)
    {
		classification = _classification;
		type = _type;
		ShowDinoList();

	}

	public void OnSelectedDino(DinoModel dino)
    {
		dinoCareList.AssignDino(dino, classification, type, ShowDinoCareList);
	}


}
