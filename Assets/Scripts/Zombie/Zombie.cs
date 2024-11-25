using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public float health = 100f;
    public GameObject bloodEffectPrefab;
    public Animator zombieAnimator;
    public float detectionRange = 10f; 
    public float attackRange = 2f;
    public float damage = 10f;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    public float chaseSpeed = 3.5f;
    public Vector3 rotationOffset;
    public Transform playerTransform;
    private PlayerHealth playerHealth;

    private NavMeshAgent navMeshAgent;

    private bool isRunning = false;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private Collider mainCollider;
    public AudioSource runSound;

    public bool isDead = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        mainCollider = GetComponent<Collider>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        navMeshAgent.updateRotation = false;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (playerTransform != null && isGrounded)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                StopZombie();
                AttackPlayer();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                direction.y = 0;
                ChasePlayer(direction);
            }
            else
            {
                StopZombie();
            }
        }
    }

    void ChasePlayer(Vector3 direction)
    {
        if (!isRunning)
        {
            zombieAnimator.SetTrigger("Run");
            runSound.Play();
            isRunning = true;
            navMeshAgent.speed = chaseSpeed;
        }

        navMeshAgent.SetDestination(playerTransform.position);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(rotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void StopZombie()
    {
        if (isRunning)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = true;
            }
            isRunning = false;
        }
    }

    void AttackPlayer()
    {
        if (isDead) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (zombieAnimator != null)
            {
                zombieAnimator.Play("Kick");
                StartCoroutine(DealDamageAfterAnimation());
                lastAttackTime = Time.time;
            }
        }
    }

    IEnumerator DealDamageAfterAnimation()
    {
        if (zombieAnimator != null)
        {
            yield return new WaitForSeconds(0.5f); 

            if (playerHealth != null && Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        mainCollider.enabled = false;
        zombieAnimator.Play("Die");
        navMeshAgent.isStopped = true;
        Destroy(gameObject, 4f);
    }
}
