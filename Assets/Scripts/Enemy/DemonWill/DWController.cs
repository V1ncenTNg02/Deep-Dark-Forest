// Control the movement of Demon Will

using System.Collections;
using ORZ.Player;
using UnityEngine;

namespace ORZ.Enemy.DemonWill
{
    public class DWController : EnemyController
    {
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 2.5f;
        public float attackDelay = 1f;
        DWLegController myDwLegController;
        private bool isCrazy = false;
        public int level = 0;

        private new void Start()
        {
            base.Start();
            myDwLegController = GetComponent<DWLegController>();
        }

        public override IEnumerator Freeze(float time)
        {
            Speed = 0;
            IsFreezing = true;
            deFreeze = StartCoroutine(DeFreeze(time));
            yield return null;
        }

        public override void LongTimeFreeze()
        {
            Speed = 0;
            IsFreezing = true;
        }

        public override IEnumerator DeFreeze(float time)
        {
            yield return new WaitForSeconds(time);
            Speed = defaultSpeed;
            IsFreezing = false;
        }

        public void AttackDetect()
        {
            if (Vector3.Distance(transform.position, ObjectGetter.player.transform.position) < 2f)
            {
                ObjectGetter.player.GetComponent<PlayerController>().TakeDamage();
            }
        }

        void Update()
        {
            if (IsFreezing) return;
            if (isCrazy) Speed = defaultSpeed;
            Vector3 direction = (ObjectGetter.player.transform.position - transform.position).normalized;
            velocity = Vector3.Lerp(velocity, new Vector3(direction.x, 0, direction.z) * speed, velocityLerpCoef * Time.deltaTime);

            // Assigning velocity to the mimic to assure great leg placement
            myDwLegController.velocity = velocity;

            transform.position += velocity * Time.deltaTime;
            // Debug.Log(Vector3.Distance(transform.position, ObjectGetter.player.transform.position));
            if (Vector3.Distance(transform.position, ObjectGetter.player.transform.position) < 2f)
            {
                Invoke("AttackDetect", attackDelay);
            }
        }

        public void Accelerate()
        {
            Debug.Log("DemonWill has been enhanced!");
            level++;
            if (level == 3 || level == 4)
            {
                transform.Find("Sphere").GetComponent<GlowingController>().Glowing(level);
            }
            if (level == 5)
            {
                isCrazy = true;
                defaultSpeed = 20.0f;
                StartCoroutine(DeFreeze(0.0f));
                attackDelay = 0.3f;
            }
            else
            {
                defaultSpeed += 1.0f;
            }

            if (IsFreezing) return;
            Speed = defaultSpeed;

        }
    }
}