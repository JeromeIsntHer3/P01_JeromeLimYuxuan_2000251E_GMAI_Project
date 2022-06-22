using UnityEngine;
using UnityEngine.AI;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class NPC : MonoBehaviour
    {
        #region Variables

        public enum NPCType
        {
            Patrol,Idle
        }
        public NPCType nPCType;

        //For Pathfinding
        public NavMeshAgent agent;
        public Transform target;
        public Rigidbody rb;

        //state variables
        public NPCStateMachine mainMachine;
        public IdleState idle;
        public SeekState seek;
        public PatrolState patrol;
        public AttackState attack;
        public DamagedState damaged;

#pragma warning disable 0649
        [SerializeField]
        private Transform handTransform;
        [SerializeField]
        private CharacterData data;
        [SerializeField]
        private Collider hitBox; //change back private later
        [SerializeField]
        private BoxCollider damageBox;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private float damageTaken;

        private GameObject currentWeapon;
        private Quaternion currentRotation;
        //Health Related Variables
        public float currHealth;
        public float prevHealth;
        public bool isHit;
        private float currTime;
        private int horizonalMoveParam = Animator.StringToHash("H_Speed");
        private int verticalMoveParam = Animator.StringToHash("V_Speed");
        #endregion

        #region Properties

        public float NormalColliderHeight => data.normalColliderHeight;
        public float MovementSpeed => data.movementSpeed;
        public float RotationSpeed => data.rotationSpeed;
        public GameObject MeleeWeapon => data.meleeWeapon;
        public float Health => data.Health;
        public int isMelee => Animator.StringToHash("IsMelee");
        public int crouchParam => Animator.StringToHash("Crouch");
        //Using Properties to get the int Param from the Animator of this character
        public int hit => Animator.StringToHash("Hit");
        public int isDead => Animator.StringToHash("IsDead");
        public int isClose => Animator.StringToHash("IsClose");

        //Find the capsulecollider component height and set the new values
        public float ColliderSize
        {
            get => GetComponent<CapsuleCollider>().height;

            set
            {
                GetComponent<CapsuleCollider>().height = value;
                Vector3 center = GetComponent<CapsuleCollider>().center;
                center.y = value / 2f;
                GetComponent<CapsuleCollider>().center = center;
            }
        }

        #endregion

        #region Methods
        public void SetNPCAnimation(float value,float blend)
        {
            SetAnimationFloat("V_Speed", value, blend);
            SetAnimationFloat("H_Speed", value * 2, blend);
        }

        public void ApplyImpulse(Vector3 force)
        {
            GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

        public void SetAnimationBool(int param, bool value)
        {
            anim.SetBool(param, value);
        }

        public void SetAnimationFloat(string param, float value, float damp)
        {
            anim.SetFloat(param, value,damp, Time.deltaTime);
        }

        public void TriggerAnimation(int param)
        {
            anim.SetTrigger(param);
        }

        public void Equip(GameObject weapon = null)
        {
            if (weapon != null)
            {
                currentWeapon = Instantiate(weapon, handTransform.position, handTransform.rotation, handTransform);
            }
            else
            {
                ParentCurrentWeapon(handTransform);
            }
        }

        public void ActivateHitBox()
        {
            hitBox.enabled = true;
        }

        public void DeactivateHitBox()
        {
            hitBox.enabled = false;
        }

        public void ActivateSwordHitBox()
        {
            damageBox.enabled = true;
        }

        public void DisableSwordHitBox()
        {
            damageBox.enabled = false;
        }

        private void ParentCurrentWeapon(Transform parent)
        {
            if (currentWeapon.transform.parent == parent)
            {
                return;
            }

            currentWeapon.transform.SetParent(parent);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
        }

        public float PlayerNPCDist()
        {
            return Vector3.Distance(agent.transform.position,target.transform.position);
        }

        public void Damage()
        {
            if (currHealth > 0)
            {
                prevHealth = currHealth;
                currHealth -= damageTaken;
                isHit = true;
                Debug.Log("Character is hit");
            }
        }
        #endregion

        #region MonoBehaviour Callbacks
        void OnEnable()
        {
            ActivateHitBox();
        }

        void OnDisable()
        {
            DeactivateHitBox();
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            currHealth = Health;
        }

        void Start()
        {
            mainMachine = new NPCStateMachine();

            idle = new IdleState(this,mainMachine);

            seek = new SeekState(this, mainMachine);

            patrol = new PatrolState(this, mainMachine);

            attack = new AttackState(this, mainMachine);

            damaged = new DamagedState(this, mainMachine); 

            mainMachine.Initialize(idle);

            Equip(MeleeWeapon);
        }

        void Update()
        {
            mainMachine.CurrentState.LogicUpdate();
        }

        void FixedUpdate()
        {
            mainMachine.CurrentState.PhysicsUpdate();
        }
        #endregion
    }
}
