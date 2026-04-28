using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [Header("Zaman Ayarlarý")]
    public float timeSpeed = 1f; // Süreyi uzatmak için 1 veya 0.5 yap

    [Header("Iþýk Kaynaklarý")]
    public Light sun;
    public Light moon;

    [Header("Skybox Materyalleri")]
    public Material morningSkybox;
    public Material eveningSkybox;

    [Header("Iþýk Þiddetleri")]
    public float dayIntensity = 1f;
    public float nightIntensity = 0.3f;

    void Update()
    {
        if (sun == null || morningSkybox == null || eveningSkybox == null)
        {
            Debug.LogError("Lütfen Inspector üzerinden Skybox ve Iþýklarý atayýn!");
            return;
        }

        // 1. GÜNEÞÝN DÖNMESÝ
        sun.transform.Rotate(Vector3.right * timeSpeed * Time.deltaTime);

        // 2. AYIN TAKÝBÝ
        if (moon != null)
            moon.transform.rotation = sun.transform.rotation * Quaternion.Euler(180f, 0f, 0f);

        // 3. GÜNEÞÝN YÜKSEKLÝK DEÐERÝ (y: -1 ile 1 arasý)
        float sunY = sun.transform.forward.y;

        // 4. SKYBOX VE IÞIK KONTROLÜ
        if (sunY > 0) // Güneþ ufkun altýnda (GECE)
        {
            // Skybox'ý akþam materyaline çevir
            if (RenderSettings.skybox != eveningSkybox)
            {
                RenderSettings.skybox = eveningSkybox;
                DynamicGI.UpdateEnvironment(); // Etrafý yeni ýþýða göre tazele
            }

            // Güneþin þiddetini yavaþça sýfýrla, Ayýn þiddetini aç
            sun.intensity = Mathf.Lerp(sun.intensity, 0, Time.deltaTime);
            if (moon != null) moon.intensity = Mathf.Lerp(moon.intensity, nightIntensity, Time.deltaTime);
        }
        else // Güneþ ufkun üstünde (GÜNDÜZ)
        {
            // Skybox'ý sabah materyaline çevir
            if (RenderSettings.skybox != morningSkybox)
            {
                RenderSettings.skybox = morningSkybox;
                DynamicGI.UpdateEnvironment();
            }

            // Güneþin þiddetini aç, Ayýn þiddetini kapa
            sun.intensity = Mathf.Lerp(sun.intensity, dayIntensity, Time.deltaTime);
            if (moon != null) moon.intensity = Mathf.Lerp(moon.intensity, 0, Time.deltaTime);
        }
    }
}