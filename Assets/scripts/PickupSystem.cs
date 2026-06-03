using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSystem : MonoBehaviour
{
    [Header("Gerekli Bileşenler")]
    public Transform cameraTransform;

    [Header("Etkileşim Ayarları")]
    public LayerMask tasinabilirLayer;  // Odunlar, kütükler vb.
    public LayerMask toplanabilirLayer; // Çantaya girecek eşyalar (Silah, sağlık kiti vb.)
    public float pickupRadius = 3f;
    
    [Header("Taşıma Ayarları (Sadece Odunlar İçin)")]
    public float holdDistance = 1.5f;
    public float holdHeight = 1.5f;

    private GameObject heldObject;
    private Rigidbody heldObjRb;
    private Collider heldObjCollider;

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // E tuşuna basıldığında
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Eğer elimiz boşsa yerden bir şey almaya çalış
            if (heldObject == null)
                TryPickup();
            // Eğer elimizde odun varsa bırak
            else
                DropObject(); 
        }

        // Eğer elimizde odun varsa onu kameranın önünde tutmaya devam et
        if (heldObject != null)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 targetPosition = transform.position + (Vector3.up * holdHeight) + (camForward * holdDistance);

            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPosition, Time.deltaTime * 15f);
            heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, cameraTransform.rotation, Time.deltaTime * 15f);
        }
    }

    void TryPickup()
    {
        // Tarama merkezini ayak ucundan göğüs hizasına alıyoruz
        Vector3 scanCenter = transform.position + Vector3.up * 1f;

        // Hem Taşınabilirleri (Odun) hem Toplanabilirleri (Silah) aynı anda tara
        LayerMask combinedLayer = tasinabilirLayer | toplanabilirLayer;
        Collider[] hitColliders = Physics.OverlapSphere(scanCenter, pickupRadius, combinedLayer);

        if (hitColliders.Length > 0)
        {
            Collider closestCollider = null;
            float minDistance = Mathf.Infinity;

            // Bize en yakın olan objeyi bul
            foreach (Collider col in hitColliders)
            {
                float distance = Vector3.Distance(scanCenter, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCollider = col;
                }
            }

            if (closestCollider != null)
            {
                GameObject bulunanObje = closestCollider.gameObject;

                // --- TRAFİK POLİSİ: Bu obje HANGİ katmanda? ---
                
                // 1. DURUM: Obje "tasinabilir" katmanındaysa (Odun vb. fiziki taşıma)
                if (((1 << bulunanObje.layer) & tasinabilirLayer) != 0)
                {
                    heldObject = bulunanObje;
                    heldObjRb = heldObject.GetComponent<Rigidbody>();
                    heldObjCollider = heldObject.GetComponent<Collider>();

                    if (heldObjRb != null) heldObjRb.isKinematic = true;
                    if (heldObjCollider != null) heldObjCollider.isTrigger = true;
                }
                // 2. DURUM: Obje "Toplanabilir" katmanındaysa (Çantaya girecek eşya)
                else if (((1 << bulunanObje.layer) & toplanabilirLayer) != 0)
                {
                    // Objeden köprü kodumuzu ve kimliğini istiyoruz
                    YerdekiEsya yerdeki = bulunanObje.GetComponent<YerdekiEsya>();
                    
                    if (yerdeki != null && yerdeki.kimlikKarti != null)
                    {
                        // Barkodu başarıyla okuduk!
                        Debug.Log("Çantaya giren eşya: " + yerdeki.kimlikKarti.esyaAdi);
                        
                        EnvanterSistemi.Instance.Ekle(yerdeki.kimlikKarti);
                        
                        // Eşyayı sahneden siliyoruz (Çantaya girdi varsayıyoruz)
                        Destroy(bulunanObje);
                    }
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObjCollider != null) heldObjCollider.isTrigger = false;
        if (heldObjRb != null) heldObjRb.isKinematic = false;
        heldObject = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Gizmo çizerken göğüs hizasını göster
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, pickupRadius);
    }
}