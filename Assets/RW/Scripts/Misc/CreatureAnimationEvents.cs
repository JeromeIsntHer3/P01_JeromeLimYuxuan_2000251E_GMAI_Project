using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class CreatureAnimationEvents : MonoBehaviour
    {
        private NPC_Creature creature;

        private void Start()
        {
            creature = GetComponentInParent<NPC_Creature>();
        }

        public void StabHitboxOn()
        {
            creature.EnableStabHitbox();
        }

        public void StabHitboxOff()
        {
            creature.DisableStabHitbox();
        }
    }
}