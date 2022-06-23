using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //This class is used to take in animation events from the stab attack
    //animation, which is used to turn on and off the collider of the damage
    //box of the creature
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