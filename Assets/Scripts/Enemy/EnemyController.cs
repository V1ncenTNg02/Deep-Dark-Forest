using System.Collections;
using UnityEngine;

namespace ORZ.Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        [Header("Control")]
        public float defaultSpeed = 5.0f;

        protected float speed;
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public bool IsFreezing { get; set; } = false;
        protected Coroutine deFreeze = null;

        // Start is called before the first frame update
        protected void Start() => speed = defaultSpeed;

        // Freeze the monster, automatically de-freeze after "time" seconds
        public abstract IEnumerator Freeze(float time);

        // De-freeze the monster after "time" seconds
        public abstract IEnumerator DeFreeze(float time);

        public void StopDeFreeze()
        {
            if (deFreeze != null)
            {
                StopCoroutine(deFreeze);
            }
        }

        // Freeze the monster until Defreeze is called manually
        public abstract void LongTimeFreeze();
    }

}
