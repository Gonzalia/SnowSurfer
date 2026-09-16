using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Giro y acrobacias")]
    [Tooltip("Aceleración en grados/s². Más alto permite iniciar vueltas y contragirar más rápido.")]
    [SerializeField, Min(0f)] float spinAcceleration = 1000f;
    [Tooltip("Velocidad máxima en grados/s. Dividí por 360 para obtener vueltas por segundo.")]
    [SerializeField, Min(0f)] float maxSpinSpeed = 600f;
    [Tooltip("Freno natural del giro. Un valor bajo conserva la rotación al soltar el control.")]
    [SerializeField, Min(0f)] float angularDamping = 0.35f;

    [Header("Boost (W / arriba)")]
    [Tooltip("Aceleración horizontal adicional mientras mantenés W o arriba y la tabla toca nieve.")]
    [SerializeField, Min(0f)] float boostAcceleration = 25f;
    [Tooltip("El boost deja de empujar al alcanzar esta velocidad horizontal. No frena si ya vas más rápido.")]
    [SerializeField, Min(0f)] float maxBoostSpeed = 30f;
    InputAction moveAction;
    Rigidbody2D body;
    CapsuleCollider2D boardCollider;
    int snowLayerMask;
    Vector2 moveVector;
    float steering;
    public bool RunEnded { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boardCollider = GetComponent<CapsuleCollider2D>();
        snowLayerMask = LayerMask.GetMask("Floor");
        body.angularDamping = angularDamping;
    }

    void Start()
    {
        moveAction = InputSystem.actions?.FindAction("Move");
    }

    void Update()
    {
        moveVector = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
        RotatePlayer();
    }

    void FixedUpdate()
    {
        if (RunEnded) return;
        float spinDelta = -steering * spinAcceleration * Time.fixedDeltaTime;
        body.angularVelocity = Mathf.Clamp(body.angularVelocity + spinDelta, -maxSpinSpeed, maxSpinSpeed);
        BoostPlayer();
    }

    public bool EndRun()
    {
        if (RunEnded) return false;
        RunEnded = true;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.simulated = false;
        return true;
    }

    void RotatePlayer()
    {
        steering = moveVector.x;

    }

    void BoostPlayer()
    {
        if (moveVector.y <= 0f || body.linearVelocity.x >= maxBoostSpeed) return;
        // Check the board's physical contact, not the head's crash trigger.
        if (boardCollider == null || !boardCollider.IsTouchingLayers(snowLayerMask)) return;

        Vector2 velocity = body.linearVelocity;
        float boostDelta = moveVector.y * boostAcceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.Min(velocity.x + boostDelta, maxBoostSpeed);
        body.linearVelocity = velocity;
    }
}
