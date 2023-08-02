using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMana : MonoBehaviour
{
    public Image manaBar;
    public Text manaText;
    public float OurMana;
    public float maxMana = 10f;
    private float currentMana;

    private void Start()
    {
        currentMana = 6;
        UpdateManaBar();
    }
    public float GetCurrentMana()
    {
        return currentMana;
    }
    private void Update()
    {
        // Bu kısmı değiştirelim
        if (currentMana < OurMana)
        {
            float deltaMana = Time.deltaTime * 0.08f * OurMana;
            currentMana = Mathf.Clamp(currentMana + deltaMana, 0f, OurMana);
            UpdateManaBar();
        }

        // Bu kısmı da değiştirelim
        if (currentMana < 0)
        {
            currentMana = 0;
            UpdateManaBar();
        }
        manaText.text = "" + Mathf.FloorToInt(currentMana);
    }

    public void ReduceMana(float mana)
    {
        currentMana -= mana;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        manaBar.fillAmount = currentMana / maxMana;
        manaText.text = Mathf.FloorToInt(currentMana).ToString();
    }
    public void PlayCard(CardData card)
    {
        if (currentMana >= card.manaCost)
        {
            // Yeterli mana varsa kartı oynat.
            ReduceMana(card.manaCost);
            // Kartın gerçekleştireceği diğer işlemler burada yapılır.
        }
        else
        {
            Debug.Log("Yeterli mana yok!");
            // Yeterli mana yoksa kartı oynatma ve işlemi iptal et.
        }
    }
}
