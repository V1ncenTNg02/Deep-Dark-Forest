using ORZ.Utility;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace ORZ.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 2;
        [SerializeField] private float runSpeed = 3;
        [SerializeField] private float stamina = 3;

        public GameObject StaminaBar;
        private float staminaRemain;
        public float StaminaRemain
        {
            get => staminaRemain;
            set
            {
                staminaRemain = value;
                float t = staminaRemain / stamina;
                float width = Mathf.Lerp(0.0f, 394.0f, t);
                Rect rect = StaminaBar.GetComponent<RectTransform>().rect;
                StaminaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, rect.height);

                Color color = Color.Lerp(Color.red, Color.white, t);
                StaminaBar.GetComponent<Image>().color = color;
            }
        }

        private bool runState = true;

        [SerializeField] private float recoverStaminaWalk = 1.5f;
        [SerializeField] private float recoverStaminaIdle = 2;
        [SerializeField] private float staminaConsumeRate = 1.4f;
        private float currentConsumeRate = 1f;

        private float changeCoolDown = 0.4f;
        private float coolDown;

        private PlayerAnimationController animator;
        private Rigidbody rb;

        private GameObject cam;
        private State state = State.Idle;

        public bool isFreezing = false;

        public bool CanMove { get; set; } = true;

        private enum State
        {
            Idle,
            Walk,
            Run,
        }

        public float getSpeed()
        {
            return speed;
        }

        public float getRunSpeed()
        {
            return runSpeed;
        }

        public void setSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        public void setRunSpeed(float newRunSpeed)
        {
            runSpeed = newRunSpeed;
        }


        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<PlayerAnimationController>();
            cam = GameObject.Find("Camera");
        }

        void Start()
        {
            StaminaRemain = stamina;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!CanMove) return;
            MoveControl();
            if (StaminaRemain >= stamina)
            {
                currentConsumeRate = 1f;
            }
            Action();
        }

        void MoveControl()
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                State movePattern = Input.GetButton("Run") && runState ? State.Run : State.Walk;
                state = movePattern;
                string s = movePattern == State.Walk ? "Walk" : "Run";
            }
            else
            {
                coolDown = changeCoolDown;
                state = State.Idle;
            }

            if (coolDown > 0)
            {
                coolDown -= Time.fixedDeltaTime;
                if (coolDown < 0) coolDown = 0;
            }
        }

        void Action()
        {
            switch (state)
            {
                case State.Idle: Idle(); break;
                case State.Walk: Walk(); break;
                case State.Run: Run(); break;
            }
        }

        void Idle()
        {
            rb.velocity = Vector3.zero;
            StaminaRecovery(recoverStaminaIdle);
            animator.Idle();
        }

        void Walk()
        {
            Move(speed);
            StaminaRecovery(recoverStaminaWalk);

            animator.Walk();
        }

        void Run()
        {
            Move(runSpeed);
            StaminaRemain -= Time.fixedDeltaTime * currentConsumeRate;
            if (StaminaRemain <= 0)
            {
                runState = false;
                currentConsumeRate = staminaConsumeRate;
                StaminaRemain = 0;
            }

            animator.Run();
        }


        void Move(float speed)
        {
            // Correct the player's front direction to camera's front direction, and calculate new velocity
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 inputVelocity = new Vector2(horizontal, vertical).normalized;
            float angle = Utils.AngleOfDirection(inputVelocity);
            rb.velocity = new Vector3(inputVelocity.x, 0, inputVelocity.y) * speed;

            transform.localRotation = Quaternion.Euler(0, angle, 0);
        }

        void StaminaRecovery(float rate)
        {
            if (coolDown > 0)
            {
                rate = recoverStaminaWalk;
            }

            if (StaminaRemain > 0.8f)
            {
                runState = true;
            }
            if (StaminaRemain < stamina)
            {
                StaminaRemain += rate * Time.fixedDeltaTime;
                if (StaminaRemain > stamina)
                {
                    StaminaRemain = stamina;
                }
            }
        }

        public void TakeDamage()
        {
            ObjectGetter.player.SetActive(false);
            ObjectGetter.gameManager.GameOver();
        }

        public void StartEvent(string eve) => StartCoroutine(eve);
        public void StopEvent(string eve) => StopCoroutine(eve);
        public void StopAllEvent() => StopAllCoroutines();

        
    }

}
