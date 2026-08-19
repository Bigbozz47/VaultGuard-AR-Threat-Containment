using UnityEngine;

public class JailAnimation : MonoBehaviour
{
    [Header("ANIMATORS")]
    // Animator untuk virus kecil yang ada di dalam root get_caught
    public Animator virusAnimator; 
    // Animator untuk rantai
    public Animator chainAnimator; 

    [Header("PENJARA OBJEK")]
    // Objek fisik penjara
    public GameObject cage1; 
    public GameObject cage2; 

    public void PlayCaughtSequence()
    {
        // tampilkan penjara fisik
        if (cage1) cage1.SetActive(true);
        if (cage2) cage2.SetActive(true);

        // animasi di virus kecil (pose terikat/tertangkap)
        if (virusAnimator)
            virusAnimator.SetTrigger("Caught");

        // animasi di rantai (gerakan mengikat)
        if (chainAnimator)
            chainAnimator.SetTrigger("Bind"); 
            
        // Catatan: Pastikan Controller virus kecil memiliki Trigger "Caught" 
        // dan Controller rantai memiliki Trigger "Bind".
    }

    public void ResetState()
    {
        // sembunyikan penjara
        if (cage1) cage1.SetActive(false);
        if (cage2) cage2.SetActive(false);

        // reset animasi ke Idle/default state
        // Menggunakan Play() untuk memaksa state awal (layer 0, waktu 0)
        if (virusAnimator)
            virusAnimator.Play("Idle", 0, 0);

        if (chainAnimator)
            chainAnimator.Play("Idle", 0, 0);
    }
}