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
        private float gravity = -9.81f;
        private float verticalVelocity;
        private float xRotation = 0f;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction runAction;

        private void Awake()
        {
            // Obtener componentes necesarios
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            firstPersonCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            playerInput = GetComponent<PlayerInput>();

            // Obtener acciones del Input System
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            runAction = playerInput.actions["Run"];
        }

        private void Start()
        {
            GameManager.GetGameManager().OnChangePlayerInput+=SetEnablePlayerInput;
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovement();
            HandleAnimations();
        }

        private void HandleMouseLook()
        {
            // Obtener la entrada del ratón
            Vector2 lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

            // Rotar el jugador en el eje Y (horizontal)
            transform.Rotate(Vector3.up * lookInput.x);

            // Rotar la cámara en el eje X (vertical)
            xRotation -= lookInput.y;
            xRotation = Mathf.Clamp(xRotation, -80f, 60f);

            firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        private void HandleMovement()
        {
            // Obtener entrada de movimiento y estado de correr
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            bool isRunning = runAction.ReadValue<float>() > 0.1f;

            // Determinar velocidad actual
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Dirección de movimiento en relación con la orientación actual
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

            // Aplicar gravedad
            if (characterController.isGrounded)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            moveDirection.y = verticalVelocity;

            // Mover al personaje
            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleAnimations()
        {
            // Calcular la velocidad horizontal
            Vector3 horizontalVelocity = new(characterController.velocity.x, 0, characterController.velocity.z);
            float speedPercent = horizontalVelocity.magnitude / runSpeed;

            // Actualizar el parámetro 'Speed' en el Animator
            animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
        }

        public void SetEnablePlayerInput(bool enable)
        {
            playerInput.enabled = enable;
        }
    }
}
