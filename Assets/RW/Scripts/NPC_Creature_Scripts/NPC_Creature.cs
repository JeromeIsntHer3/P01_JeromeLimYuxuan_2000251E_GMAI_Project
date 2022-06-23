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

        private void Update()
        {
            Debug.DrawLine(go.transform.position,transform.position);
        }

        #region Tasks

        [Task]
        //To detect and find all the objects with string tag
        //which should be set to "Characters" in order to
        //take in and hold the gameObjects within the objInRange
        //array which are the gameobjects which the creature can
        //target.
        //This task returns false so that it move on to the next
        //tree but still gets ran every time the BT is repeated
        //so that if new targets are spawned the creature can target
        //them if they are close enough
        public bool CheckTargets()                                  
        {                                                           
            objInRange = GameObject.FindGameObjectsWithTag(objtag); 
            if (objInRange != null)                                 
            {                                                       
                return false;                                       
            }                                                       
            return false;                                           
        }                                                           

        [Task]
        //Animation floats are set according
        //to the velocty of the agent so that
        //the animation fits with the speed
        //of the creature.
        //The agent will move to the closest
        //target when this task is ran and
        //it will always succeed as there should
        //be target with the way the BT is setup
        public void MoveToTarget()                                                         
        {                                                                                  
            anim.SetFloat(Move_Y, agent.velocity.magnitude, smoothBlend,Time.deltaTime);   
            anim.SetFloat(Move_X, agent.angularSpeed, smoothBlend,Time.deltaTime);         
            agent.SetDestination(closestGO.transform.position);                            
            Task.current.Succeed();                                                        
        }                                                                                  
                                                                                           
        [Task]
        //Uses a forloop to go thru the
        //gameobject array (objInRange)
        //and check the distances of each
        //one of them and if they are closer
        //than the previous gameobject 
        //it will be set as the new closest
        //gameobject which is the new target
        //of the creature.
        //The task will fail so that it can
        //run through on repeat similarly
        //to "CheckTargets" so that it is 
        //mainly ignored by the BT but will
        //update if there are any targets
        public void CheckNearestTarget()                                                                 
        {                                                                                                
            foreach (GameObject go in objInRange)                                                        
            {                                                                                            
                this.go = go;                                                                            
                float distToCurrObj = Vector3.Distance(this.go.transform.position, transform.position);  
                if (distToCurrObj < distToClosestGo)                                                     
                {                                                                                        
                    distToClosestGo = distToCurrObj;                                                     
                    closestGO = go;                                                                      
                }                                                                                        
            }                                                                                            
            Task.current.Fail();                                                                         
        }

        [Task]
        //Checks the distance between the
        //target and creature and if the
        //distance is lesser(within) than
        //the seek range it returns the 
        //task as true which in the BT will
        //follow up into "MoveToTarget" 
        //and the creature will towards the
        //target gameobject
        public bool IsTargetClose()                                                             
        {                                                                                       
            float distToCurrObj = Vector3.Distance(go.transform.position, transform.position);  
            distToClosestGo = distToCurrObj;                                                    
            if (distToClosestGo <= seekRange)                                                   
            {                                                                                   
                return true;                                                                    
            }                                                                                   
            return false;                                                                       
        }                                                                                       

        [Task]
        //Check the distance between the 
        //target and the creature and if
        //the distance between them is
        //lesser(within) than the attack
        //range of the creature, task will
        //succeed and move on the "Attack"
        //Task to attack its target
        public bool CloseToAttack()                                                            
        {                                                                                      
            float distToCurrObj = Vector3.Distance(go.transform.position, transform.position); 
            distToClosestGo = distToCurrObj;                                                   
            if (distToClosestGo <= attackRange)                                                
            {                                                                                  
                agent.ResetPath();                                                             
                return true;                                                                   
            }                                                                                  
            return false;                                                                      
        }                                                                                      

        [Task]
        //Setting the animation floats to 0 and blending
        //the animtion to smooth out the creature stopping
        //and the StabAttack animation will play which
        //will trigger the animation events which will active
        //the damage box in front of the creature and damage
        //the character type in front of it
        public void Attack()                                               
        {                                                                  
            anim.SetFloat(Move_Y, 0, smoothBlend, Time.deltaTime);         
            anim.SetFloat(Move_X, 0, smoothBlend, Time.deltaTime);         
            anim.SetTrigger(StabAttack);                                   
            Task.current.Succeed();                                        
        }                                                                  

        [Task]
        //A simple check to see if the creature can be hit
        //which take the isHit boolean as its return,
        //as the isHit is changed in other tasks and methods
        public bool IsHit()         
        {                           
            return isHit;           
        }                           

        [Task]
        //When the Damaged Task is run remove health of
        //the creature and then succeed the task s
        public void Damaged()                
        {                                    
            prevHealth = currHealth;         
            currHealth -= 50f;               
            Task.current.Succeed();          
        }                                    

        [Task]
        //Task checks if the creature is dead by 
        //checking the health is 0 or lesser and if it
        //is show the death animation and destory
        //the gameobject
        //Return as true if it is dead and if not false
        //and the task is failed and moves on
        public bool IsDead()                    
        {                                       
            if (currHealth <= 0)
            {                                   
                anim.SetBool(Dead, true);       
                Destroy(gameObject);            
                return true;                    
            }                                   
            return false;                       
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