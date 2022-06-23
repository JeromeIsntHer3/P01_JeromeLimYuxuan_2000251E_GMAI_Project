using UnityEngine;
using System;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class HitBox : MonoBehaviour,IDamageable
    {
        Character character;
        NPC npc;
        NPC_Creature creature;

        public enum Type
        {
            Charater,
            NPC,
            Creature
        }

        public Type characterType;

        private void Awake()
        {
            switch (characterType)
            {
                case Type.Charater:
                    character = GetComponentInParent<Character>();
                    break;
                case Type.NPC:
                    npc = GetComponentInParent<NPC>();
                    break;
                case Type.Creature:
                    creature = GetComponentInParent<NPC_Creature>();
                    break;
                default:
                    break;
            }
        }
        
        public void Damage()
        {
            if (character != null)
            {
                character.Damage();
            }
            else if(npc != null)
            {
                npc.Damage();
            }
            else if(creature != null)
            {
                creature.Damage();
            }
        }
    }
}