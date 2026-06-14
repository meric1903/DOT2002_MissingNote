using UnityEngine;
using UnityEngine.UI;

public class NisanKontrol : MonoBehaviour
{
    public static NisanKontrol Instance;
    private Image aimResmi;

    [Header("Durumlar")]
    // Sinematik başladığında başka kodlardan burayı true yapacağız
    public bool sinematikOynuyor = false; 

    void Awake()
    {
        // Kodun her yerden tek tıkla ulaşılabilir olması için Instance (Singleton) yapıyoruz
        if (Instance == null) Instance = this;
        
        aimResmi = GetComponent<Image>();
    }

    void Update()
    {
        // 1. KURAL: Zaman durmuşsa (Envanter, Ayarlar Menüsü vb. açıksa) aim'i gizle
        // 2. KURAL: Sinematik oynuyorsa aim'i gizle
        if (Time.timeScale == 0f || sinematikOynuyor)
        {
            aimResmi.enabled = false;
        }
        else
        {
            // Aktif oyundaysak ve zaman akıyorsa göster
            aimResmi.enabled = true;
        }
    }
}