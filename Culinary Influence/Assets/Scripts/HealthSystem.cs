using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float _currentPercentage = 1.0f;

    public event Action<float, Vector2> OnDamaged;

    public void Damage(float amount, Vector2 direction)
    {
        _currentPercentage += amount;
        OnDamaged?.Invoke(_currentPercentage, direction);

        Debug.Log($"Ouch! You damaged me with {amount} from {direction}! My Percentage is now {_currentPercentage}!");
    }
}