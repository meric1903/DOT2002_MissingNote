using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSystem : MonoBehaviour
{
    [Header("Gerekli Bileþenler")]
    public Transform cameraTransform; // Kamerayý buraya sürükleyeceðiz

    [Header("Taþýma Ayarlarý")]
    public LayerMask pickupLayer; // 'Tasinabilir' katmanýný seçeceðiz
    public float pickupRadius = 3f; // Eþyayý alabilme mesafesi
    public float holdDistance = 1.5f; // Eþyanýn karakterin ne kadar önünde duracaðý
    public float holdHeight = 1.5f; // Eþyanýn yerden yüksekliði (göðüs hizasý)

    private GameObject heldObject;
    private Rigidbody heldObjRb;
    private Collider heldObjCollider;

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // E tuþuna basýldýðýnda (TUT / BIRAK Geçiþi)
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (heldObject == null)
                TryPickup();  // Elimiz boþsa yerden al
            else
                DropObject(); // Elimizde bir þey varsa býrak
        }

        // Eðer elimizde bir obje varsa, onu sürekli kameranýn baktýðý yönde tut
        if (heldObject != null)
        {
            // Kameranýn yönünü al ama yukarý/aþaðý bakmayý (Y eksenini) iptal et
            // Böylece sen yere baksan bile kutu yerin dibine girmez, göðüs hizanda kalýr.
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            // Objeyi tutacaðýmýz ideal pozisyonu hesapla (Karakterin merkezi + yukarý + kameranýn baktýðý yön)
            Vector3 targetPosition = transform.position + (Vector3.up * holdHeight) + (camForward * holdDistance);

            // Objeyi o pozisyona yumuþakça (Lerp) taþý
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPosition, Time.deltaTime * 15f);

            // Objeyi kameranýn açýsýna göre döndür (Kutu da seninle birlikte dönsün)
            heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, cameraTransform.rotation, Time.deltaTime * 15f);
        }
    }

    void TryPickup()
    {
        // Karakterin etrafýnda görünmez bir küre oluþtur ve içine giren 'Tasinabilir' objeleri bul
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRadius, pickupLayer);

        if (hitColliders.Length > 0)
        {
            // Eðer etrafta taþýnabilir obje varsa, ilk bulduðunu al
            heldObject = hitColliders[0].gameObject;
            heldObjRb = heldObject.GetComponent<Rigidbody>();
            heldObjCollider = heldObject.GetComponent<Collider>();

            // 1. Fiziði kapat ki biz taþýrken saða sola çarpýp fýrlamasýn
            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = true;
            }

            // 2. Çarpýþmayý "Trigger" yap ki karakterin kendi vücudunu itip onu uzaya fýrlatmasýn!
            if (heldObjCollider != null)
            {
                heldObjCollider.isTrigger = true;
            }
        }
    }

    void DropObject()
    {
        // 1. Önce objenin katý halini (çarpýþmasýný) geri ver
        if (heldObjCollider != null)
        {
            heldObjCollider.isTrigger = false;
        }

        // 2. Fiziðini ve yerçekimini tekrar aktif et
        if (heldObjRb != null)
        {
            heldObjRb.isKinematic = false;
        }

        // 3. Hafýzadan sil
        heldObject = null;
    }

    // Bu fonksiyon sadece Unity editöründe çalýþýr. Eþya alma menzilini (küreyi) kýrmýzý çizgilerle gösterir.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}