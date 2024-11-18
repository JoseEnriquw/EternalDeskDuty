using Assets.Scripts.Commons.GameManager;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 5f;
        [SerializeField] private float mouseSensitivity = 100f;

        private CharacterController characterController;
        private Animator animator;
        private CinemachineVirtualCamera firstPersonCamera;

        private Vector3 moveDirection = Vector3.zero;
        private readonly float gravity = -9.81f;
        private float verticalVelocity;
        private float xRotation = 0f;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction runAction;

        private void Awake()
        {
            // Get necessary components
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            firstPersonCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            playerInput = GetComponent<PlayerInput>();

            // Get Input System actions
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            runAction = playerInput.actions["Run"];
        }

        private void Start()
        {
            GameManager.GetGameManager().OnChangePlayerInput += SetEnablePlayerInput;
            GameManager.GetGameManager().OnChangePlayerPosition += ChangePosition;
            GameManager.GetGameManager().OnChangePlayerRotation += SetLookAtTarget;

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovement();
            HandleAnimations();
        }

        private void HandleMouseLook()
        {
            if (!playerInput.enabled) return;

            // Get mouse input
            Vector2 lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

            // Rotate the player horizontally
            transform.Rotate(Vector3.up * lookInput.x);

            // Rotate the camera vertically
            xRotation -= lookInput.y;
            xRotation = Mathf.Clamp(xRotation, -80f, 60f);

            firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        private void HandleMovement()
        {
            if (!playerInput.enabled) return;

            // Get movement input and run state
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            bool isRunning = runAction.ReadValue<float>() > 0.1f;

            // Determine current speed
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Movement direction relative to current orientation
            Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = transform.right * direction.x + transform.forward * direction.z;
                moveDirection = moveDir * currentSpeed;
            }
            else
            {
                moveDirection = Vector3.zero;
            }

            // Apply gravity
            if (characterController.isGrounded)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            moveDirection.y = verticalVelocity;

            // Move the character
            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleAnimations()
        {
            // Calculate horizontal speed
            Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
            float speedPercent = horizontalVelocity.magnitude / runSpeed;

            // Update 'Speed' parameter in Animator
            animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
        }

        public void SetEnablePlayerInput(bool enable)
        {
            playerInput.enabled = enable;
        }

        public void SetLookAtTarget(Transform target)
        {
            RotateTowardsTarget(target);
        }

        private void RotateTowardsTarget(Transform target)
        {
            // Calculate the direction to the target
            Vector3 directionToTarget = target.position - transform.position;

            // Rotate the player horizontally to face the target
            Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;
            if (horizontalDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
                transform.rotation = targetRotation;
            }

            // Apply the rotation to the camera
            firstPersonCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void OnDestroy()
        {
            GameManager.GetGameManager().OnChangePlayerInput -= SetEnablePlayerInput;
            GameManager.GetGameManager().OnChangePlayerPosition -= ChangePosition;
            GameManager.GetGameManager().OnChangePlayerRotation -= SetLookAtTarget;
        }

        private void ChangePosition(Transform newTransform)
        {
            transform.position = newTransform.position;
            transform.rotation = newTransform.rotation;
        }
    }
}
