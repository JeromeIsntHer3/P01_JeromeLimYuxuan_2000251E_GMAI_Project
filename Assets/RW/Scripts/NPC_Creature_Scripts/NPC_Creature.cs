using UnityEngine;
using Panda;
using UnityEngine.AI;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class NPC_Creature : MonoBehaviour
    {
        //String Tag to know what the creature can take in as a target
        public string objtag;
        //Current Health of the creature
        public float currHealth;
        //Distance at which the creature will detect other character types
        public float seekRange;
        //Distance at which the creature will attack the current target
        public float attackRange;
        //Float to smooth out the animation transition within the blend tree
        public float smoothBlend;
        //GameObject array to hold the gameobjects the creature can target
        public GameObject[] objInRange;
        //Hurtbox of the creature that will be activated when it is attacking
        public BoxCollider damageBox;

        //Prev Health to hold the health of the creature before it was damaged
        private float prevHealth;
        //Distance to the closest gameobject
        private float distToClosestGo;
        //Check to see if the creature has been hit 
        private bool isHit;
        //Hold animator component of the creature
        private Animator anim;
        //Hold navmeshagent component of the creature
        private NavMeshAgent agent;
        //Hold the closest game object to the creature
        private GameObject closestGO;
        //Generic gameobject to be replaced in loops
        private GameObject go;        

        //Animations to Int Params
        public int Move_X => Animator.StringToHash("Move_X");
        public int Move_Y => Animator.StringToHash("Move_Y");
        public int StabAttack => Animator.StringToHash("Stab Attack");
        public int TakeDamage => Animator.StringToHash("Take Damage");
        public int Dead => Animator.StringToHash("IsDead");


        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            //Setting to Infinity so that it can detect the closest enemy even if is part away
            distToClosestGo = Mathf.Infinity;
            //Set the base health of the creature to 50
            currHealth = 50f;
        }

        #region Tasks
        [Task]
        public bool CheckTargets()                                  //To detect and find all the objects with string tag
        {                                                           //which should be set to "Characters" in order to
            objInRange = GameObject.FindGameObjectsWithTag(objtag); //take in and hold the gameObjects within the objInRange
            if (objInRange != null)                                 //array which are the gameobjects which the creature can
            {                                                       //target.
                return false;                                       //This task returns false so that it move on to the next
            }                                                       //tree but still gets ran every time the BT is repeated
            return false;                                           //so that if new targets are spawned the creature can target
        }                                                           //them if they are close enough

        [Task]
        public void MoveToTarget()                                                         //Animation floats are set according
        {                                                                                  //to the velocty of the agent so that
            anim.SetFloat(Move_Y, agent.velocity.magnitude, smoothBlend,Time.deltaTime);   //the animation fits with the speed
            anim.SetFloat(Move_X, agent.angularSpeed, smoothBlend,Time.deltaTime);         //of the creature.
            agent.SetDestination(closestGO.transform.position);                            //The agent will move to the closest
            Task.current.Succeed();                                                        //target when this task is ran and
        }                                                                                  //it will always succeed as there should
                                                                                           //be target with the way the BT is setup
        [Task]
        public void CheckNearestTarget()                                                            //Uses a forloop to go thru the
        {                                                                                           //gameobject array (objInRange)
            for(int i = 0; i < objInRange.Length; i++)                                              //and check the distances of each
            {                                                                                       //one of them and if they are closer
                go = objInRange[i];                                                                 //than the previous gameobject 
                float distToCurrObj = Vector3.Distance(go.transform.position, transform.position);  //it will be set as the new closest
                if(distToCurrObj < distToClosestGo)                                                 //gameobject which is the new target
                {                                                                                   //of the creature.
                    distToClosestGo = distToCurrObj;                                                //The task will fail so that it can
                    closestGO = go;                                                                 //run through on repeat similarly
                }                                                                                   //to "CheckTargets" so that it is 
            }                                                                                       //mainly ignored by the BT but will
            Task.current.Fail();                                                                    //update if there are any targets
        }

        [Task]
        public bool IsTargetClose()                                                             //Checks the distance between the
        {                                                                                       //target and creature and if the
            float distToCurrObj = Vector3.Distance(go.transform.position, transform.position);  //distance is lesser(within) than
            distToClosestGo = distToCurrObj;                                                    //the seek range it returns the 
            if (distToClosestGo <= seekRange)                                                   //task as true which in the BT will
            {                                                                                   //follow up into "MoveToTarget" 
                return true;                                                                    //and the creature will towards the
            }                                                                                   //target gameobject
            return false;                                                                       
        }                                                                                       

        [Task]
        public bool CloseToAttack()                                                            //Check the distance between the 
        {                                                                                      //target and the creature and if
            float distToCurrObj = Vector3.Distance(go.transform.position, transform.position); //the distance between them is
            distToClosestGo = distToCurrObj;                                                   //lesser(within) than the attack
            if (distToClosestGo <= attackRange)                                                //range of the creature, task will
            {                                                                                  //succeed and move on the "Attack"
                agent.ResetPath();                                                             //Task to attack its target
                return true;                                                                   
            }                                                                                  
            return false;                                                                      
        }                                                                                      

        [Task]
        public void Attack()                                               //Setting the animation floats to 0 and blending
        {                                                                  //the animtion to smooth out the creature stopping
            anim.SetFloat(Move_Y, 0, smoothBlend, Time.deltaTime);         //and the StabAttack animation will play which
            anim.SetFloat(Move_X, 0, smoothBlend, Time.deltaTime);         //will trigger the animation events which will active
            anim.SetTrigger(StabAttack);                                   //the damage box in front of the creature and damage
            Task.current.Succeed();                                        //the character type in front of it
        }                                                                  

        [Task]
        public bool IsHit()         //A simple check to see if the creature can be hit
        {                           //which take the isHit boolean as its return,
            return isHit;           //as the isHit is changed in other tasks and methods
        }                           

        [Task]
        public void Damaged()                
        {                                    
            prevHealth = currHealth;         //When the Damaged Task is run remove health of
            currHealth -= 50f;               //the creature and then succeed the task 
            Task.current.Succeed();          
        }                                    

        [Task]
        public bool IsDead()                    
        {                                       
            if (currHealth <= 0)
            {                                   //Task checks if the creature is dead by 
                anim.SetBool(Dead, true);       //checking the health is 0 or lesser and if it
                Destroy(gameObject);            //is show the death animation and destory
                return true;                    //the gameobject
            }                                   //Return as true if it is dead and if not false
            return false;                       //and the task is failed and moves on
        }
        #endregion


        #region Methods/Functions
        public void EnableStabHitbox()          
        {                                       
            damageBox.enabled = true;           
        }
                                                //Simple Functions to enable and disable the damageboxes
        public void DisableStabHitbox()         //that are referenced from the creature animation events
        {                                       //scripts that take in the events and trigger these
            damageBox.enabled = false;          //funtions
        }

        public void Damage()                     //Damage function that is triggered through interaction
        {                                        //with a damagebox and runs the interface damage
            isHit = true;                        //function and sets isHit to true
        }
        #endregion
    }
}