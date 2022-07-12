using UnityEngine;

namespace RPGCharacterAnimsFREE
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(RPGCharacterController))]
    public class RPGCharacterNavigationController : MonoBehaviour
    {
        // Components.
        [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
        private RPGCharacterController rpgCharacterController;
        private RPGCharacterMovementController rpgCharacterMovementController;
        private Animator animator;
        [HideInInspector] public bool isNavigating;

        void Awake()
        {
            // In order for the navMeshAgent not to interfere with other movement, we want it to be
            // enabled ONLY when we are actually using it.
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            navMeshAgent.enabled = false;

            rpgCharacterController = GetComponent<RPGCharacterController>();
            rpgCharacterMovementController = GetComponent<RPGCharacterMovementController>();
            rpgCharacterController.SetHandler("Navigation", new Actions.Navigation(this));
		}

        void Start()
        {
            // Check if Animator exists, otherwise pause script.
            animator = rpgCharacterController.animator;
            if (animator == null) {
				Debug.LogError("No Animator component found!");
				Debug.Break();
            }
        }

        void Update()
        {
			if (isNavigating) {
				RotateTowardsMovementDir();
				if (navMeshAgent.velocity.sqrMagnitude > 0) {
					animator.SetBool("Moving", true);
					animator.SetFloat("Velocity Z", navMeshAgent.velocity.magnitude);
				} else {
					animator.SetFloat("Velocity Z", 0);
				}
			}

            // Disable the navMeshAgent once the character has reached its destination.
            if (isNavigating && !navMeshAgent.hasPath) { StopNavigating(); }
        }

        /// <summary>
        /// Navigate to the destination using Unity's NavMeshAgent.
        /// </summary>
        /// <param name="destination">Point in world space to navigate to.</param>
        public void MeshNavToPoint(Vector3 destination)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(destination);
            isNavigating = true;
            if (rpgCharacterMovementController != null) { rpgCharacterMovementController.enabled = false; }
        }

        /// <summary>
        /// Stop navigating to the current destination.
        /// </summary>
        public void StopNavigating()
        {
            isNavigating = false;
            navMeshAgent.enabled = false;
            if (rpgCharacterMovementController != null) { rpgCharacterMovementController.enabled = true; }
        }

        private void RotateTowardsMovementDir()
        {
            if (navMeshAgent.velocity.magnitude > 0.01f) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(navMeshAgent.velocity), Time.deltaTime * navMeshAgent.angularSpeed);
            }
        }
    }
}