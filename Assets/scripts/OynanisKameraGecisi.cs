using UnityEngine;
using UnityEngine.InputSystem;

public class OynanisKameraGecisi : MonoBehaviour
{
    [Header("Gerekli Sistemler")]
    public Transform fpsGozNoktasi; 
    public Transform oyuncuGovdesi; 
    
    [Header("FPS Fare Ayarları")]
    public float fareHassasiyeti = 0.2f; 

    private bool ilkSahisMi = false;
    private Transform orijinalParent;
    private CameraFollow eskiTakipKodu; 
    private float asagiYukariBakis = 0f; 

    private PlayerController yürümeKodu;

    void Start()
    {
        orijinalParent = transform.parent;
        eskiTakipKodu = GetComponent<CameraFollow>(); 

        if (oyuncuGovdesi != null)
        {
            yürümeKodu = oyuncuGovdesi.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        // Çanta menüsü açıkken (zaman durmuşsa) kameranın fareyi dinlemesini engelliyoruz
        if (Time.timeScale == 0f) return;

        if (Mouse.current != null)
        {
            float scrollY = Mouse.current.scroll.ReadValue().y;

            if (scrollY > 0 && !ilkSahisMi) GecisYapFPS();
            else if (scrollY < 0 && ilkSahisMi) GecisYapTPS();

            if (ilkSahisMi) FpsFareKontrolu();
        }
    }

    void FpsFareKontrolu()
    {
        Vector2 fareHareketi = Mouse.current.delta.ReadValue();
        float fareX = fareHareketi.x * fareHassasiyeti;
        float fareY = fareHareketi.y * fareHassasiyeti;

        asagiYukariBakis -= fareY;
        asagiYukariBakis = Mathf.Clamp(asagiYukariBakis, -90f, 90f);

        transform.localRotation = Quaternion.Euler(asagiYukariBakis, 0f, 0f);

        if (oyuncuGovdesi != null)
        {
            oyuncuGovdesi.Rotate(Vector3.up * fareX);
        }
    }

    void GecisYapFPS()
    {
        ilkSahisMi = true;
        if (eskiTakipKodu != null) eskiTakipKodu.enabled = false;
        
        if (yürümeKodu != null) yürümeKodu.fpsModundaMi = true;

        if (fpsGozNoktasi != null)
        {
            transform.SetParent(fpsGozNoktasi);
            transform.localPosition = Vector3.zero;
            asagiYukariBakis = 0f; 
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f); 
        }
    }

    void GecisYapTPS()
    {
        ilkSahisMi = false;
        transform.SetParent(orijinalParent);
        if (eskiTakipKodu != null) eskiTakipKodu.enabled = true;
        
        if (yürümeKodu != null) yürümeKodu.fpsModundaMi = false;
    }
}