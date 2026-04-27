using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input sistemi

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target; // Karakterin
    public float distance = 6f; // Kameranýn karaktere uzaklýðý
    public float heightOffset = 1.5f; // Kameranýn hedefin neresine (boyun/kafa) bakacaðý

    [Header("Fare Hassasiyeti")]
    public float sensitivityX = 0.3f; // Saða sola dönme hýzý
    public float sensitivityY = 0.2f; // Aþaðý yukarý dönme hýzý

    [Header("Açý Sýnýrlarý")]
    public float yMinLimit = -20f; // Kamera en fazla ne kadar aþaðý inebilir
    public float yMaxLimit = 60f;  // Kamera en fazla ne kadar yukarý çýkabilir

    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        // Oyuna baþladýðýmýzda fare imlecini ekrana kilitle ve gizle (Etrafý rahatça izlemek için)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Baþlangýçta kameranýn mevcut açýsýný al
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Fareden gelen hareketi oku
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            // Farenin X ve Y hareketini açýlara ekle
            currentX += mouseDelta.x * sensitivityX;
            currentY -= mouseDelta.y * sensitivityY; // Y ekseni genelde ters çevrilir (- kullanýrýz)
        }

        // 2. Y eksenini sýnýrla (Kameranýn karakterin altýndan geçmesini veya tepede takla atmasýný engeller)
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);

        // 3. Fareden aldýðýmýz X ve Y açýlarýný bir rotasyona çevir
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 4. Kameranýn pozisyonunu hedefin etrafýnda, ayarladýðýmýz mesafeye göre hesapla
        Vector3 position = target.position + (Vector3.up * heightOffset) - (rotation * Vector3.forward * distance);

        // 5. Hesaplanan pozisyon ve rotasyonu kameraya uygula
        transform.rotation = rotation;
        transform.position = position;
    }
}