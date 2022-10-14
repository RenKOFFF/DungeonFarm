using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    public Vector2 lastMotionVector;

    private Animator animator;
    private Vector2 motionVector;
    private Rigidbody2D rigidbody2d;
    private bool isMoving;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        const string horizontalAxisName = "Horizontal";
        const string verticalAxisName = "Vertical";

        var horizontalAxisRaw = Input.GetAxisRaw(horizontalAxisName);
        var verticalAxisRaw = Input.GetAxisRaw(verticalAxisName);

        motionVector = new Vector2(horizontalAxisRaw, verticalAxisRaw);

        animator.SetFloat(horizontalAxisName, horizontalAxisRaw);
        animator.SetFloat(verticalAxisName, verticalAxisRaw);

        isMoving = horizontalAxisRaw != 0 || verticalAxisRaw != 0;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            lastMotionVector = new Vector2(horizontalAxisRaw, verticalAxisRaw).normalized;

            animator.SetFloat("LastHorizontal", horizontalAxisRaw);
            animator.SetFloat("LastVertical", verticalAxisRaw);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * speed;
    }
}
