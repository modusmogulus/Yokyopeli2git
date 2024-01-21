using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using EasyTransition;

namespace Q3Movement
{
    /// <summary>
    /// This script handles Quake III CPM(A) mod style player movement logic.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Q3PlayerController : MonoBehaviour
    {
        [System.Serializable]
        public class MovementSettings
        {
            public float MaxSpeed;
            public float Acceleration;
            public float Deceleration;

            public MovementSettings(float maxSpeed, float accel, float decel)
            {
                MaxSpeed = maxSpeed;
                Acceleration = accel;
                Deceleration = decel;
            }
        }

        [Header("Aiming")]
        [SerializeField] private Camera m_Camera;
        [SerializeField] private GameObject m_Head;
        [SerializeField] public MouseLook m_MouseLook = new MouseLook();
        [Header("Movement")]
        [SerializeField] private float m_Friction = 6;
        [SerializeField] private float m_Gravity = 20;
        [SerializeField] private float m_JumpForce = 8;
        [Tooltip("Automatically jump when holding jump button")]
        [SerializeField] private bool m_AutoBunnyHop = false;
        [Tooltip("How precise air control is")]
        [SerializeField] private float m_AirControl = 0.3f;
        [SerializeField] private MovementSettings m_GroundSettings = new MovementSettings(7, 14, 10);
        [SerializeField] private MovementSettings m_AirSettings = new MovementSettings(7, 2, 2);
        [SerializeField] private MovementSettings m_StrafeSettings = new MovementSettings(1, 50, 50);
        [SerializeField] private Animator headAnimator;
        public TransitionSettings deathTransition;
        /// <summary>
        /// Returns player's current speed.
        /// </summary>
        public float Speed { get { return m_Character.velocity.magnitude; } }

        private CharacterController m_Character;
        private Vector3 m_MoveDirectionNorm = Vector3.zero;
        private Vector3 m_PlayerVelocity = Vector3.zero;

        // Used to queue the next jump just before hitting the ground.
        private bool m_JumpQueued = false;
        private bool isCurrentlyGrounded = true;
        // Used to display real time friction values.
        private float m_PlayerFriction = 0;
        public bool m_inRollZone = false;
        private Vector3 m_MoveInput;
        private Transform m_Tran;
        private Transform m_CamTran;
        public bool m_IsRolling = false;
        private bool m_IsCrouching = false;
        private bool m_WasGrounded = false;
        private float m_CrouchHeight = 0.5f; 
        private float m_OriginalHeight;
        private float m_LandingTimer = 0.0f;
        private float m_currentfallSpeed = 0.0f;
        private bool intentsRoll;
        private bool damageApplied;
        private bool hasRolled;
        public bool showControlTips = false;
        //-- HP --
        public float maxhp = 100.0f;
        public float  health = 100.0f;
        public PostProcessVolume damageFx;
        public PostProcessVolume fallFx;
        public bool m_parkourEnabled = true;
        private void Start()
        {
            health = maxhp;
            m_Tran = transform;
            m_Character = GetComponent<CharacterController>();

            if (!m_Camera)
                m_Camera = Camera.main;

            m_CamTran = m_Camera.transform;
            m_MouseLook.Init(m_Tran, m_CamTran);
            headAnimator = m_Head.GetComponent<Animator>();
            MainGameObject.Instance.playerController = this;
        }

        public IEnumerator CrouchCoroutine()
        {
            m_OriginalHeight = m_Character.height;
            if (m_IsCrouching = true) { m_Character.height = m_CrouchHeight; }
            
            
            

            // Play crouch animation or do any other necessary actions
            // Example: triggering an animation in the animator controller

            yield return null; // Or yield return new WaitForSeconds(animationLength);

            // Reset the character controller height to original height
            m_Character.height = m_OriginalHeight;

            m_IsCrouching = false;
        }


        public IEnumerator RollCoroutine()
        {
            bool m_IsRolling = true;

            // Get the Animator component from the m_Head object

            // Check if the Animator component exists
            if (headAnimator != null)
            {
                // Trigger the 'Roll' animation or set a bool parameter to true
                headAnimator.SetTrigger("Roll"); // Assuming using a trigger parameter named "Roll"
                                                 // Or
                                                 // headAnimator.SetBool("IsRolling", true); // Example with a bool parameter named "IsRolling"
            }
            else
            {
                Debug.LogError("Animator component not found on m_Head object!");
            }

            yield return new WaitForSeconds(0.1f); // Adjust this duration based on your animation length

            // Reset the animation state or any other necessary actions
            if (headAnimator != null)
            {
                headAnimator.ResetTrigger("Roll"); // Assuming a trigger parameter named "Roll"
                                                   // Or
                                                   // headAnimator.SetBool("IsRolling", false); // Example with a bool parameter named "IsRolling"
            }

            m_IsRolling = false;
            hasRolled = true;
        }
        
        private void ApplyFallDamage(float multiplier, float threshold)
        {
            if (m_currentfallSpeed > threshold) { AudioManager.Instance.PlayAudio("SFX_Damage"); health = Mathf.Clamp(health - (m_currentfallSpeed * multiplier), 0.0f, maxhp); }
            
        }
        public void DealDamage(float dmg, int type)
        {
            health = Mathf.Clamp(health - dmg, 0.0f, maxhp);
        }

        private void Die()
        {
            TransitionManager.Instance().Transition(SceneManager.GetActiveScene().name, deathTransition, 0);
        }

        private void Regen()
        {
            health = Mathf.Clamp(health + 0.05f, 0f, maxhp); //Hp regen
        }
        private void Update()
        {
            Climb();

            // Control Tip Text for parkour roll
            if (MainGameObject.Instance.controlTips != null && m_currentfallSpeed / (40 - 30) > 0.9) { MainGameObject.Instance.controlTips.text = "shift"; }
            else { 
                if ( MainGameObject.Instance.controlTips != null && MainGameObject.Instance.controlTips.text == "shift") { MainGameObject.Instance.controlTips.text = ""; }
            }
            fallFx.weight = Mathf.Clamp((m_currentfallSpeed - 3) / 20, 0f, 1f);

            //print(health.ToString());
            isCurrentlyGrounded = m_Character.isGrounded;
            m_MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            m_MouseLook.UpdateCursorLock();    
            QueueJump();
            if (isCurrentlyGrounded && !m_WasGrounded)
            {
                m_LandingTimer = 0.0f;
            }
            m_WasGrounded = isCurrentlyGrounded;

            Regen();
            damageFx.weight = health / maxhp * -1 + 1;

            if (health <= 1) { Die(); } //Death

            if (Input.GetKey(KeyCode.LeftShift))
            {
                intentsRoll = true;
            }
            else
            {
                intentsRoll = false;
            }

                if (isCurrentlyGrounded)
                {
                    GroundMove();
                    if (MainGameObject.Instance.controlTips != null && MainGameObject.Instance.controlTips.text == "shift") { MainGameObject.Instance.controlTips.text = ""; }
                    m_LandingTimer += Time.deltaTime; // Increment the landing timer when the player is grounded

                    if (intentsRoll && m_inRollZone == true && hasRolled == false)
                    {
                        StartCoroutine(RollCoroutine());
                        hasRolled = true;
                    }
                    if (m_LandingTimer <= 0.2f && m_LandingTimer >= 0.01f)
                    {
                    // Check for roll input within the first 0.2 seconds of landing
                    if (damageApplied == false && hasRolled == false)
                        {
                        if(intentsRoll) { 
                            if (m_currentfallSpeed >= 10.0) {
                                if (!m_IsRolling && m_Character.isGrounded)
                                    {
                                        hasRolled = true;
                                        StartCoroutine(RollCoroutine());
                                        ApplyFallDamage(0.5f, 40f);
                                        MainGameObject.Instance.score += 20;
                                        AudioManager.Instance.PlayAudio("SFX_Coin");
                                    }
                                }
                            }
                        }
                    }
                    if (intentsRoll == false)
                    {
                        if (damageApplied == false && hasRolled == false)
                        {
                            ApplyFallDamage(2f, 10f);
                            damageApplied = true;
                        }
                    }
                }

            else
            {
                m_currentfallSpeed = m_PlayerVelocity.y *- 1;
                AirMove();
                damageApplied = false;
                hasRolled = false;
            }

            // Rotate the character and camera.
            m_MouseLook.LookRotation(m_Tran, m_CamTran);

            // Move the character.
            m_Character.Move(m_PlayerVelocity * Time.deltaTime);

            
            if (Input.GetKeyDown(KeyCode.Numlock))
            {
                m_OriginalHeight = m_Character.height;
                m_Character.height = m_CrouchHeight;
                m_IsCrouching = true;
                //StartCoroutine(CrouchCoroutine());
            }
            if (Input.GetKeyUp(KeyCode.Numlock))
            {
                Vector3 pos = transform.position;
                pos.y += m_OriginalHeight - m_CrouchHeight + 0.001f;
                m_Character.height = m_OriginalHeight;
                transform.position = pos;
                m_IsCrouching = false;
            }
        }

        private void Climb()
        {
            var p = transform.position;
            var d = transform.forward;
            var hit1 = Physics.Raycast(p, d, out var h1, 1f * (m_PlayerVelocity.magnitude / 7));
            var hit2 = Physics.Raycast(p + Vector3.up * 1/*meters*/, d, out var h2, 1f);

            if (hit1 && !hit2 && m_JumpQueued && m_parkourEnabled) {
                //Debug.Log($"Hit {h1.collider.gameObject.name} at point: {h1.point}, normal: {h1.normal}");
                m_PlayerVelocity.y = 8;
                if(headAnimator != null)
                {
                    headAnimator.SetTrigger("Vault");
                    MainGameObject.Instance.score += 5;
                    AudioManager.Instance.PlayAudio("SFX_Coin");
                }
            }
            else
            {
                //Debug.Log("Conditions for second raycast hit not met.");
            }
        }

        // Queues the next jump.
        private void QueueJump()
        {
            if (m_AutoBunnyHop && !intentsRoll)
            {
                //print(intentsRoll);
                m_JumpQueued = Input.GetButton("Jump");
                return;
            }

            if (Input.GetButtonDown("Jump") && !m_JumpQueued && !intentsRoll)
            {
                m_JumpQueued = true;
            }

            if (Input.GetButtonUp("Jump") || intentsRoll)
            {
                m_JumpQueued = false;
            }
        }

        // Handle air movement.
        private void AirMove()
        {
            float accel;

            var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
            wishdir = m_Tran.TransformDirection(wishdir);

            float wishspeed = wishdir.magnitude;
            wishspeed *= m_AirSettings.MaxSpeed;

            wishdir.Normalize();
            m_MoveDirectionNorm = wishdir;

            // CPM Air control.
            float wishspeed2 = wishspeed;
            if (Vector3.Dot(m_PlayerVelocity, wishdir) < 0)
            {
                accel = m_AirSettings.Deceleration;
            }
            else
            {
                accel = m_AirSettings.Acceleration;
            }

            // If the player is ONLY strafing left or right
            if (m_MoveInput.z == 0 && m_MoveInput.x != 0)
            {
                if (wishspeed > m_StrafeSettings.MaxSpeed)
                {
                    wishspeed = m_StrafeSettings.MaxSpeed;
                }

                accel = m_StrafeSettings.Acceleration;
            }

            Accelerate(wishdir, wishspeed, accel);
            if (m_AirControl > 0)
            {
                AirControl(wishdir, wishspeed2);
            }

            // Apply gravity
            m_PlayerVelocity.y -= m_Gravity * Time.deltaTime;
        }

        // Air control occurs when the player is in the air, it allows players to move side 
        // to side much faster rather than being 'sluggish' when it comes to cornering.
        private void AirControl(Vector3 targetDir, float targetSpeed)
        {
            // Only control air movement when moving forward or backward.
            if (Mathf.Abs(m_MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
            {
                return;
            }

            float zSpeed = m_PlayerVelocity.y;
            m_PlayerVelocity.y = 0;
            /* Next two lines are equivalent to idTech's VectorNormalize() */
            float speed = m_PlayerVelocity.magnitude;
            m_PlayerVelocity.Normalize();

            float dot = Vector3.Dot(m_PlayerVelocity, targetDir);
            float k = 32;
            k *= m_AirControl * dot * dot * Time.deltaTime;

            // Change direction while slowing down.
            if (dot > 0)
            {
                m_PlayerVelocity.x *= speed + targetDir.x * k;
                m_PlayerVelocity.y *= speed + targetDir.y * k;
                m_PlayerVelocity.z *= speed + targetDir.z * k;

                m_PlayerVelocity.Normalize();
                m_MoveDirectionNorm = m_PlayerVelocity;
            }

            m_PlayerVelocity.x *= speed;
            m_PlayerVelocity.y = zSpeed; // Note this line
            m_PlayerVelocity.z *= speed;
        }

        // Handle ground movement.
        private void GroundMove()
        {
            if (m_MouseLook.GetCursorLock() == false) { ApplyFriction(0.5f); headAnimator.SetBool("Running", false); return;  }
            // Do not apply friction if the player is queueing up the next jump
            if (!m_JumpQueued)
            {
                ApplyFriction(1.0f);
                if (m_IsCrouching == true)
                {
                    ApplyFriction(1.5f);
                }
            }
            else
            {
                ApplyFriction(0);
            }


            var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
            wishdir = m_Tran.TransformDirection(wishdir);
            wishdir.Normalize();
            m_MoveDirectionNorm = wishdir;

            var wishspeed = wishdir.magnitude;
            wishspeed *= m_GroundSettings.MaxSpeed;

            if (Mathf.Ceil(m_PlayerVelocity.x) != 0 && Mathf.Ceil(m_PlayerVelocity.z) != 0 && m_Character.isGrounded)
            {
                headAnimator.SetBool("Running", true);
            }
            else
            {
                headAnimator.SetBool("Running", false);
            }
            Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);

            // Reset the gravity velocity
            m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;

            if (m_JumpQueued)
            {
                m_PlayerVelocity.y = m_JumpForce;
                m_JumpQueued = false;
            }
        }

        private void ApplyFriction(float t)
        {
            // Equivalent to VectorCopy();
            Vector3 vec = m_PlayerVelocity; 
            vec.y = 0;
            float speed = vec.magnitude;
            float drop = 0;

            // Only apply friction when grounded.
            if (m_Character.isGrounded)
            {
                float control = speed < m_GroundSettings.Deceleration ? m_GroundSettings.Deceleration : speed;
                drop = control * m_Friction * Time.deltaTime * t;
            }

            float newSpeed = speed - drop;
            m_PlayerFriction = newSpeed;
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            if (speed > 0)
            {
                newSpeed /= speed;
            }

            m_PlayerVelocity.x *= newSpeed;
            
            m_PlayerVelocity.z *= newSpeed;
        }

        
        private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
        {
            float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
            float addspeed = targetSpeed - currentspeed;
            if (addspeed <= 0)
            {
                return;
            }

            float accelspeed = accel * Time.deltaTime * targetSpeed;
            if (accelspeed > addspeed)
            {
                accelspeed = addspeed;
            }

            m_PlayerVelocity.x += accelspeed * targetDir.x;
            m_PlayerVelocity.z += accelspeed * targetDir.z;
        }

        public void Teleport(Vector3 pos)
        {
            Vector3 posi = pos;
            transform.position = posi;
        }
    }
}