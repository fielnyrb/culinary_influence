using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    private static readonly int LightAttack = Animator.StringToHash("Light Attack");

    [Header("Physics")] [SerializeField] private Vector2 centerOfMass;

    [Header("Walking")] [SerializeField] private float speed;
    [SerializeField] [Range(0, .3f)] private float movementSmoothing = .05f;

    [Header("Jumping")] [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;

    [Header("Attack")] [SerializeField] private float lightAttackDistance;

    [Header("Other")] [SerializeField] private Animator animator;

    private Rigidbody2D _body;

    private bool _isGrounded;

    private Vector2 _storedVelocity = Vector2.zero;

    public float Direction { get; private set; }
    public float FacingDirection { get; private set; }


    private Vector3 GroundCheckPosition => transform.position + (Vector3) groundCheckOffset;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateMovement();
        CalculateGroundCheck();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position + (Vector3) centerOfMass, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheckPosition, groundCheckRadius);
    }

    private void OnMove(InputValue input)
    {
        var direction = input.Get<Vector2>();
        Direction = direction.x;

        if (Direction == 0.0f)
        {
            return;
        }

        // the direction can range from -1.0...1.0
        // the facing direction however, requires to be a whole number (-1 or 1)
        FacingDirection = Mathf.Sign(Direction);
    }

    private void OnJump()
    {
        if (!_isGrounded)
        {
            return;
        }

        _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    private void OnAttack()
    {
        animator.SetTrigger(LightAttack);
    }

    public void LightThrust()
    {
        _body.AddForce(Vector2.right * FacingDirection * lightAttackDistance, ForceMode2D.Impulse);
    }

    private void CalculateGroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(
            GroundCheckPosition,
            groundCheckRadius,
            groundMask
        );
    }

    private void CalculateMovement()
    {
        Vector2 velocity = _body.velocity;
        float stepCount = Direction * speed;

        Vector3 targetVelocity = new Vector2(stepCount, velocity.y);

        // And then smoothing it out and applying it to the character
        _body.velocity = Vector2.SmoothDamp(
            velocity,
            targetVelocity,
            ref _storedVelocity,
            movementSmoothing
        );
    }

    public void Damage(float amount, float direction)
    {
        Debug.Log("Ouch! You damaged me with " +  amount.ToString() + " in " + direction.ToString() + " direction!");
    }
}