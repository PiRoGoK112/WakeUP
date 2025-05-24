using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float detectionRadius = 64f;
    public float attackRange = 0.8f;
    public int maxHealth = 3;

    public Transform attackPoint;         // Кончик руки врага
    public LayerMask playerLayer;

    private int currentHealth;
    private Transform player;
    private Rigidbody2D rb;

    public float attackCooldown = 1.5f;   // Задержка между атаками
    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        VictoryManager.enemiesAlive++;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRadius)
        {
            MoveTowardsPlayer();

            if (distance < attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                TryAttack();
                lastAttackTime = Time.time;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // Отзеркаливание по направлению движения
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x > 0 ? -1f : 1f;
            transform.localScale = scale;
        }
    }

    void TryAttack()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("attackPoint не назначен!");
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, 0.3f, playerLayer);
        if (hit)
        {
            Debug.Log("Enemy hit player!");
            hit.GetComponent<PlayerController>()?.TakeDamage(1);
        }
        else
        {
            Debug.Log("Enemy tried to attack, but player is out of range.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        VictoryManager.enemiesAlive--;
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, 0.3f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
