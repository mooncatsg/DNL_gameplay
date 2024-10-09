using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using DinoExtensions;
public class UIPopupBase : MonoBehaviour
{
    [SerializeField] public GameObject background;
    [SerializeField] Image blackPanel;
    public bool isDestroyWhenHide = false;
    public bool isFadeBackground = false;
    private TRANSITION_TYPE showType = TRANSITION_TYPE.ZOOM;
    private TRANSITION_TYPE hideType = TRANSITION_TYPE.ZOOM;
    public float DURATION_MOVE = 0.1f;
    public float DURATION_ZOOM = 0.1f;
    public float BLACK_OPACITY = 150.0f / 255.0f;
    private bool _isAnimRunning = false;
    private Vector3 _originPosition;

    public virtual void Start()
    {
        this._originPosition = this.background.transform.position;
        if (this.blackPanel)
        {
            var btnBlackClose = this.blackPanel.GetComponent<Button>();
            if (btnBlackClose)
            {
                btnBlackClose.onClick.AddListener(delegate { OnBtnCloseClicked(btnBlackClose); });
            }
        }
    }

    public virtual void Show(TRANSITION_TYPE showType = TRANSITION_TYPE.ZOOM, TRANSITION_TYPE hideType = TRANSITION_TYPE.ZOOM, Action callback = null)
    {
        if (this._isAnimRunning) 
            return;
        this._isAnimRunning = true;
        this.showType = showType;
        this.hideType = hideType;
        float durationFade = 0.0f;
        if (this.showType == TRANSITION_TYPE.ZOOM)
        {

            durationFade = DURATION_ZOOM;
            this.background.transform.localScale = Vector3.one * 0.8f;
            this.background.transform.DOScale(Vector3.one, DURATION_ZOOM).SetEase(Ease.OutBack).OnComplete(() =>
            {
                this.OnShowComplete(callback);
            });
        }
        else
        {
            durationFade = DURATION_MOVE;
            this.ShowByMove(() =>
            {
                this.OnShowComplete(callback);
            });
        }

        if (this.blackPanel)
        {
            this.blackPanel.SetAlpha(0.0f);
            this.blackPanel.gameObject.SetActive(true);
            this.blackPanel.DOFade(BLACK_OPACITY, durationFade);
        }

        this.FadeIn(durationFade);
    }

    public void hide(Action callback)
    {
        if (this._isAnimRunning) return;
        this._isAnimRunning = true;
        float durationFade = 0.0f;

        if (this.hideType == TRANSITION_TYPE.ZOOM)
        {
            durationFade = DURATION_ZOOM;
            this.background.transform.DOScale(Vector3.one * 0.8f, DURATION_ZOOM).SetEase(Ease.InBack).OnComplete(() =>
            {
                this.OnHideComplete(callback);
            });
        }
        else
        {
            durationFade = DURATION_MOVE;

            this.HideByMove(() =>
            {
                this.OnHideComplete(callback);
            });

        }

        if (this.blackPanel)
        {
            this.blackPanel.DOFade(0, durationFade);
        }

        this.FadeOut(durationFade);
    }

    private void OnHideComplete(Action callback)
    {
        this._isAnimRunning = false;
        if (callback != null)
        {
            callback();
        }

        if (this.isDestroyWhenHide)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (this.blackPanel)
            {
                this.blackPanel.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
    }

    private void OnShowComplete(Action callback)
    {
        this._isAnimRunning = false;
        if (callback != null)
        {
            callback();
        }
    }

    private void ShowByMove(Action callback)
    {
        var target_position = this._originPosition;
        var start_posision = new Vector3();
        var width = Screen.width;
        var height = Screen.height;

        switch (this.showType)
        {
            case TRANSITION_TYPE.TOP:
                start_posision = new Vector3(target_position.x, target_position.y + height * 1.5f);
                break;

            case TRANSITION_TYPE.BOTTOM:
                start_posision = new Vector3(target_position.x, target_position.y - height * 1.5f);
                break;

            case TRANSITION_TYPE.LEFT:
                start_posision = new Vector3(target_position.x - width * 1.5f, target_position.y);
                break;

            case TRANSITION_TYPE.RIGHT:
                start_posision = new Vector3(target_position.x + width * 1.5f, target_position.y);
                break;

            default:
                start_posision = new Vector3(target_position.x, target_position.y + height * 1.5f);
                break;
        }

        this.background.transform.position = start_posision;
        this.background.transform.DOMove(target_position, DURATION_MOVE).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
            }
        });
    }

    private void HideByMove(Action callback)
    {
        var target_position = new Vector3();
        var start_posision = this.background.transform.position;
        var width = Screen.width;
        var height = Screen.height;

        switch (this.hideType)
        {
            case TRANSITION_TYPE.TOP:
                target_position = new Vector3(start_posision.x, start_posision.y + height * 1.5f);
                break;

            case TRANSITION_TYPE.BOTTOM:
                target_position = new Vector3(start_posision.x, start_posision.y - height * 1.5f);
                break;

            case TRANSITION_TYPE.LEFT:
                target_position = new Vector3(start_posision.x - width * 1.5f, start_posision.y);
                break;

            case TRANSITION_TYPE.RIGHT:
                target_position = new Vector3(start_posision.x + width * 1.5f, start_posision.y);
                break;

            default:
                target_position = new Vector3(start_posision.x, start_posision.y + height * 1.5f);
                break;
        }

        this.background.transform.DOMove(target_position, DURATION_MOVE).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
                this.background.transform.position = start_posision;
            }
        });
    }

    private void FadeIn(float duration)
    {
        if (this.isFadeBackground)
        {
            var canvasGroup = this.background.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = this.background.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1.0f, duration);
        }
    }

    private void FadeOut(float duration)
    {
        if (this.isFadeBackground)
        {
            var canvasGroup = this.background.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = this.background.AddComponent<CanvasGroup>();
            }
            canvasGroup.DOFade(0.0f, duration + 0.2f);
        }
    }

    public void OnClose(Button btn, Action callback)
    {
        if (btn)
        {
            btn.interactable = false;
        }

        this.hide(() =>
        {
            if (btn)
            {
                btn.interactable = true;
            }

            if (callback != null)
            {
                callback();
            }
        });
    }

    public virtual void OnBtnCloseClicked(Button btn)
    {
        this.OnClose(btn, null);
    }

    public void setDestroyWhenHide(bool isDestroyWhenHide = true)
    {
        this.isDestroyWhenHide = isDestroyWhenHide;
    }

}
