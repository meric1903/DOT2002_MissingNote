using UnityEngine;
using System.Collections;

public class HavaliFisek : MonoBehaviour
{
    private bool hasFired = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFired)
        {
            if (!Physics.Raycast(other.transform.position + Vector3.up, Vector3.up, 20f))
            {
                StartCoroutine(FisekSureci(other.transform));
                hasFired = true;
            }
        }
    }

    IEnumerator FisekSureci(Transform playerTransform)
    {
        // 1. FİŞEĞİ OLUŞTUR VE FIRLAT
        GameObject fisek = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        fisek.transform.position = playerTransform.position + Vector3.up * 2f;
        fisek.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
        fisek.GetComponent<Renderer>().material.color = Color.red;

        Rigidbody rb = fisek.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 700f);

        // İz efekti
        TrailRenderer trail = fisek.AddComponent<TrailRenderer>();
        trail.time = 0.5f;
        trail.startWidth = 0.15f;
        trail.endWidth = 0f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.colorGradient = GetFireGradient();

        // Işık efekti
        Light light = fisek.AddComponent<Light>();
        light.color = Color.red;
        light.intensity = 5f;

        // 2. YÜKSELİŞİ BEKLE VE DURDUR
        // Yaklaşık 1.5 saniye sonra fişek en tepeye ulaşır
        yield return new WaitForSeconds(1.2f);
        
        rb.isKinematic = true; // Yerçekimini ve hızı iptal et (Havada asılı kalır)

        // 3. HAVADA ASILI KALMA SÜRESİ
        yield return new WaitForSeconds(2.0f);

        // 4. PATLAMA EFEKTİ
        Patlat(fisek.transform.position);

        // Fişeği yok et
        Destroy(fisek);
    }

    void Patlat(Vector3 pos)
    {
        // Basit bir patlama görseli (Bir sürü küçük parça saçalım)
        for (int i = 0; i < 15; i++)
        {
            GameObject parca = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            parca.transform.position = pos;
            parca.transform.localScale = Vector3.one * 0.2f;
            parca.GetComponent<Renderer>().material.color = Color.red;

            Rigidbody rb = parca.AddComponent<Rigidbody>();
            // Rastgele yönlere fırlat
            rb.AddForce(Random.insideUnitSphere * 300f);

            // Parçaların da iz bırakmasını sağla
            TrailRenderer t = parca.AddComponent<TrailRenderer>();
            t.time = 0.3f;
            t.startWidth = 0.1f;
            t.material = new Material(Shader.Find("Sprites/Default"));
            
            Destroy(parca, 1.5f);
        }
        Debug.Log("BOOM! Fişek patladı.");
    }

    Gradient GetFireGradient()
    {
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        return g;
    }
}