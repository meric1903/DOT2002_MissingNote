using UnityEngine;

public class EkipmanSistemi : MonoBehaviour
{
    public static EkipmanSistemi Instance;

    [Header("Bağlantılar")]
    public Transform silahTutucu; // Sağ elin içindeki boş obje

    private GameObject eldekiSilah; // Şu an elimizde olan fiziksel obje

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Çantadan bir silaha tıklandığında bu fonksiyon çalışacak
    public void SilahKusan(EsyaVerisi esya)
    {
        // Tıklanan şey silah değilse (örn: sağlık kitiyse) hiçbir şey yapma
        if (esya.tur != EsyaTuru.Silah) return; 

        // Eğer elimizde zaten bir silah varsa, yenisini almadan önce eskisini yok et
        if (eldekiSilah != null)
        {
            Destroy(eldekiSilah);
        }

        // Yeni silahı yarat ve karakterin elindeki yuvaya yerleştir
        eldekiSilah = Instantiate(esya.esyaPrefab, silahTutucu.position, silahTutucu.rotation);
        eldekiSilah.transform.SetParent(silahTutucu);

        // ÖNEMLİ: Yerden alma kodlarını kopyadan siliyoruz ki elimizdeki silaha tekrar "E" ile basmayalım
        if (eldekiSilah.GetComponent<Collider>() != null) 
            Destroy(eldekiSilah.GetComponent<Collider>());
            
        if (eldekiSilah.GetComponent<YerdekiEsya>() != null) 
            Destroy(eldekiSilah.GetComponent<YerdekiEsya>());
    }
}