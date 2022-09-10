using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public void IncreaseHealth(TextMeshProUGUI healthStatus, int amountToAdd)
    {
        healthStatus.text = amountToAdd.ToString() + "%";
    }
}