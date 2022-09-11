using System;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public TextMeshProUGUI playerHealth;
    private HealthSystem _healthSystem;
    

    public void Init(HealthSystem healthSystem)
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        }
        _healthSystem = healthSystem;
        
        playerHealth.text = "0%";
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void OnDestroy()
    {
        if (_healthSystem)
        {
            _healthSystem.OnDamaged -= HealthSystem_OnDamaged;    
        }
        
    }

    private void HealthSystem_OnDamaged(float arg1, Vector2 arg2)
    {
        playerHealth.text = $"{arg1}%";
    }
}