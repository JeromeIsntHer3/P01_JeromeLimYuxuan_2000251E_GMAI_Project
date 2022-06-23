using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //This class is used to take in aniamtion events from the swing sword
    //animation, which are used to turn on and off the collider of the damage
    //box of the player
    public class CharAnimationEvents : MonoBehaviour
    {
        private Character character;

        private void Start()
        {
            character = GetComponentInParent<Character>();
        }

        public void SwordHitBoxEnable()
        {
            character.ActivateSwordHitBox();
        }

        public void SwordHitBoxDisable()
        {
            character.DisableSwordHitBox();
        }
    }
}