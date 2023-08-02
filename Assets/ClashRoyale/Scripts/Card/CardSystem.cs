using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CardSystem : MonoBehaviour
{

    public SO card;

    [SerializeField] private TMP_Text manaBar;
    [SerializeField] private Image spriteCard;
    void Start()
    {
        manaBar.text = card.manaBar.ToString();

        spriteCard.sprite = card.playerPhoto;
    }

    
}
