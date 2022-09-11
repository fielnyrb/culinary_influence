using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    private const float MaxDamageForce = 100.0f;

    [Header("Physics")] [SerializeField] private Vector2 centerOfMass;

    [Header("Walking")] [SerializeField] private float speed;
    [SerializeField] private float speedInAir;

    [Header("Jumping")] [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;

    [Header("Attack")] [SerializeField] private AttackController attackController;
    [SerializeField] public HealthSystem healthSystem;
    [SerializeField] public ParticleSystem lightAttackParticles;
    [SerializeField] public ParticleSystem heavyAttackParticles;

    private Rigidbody2D _body;

    private float _direction;

    private float _facingDirection;

    private bool _isGrounded;

    private Vector3 GroundCheckPosition => transform.position + (Vector3) groundCheckOffset;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.centerOfMass = centerOfMass;

        healthSystem.OnDamaged += Damage;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        CalculateMovement();
        CalculateGroundCheck();
    }

    private void OnDestroy()
    {
        healthSystem.OnDamaged -= Damage;
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
        _direction = direction.x;

        if (_direction == 0.0f)
        {
            return;
        }

        // the direction can range from -1.0...1.0
        // the facing direction however, requires to be a whole number (-1 or 1)
        _facingDirection = Mathf.Sign(_direction);
    }

    private void OnJump()
    {
        if (!_isGrounded)
        {
            return;
        }

        _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    private void OnAttackLight()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        attackController.AttackLight(_facingDirection, thrust => _body.AddForce(thrust, ForceMode2D.Impulse));
        lightAttackParticles.Play();
    }

    private void OnAttackHeavy()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        attackController.AttackHeavy(_facingDirection, thrust => _body.AddForce(thrust, ForceMode2D.Impulse));
        heavyAttackParticles.Play();
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
        float s = _isGrounded ? speed : speedInAir;
        _body.AddForce(Vector2.right * _direction * s * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Damage(float amount, Vector2 direction)
    {
        //enabled = false;
        float appliedForce = MaxDamageForce / 100 * amount;
        _body.AddForce(direction * appliedForce, ForceMode2D.Impulse);
    }
}