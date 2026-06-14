using UnityEngine;
using Unity.Cinemachine;

public class KameraOtomasyon : MonoBehaviour
{
    [Header("Geçiþ Yapýlacak Kameralar")]
    public CinemachineCamera kapanacakKamera; // Eski kameran (Önceliði 0 olacak)
    public CinemachineCamera acilacakKamera;  // Yeni kameran (Önceliði 100 olacak)

    private void OnTriggerEnter(Collider other)
    {
        // Karakter (Player) bu küpün (Trigger) içine girdiðinde
        if (other.CompareTag("Player"))
        {
            // Eski kameranýn önceliðini düþür (Geri plana at)
            if (kapanacakKamera != null)
            {
                kapanacakKamera.Priority = 0;
            }

            // Yeni kameranýn önceliðini yükselt (Ana ekrana al)
            if (acilacakKamera != null)
            {
                acilacakKamera.Priority = 100;
            }
        }
    }
}