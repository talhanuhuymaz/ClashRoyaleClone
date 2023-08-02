using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTower : Unit
{


    private void Update()
    {
        FindTarget();

        if (currentTarget != null) {

            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = attackSpeed; 
            }
            // Update attack cooldown
            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }
    public void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Unit enemy))
            {

                // If the target's team is different from agent's team and 
                // the agent can attack the target.
                if (team != enemy.team && targets.Contains(enemy.unitType))
                {
                    currentTarget = collider.transform;
                    //Debug.Log(currentTarget.name);
                    return;
                }
            }
        }
        // If the target is not found set currentTarget null.
        currentTarget = null;
    }
    private void OnDrawGizmosSelected()
    {
        

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
