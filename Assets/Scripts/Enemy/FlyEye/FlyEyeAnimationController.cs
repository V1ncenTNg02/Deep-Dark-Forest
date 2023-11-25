using UnityEngine;
using Random = UnityEngine.Random;

namespace ORZ.Enemy.FlyEye
{
    public class FlyEyeAnimationController : MonoBehaviour
    {
        private Animator animator;

        private float locomotion;
        public float Locomotion
        {
            get => locomotion;
            set
            {
                locomotion = value;
                animator.SetFloat("locomotion", Mathf.Clamp(value, -1f, 1f));
            } 
        }

        private float alertLevel;

        public float AlertLevel
        {
            get => alertLevel;
            set
            {
                alertLevel = value;
                animator.SetFloat("alertLevel", Mathf.Clamp(value, 0f, 1f));
            }
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// EyeBall Attack by randomly choose one of attack motion
        /// </summary>
        public void Attack()
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if (asi.IsTag("Attack")) return;
            switch (Random.Range(0,2))
            {
                case 0: animator.SetTrigger("attack1"); break;
                case 1: animator.SetTrigger("attack2"); break;
                default: Debug.LogError("Something must going wrong here..."); break;
            }
        }

        public void IdleBreak()
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if (asi.IsName("IdleBreak")) return;
            if (Random.Range(0f, 1f) > 0.2f) return;
            animator.SetTrigger("idleBreak");
        }

        public void GotHit()
        {
            animator.SetTrigger("gotHit");
        }

        public void Alert()
        {
            animator.SetTrigger("alert");
        }

        public void Pause()
        {
            animator.speed = 0f;
        }

        public void Play()
        {
            animator.speed = 1.0f;
        }
    }

}
