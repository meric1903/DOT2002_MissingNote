using UnityEngine;

public class TimedFlareSystem : MonoBehaviour
{
    [Header("Fişek Ayarları")]
    public float cikisSuresi = 3.0f; // Tam olarak kaç saniye yükselecek?
    public float cikisHizi = 15.0f;  // Yukarı tırmanma hızı
    public Color fisekRengi = Color.white; // Aydınlatma fişeği genelde beyaz/sarı olur
    
    private bool ateslendi = false;

    private void OnTriggerEnter(Collider other)
    {
        // Oyuncu bölgeye girdiğinde
        if (other.CompareTag("Player") && !ateslendi)
        {
            // Raycast ile gökyüzü kontrolü (isteğe bağlı, engelde patlamasın diye)
            if (!Physics.Raycast(transform.position, Vector3.up, 50f))
            {
                Firlat();
                ateslendi = true;
            }
        }
    }

    void Firlat()
    {
        // 1. Fişek gövdesini oluştur
        GameObject fisek = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fisek.transform.position = transform.position + Vector3.up * 1f;
        fisek.transform.localScale = new Vector3(0.3f, 0.4f, 0.3f); // Hafif uzun mermi formu
        
        // 2. İz (Trail) Ekle - Mermi gibi çıktığını görmek için
        TrailRenderer tr = fisek.AddComponent<TrailRenderer>();
        tr.time = 0.4f;
        tr.startWidth = 0.15f;
        tr.endWidth = 0.05f;
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = fisekRengi;
        tr.endColor = new Color(fisekRengi.r, fisekRengi.g, fisekRengi.b, 0);

        // 3. Işık Ekle
        Light l = fisek.AddComponent<Light>();
        l.color = fisekRengi;
        l.intensity = 2f;

        // 4. Hareket ve Patlama Mantığını Yöneten Scripti Ekle
        var logic = fisek.AddComponent<FlareMovementLogic>();
        logic.Setup(cikisSuresi, cikisHizi, fisekRengi);
    }
}

public class FlareMovementLogic : MonoBehaviour
{
    private float timer;
    private float speed;
    private float duration;
    private Color flareColor;
    private bool isMoving = true;

    public void Setup(float _duration, float _speed, Color _color)
    {
        duration = _duration;
        speed = _speed;
        flareColor = _color;
    }

    void Update()
    {
        if (isMoving)
        {
            // Belirlenen süre boyunca dümdüz yukarı hareket et
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                Patlat();
            }
        }
    }

    void Patlat()
    {
        isMoving = false;

        // 1. Şiddetli Aydınlatma (Magnezyum yanması gibi)
        Light l = GetComponent<Light>();
        l.intensity = 50f;
        l.range = 100f;

        // 2. Görsel Dağılma (Parçacık efekti - Havai fişek gibi değil, yanma artığı gibi)
        GameObject burst = new GameObject("FlareBurn");
        burst.transform.position = transform.position;
        ParticleSystem ps = burst.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startColor = flareColor;
        main.startSize = 0.1f;
        main.startSpeed = 3f; // Yavaş dağılsın
        main.gravityModifier = 0.5f; // Parçalar hafifçe yere doğru süzülsün
        main.duration = 2f;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = ps.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 40) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;

        burst.GetComponent<ParticleSystemRenderer>().material = new Material(Shader.Find("Particles/Standard Unlit"));
        ps.Play();

        // 3. Obje Temizliği
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 3f); // Işık 3 saniye daha ortamı aydınlatıp sonra söner
    }
}