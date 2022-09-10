using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public TextMeshProUGUI playerHealth;

    public void Init(HealthSystem healthSystem)
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(float arg1, Vector2 arg2)
    {
        playerHealth.text = $"{arg1}%";
    }

}
