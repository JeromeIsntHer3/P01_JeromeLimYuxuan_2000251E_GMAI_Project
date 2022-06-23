using UnityEngine;

//This script detects if the other gameobject derives from the IDamageable
//interface which then calls the Damage Function in that gameobject
//which then removes health from that gameobject and the gameobject will react
//accordingly to what is scripted to itself
public class DamageBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if(damageable != null)
            damageable.Damage();
    }
}