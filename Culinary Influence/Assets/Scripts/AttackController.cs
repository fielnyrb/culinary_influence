using System;
using System.Linq;
using UnityEngine;

public enum ThrustDirection
{
    Horizontal,
    Vertical
}

[Serializable]
public class AttackDefinition
{
    [Range(0f, 100f)] public float damageAmount;
    public float cooldown;
    public float thrustForce;
    public ThrustDirection direction;
    public AudioClip[] clips;

    private float _lastTimeSinceAttack;

    public bool CanAttack => Time.time - _lastTimeSinceAttack > cooldown;

    public void RecordAttack()
    {
        _lastTimeSinceAttack = Time.time;
    }

    public AudioClip getRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}

public class AttackController : MonoBehaviour
{
    [SerializeField] private AttackDefinition lightAttack;
    [SerializeField] private AttackDefinition heavyAttack;

    [SerializeField] private LayerMask target;
    [SerializeField] private float areaRadius;
    [SerializeField] private AudioSource source;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }

    private static Vector2 GenerateThrustForce(float facingDirection, AttackDefinition attack)
    {
        Vector2 thrustDirection = attack.direction switch
        {
            ThrustDirection.Horizontal => Vector2.right,
            ThrustDirection.Vertical => Vector2.up,
            _ => throw new ArgumentOutOfRangeException()
        };

        return thrustDirection * facingDirection * attack.thrustForce;
    }

    public void AttackLight(float facingDirection, Action<Vector2> onThrust)
    {
        TryAttackOpponent(lightAttack, facingDirection, onThrust);
        source.clip = lightAttack.getRandomClip();
        source.Play();
    }

    public void AttackHeavy(float facingDirection, Action<Vector2> onThrust)
    {
        TryAttackOpponent(heavyAttack, facingDirection, onThrust);
        source.clip = lightAttack.getRandomClip();
        source.Play();
    }

    private bool TryGetOpponent(out HealthSystem health)
    {
        health = null;

        var targets = new Collider2D[2];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, areaRadius, targets, target);
        if (size <= 0)
        {
            return false;
        }

        Collider2D opponent = targets
            .Where(t => t)
            .FirstOrDefault(t => t.gameObject != gameObject);
        if (opponent == null)
        {
            return false;
        }

        health = opponent.GetComponent<HealthSystem>();
        return health != null;
    }

    private void TryAttackOpponent(AttackDefinition attack, float facingDirection, Action<Vector2> onThrust)
    {
        if (!attack.CanAttack)
        {
            return;
        }

        attack.RecordAttack();

        onThrust(GenerateThrustForce(facingDirection, attack));

        if (!TryGetOpponent(out HealthSystem opponent))
        {
            return;
        }

        Vector3 direction = (opponent.transform.position - transform.position).normalized;

        if (Mathf.Sign(direction.x) != Mathf.Sign(facingDirection))
        {
            return;
        }

        opponent.Damage(attack.damageAmount, direction);
    }
}