using UnityEngine;

public class MarkerMonsterController : MonoBehaviour
{
    [Header("Animator semua monster di marker ini")]
    public Animator[] animators;

    void Start()
    {
        // Di awal scene, matikan semua Animator supaya tidak jalan sebelum scan
        foreach (var anim in animators)
        {
            if (anim != null)
                anim.enabled = false;
        }
    }

    // Dipanggil setiap kali marker KETEMU (On Target Found)
    public void OnMarkerFound()
    {
        foreach (var anim in animators)
        {
            if (anim == null) continue;

            anim.gameObject.SetActive(true);   // kalau sebelumnya di-disable
            anim.enabled = true;

            // Reset semua parameter & state ke awal
            anim.Rebind();
            anim.Update(0f);

            // TIDAK perlu Play("Spawn") lagi,
            // karena default state sudah Spawn
        }
    }

    // Dipanggil saat marker HILANG (On Target Lost)
    public void OnMarkerLost()
    {
        foreach (var anim in animators)
        {
            if (anim == null) continue;

            // Stop animator biar nggak lanjut di belakang layar
            anim.enabled = false;
            // Optional: sembunyikan monsternya
            // anim.gameObject.SetActive(false);
        }
    }
}
