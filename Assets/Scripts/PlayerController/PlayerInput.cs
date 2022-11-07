using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace PlayerController {
    public class PlayerInput : MonoBehaviour {
        public FrameInput FrameInput { get; private set; }

        private void Update() => FrameInput = Gather();

#if ENABLE_INPUT_SYSTEM
        private PlayerInputActions _actions;
        private InputAction _move, _jump;

        private void Awake() {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Movement;
            _jump = _actions.Player.Jump;
        }

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();

        private FrameInput Gather() {
            return new FrameInput {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                Move = _move.ReadValue<Vector2>()
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
    }
}