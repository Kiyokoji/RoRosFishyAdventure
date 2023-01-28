using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace PlayerController {
    public class PlayerInputs : MonoBehaviour {
        public FrameInput FrameInput { get; private set; }

        private void Update() => FrameInput = Gather();

#if ENABLE_INPUT_SYSTEM

        private InputActionAsset inputAsset;
        private InputActionMap player;

        //private PlayerInputActions _actions;
        private InputAction _move, _jump, _interact, _interactLeft, _interactRight, _flashlight, _mousePos, _rightStick;

        private void Awake()
        {
            inputAsset = this.GetComponent<PlayerInput>().actions;

            player = inputAsset.FindActionMap("Player");

            _move = player.FindAction("Movement");
            _jump = player.FindAction("Jump");
            _interact = player.FindAction("Interact");
            _interactLeft = player.FindAction("InteractLeft");
            _interactRight = player.FindAction("InteractRight");
            _flashlight = player.FindAction("Flashlight");
            _mousePos = player.FindAction("MousePos");
            _rightStick = player.FindAction("RightStick");

            //_actions = new PlayerInputActions();
            //_move = _actions.Player.Movement;
            //_jump = _actions.Player.Jump;
        }

        private void OnEnable() => player.Enable();

        private void OnDisable() => player.Disable();

        private FrameInput Gather()
        {
            return new FrameInput {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                Move = _move.ReadValue<Vector2>(),
                InteractDown = _interact.WasPressedThisFrame(),
                InteractHeld = _interact.IsPressed(),
                InteractUp = _interact.WasReleasedThisFrame(),
                LeftInteract = _interactLeft.IsPressed(),
                RightInteract = _interactRight.IsPressed(),
                Flashlight = _flashlight.WasPressedThisFrame(),
                MousePos = _mousePos.ReadValue<Vector2>(),
                RightStick = _rightStick.ReadValue<Vector2>()
            };
        }

#elif ENABLE_LEGACY_INPUT_MANAGER
        private FrameInput Gather() {
            return new FrameInput {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
            };
        }
#endif
    }

    public struct FrameInput {
        public Vector2 Move;
        public bool JumpDown;
        public bool JumpHeld;
        public bool InteractDown;
        public bool InteractHeld;
        public bool InteractUp;
        public bool LeftInteract;
        public bool RightInteract;
        public bool Flashlight;
        public Vector2 MousePos;
        public Vector2 RightStick;
    }
}