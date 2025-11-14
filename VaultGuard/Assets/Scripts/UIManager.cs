using UnityEngine;
using UnityEngine.UI;
using TMPro; // Diperlukan untuk TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelKuis;
    public GameObject panelSkor;
    // Ditambahkan sekarang untuk menghemat waktu di Hari 6 
    public GameObject panelLoading; 

    [Header("Panel Kuis")]
    public TextMeshProUGUI questionText;
    // Menggunakan array lebih bersih dan skalabel 
    public Button answerButtons;   
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI scoreText;

    // Fungsi Stub (API Skrip) yang akan diisi di hari-hari berikutnya
    // 

    /// <summary>
    /// Menampilkan panel kuis, menyembunyikan panel hasil,
    /// dan mengisi teks pertanyaan dan jawaban.
    /// </summary>
    public void ShowKuisPanel(string question, string answers)
    {
        // TODO: Akan diisi di Hari 4
        // questionText.text = question;
        //... (loop melalui answerButtons)...
        // panelKuis.SetActive(true);
        // panelSkor.SetActive(false);
    }

    /// <summary>
    /// Menampilkan panel hasil (Benar/Salah) dan memperbarui skor.
    /// </summary>
    public void ShowResult(bool isCorrect, int newScore)
    {
        // TODO: Akan diisi di Hari 4
        // statusText.text = isCorrect? "BENAR!" : "SALAH!";
        // scoreText.text = "Skor: " + newScore;
        // panelKuis.SetActive(false);
        // panelSkor.SetActive(true);
    }

    /// <summary>
    /// Mengontrol visibilitas panel loading (untuk panggilan API).
    /// </summary>
    public void ShowLoading(bool isActive)
    {
        // TODO: Akan diisi di Hari 6
        // panelLoading.SetActive(isActive);
    }
}