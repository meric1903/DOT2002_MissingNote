using UnityEngine;

public class EkipmanSistemi : MonoBehaviour
{
    public static EkipmanSistemi Instance;

    [Header("Referanslar")]
    public Transform silahtutucu; // Karakterin elindeki o yuva objesi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Silahı ele getiren veya zaten eldeyse kılıfına kaldıran ana fonksiyon
    /// </summary>
    public void SilahKusan(GameObject silahPrefab)
    {
        // 🔍 TAKİP LOGU 1: Fonksiyonun tetiklenip tetiklenmediğini kontrol ediyoruz
        Debug.Log("1. Çantadan silaha tıklandı, kuşanma kodu tetiklendi!");

        if (silahPrefab == null)
        {
            Debug.LogError("HATA: Çantadan gelen silah dosyasının içi boş (Null)!");
            return;
        }

        if (silahtutucu == null)
        {
            // 🔥 EĞER SİLAH ELİNE GELMİYORSA KONSOLDA BU KIRMIZI YAZI ÇIKACAK:
            Debug.LogError("HATA: Sahnede EkipmanSistemi üzerindeki 'Silah Tutucu' kutusu BOŞ kalmış! Lütfen elindeki yuvayı Inspector'dan buraya sürükle.");
            return;
        }

        // --- SADECE ELDEKİ SİLAHI DENETLEYEN KISIM ---
        if (silahtutucu.childCount > 0)
        {
            GameObject eldekiSilah = silahtutucu.GetChild(0).gameObject;

            if (eldekiSilah.name.Contains(silahPrefab.name))
            {
                Destroy(eldekiSilah);
                Debug.Log("2. Aynı silaha tekrar basıldı: Silah kılıfına kaldırıldı.");
                return; 
            }
            else
            {
                Destroy(eldekiSilah);
            }
        }

        // --- YENİ SİLAHI DOĞURMA KISMI ---
        GameObject yeniSilah = Instantiate(silahPrefab, silahtutucu);
        
        yeniSilah.transform.localPosition = Vector3.zero;
        yeniSilah.transform.localRotation = Quaternion.identity;
        yeniSilah.transform.localScale = Vector3.one;

        // 🔍 TAKİP LOGU 3: Her şey başarıyla bittiyse yeşil/beyaz ışık yakar
        Debug.Log("3. BAŞARI: " + silahPrefab.name + " objesi elindeki yuvada başarıyla oluşturuldu!");
    }
}