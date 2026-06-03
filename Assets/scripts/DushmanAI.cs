using UnityEngine;
using UnityEngine.AI;

public class DushmanAI : MonoBehaviour
{
    [Header("Hayvan Kimliği ve Ayarları")]
    public string hayvanAdi = "Vahşi Ayı";
    public float vuracagiHasar = 40f;  
    public float hareketHizi = 3.5f;   
    public float saldiriMesafesi = 2f; 
    public float takipMesafesi = 15f;  

    [Header("Saldırı Zamanlaması")]
    public float saldiriHizi = 2f;     
    private float saldiriZamanlayici;

    [Header("Rastgele Gezinme (Devriye)")]
    public float gezinmeYaricapi = 20f; 
    public float beklemeSuresi = 4f;    
    private float gezinmeZamanlayici;

    [Header("Bağlantılar")]
    // İŞTE INSPECTOR'DA GÖRÜNECEK OLAN KUTUMUZ!
    public Transform oyuncuHedef; 

    private Animator hayvanAnim; 
    private NavMeshAgent ajan;
    private float mesafe;

    void Start()
    {
        ajan = GetComponent<NavMeshAgent>();
        hayvanAnim = GetComponentInChildren<Animator>(); 
        
        // Eğer Inspector'dan elinle koymayı unutursan diye otomatik bulma sigortası
        if (oyuncuHedef == null)
        {
            GameObject oyuncuObjesi = GameObject.Find("karakterprefab");
            if (oyuncuObjesi != null) oyuncuHedef = oyuncuObjesi.transform;
        }

        if (ajan != null) ajan.speed = hareketHizi;

        YeniRastgeleHedefSec();
    }

    void Update()
    {
        if (oyuncuHedef == null || ajan == null) return;

        mesafe = Vector3.Distance(transform.position, oyuncuHedef.position);

        // 1. DURUM: SALDIRI
        if (mesafe <= saldiriMesafesi)
        {
            ajan.isStopped = true; 
            
            if (Time.time > saldiriZamanlayici)
            {
                SaldiriYap();
                saldiriZamanlayici = Time.time + saldiriHizi; 
            }
        }
        // 2. DURUM: TAKİP
        else if (mesafe <= takipMesafesi)
        {
            ajan.isStopped = false;
            ajan.SetDestination(oyuncuHedef.position); 
            ajan.speed = 6f; // Peşinden koşarken hızlansın (Run animasyonu)
        }
        // 3. DURUM: RASTGELE GEZİNME
        else
        {
            ajan.isStopped = false;
            ajan.speed = 3f; // Sakince gezinirken yavaşlasın (Walk animasyonu)

            if (!ajan.pathPending && ajan.remainingDistance <= ajan.stoppingDistance)
            {
                if (Time.time > gezinmeZamanlayici)
                {
                    YeniRastgeleHedefSec();
                    gezinmeZamanlayici = Time.time + beklemeSuresi;
                }
            }
        }

        if (hayvanAnim != null)
        {
            hayvanAnim.SetFloat("Hiz", ajan.velocity.magnitude);
        }
    }

    void YeniRastgeleHedefSec()
    {
        Vector3 rastgeleYon = Random.insideUnitSphere * gezinmeYaricapi;
        rastgeleYon += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rastgeleYon, out hit, gezinmeYaricapi, 1))
        {
            ajan.SetDestination(hit.position);
        }
    }

    void SaldiriYap()
    {
        if (CanSistemi.Instance != null)
        {
            CanSistemi.Instance.HasarAl(vuracagiHasar);
            Debug.Log("<color=red>" + hayvanAdi + " sana saldırdı! Vurulan Hasar: " + vuracagiHasar + "</color>");
            
            if (hayvanAnim != null)
            {
                hayvanAnim.SetTrigger("Saldiri");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, saldiriMesafesi);
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireSphere(transform.position, takipMesafesi);
    }
}