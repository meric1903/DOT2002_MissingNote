using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input sistemini dahil ediyoruz

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 3f;  // Yürüme hýzý
    public float runSpeed = 6f;   // Koþma hýzý
    public float turnSpeed = 10f; // Dönme hassasiyeti

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        bool isRunning = false;

        // 1. Yeni sistemde klavyeden giriþleri alýyoruz
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;

            // Sol Shift tuþuna basýlýp basýlmadýðýný kontrol et
            if (Keyboard.current.leftShiftKey.isPressed) isRunning = true;
        }

        // 2. Hareket yönünü belirle
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. Mevcut hýzý belirle
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 4. Animator için speed parametresini belirle (0, 1 veya 2)
        float targetSpeedParameter = 0f;
        if (direction.magnitude >= 0.1f)
        {
            targetSpeedParameter = isRunning ? 2f : 1f;
        }

        animator.SetFloat("speed", targetSpeedParameter, 0.1f, Time.deltaTime);

        // 5. Eðer bir hareket girdisi varsa karakteri hareket ettir ve döndür
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        // 6. Yerçekimi
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, -9.81f * Time.deltaTime, 0));
        }
    }
}