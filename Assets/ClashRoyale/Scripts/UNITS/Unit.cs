using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

public abstract class Unit : MonoBehaviour
{
    public string unitName;

    public List<UnitType> targets;

    public Team team;

    public UnitType unitType;

    public float attackSpeed;

    public float attackRange;

    public int hitPoints;

    public int damage;

    public Transform currentTarget;

    public float attackCooldown;

    public Slider healthBar;

    private AsyncOperationHandle<SceneInstance> sceneHandles;
    public void Attack()
    {
        // Deal damage to the target
        if (currentTarget != null)
        {
            //Debug.Log(gameObject.name + " Attacking " + currentTarget.name);
            Unit targetUnit = currentTarget.GetComponent<Unit>();
            if (targetUnit != null && targetUnit.hitPoints > 0)
            {
                targetUnit.TakeDamage(damage);
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    public void TakeDamage(int takenDamage)
    {
        
        hitPoints -= takenDamage;
        UpdateHealthBar();
        if (hitPoints <= 0)
        {

            if (gameObject.CompareTag("Base"))
            {
                // Restart the game here
                RestartGame();
            }
            else
            {
                Destroy(gameObject);
            }
            
            return;
        }
    }
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = hitPoints; // Update the health bar's value to match the current hit points
        }
    }
    private void RestartGame()
    {

        sceneHandles = Addressables.LoadSceneAsync("Assets/ClashRoyale/Scenes/Start.unity", LoadSceneMode.Single, activateOnLoad: true);
    }
}

public enum UnitType
{
    Ground,
    Air,
    Tower
}

public enum Team
{
    Blue,
    Red,
}