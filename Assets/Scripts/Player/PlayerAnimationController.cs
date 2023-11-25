using UnityEngine;

namespace ORZ.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;
        
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Idle()
        {
            animator.SetInteger("State", 0);
        }

        public void Walk()
        {
            animator.SetInteger("State", 1);
        }

        public void Run()
        {
            animator.SetInteger("State", 2);
        }

        public Animator GetAnimator()
        {
            return animator;
        }

    }

}
