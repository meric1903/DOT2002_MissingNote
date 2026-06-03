using UnityEngine;

public class SilahTutusIK : MonoBehaviour
{
    private Animator anim;
    
    [Header("Ayarlar")]
    public Transform sagElHedefi; // Elin uzanacağı o görünmez nokta
    public Transform silahTutucu; // İçinde silah var mı diye kontrol edeceğimiz asıl yuva

    void Start()
    {
        // Karakterin üstündeki Animator'ü otomatik bul
        anim = GetComponent<Animator>();
    }

    // Bu özel fonksiyon, Animator'de "IK Pass" açıldığı için Unity tarafından otomatik çalıştırılır
    void OnAnimatorIK(int layerIndex)
    {
        if (anim == null) return;

        // EĞER SİLAH TUTUCUNUN İÇİNDE BİR OBJE VARSA (Yani silahı kuşanmışsak)
        if (silahTutucu != null && silahTutucu.childCount > 0)
        {
            // Eli ve bileği o hedefe %100 oranında (1f) kilitle
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            
            if (sagElHedefi != null)
            {
                anim.SetIKPosition(AvatarIKGoal.RightHand, sagElHedefi.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, sagElHedefi.rotation);
            }
        }
        else
        {
            // EĞER SİLAH YOKSA: IK sistemini kapat (%0), kol normal yürüme animasyonuna geri dönsün
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        }
    }
}