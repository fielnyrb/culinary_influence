using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Ignore yourself
        if (col.gameObject == gameObject)
        {
            return;
        }

        Vector3 towardsTarget = col.transform.position - controller.transform.position;
        Vector3 direction = towardsTarget.normalized;

        if (Mathf.Sign(direction.x) == Mathf.Sign(controller.Direction))
        {
            Debug.Log("asd");
        }
    }
}