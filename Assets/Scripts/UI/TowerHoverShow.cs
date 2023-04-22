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

    private void Show() => LeanTween.move(gameObject, openPos.position, moveTime);
    private void Hide() => LeanTween.move(gameObject, closePos.position, moveTime);
}
