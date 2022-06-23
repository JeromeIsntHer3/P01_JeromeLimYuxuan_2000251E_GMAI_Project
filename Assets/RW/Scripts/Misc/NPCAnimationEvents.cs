using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity {

    //This class is used to take in the animation events of the enemy
    //NPC character and then toggle the hitbox of the sword on and off
    public class NPCAnimationEvents : MonoBehaviour
    {
        private NPC npc;

        private void Start()
        {
            npc = GetComponentInParent<NPC>();
        }

        public void SwordHitBoxEnable()
        {
            npc.ActivateSwordHitBox();            
        }
        public void SwordHitBoxDisable()
        {
            npc.DisableSwordHitBox();
        }
    }
}