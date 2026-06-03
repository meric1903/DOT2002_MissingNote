using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input sistemi

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarlar�")]
    public Transform target; // Karakterin
    public float heightOffset = 1.5f; // Kameran�n hedefin neresine (boyun/kafa) bakaca��

    [Header("Mesafe Ayarlar� (��/D�� Mekan)")]
    public float outdoorDistance = 6f; // D��ar�dayken kameran�n uzakl��� (Eski distance)
    public float indoorDistance = 2f;  // Evin i�indeyken kameran�n uzakl���
    public float zoomSpeed = 5f;       // ��eri/D��ar� ge�erken yak�nla�ma h�z�

    [Header("Fare Hassasiyeti")]
    public float sensitivityX = 0.3f; // Sa�a sola d�nme h�z�
    public float sensitivityY = 0.2f; // A�a�� yukar� d�nme h�z�

    [Header("A�� S�n�rlar�")]
    public float yMinLimit = -20f; // Kamera en fazla ne kadar a�a�� inebilir
    public float yMaxLimit = 60f;  // Kamera en fazla ne kadar yukar� ��kabilir

    private float currentX = 0f;
    private float currentY = 0f;

    // Zoom ge�i�i i�in olu�turdu�umuz yeni de�i�kenler
    private float currentDistance;
    private float targetDistance;

    void Start()
    {
        // Oyuna ba�lad���m�zda fare imlecini ekrana kilitle ve gizle (Etraf� rahat�a izlemek i�in)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ba�lang��ta kameran�n mevcut a��s�n� al
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        // Ba�lang��ta oyun d��ar�da ba�l�yorsa mesafeyi outdoor (d�� mekan) olarak ayarla
        currentDistance = outdoorDistance;
        targetDistance = outdoorDistance;
    }

    // Bu fonksiyonu evin i�indeki g�r�nmez kutu (Trigger) �al��t�racak
    public void SetIndoorMode(bool isIndoor)
    {
        if (isIndoor)
        {
            targetDistance = indoorDistance; // ��eri girdiysek hedef mesafeyi k�salt
        }
        else
        {
            targetDistance = outdoorDistance; // D��ar� ��kt�ysak hedef mesafeyi uzat
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (Time.timeScale == 0f) return;
        // 1. Fareden gelen hareketi oku
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            // Farenin X ve Y hareketini a��lara ekle
            currentX += mouseDelta.x * sensitivityX;
            currentY -= mouseDelta.y * sensitivityY; // Y ekseni genelde ters �evrilir (- kullan�r�z)
        }

        // 2. Y eksenini s�n�rla (Kameran�n karakterin alt�ndan ge�mesini veya tepede takla atmas�n� engeller)
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);

        // YEN� EKLENEN KISIM: Kameran�n o anki mesafesini, hedef mesafeye do�ru yumu�ak�a yakla�t�r
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);

        // 3. Fareden ald���m�z X ve Y a��lar�n� bir rotasyona �evir
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 4. Kameran�n pozisyonunu hedefin etraf�nda, HESAPLANAN YEN� MESAFEYE (currentDistance) g�re bul
        Vector3 position = target.position + (Vector3.up * heightOffset) - (rotation * Vector3.forward * currentDistance);

        // 5. Hesaplanan pozisyon ve rotasyonu kameraya uygula
        transform.rotation = rotation;
        transform.position = position;
    }
}