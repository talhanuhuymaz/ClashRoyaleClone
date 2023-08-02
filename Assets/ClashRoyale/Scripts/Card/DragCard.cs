using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject PrefabToInstantiate;
    [SerializeField] RectTransform UIDragElement;
    [SerializeField] RectTransform Canvas;

    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;

    private PlayerMana playerMana;
    private CardData cardData;

    [SerializeField] Text notEnoughManaText;
    void Start()
    {
        mOriginalPosition = UIDragElement.localPosition;
        playerMana = FindObjectOfType<PlayerMana>();
        cardData = GetComponent<CardData>();
    }



    public void OnBeginDrag(PointerEventData data)
    {
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
    }




    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;
            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }




    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));

        float manaCost = cardData.manaCost;

        if (playerMana.GetCurrentMana() >= manaCost)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider.CompareTag("EnemyBase"))
                {
                    Debug.Log("You can't throw the card to ENEMYBASE!");
                    // Even if there is enough mana, if he tries to throw the card to ENEMYBASE, cancel it.
                    return;
                }
                // If you aren't throw the card to ENEMYBASE then play the card.
                playerMana.ReduceMana(manaCost);
                Vector3 worldPoint = hit.point;
                CreateObject(worldPoint);
            }
        }
        else
        {
            notEnoughManaText.gameObject.SetActive(true);
            StartCoroutine(HideNotEnoughManaText());
            StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));
        }
    }

    private IEnumerator HideNotEnoughManaText()
    {
        yield return new WaitForSeconds(2.0f); // Adjust the delay duration as needed
        notEnoughManaText.gameObject.SetActive(false);
    }

    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;
        while (elapsedTime < duration)
        {
            r.localPosition = Vector2.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        r.localPosition = targetPosition;
    }

    public void CreateObject(Vector3 position)
    {
        GameObject obj = Instantiate(PrefabToInstantiate, position + Vector3.up * 0.5f, Quaternion.identity);
    }
}