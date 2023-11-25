using System.Collections;
using ORZ.Player;
using UnityEngine;
using UnityEngine.AI;

namespace ORZ.Enemy.FlyEye
{
    [RequireComponent(typeof(FlyEyeAnimationController))]
    public class FlyEyeController : EnemyController
    {
        private NavMeshAgent agent;
        private FlyEyeAnimationController ac;
        public GameObject attackRegion;
        private bool attackCoolingDown = false;
        private bool targetDetected = false;

        [SerializeField] private float detectRadius = 10.0f;

        new void Start()
        {
            base.Start();
            ac = GetComponent<FlyEyeAnimationController>();
            agent = GetComponent<NavMeshAgent>();
            agent.isStopped = true;
            agent.speed = speed;
        }

        private void Update()
        {
            targetDetected = Vector3.Distance(ObjectGetter.player.transform.position, transform.position) <= detectRadius;
            if (targetDetected)
            {
                agent.isStopped = false;
                SetDestination();
                Move();
                Attack();
            }
            else
            {
                agent.isStopped = true;
                Idle();
            }
        }

        private void SetDestination()
        {
            if (ObjectGetter.player == null) return;
            agent.SetDestination(ObjectGetter.player.transform.position);
        }

        private void FinishAttackCoolDown()
        {
            attackCoolingDown = false;
        }

        private void Move()
        {
            
            ac.Locomotion = agent.velocity.magnitude / agent.speed;
            ac.AlertLevel = agent.stoppingDistance - (ObjectGetter.player.transform.position - transform.position).magnitude;
        }

        private void Attack()
        {
            if ((ObjectGetter.player.transform.position - transform.position).magnitude < 0.4f && !attackCoolingDown)
            {
                ac.Attack();
                attackCoolingDown = true;
                Invoke(nameof(FinishAttackCoolDown), 0.2f);
                AttackDetect();
            }
        }

        void AttackDetect()
        {
            // Debug.Log("Here!");
            if (attackRegion.GetComponent<CapsuleCollider>().bounds.Contains(ObjectGetter.player.transform.position))
            {
                ObjectGetter.player.GetComponent<PlayerController>().TakeDamage();
            }
        }

        private void Idle()
        {
            ac.Locomotion = 0;
            ac.AlertLevel = 0;
            ac.IdleBreak();
        }

        public override IEnumerator Freeze(float time)
        {
            LongTimeFreeze();
            deFreeze = StartCoroutine(DeFreeze(time));
            yield return null;
        }

        public override void LongTimeFreeze()
        {
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            IsFreezing = true;
            ac.Pause();
        }

        public override IEnumerator DeFreeze(float time)
        {
            yield return new WaitForSeconds(time);
            agent.speed = defaultSpeed;
            IsFreezing = false;
            ac.Play();
        }

    }

}
