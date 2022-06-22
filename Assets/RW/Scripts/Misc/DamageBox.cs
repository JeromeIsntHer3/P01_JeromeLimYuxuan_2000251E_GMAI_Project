using UnityEngine;

public class DamageBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if(damageable != null)
            damageable.Damage();
    }
}