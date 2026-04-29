using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input sistemi

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target; // Karakterin
    public float heightOffset = 1.5f; // Kameranýn hedefin neresine (boyun/kafa) bakacaðý

    [Header("Mesafe Ayarlarý (Ýç/Dýþ Mekan)")]
    public float outdoorDistance = 6f; // Dýþarýdayken kameranýn uzaklýðý (Eski distance)
    public float indoorDistance = 2f;  // Evin içindeyken kameranýn uzaklýðý
    public float zoomSpeed = 5f;       // Ýçeri/Dýþarý geçerken yakýnlaþma hýzý

    [Header("Fare Hassasiyeti")]
    public float sensitivityX = 0.3f; // Saða sola dönme hýzý
    public float sensitivityY = 0.2f; // Aþaðý yukarý dönme hýzý

    [Header("Açý Sýnýrlarý")]
    public float yMinLimit = -20f; // Kamera en fazla ne kadar aþaðý inebilir
    public float yMaxLimit = 60f;  // Kamera en fazla ne kadar yukarý çýkabilir

    private float currentX = 0f;
    private float currentY = 0f;

    // Zoom geçiþi için oluþturduðumuz yeni deðiþkenler
    private float currentDistance;
    private float targetDistance;

    void Start()
    {
        // Oyuna baþladýðýmýzda fare imlecini ekrana kilitle ve gizle (Etrafý rahatça izlemek için)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Baþlangýçta kameranýn mevcut açýsýný al
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        // Baþlangýçta oyun dýþarýda baþlýyorsa mesafeyi outdoor (dýþ mekan) olarak ayarla
        currentDistance = outdoorDistance;
        targetDistance = outdoorDistance;
    }

    // Bu fonksiyonu evin içindeki görünmez kutu (Trigger) çalýþtýracak
    public void SetIndoorMode(bool isIndoor)
    {
        if (isIndoor)
        {
            targetDistance = indoorDistance; // Ýçeri girdiysek hedef mesafeyi kýsalt
        }
        else
        {
            targetDistance = outdoorDistance; // Dýþarý çýktýysak hedef mesafeyi uzat
        }
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

        // YENÝ EKLENEN KISIM: Kameranýn o anki mesafesini, hedef mesafeye doðru yumuþakça yaklaþtýr
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);

        // 3. Fareden aldýðýmýz X ve Y açýlarýný bir rotasyona çevir
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 4. Kameranýn pozisyonunu hedefin etrafýnda, HESAPLANAN YENÝ MESAFEYE (currentDistance) göre bul
        Vector3 position = target.position + (Vector3.up * heightOffset) - (rotation * Vector3.forward * currentDistance);

        // 5. Hesaplanan pozisyon ve rotasyonu kameraya uygula
        transform.rotation = rotation;
        transform.position = position;
    }
}