using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity {

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