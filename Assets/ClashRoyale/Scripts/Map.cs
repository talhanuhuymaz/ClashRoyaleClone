using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Map : MonoBehaviour
{
    public static Map Instance;
    public Transform tower;
    public Transform Sidetower;
    public Image healthBar;
    private float towerHP = 100;
    //----------------------------
    public Image healthenemyBar;
    private float enemyHP = 100;
    //--------------------------------------
    private void Awake()
    {
        Instance = this;
    }
  
    public void DamageBaseTower()
    {
        towerHP -= 20;
        healthBar.fillAmount = towerHP / 100;
    }

    public void DamageEnemy()
    {
        enemyHP -= 20;
        healthenemyBar.fillAmount = enemyHP / 100;
        if (enemyHP == 0)
        {
            Destroy(GameObject.FindWithTag("Enemy"));
        }
    }
}
