using UnityEngine;

namespace ORZ.Enemy.Ghost
{
    public class GhostAnimatorController : MonoBehaviour
    {

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Idle()
        {
            animator.SetBool("Walk", false);
            animator.SetFloat("RandomBreak", Random.Range(0.0f, 1.0f));
        }

        public void Walk()
        {
            animator.SetBool("Walk", true);
        }

        public void Attack()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetBool("Attack", true);
            }
        }

        public void StopAttack()
        {
            animator.SetBool("Attack", false);
        }

        public Animator GetAnimator()
        {
            return animator;
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
