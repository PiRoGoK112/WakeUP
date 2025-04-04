using UnityEngine;
using UnityEngine.Rendering;

public class SwordVisual : MonoBehaviour
{
    private Animator animator;
    private const string ATTACK = "Attack";
    [SerializeField] private Sword sword;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }
}
