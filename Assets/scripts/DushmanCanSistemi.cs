using UnityEngine;
using UnityEngine.AI;

public class DushmanCanSistemi : MonoBehaviour
{
    [Header("Can Ayarları")]
    public float maksimumCan = 100f; // Ayı için 150, Kurt için 50 yapabilirsin
    private float mevcutCan;
    
    private Animator hayvanAnim;
    private NavMeshAgent ajan;
    private DushmanAI yapayZeka;

    private bool oluMu = false;

    void Start()
    {
        mevcutCan = maksimumCan;
        
        // Hayvanın üzerindeki sistemleri otomatik buluyoruz
        hayvanAnim = GetComponentInChildren<Animator>();
        ajan = GetComponent<NavMeshAgent>();
        yapayZeka = GetComponent<DushmanAI>();
    }

    // Silahımızdan çıkan merminin çağıracağı hasar fonksiyonu
    public void HasarAl(float hasarMiktari)
    {
        if (oluMu) return; // Zaten ölüyse tekrar hasar yemesin

        mevcutCan -= hasarMiktari;
        Debug.Log("<color=orange>" + gameObject.name + " Vuruldu! Kalan Can: " + mevcutCan + "</color>");

        if (mevcutCan <= 0)
        {
            Olum();
        }
    }

    void Olum()
    {
        oluMu = true;
        Debug.Log("<color=red>" + gameObject.name + " Öldü!</color>");
        
        // 1. Hayvanın motorunu kapat (Koşmayı bırakır)
        if (ajan != null) ajan.enabled = false;
        
        // 2. Hayvanın yapay zekasını kapat (Saldırmayı bırakır)
        if (yapayZeka != null) yapayZeka.enabled = false;

        // 3. Ölüm animasyonunu tetikle 
        // (Bunun için Animatör'e 'Olum' adında bir Trigger ekleyip ölüm animasyonuna bağlayabilirsin)
        if (hayvanAnim != null) hayvanAnim.SetTrigger("Olum");

        // İstersen cesedi 10 saniye sonra sahneden tamamen sil
        Destroy(gameObject, 10f);
    }
}