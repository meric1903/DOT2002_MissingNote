using UnityEngine;
// Eğer yeni input sistemini kullanıyorsan bu kütüphane gerekir
using UnityEngine.InputSystem; 

public class ElfeneriKontrol : MonoBehaviour
{
    public Light elFeneriIsigi; 

    void Start()
    {
        if (elFeneriIsigi != null)
        {
            elFeneriIsigi.enabled = false; 
        }
    }

    void Update()
    {
        // Yeni Input System için F tuşu kontrolü
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (elFeneriIsigi != null)
            {
                elFeneriIsigi.enabled = !elFeneriIsigi.enabled;
            }
        }
    }
}
