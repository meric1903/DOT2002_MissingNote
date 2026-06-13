using UnityEngine;
using System.IO;

// 1. ADIM: Kaydedilecek verilerin şablonu (Kutusu)
[System.Serializable]
public class OyuncuVerileri
{
    public float canMiktari;
    public int mermiSayisi;
    public Vector3 karakterPozisyonu;
    public bool silahaSahipMi;
}

// 2. ADIM: Kaydetme ve Yükleme Motoru
public class SaveManager : MonoBehaviour
{
    private string dosyaYolu;

    // Projedeki diğer kodların bu sisteme kolayca ulaşması için Singleton yapısı
    public static SaveManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Klasör yolunu belirle (Her bilgisayarda otomatik güvenli bir klasör seçer)
        dosyaYolu = Path.Combine(Application.persistentDataPath, "oyun_kayit.json");
    }

    // DIŞARIDAN ÇAĞRILACAK KAYDETME FONKSİYONU
    public void VerileriKaydet(float can, int mermi, Vector3 pozisyon, bool silahVarMi)
    {
        OyuncuVerileri veriKutusu = new OyuncuVerileri();
        veriKutusu.canMiktari = can;
        veriKutusu.mermiSayisi = mermi;
        veriKutusu.karakterPozisyonu = pozisyon;
        veriKutusu.silahaSahipMi = silahVarMi;

        // C# nesnesini JSON metnine dönüştür (true parametresi okunabilir şekilde alt alta dizer)
        string jsonMetni = JsonUtility.ToJson(veriKutusu, true);

        // Metni dosyaya yaz
        File.WriteAllText(dosyaYolu, jsonMetni);
        Debug.Log("Oyun Başarıyla Kaydedildi: " + dosyaYolu);
    }

    // DIŞARIDAN ÇAĞRILACAK YÜKLEME FONKSİYONU
    public OyuncuVerileri VerileriYukle()
    {
        if (File.Exists(dosyaYolu))
        {
            // Dosyadaki metni oku
            string jsonMetni = File.ReadAllText(dosyaYolu);

            // Metni tekrar C# nesnesine (kutusuna) dönüştür
            OyuncuVerileri yuklenenVeri = JsonUtility.FromJson<OyuncuVerileri>(jsonMetni);
            Debug.Log("Oyun Verileri Başarıyla Yüklendi.");
            return yuklenenVeri;
        }
        else
        {
            Debug.LogWarning("Kayıt dosyası bulunamadı! İlk defa oynanıyor olabilir.");
            return null;
        }
    }

    // Eğer dosyayı tamamen sıfırlamak istersen:
    public void KaydiSil()
    {
        if (File.Exists(dosyaYolu))
        {
            File.Delete(dosyaYolu);
            Debug.Log("Kayıt Dosyası Silindi.");
        }
    }
}