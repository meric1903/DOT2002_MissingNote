using UnityEngine;
using Unity.Cinemachine;

public class KameraOtomasyon : MonoBehaviour
{
    public CinemachineCamera icKamera;
    public CinemachineCamera disKamera;

    // 2D takýsýný sildik, artýk 3D fizik kullanýyoruz
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            icKamera.Priority = 0;
            disKamera.Priority = 100;
        }
    }
}