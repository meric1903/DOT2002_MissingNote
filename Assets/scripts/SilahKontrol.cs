using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; 

public class SilahKontrol : MonoBehaviour
{
    [Header("Nişan ve Menzil Ayarları")]
    public float menzil = 100f;
    public float hasarMiktari = 25f; 

    [Header("Mermi ve Şarjör Ayarları")]
    public int mermiKapasitesi = 10; 
    public int toplamMermi = 30;     
    public float reloadSuresi = 2f; 
    
    private int mevcutMermi;
    private bool sarjorDegisiyor = false; 

    private LineRenderer isinIzleyici;
    private Transform namluUcu;
    private Camera oyuncuKamerasi; 

    void Start()
    {
        mevcutMermi = mermiKapasitesi;
        oyuncuKamerasi = Camera.main; 
        namluUcu = transform.Find("Barrel_Location");

        isinIzleyici = GetComponent<LineRenderer>();
        if (isinIzleyici == null) isinIzleyici = gameObject.AddComponent<LineRenderer>();
        IşınAyarlarınıYap();
    }

    // 🎨 MERMİ İZİNİ İNCELTEN VE SARI YAPAN KISIM
    void IşınAyarlarınıYap()
    {
        // 📏 Çizgiyi jilet gibi incelttik (Işın görüntüsü kayboldu)
        isinIzleyici.startWidth = 0.02f;
        isinIzleyici.endWidth = 0.005f; // Arkaya doğru süzülerek incelen kuyruk
        isinIzleyici.positionCount = 2;
        isinIzleyici.enabled = false;
        isinIzleyici.material = new Material(Shader.Find("Sprites/Default"));
        
        // 🔥 FİLMLERDEKİ SARI-TURUNCU PARLAMA RENKLERİ:
        isinIzleyici.startColor = new Color(1f, 0.85f, 0.2f, 1f); // Namludan çıkan parlak sarı/altın
        isinIzleyici.endColor = new Color(1f, 0.4f, 0f, 0f);      // Havada şeffaflaşıp kaybolan turuncu kuyruk
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; 
        if (transform.parent == null) return; 
        if (NisanKontrol.Instance != null && NisanKontrol.Instance.sinematikOynuyor) return; 
        if (sarjorDegisiyor) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (mevcutMermi > 0)
            {
                AtesEt();
            }
            else
            {
                if (toplamMermi > 0) StartCoroutine(SarjorDegistirSenaryosu());
            }
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame && mevcutMermi < mermiKapasitesi && toplamMermi > 0)
        {
            StartCoroutine(SarjorDegistirSenaryosu());
        }
    }

    void AtesEt()
    {
        mevcutMermi--;
        
        Vector3 isinBitisNoktasi = oyuncuKamerasi.transform.position + (oyuncuKamerasi.transform.forward * menzil);
        RaycastHit hit;

        if (Physics.Raycast(oyuncuKamerasi.transform.position, oyuncuKamerasi.transform.forward, out hit, menzil))
        {
            isinBitisNoktasi = hit.point;
        }

        if (namluUcu != null) StartCoroutine(MermiIsiniFlashi(namluUcu.position, isinBitisNoktasi));
        else StartCoroutine(MermiIsiniFlashi(transform.position, isinBitisNoktasi));
    }

    // ⚡ LAZER GÖRÜNTÜSÜNÜ SALİSELİK ÇAKMAYA ÇEVİREN ZAMANLAYICI
    IEnumerator MermiIsiniFlashi(Vector3 baslangic, Vector3 bitis)
    {
        isinIzleyici.SetPosition(0, baslangic);
        isinIzleyici.SetPosition(1, bitis);
        isinIzleyici.enabled = true;
        
        // ⏱️ Süreyi 0.04'ten 0.02'ye çektik! Çizgi artık ekranda kalmıyor, mermi gibi anlık çakıp sönüyor
        yield return new WaitForSeconds(0.02f); 
        
        isinIzleyici.enabled = false;
    }

    IEnumerator SarjorDegistirSenaryosu()
    {
        sarjorDegisiyor = true;
        yield return new WaitForSeconds(reloadSuresi);

        int gerekenMermi = mermiKapasitesi - mevcutMermi;

        if (toplamMermi >= gerekenMermi)
        {
            mevcutMermi = mermiKapasitesi;
            toplamMermi -= gerekenMermi;
        }
        else
        {
            mevcutMermi += toplamMermi;
            toplamMermi = 0;
        }

        sarjorDegisiyor = false;
    }

    public int GetMevcutMermi() { return mevcutMermi; }
    public int GetToplamMermi() { return toplamMermi; } 
}