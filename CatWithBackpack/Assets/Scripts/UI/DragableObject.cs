using Architecture;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private EventReference dragSFX;
    [SerializeField] private EventReference dropSFX;

    [SerializeField] public UnityEvent OnDragStart;
    [SerializeField] public UnityEvent OnDragEnd;

    public void OnBeginDrag(PointerEventData eventData)
    {
        SoundManager.Instance.Shoot(dragSFX);

        OnDragStart?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SoundManager.Instance.Shoot(dropSFX);

        OnDragEnd?.Invoke();
    }
}
