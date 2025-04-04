using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    [SerializeField] private float movingSpeed = 5f;
    Vector2 inputVector;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool IsRun = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();  
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if(Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            IsRun = true;
        }
        else
        {
            IsRun = false;
        }
    }

    public bool IsRunning()
    {
        return IsRun;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public Vector3 GetPlayerArmsScreenPosition()
    {
        Vector3 playerArmsScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerArmsScreenPosition;
    }
}
