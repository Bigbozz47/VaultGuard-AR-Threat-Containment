using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    // Animator untuk monster utama (Virus_Merah)
    public Animator animator;

    void Awake()
    {
        // Mencari komponen Animator pada GameObject ini jika belum terisi
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlaySpawn()
    {
        animator.SetTrigger("Spawn");
    }

    public void PlayIdle()
    {
        animator.SetBool("Idle", true);
    }

    public void PlayEscape()
    {
        animator.SetTrigger("Escape");
        animator.SetBool("Idle", false);
    }

    // Hanya mematikan Idle, membiarkan JailAnimation menangani visual Caught
    public void PlayCaughtPose()
    {
        // Pastikan monster utama berhenti bergerak/idle saat tertangkap
        animator.SetBool("Idle", false); 
        
        // CATATAN: Jika monster utama memiliki pose Caught yang spesifik (bukan Idle),
        // Anda bisa menambahkan animator.SetTrigger("Caught"); di sini
        // dan menambahkannya ke Controller monster utama.
    }
}