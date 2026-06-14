using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class AnaMenuManager : MonoBehaviour
{
    [Header("Butonlar")]
    public Button devamEtButonu;

    private string dosyaYolu;

    void Start()
    {
        // Fareyi serbest bırak (Menüde butonlara tıklayabilmek için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // JSON dosyasının bilgisayardaki kayıtlı olduğu yolu bul
        dosyaYolu = Path.Combine(Application.persistentDataPath, "oyun_kayit.json");

        // KAYIT KONTROLÜ
        if (File.Exists(dosyaYolu))
        {
            devamEtButonu.interactable = true; // Kayıt varsa butonu aç
        }
        else
        {
            devamEtButonu.interactable = false; // Kayıt yoksa butonu kapat (tıklanamaz yap)
        }
    }

    public void YeniOyunButonu()
    {
        // Yeni oyuna başlarken eski JSON dosyasını sil ki sıfırdan başlasın
        if (File.Exists(dosyaYolu)) 
        {
            File.Delete(dosyaYolu);
        }
        
        SceneManager.LoadScene(0); // Senin oyun sahnene (0) git
    }

    public void DevamEtButonu()
    {
        SceneManager.LoadScene(0); // Senin oyun sahnene (0) git (Oradaki kod verileri kendi yükleyecek)
    }

    public void OyundanCikButonu()
    {
        Debug.Log("Oyundan Çıkıldı");
        Application.Quit();
    }
}