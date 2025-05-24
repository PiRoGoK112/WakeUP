using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Rigidbody2D rb;

    public Animator bodyAnimator;
    public Animator armsAnimator;
    

    public Transform armsTransform;
    public Camera mainCamera;

    private Vector2 movement;

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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;// Ограничиваем угол
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
        }
    }

    void FixedUpdate()
    {
        // Перемещение персонажа
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
