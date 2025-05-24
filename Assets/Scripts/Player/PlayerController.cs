using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Rigidbody2D rb;

    public Animator bodyAnimator;
    public Animator armsAnimator;

    public Transform armsTransform;
    public Camera mainCamera;

    public int maxHealth = 5;
    private int currentHealth;

    public float attackRange = 1.5f;    // радиус удара
    public float attackAngle = 90f;     // угол удара
    public int attackDamage = 1;

    private Vector2 movement;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Ввод движения
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Анимация тела через параметр isRunning
        bool isRunning = movement != Vector2.zero;
        bodyAnimator.SetBool("isRunning", isRunning);

        // Отзеркаливание спрайта по направлению движения
        if (movement.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = movement.x > 0 ? 1f : -1f;
            transform.localScale = scale;
        }

        // Поворот рук к курсору
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - armsTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -90f, 90f);
        armsTransform.rotation = Quaternion.Euler(0, 0, angle);

        // Отражение рук по вертикали, если курсор слева
        Vector3 armScale = armsTransform.localScale;
        armScale.y = (angle > 90 || angle < -90) ? -1 : 1;
        armsTransform.localScale = armScale;

        // Атака на ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            armsAnimator.Play("arms_attack");
            PerformAttack();
        }
    }

    void FixedUpdate()
    {
        // Перемещение персонажа
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void PerformAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        Debug.Log($"Количество попаданий в радиусе: {hits.Length}");
        foreach (var hit in hits)
        {
            Debug.Log($"Проверяем объект: {hit.name}, тег: {hit.tag}");
            if (hit.CompareTag("Zomb"))
            {
                Vector2 toEnemy = (hit.transform.position - armsTransform.position).normalized;
                Vector2 attackDir = armsTransform.right;
                float angleBetween = Vector2.Angle(toEnemy, attackDir);
                Debug.Log($"Угол между рукой и врагом: {angleBetween}");

                if (angleBetween < attackAngle / 2f)
                {
                    EnemyController enemy = hit.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        Debug.Log("Наносим урон врагу: " + hit.name);
                        enemy.TakeDamage(attackDamage);
                    }
                    else
                    {
                        Debug.LogWarning("EnemyController не найден на объекте " + hit.name);
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Игрок получил урон. Осталось здоровья: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Игрок погиб.");
        SceneManager.LoadScene(0);
        // Здесь можно будет добавить анимацию смерти, рестарт уровня и т.д.
    }

    void OnDrawGizmosSelected()
    {
        if (armsTransform == null) return;

        Gizmos.color = Color.red;
        Vector3 right = armsTransform.right;
        Vector3 pos = transform.position;

        Quaternion leftRot = Quaternion.Euler(0, 0, -attackAngle / 2);
        Quaternion rightRot = Quaternion.Euler(0, 0, attackAngle / 2);

        Vector3 leftDir = leftRot * right;
        Vector3 rightDir = rightRot * right;

        Gizmos.DrawLine(pos, pos + leftDir * attackRange);
        Gizmos.DrawLine(pos, pos + rightDir * attackRange);
    }
}
