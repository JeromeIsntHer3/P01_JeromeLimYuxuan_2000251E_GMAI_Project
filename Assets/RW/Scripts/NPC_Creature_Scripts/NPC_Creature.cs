using UnityEngine;
using Panda;
using UnityEngine.AI;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class NPC_Creature : MonoBehaviour
    {
        public string objtag;
        public float currHealth;
        public float seekRange;
        public float attackRange;
        public float smoothBlend;
        public GameObject[] objInRange;
        public BoxCollider damageBox;

        private float prevHealth;
        private float distToClosestGo;
        private bool isHit;
        private Animator anim;
        private NavMeshAgent agent;
        private GameObject closestGO;
        private GameObject go;        

        //Animations
        public int Move_X => Animator.StringToHash("Move_X");
        public int Move_Y => Animator.StringToHash("Move_Y");
        public int StabAttack => Animator.StringToHash("Stab Attack");
        public int TakeDamage => Animator.StringToHash("Take Damage");
        public int Dead => Animator.StringToHash("IsDead");


        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            distToClosestGo = Mathf.Infinity;
            currHealth = 50f;
        }

        void Update()
        {

        }

        #region Tasks
        [Task]
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
        public void MoveToTarget()
        {
            anim.SetFloat(Move_Y, agent.velocity.magnitude, smoothBlend,Time.deltaTime);
            anim.SetFloat(Move_X, agent.angularSpeed, smoothBlend,Time.deltaTime);
            agent.SetDestination(closestGO.transform.position);
            Task.current.Succeed();
        }

        [Task]
        public void CheckNearestTarget()
        {
            for(int i = 0; i < objInRange.Length; i++)
            {
                go = objInRange[i];
                float distToCurrObj = Vector3.Distance(go.transform.position, transform.position);
                if(distToCurrObj < distToClosestGo)
                {
                    distToClosestGo = distToCurrObj;
                    closestGO = go;
                }
            }
            Task.current.Fail();
        }

        [Task]
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
        public void Attack()
        {
            anim.SetFloat(Move_Y, 0, smoothBlend, Time.deltaTime);
            anim.SetFloat(Move_X, 0, smoothBlend, Time.deltaTime);
            anim.SetTrigger(StabAttack);
            Task.current.Succeed();
        }

        [Task]
        public bool IsHit()
        {
            return isHit;
        }

        [Task]
        public void Damaged()
        {
            prevHealth = currHealth;
            currHealth -= 50f;
            Task.current.Succeed();
        }

        [Task]
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

        public void DisableStabHitbox()
        {
            damageBox.enabled = false;
        }

        public void Damage()
        {
            isHit = true;
            Debug.Log(IsHit());
        }
        #endregion
    }
}