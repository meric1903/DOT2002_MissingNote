using UnityEngine;

public class RaycastFishek : MonoBehaviour
{
    // Hocam, bu kutucuğa hazırladığımız kırmızı efekti koyacağız.
    public GameObject efektPrefab; 
    public float menzil = 100f; // Fişek ne kadar uzağa gitsin?

    void Update()
    {
        // Sol tıkla veya Space ile ateş et
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            AtesEt();
        }
    }

    void AtesEt()
    {
        // 1. IŞIN TANIMLAMA: Objenin merkezinden, ileriye doğru bir ışın (Ray)
        Ray isin = new Ray(transform.position, transform.forward);
        RaycastHit temas; // Çarpışma bilgilerini burada saklayacağız.

        // 2. RAYCAST FIRLATMA: Işın bir şeye çarptı mı?
        if (Physics.Raycast(isin, out temas, menzil))
        {
            // Eğer bir şeye çarptıysa (duvar, ağaç vb.) tam çarptığı noktada efekti oluştur.
            Instantiate(efektPrefab, temas.point, Quaternion.identity);
            
            // Hocam bakın, çarptığımız nesnenin adını konsola yazdırıyoruz:
            Debug.Log("Raycast şuna çarptı: " + temas.collider.name);
        }
        else
        {
            // Eğer gökyüzü boşsa, ışın menzili kadar ileride patlasın.
            Vector3 boslukNoktasi = transform.position + (transform.forward * menzil);
            Instantiate(efektPrefab, boslukNoktasi, Quaternion.identity);
        }
    }
}