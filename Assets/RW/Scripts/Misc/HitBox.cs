using UnityEngine;
using System;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //This class is used to detect the damagebox from other characters
    //and then transfer the damage to the main character class according
    //to which character type it is
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
            //Detects which type of character the script is on
            //and sets the right class as its parent
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
        
        //This runs when the damage box detects this hitbox and then
        //using the IDamageable interface it runs the damage function
        //according the character type
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