using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Bileþenler")]
    public Transform cameraTransform;
    private Animator animator;
    private CharacterController controller;

    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float turnSpeed = 15f;

    [Header("Fiziksel Boyut Ayarlarý")]
    public float normalHeight = 2f;
    public float crouchHeight = 1f;
    public Vector3 normalCenter = new Vector3(0, 1f, 0);
    public Vector3 crouchCenter = new Vector3(0, 0.5f, 0);

    [Header("Zýplama ve Yerçekimi")]
    public float jumpHeight = 1.5f;
    public float gravity = -15f; // Düþüþ hissini toklaþtýrmak için yerçekimini biraz artýrdým

    // YENÝ EKLENENLER: Zýplama Tamponu (Jump Buffer)
    public float jumpBufferTime = 0.2f; // Space'e basýldýðýný ne kadar süre aklýnda tutsun
    private float jumpBufferCounter;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // 1. Yer Kontrolü
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        animator.SetBool("isGrounded", isGrounded);

        // 2. Girdileri Al
        float horizontal = 0f;
        float vertical = 0f;
        bool isRunning = false;
        bool isCrouching = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) vertical += 1f;
            if (Keyboard.current.sKey.isPressed) vertical -= 1f;
            if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed) horizontal += 1f;

            if (Keyboard.current.leftCtrlKey.isPressed) isCrouching = true;
            else if (Keyboard.current.leftShiftKey.isPressed) isRunning = true;

            // YENÝ ZIPLAMA MANTIÐI: Space'e basýldýysa sayacý doldur
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                // Basýlmadýysa sayacý zamanla azalt
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        // 3. ZIPLAMA KONTROLÜ (Eðer hafýzada zýplama varsa ve yerdeysek zýpla)
        if (jumpBufferCounter > 0f && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("jump");
            jumpBufferCounter = 0f; // Zýpladýk, hafýzayý sýfýrla
        }

        // 4. Kameraya Göre Hareket Yönü
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // 5. Hýz ve Yatay Hareket
        float currentSpeed = walkSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        else if (isRunning) currentSpeed = runSpeed;

        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        // 6. Eðilme Fiziði (Kapsül Küçültme)
        if (isCrouching)
        {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * 10f);
            controller.center = Vector3.Lerp(controller.center, crouchCenter, Time.deltaTime * 10f);
        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, normalHeight, Time.deltaTime * 10f);
            controller.center = Vector3.Lerp(controller.center, normalCenter, Time.deltaTime * 10f);
        }

        // 7. Yerçekimini Uygula (Dikey Hareket)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 8. Animator Parametreleri
        animator.SetBool("isCrouching", isCrouching);
        float targetSpeedParam = (moveDirection.magnitude > 0.1f) ? (isRunning && !isCrouching ? 2f : 1f) : 0f;
        animator.SetFloat("speed", targetSpeedParam, 0.1f, Time.deltaTime);
    }
}