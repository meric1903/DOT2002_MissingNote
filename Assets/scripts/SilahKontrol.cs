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
    
    public int mevcutMermi; 
    private bool sarjorDegisiyor = false; 

    private LineRenderer isinIzleyici;
    private Transform namluUcu;
    private Camera oyuncuKamerasi; 
    
    // SES İÇİN EKLENEN DEĞİŞKEN
    private AudioSource atesSesi; 

    public static int hafizaMevcutMermi = -1;
    public static int hafizaToplamMermi = -1;

    void Start()
    {
        oyuncuKamerasi = Camera.main; 
        namluUcu = transform.Find("Barrel_Location");

        // OBJENİN ÜZERİNDEKİ SES BİLEŞENİNİ OTOMATİK BULUR
        atesSesi = GetComponent<AudioSource>(); 

        isinIzleyici = GetComponent<LineRenderer>();
        if (isinIzleyici == null) isinIzleyici = gameObject.AddComponent<LineRenderer>();
        IşınAyarlarınıYap();

        if (hafizaMevcutMermi == -1) 
        {
            mevcutMermi = mermiKapasitesi; 
            hafizaMevcutMermi = mevcutMermi;
        } 
        else 
        {
            mevcutMermi = hafizaMevcutMermi; 
        }

        if (hafizaToplamMermi == -1)
        {
            hafizaToplamMermi = toplamMermi;
        }
        else 
        {
            toplamMermi = hafizaToplamMermi; 
        }
    }

    void IşınAyarlarınıYap()
    {
        isinIzleyici.startWidth = 0.02f;
        isinIzleyici.endWidth = 0.005f; 
        isinIzleyici.positionCount = 2;
        isinIzleyici.enabled = false;
        isinIzleyici.material = new Material(Shader.Find("Sprites/Default"));
        
        isinIzleyici.startColor = new Color(1f, 0.85f, 0.2f, 1f); 
        isinIzleyici.endColor = new Color(1f, 0.4f, 0f, 0f);      
    }

    void Update()
    {
        if (transform.parent == null || transform.parent.name != "silahtutucu") return; 

        if (Time.timeScale == 0f) return; 
        if (NisanKontrol.Instance != null && NisanKontrol.Instance.sinematikOynuyor) return; 
        if (sarjorDegisiyor) return;

        hafizaMevcutMermi = mevcutMermi;
        hafizaToplamMermi = toplamMermi;

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
        
        // SESİ ÇALMASI İÇİN EKLENEN KOMUT
        if (atesSesi != null) atesSesi.Play(); 
        
        Vector3 isinBitisNoktasi = oyuncuKamerasi.transform.position + (oyuncuKamerasi.transform.forward * menzil);
        RaycastHit hit;

        if (Physics.Raycast(oyuncuKamerasi.transform.position, oyuncuKamerasi.transform.forward, out hit, menzil))
        {
            isinBitisNoktasi = hit.point;

            DushmanCanSistemi vurulanDushman = hit.collider.GetComponent<DushmanCanSistemi>();
            
            if (vurulanDushman == null)
            {
                vurulanDushman = hit.collider.GetComponentInParent<DushmanCanSistemi>();
            }
            
            if (vurulanDushman != null)
            {
                vurulanDushman.HasarAl(hasarMiktari);
            }
        }

        if (namluUcu != null) StartCoroutine(MermiIsiniFlashi(namluUcu.position, isinBitisNoktasi));
        else StartCoroutine(MermiIsiniFlashi(transform.position, isinBitisNoktasi));
    }

    IEnumerator MermiIsiniFlashi(Vector3 baslangic, Vector3 bitis)
    {
        isinIzleyici.SetPosition(0, baslangic);
        isinIzleyici.SetPosition(1, bitis);
        isinIzleyici.enabled = true;
        
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