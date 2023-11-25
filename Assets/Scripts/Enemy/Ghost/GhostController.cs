using System.Collections;
using ORZ.Player;
using ORZ.Utility;
using UnityEngine;

namespace ORZ.Enemy.Ghost
{
    public class GhostController : EnemyController
    {
        [SerializeField] protected float stopDistance = 0.8f;

        private GhostAnimatorController animator;
        private bool tracing = false;
        public GameObject attackRegion;
        public float attackDelay = 1f;
        private bool attacking = false;

        void Awake()
        {
            animator = GetComponent<GhostAnimatorController>();
        }

        new void Start()
        {
            base.Start();
        }

        public override IEnumerator Freeze(float time)
        {
            tracing = false;
            IsFreezing = true;
            animator.Pause();
            deFreeze = StartCoroutine(DeFreeze(time));
            yield return null;
        }

        public override void LongTimeFreeze()
        {
            tracing = false;
            IsFreezing = true;
            animator.Pause();
        }

        public override IEnumerator DeFreeze(float time)
        {
            yield return new WaitForSeconds(time);
            tracing = GetComponent<SphereCollider>().bounds.Contains(ObjectGetter.player.transform.position);
            IsFreezing = false;
            animator.Play();
        }

        void FixedUpdate()
        {
            Action();
            if (tracing)
            {
                Move();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                tracing = true;
            }
        }

        void Move()
        {
            if (IsFreezing) return;
            float distance = Vector3.Distance(ObjectGetter.player.transform.position, transform.position);
            if ( distance > stopDistance)
            {
                Vector2 direction = Utils.Direction3To2(transform.position, ObjectGetter.player.transform.position);
                float angle = Utils.AngleOfDirection(direction);
                transform.localEulerAngles = new Vector3(0, angle, 0);
                transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
            }

            if (attackRegion.GetComponent<BoxCollider>().bounds.Contains(ObjectGetter.player.transform.position))
            {
                Attack();
            }
            else
            {
                animator.StopAttack();
                tracing = distance > stopDistance;
            }

        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                tracing = false;
            }
        }

        void Attack()
        {
            animator.Attack();
            if (!attacking)
            {
                attacking = true;
                Invoke("AttackDetect", attackDelay);
            }
            
        }

        void AttackDetect()
        {
            if (attackRegion.GetComponent<BoxCollider>().bounds.Contains(ObjectGetter.player.transform.position))
            {
                ObjectGetter.player.GetComponent<PlayerController>().TakeDamage();
            }

            attacking = false;
        }

        void Action()
        {
            if (!animator.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (tracing)
                {
                    animator.Walk();
                }
                else
                {
                    animator.Idle();
                }
            }
        }
    }
}
