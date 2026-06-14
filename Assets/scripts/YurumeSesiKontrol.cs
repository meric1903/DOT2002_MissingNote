using UnityEngine;
using UnityEngine.InputSystem; 

public class YurumeSesiKontrol : MonoBehaviour
{
    private AudioSource yurumeSesi;
    private Vector3 oncekiPozisyon;

    [Header("Ses Hızı Ayarları")]
    public float normalYurumeHizi = 1f;    
    public float kosmaHiziCarpani = 1.25f;  

    void Start()
    {
        yurumeSesi = GetComponent<AudioSource>();
        oncekiPozisyon = transform.position; // Başlangıç pozisyonunu hafızaya al
    }

    void Update()
    {
        if (yurumeSesi == null) return;

        // Karakterin Y eksenini (zıplama/düşme) yoksayarak sadece yatayda ne kadar kaydığını hesapla
        Vector3 suankiPozisyon = transform.position;
        suankiPozisyon.y = 0; 
        
        Vector3 hesaplananOncekiPoz = oncekiPozisyon;
        hesaplananOncekiPoz.y = 0;

        float hareketMiktari = Vector3.Distance(suankiPozisyon, hesaplananOncekiPoz);
        oncekiPozisyon = transform.position; // Bir sonraki salise için konumu kaydet

        // Eğer karakter milim bile olsa kaymışsa (hareket ediyorsa)
        if (hareketMiktari > 0.005f)
        {
            // Shift'e basılıyorsa pitch'i artır, yoksa normal çal
            if (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed)
            {
                yurumeSesi.pitch = kosmaHiziCarpani; 
            }
            else
            {
                yurumeSesi.pitch = normalYurumeHizi; 
            }

            if (!yurumeSesi.isPlaying)
            {
                yurumeSesi.Play(); 
            }
        }
        else
        {
            // Karakter tamamen durduğunda sesi duraklat
            if (yurumeSesi.isPlaying)
            {
                yurumeSesi.Pause(); 
            }
        }
    }
}