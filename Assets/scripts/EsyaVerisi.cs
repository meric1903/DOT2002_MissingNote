using UnityEngine;

// Bu mucizevi satır, Unity'de sağ tıklayarak kendi eşyalarımızı yaratmamızı sağlar
[CreateAssetMenu(fileName = "YeniEsya", menuName = "Envanter/Esya")]
public class EsyaVerisi : ScriptableObject
{
    [Header("Eşya Bilgileri")]
    public string esyaAdi;
    [TextArea] public string esyaAciklamasi; // UI'da görünmesi için küçük bir açıklama
    public Sprite esyaIkonu; // Çantada görünecek resim (UI için)
    
    [Header("Eşya Türü")]
    public EsyaTuru tur;
    
    [Header("Fiziksel Obje")]
    public GameObject esyaPrefab; // Eşyayı ileride çantadan yere geri atmak istersek diye
}

// Eşyalarımızın türlerini burada belirliyoruz. 
// The Forest gibi oyunlar için bu tür ayrımı çok işine yarayacak.
public enum EsyaTuru
{
    Silah,      // Mızrak, Tabanca vb.
    Malzeme,    // İp, Taş, Reçine vb. (Üretim için)
    Tuketilebilir // Can yenileyen şeyler vb.
}