using UnityEngine;
using UnityEngine.UI; // UI (Slider) kodlarını kullanmak için gerekli

public class SesAyarKontrol : MonoBehaviour
{
    [Header("UI Bağlantısı")]
    public Slider anaSesSlideri;

    void Start()
    {
        // 1. Daha önce kaydedilmiş bir ses ayarı var mı bak. Yoksa varsayılan olarak 1 (%100) yap.
        float kayitliSes = PlayerPrefs.GetFloat("AnaSesSeviyesi", 1f);
        
        // 2. Oyundaki genel kulağın (Master Volume) sesini bu kayıtlı değere eşitle
        AudioListener.volume = kayitliSes;

        // 3. Slider'ın çubuğunu, oyun açıldığında doğru konuma getir
        if (anaSesSlideri != null)
        {
            anaSesSlideri.value = kayitliSes;
            
            // Slider'ı her kaydırdığımızda "SesDegisti" fonksiyonunu otomatik çalıştır
            anaSesSlideri.onValueChanged.AddListener(SesDegisti);
        }
    }

    // Slider hareket ettikçe bu fonksiyon tetiklenir
    public void SesDegisti(float yeniSesSeviyesi)
    {
        // Oyundaki genel sesi slider'ın yeni değerine eşitle
        AudioListener.volume = yeniSesSeviyesi;

        // Ayarı kaydet ki oyunu kapatıp açtığında aynı kalsın
        PlayerPrefs.SetFloat("AnaSesSeviyesi", yeniSesSeviyesi);
        PlayerPrefs.Save();
    }
}