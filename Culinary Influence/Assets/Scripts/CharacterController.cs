using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [Header("Physics")] [SerializeField] private Vector2 centerOfMass;

    [Header("Walking")] [SerializeField] private float speed;
    [SerializeField] [Range(0, .3f)] private float movementSmoothing = .05f;

    [Header("Jumping")] [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;
    private Rigidbody2D _body;
    private bool _isGrounded;


    private Vector2 _storedVelocity = Vector2.zero;

    private Vector3 GroundCheckPosition => transform.position + (Vector3) groundCheckOffset;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.centerOfMass = centerOfMass;
    }


    // Update is called once per frame
    private void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        MoveController(hInput);

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

    private void CalculateGroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(
            GroundCheckPosition,
            groundCheckRadius,
            groundMask
        );
    }

    private void MoveController(float direction)
    {
        Vector2 velocity = _body.velocity;
        float stepCount = direction * speed;

        Vector3 targetVelocity = new Vector2(stepCount, velocity.y);

        // And then smoothing it out and applying it to the character
        _body.velocity = Vector2.SmoothDamp(
            velocity,
            targetVelocity,
            ref _storedVelocity,
            movementSmoothing
        );
    }
}