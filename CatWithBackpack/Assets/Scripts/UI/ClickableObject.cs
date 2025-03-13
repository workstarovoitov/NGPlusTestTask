using Architecture;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    [SerializeField] private bool clickOnDown = true;
    [SerializeField] private EventReference clickSFX;
    [SerializeField] private EventReference hoverSFX;

    [SerializeField] public UnityEvent OnEnter;
    [SerializeField] public UnityEvent OnExit;
    [SerializeField] public UnityEvent OnClick;

    private bool hovered;

    public void FireEvent(GameEvent gameEvent)
    {
        gameEvent?.Invoke();
    }

    public void ShootSFX(string sfxEvent)
    {
        SoundManager.Instance.Shoot(sfxEvent);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!clickOnDown) return;

        SoundManager.Instance.Shoot(clickSFX);
        OnClick?.Invoke();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (clickOnDown) return;
        SoundManager.Instance.Shoot(clickSFX);
        OnClick?.Invoke();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        SoundManager.Instance.Shoot(hoverSFX);
        OnEnter?.Invoke();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        OnExit?.Invoke();
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if (hovered) return;
        SoundManager.Instance.Shoot(hoverSFX);
        OnEnter?.Invoke();
    }
}