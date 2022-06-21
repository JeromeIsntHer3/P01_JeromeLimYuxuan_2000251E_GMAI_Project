using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{

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