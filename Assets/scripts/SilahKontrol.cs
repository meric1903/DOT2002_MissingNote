using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; 

public class SilahKontrol : MonoBehaviour
{
    [Header("Nişan ve Menzil Ayarları")]
    public float menzil = 100f;
    public float hasarMiktari = 25f; 

    [Header("Mermi ve Şarjör Ayarları")]
    public int mermiKapasitesi = 10; // Bir şarjörün alacağı mermi
    public int toplamMermi = 30;     // Cebimizdeki toplam YEDEK mermi (İstediğin gibi değiştirebilirsin)
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

    void IşınAyarlarınıYap()
    {
        isinIzleyici.startWidth = 0.03f;
        isinIzleyici.endWidth = 0.01f;
        isinIzleyici.positionCount = 2;
        isinIzleyici.enabled = false;
        isinIzleyici.material = new Material(Shader.Find("Sprites/Default"));
        isinIzleyici.startColor = new Color(1f, 0.5f, 0f, 1f); 
        isinIzleyici.endColor = new Color(1f, 0.2f, 0f, 0f);
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
                // Şarjör boşsa ve yedek mermimiz varsa otomatik doldur
                if (toplamMermi > 0) StartCoroutine(SarjorDegistirSenaryosu());
            }
        }

        // R tuşuna basınca ve şarjör tam dolu değilken, cepte de mermi varsa reload yap
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

    IEnumerator MermiIsiniFlashi(Vector3 baslangic, Vector3 bitis)
    {
        isinIzleyici.SetPosition(0, baslangic);
        isinIzleyici.SetPosition(1, bitis);
        isinIzleyici.enabled = true;
        yield return new WaitForSeconds(0.04f);
        isinIzleyici.enabled = false;
    }

    // --- AKILLI RELOAD MATEMATİĞİ ---
    IEnumerator SarjorDegistirSenaryosu()
    {
        sarjorDegisiyor = true;
        yield return new WaitForSeconds(reloadSuresi);

        // Şarjörü fullemek için kaç tane mermiye ihtiyacımız var?
        int gerekenMermi = mermiKapasitesi - mevcutMermi;

        if (toplamMermi >= gerekenMermi)
        {
            // Eğer cepte yeterince mermi varsa şarjörü fulle, cepten harcananı düş
            mevcutMermi = mermiKapasitesi;
            toplamMermi -= gerekenMermi;
        }
        else
        {
            // Eğer cepte gereken kadar mermi yoksa, cepte kalan son mermileri şarjöre ekle ve cebi sıfırla
            mevcutMermi += toplamMermi;
            toplamMermi = 0;
        }

        sarjorDegisiyor = false;
    }

    public int GetMevcutMermi() { return mevcutMermi; }
    public int GetToplamMermi() { return toplamMermi; } // UI için yedek mermiyi veren yeni köprü
}