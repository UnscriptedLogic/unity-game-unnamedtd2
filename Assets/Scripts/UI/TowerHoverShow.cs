using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerHoverShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private float moveTime;

    private void Start()
    {
        Hide();
    }

    public void OnPointerEnter(PointerEventData eventData) => Show();
    public void OnPointerExit(PointerEventData eventData) => Hide();

    private void Show()
    {
        //gameObject.transform.position = openPos.position;
        transform.DOMove(openPos.position, moveTime).SetEase(Ease.InOutSine);
        //LeanTween.move(gameObject, openPos.position, moveTime);
    }
    private void Hide()
    {
        //gameObject.transform.position = closePos.position;
        transform.DOMove(closePos.position, moveTime).SetEase(Ease.InOutSine);
        //LeanTween.move(gameObject, closePos.position, moveTime);
    }
}
