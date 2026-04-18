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
    public float turnSpeed = 15f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        bool isRunning = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) vertical += 1f;
            if (Keyboard.current.sKey.isPressed) vertical -= 1f;
            if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed) horizontal += 1f;
            if (Keyboard.current.leftShiftKey.isPressed) isRunning = true;
        }

        // 1. Kameranýn ileri ve sað yönlerini al
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // 2. Y eksenini sýfýrla 
        camForward.y = 0;
        camRight.y = 0;

        // HATANIN DÜZELTÝLDÝÐÝ YER: Vektörleri kendi üzerlerinde normalize ediyoruz
        camForward.Normalize();
        camRight.Normalize();

        // 3. Kameraya göre hareket yönünü hesapla
        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        float targetSpeedParameter = 0f;

        if (moveDirection.magnitude >= 0.1f)
        {
            targetSpeedParameter = isRunning ? 2f : 1f;

            // 4. Karakteri hareket yönüne doðru yumuþakça döndür
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // 5. Hareket ettir
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        animator.SetFloat("speed", targetSpeedParameter, 0.1f, Time.deltaTime);

        // Yerçekimi
        if (!controller.isGrounded)
            controller.Move(new Vector3(0, -9.81f * Time.deltaTime, 0));
    }
}