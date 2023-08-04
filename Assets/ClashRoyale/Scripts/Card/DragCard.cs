using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class DragCard : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    GameObject PrefabToInstantiate;

    [SerializeField]
    RectTransform UIDragElement;

    [SerializeField]
    RectTransform Canvas;

    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;

    private bool mIsDragging = false; // Flag to track dragging status

    private NetworkObject spawnedObject; // Reference to the spawned object's NetworkObject

    private PlayerMana playerMana;
    private CardData cardData;

    [SerializeField]
    Text notEnoughManaText;

    void Start()
    {
        mOriginalPosition = UIDragElement.localPosition;
        playerMana = FindObjectOfType<PlayerMana>();
        cardData = GetComponent<CardData>();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (!IsOwner)
            return;

        mIsDragging = true;
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (!mIsDragging)
            return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;
            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mIsDragging = false; // Stop dragging
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            Vector3 worldPoint = hit.point;

            if (playerMana.GetCurrentMana() >= cardData.manaCost)
            {
                if (hit.collider.CompareTag("EnemyBase"))
                {
                    Debug.Log("You can't throw the card to ENEMYBASE!");
                    return;
                }

                playerMana.ReduceMana(cardData.manaCost);
                CreateObjectOnServerRpc(worldPoint);
            }
            else
            {
                notEnoughManaText.gameObject.SetActive(true);
                StartCoroutine(HideNotEnoughManaText());
            }
        }
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

    [ServerRpc(RequireOwnership =false)]
    public void CreateObjectOnServerRpc(Vector3 position)
    {
       NetworkObject spawnedObject = Instantiate(PrefabToInstantiate, position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<NetworkObject>();
       spawnedObject.Spawn();
        ///eren
    }

    private IEnumerator HideNotEnoughManaText()
    {
        yield return new WaitForSeconds(2.0f); // delay duration as needed
        notEnoughManaText.gameObject.SetActive(false);
    }
}