using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSystem : MonoBehaviour
{
    [Header("Gerekli Bileþenler")]
    public Transform cameraTransform;

    [Header("Taþýma Ayarlarý")]
    public LayerMask pickupLayer;
    public float pickupRadius = 3f;
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
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }

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
        // Tarama merkezini ayak ucundan göðüs hizasýna (1 birim yukarý) alýyoruz
        Vector3 scanCenter = transform.position + Vector3.up * 1f;

        // Kürenin içindeki tüm taþýnabilir objeleri bul
        Collider[] hitColliders = Physics.OverlapSphere(scanCenter, pickupRadius, pickupLayer);

        if (hitColliders.Length > 0)
        {
            Collider closestCollider = null;
            float minDistance = Mathf.Infinity;

            // YENÝ SÝSTEM: Bulunan objeler arasýnda tek tek dön ve BÝZE EN YAKIN olaný seç
            foreach (Collider col in hitColliders)
            {
                float distance = Vector3.Distance(scanCenter, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCollider = col;
                }
            }

            // En yakýndakini bulduysak, onu elimize al
            if (closestCollider != null)
            {
                heldObject = closestCollider.gameObject;
                heldObjRb = heldObject.GetComponent<Rigidbody>();
                heldObjCollider = heldObject.GetComponent<Collider>();

                if (heldObjRb != null) heldObjRb.isKinematic = true;
                if (heldObjCollider != null) heldObjCollider.isTrigger = true;
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
        // Gizmo çizerken de göðüs hizasýný gösterelim
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, pickupRadius);
    }
}