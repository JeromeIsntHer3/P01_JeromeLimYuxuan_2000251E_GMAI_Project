using UnityEngine;
using System;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class HitBox : MonoBehaviour,IDamageable
    {
        Character character;
        NPC npc;

        public enum Type
        {
            Charater,
            NPC
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
            if(npc != null)
            {
                npc.Damage();
            }
        }
    }
}