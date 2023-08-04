using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Uagent : Unit
{
    private NavMeshAgent agent;
    public int mana;
    public int unitAmount;
    public float movementSpeed;
    public float sightRange;
    public GameObject rightPath;
    public GameObject leftPath;
    private Animator anim;
    public float rotationSpeed = 5f;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true; // NavMeshAgent'i etkinleştirin
    }

    private void Update()
    {
        FindTarget();
        Move();
        FaceTarget(); 
    }

    private void FaceTarget()
    {
        if (currentTarget != null)
        {
            // Get the direction to the target
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            directionToTarget.y = 0f; // Ensure the character stays upright

            // Rotate the character towards the target
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
    public void Move()
    {
        if (anim != null)
        {
            anim.SetBool("run", true);
        }
        // If there is a target
        if (currentTarget != null)
        {
            
            Vector3 closestPoint = currentTarget.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            float distanceToTarget = Vector3.Distance(transform.position, closestPoint);

            // If target is not in attackRange move towards target.
            if (distanceToTarget > attackRange)
            {
                Vector3 targetDirection = closestPoint - transform.position;
                targetDirection.y = 0f;
                targetDirection.Normalize();
                agent.isStopped = false;
                agent.SetDestination(closestPoint);
            }
            // If the target entered the attackRange, stop and attack.
            else
            {
                
                if (anim != null)
                {
                    anim.SetBool("run", false);
                    anim.SetBool("attack", true);
                }
                agent.isStopped = true;
                if (attackCooldown <= 0f)
                {
                    Attack();
                   
                    attackCooldown = attackSpeed; // cooldown between attacks
                }
                // updates the cooldown between attacks
                if (attackCooldown > 0f)
                {
                    attackCooldown -= Time.deltaTime;
                }


            }
        }
        // If current target is null, move Local forward
        else
        {
            agent.isStopped = false;
            if (anim != null)
            {
                anim.SetBool("run", true);
                anim.SetBool("attack", false);
            }

            // Determine the closest path (right or left) based on the x position
            GameObject closestPath = transform.position.x >= 0f ? rightPath : leftPath;
            Vector3 pathPosition;
            if (team == Team.Blue)
            {
                // Get the positions of the path
                pathPosition = closestPath.transform.GetChild(1).position;
            }
            else
            {
                // Get the positions of the path
                pathPosition = closestPath.transform.GetChild(0).position;
            }

            // Set target to gent
            agent.SetDestination(pathPosition);
        }
    }

    public void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange);

        List<Transform> potentialTargets = new List<Transform>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Unit enemy))
            {
                // If the target's team is different from agent's team and 
                // the agent can attack the target.
                if (team != enemy.team && targets.Contains(enemy.unitType))
                {
                    potentialTargets.Add(collider.transform);
                }
            }
        }

        if (potentialTargets.Count > 0)
        {
            // Choose a random target from potentialTargets list
            int randomIndex = Random.Range(0, potentialTargets.Count);
            currentTarget = potentialTargets[randomIndex];
        }
        else
        {
            currentTarget = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}