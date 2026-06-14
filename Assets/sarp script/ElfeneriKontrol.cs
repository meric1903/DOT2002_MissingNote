using UnityEngine;
using UnityEngine.InputSystem; 

public class ElfeneriKontrol : MonoBehaviour
{
    [Header("Fener Ayarları")]
    public Light elFeneriIsigi; 
    public float donusHizi = 15f; // Fenerin nişangaha dönme yumuşaklığı

    private Camera oyuncuKamerasi;

    void Start()
    {
        oyuncuKamerasi = Camera.main;

        if (elFeneriIsigi != null)
        {
            elFeneriIsigi.enabled = false; 
        }
    }

    void Update()
    {
        // F tuşu kontrolü (Aç / Kapat)
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (elFeneriIsigi != null)
            {
                elFeneriIsigi.enabled = !elFeneriIsigi.enabled;
            }
        }
    }

    void LateUpdate()
    {
        // Optimizasyon: Eğer fener objesi yoksa, fener kapalıysa veya kamera yoksa boşuna hesaplama yapma!
        if (elFeneriIsigi == null || !elFeneriIsigi.enabled || oyuncuKamerasi == null) return;

        RaycastHit hit;
        Vector3 hedefNokta;

        // Kameranın tam ortasından ileriye (100 metre) görünmez bir ışın atıyoruz
        if (Physics.Raycast(oyuncuKamerasi.transform.position, oyuncuKamerasi.transform.forward, out hit, 100f))
        {
            // Eğer ışın ağaca, yere veya düşmana çarparsa, fenerin ışığı oraya odaklansın
            hedefNokta = hit.point;
        }
        else
        {
            // Hiçbir şeye çarpmazsa (gökyüzüne bakıyorsak), hedef doğrudan kameranın 100 metre ilerisi olsun
            hedefNokta = oyuncuKamerasi.transform.position + (oyuncuKamerasi.transform.forward * 100f);
        }

        // Fenerin bakması gereken açıyı hesapla
        Quaternion hedefRotasyon = Quaternion.LookRotation(hedefNokta - elFeneriIsigi.transform.position);

        // Feneri aniden değil, hafif yumuşak (gerçekçi bir baş/omuz hareketi gibi) o noktaya çevir
        elFeneriIsigi.transform.rotation = Quaternion.Slerp(elFeneriIsigi.transform.rotation, hedefRotasyon, Time.deltaTime * donusHizi);
    }
}