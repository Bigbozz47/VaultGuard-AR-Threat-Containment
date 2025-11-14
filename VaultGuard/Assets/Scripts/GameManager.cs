using UnityEngine;
using System.Collections.Generic; // Diperlukan untuk List/Array

/// <summary>
/// Bertindak sebagai Hub pusat untuk alur game, sesuai Panduan Tabel 2.
/// Mengelola state game, skor, dan mengoordinasikan UIManager, AIManager,
/// dan interaksi virus.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Referensi Manajer (Hubungkan di Inspector)")]
    [SerializeField]
    [Tooltip("Hubungkan ke UIManager pada _GameSystems")]
    private UIManager uiManager;

    // AIManager akan diakses melalui Singleton (AIManager.Instance)
    // Sesuai Panduan Hari 8 untuk refactor dari FindObjectOfType
    private AIManager aiManager;

    [Header("Status Game")]
    [SerializeField] private int skorPerJawabanBenar = 10;
    
    // Variabel privat untuk mengelola state
    private GameObject virusAktif;     // Virus yang sedang diklik
    private QuizData currentKuis;      // Kuis yang sedang aktif
    private int currentSkor = 0;   // Skor saat ini

    // Singleton Pattern untuk akses mudah (lebih baik dari FindObjectOfType)
    public static GameManager Instance { get; private set; }

    #region Unity Lifecycle

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opsional, jika game systems ada di scene berbeda
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Inisialisasi referensi manajer
        // Pastikan AIManager.cs ada di scene (biasanya di _GameSystems juga)
        aiManager = AIManager.Instance; 
        
        // Pastikan uiManager terhubung
        if (uiManager == null)
        {
            Debug.LogError("GameManager: UIManager belum terhubung di Inspector!");
        }
        
        // Reset skor saat game dimulai
        currentSkor = 0;
    }

    #endregion

    #region Alur Game Inti (Core Game Loop)

    /// <summary>
    /// Dipanggil oleh VirusInteraction.cs saat virus di-tap.
    /// Memulai alur permintaan kuis.
    /// </summary>
    /// <param name="virus">GameObject virus yang di-tap.</param>
    public void OnVirusTapped(GameObject virus)
    {
        virusAktif = virus;
        uiManager.ShowLoading(true); // Tampilkan panel loading

        // Asumsi VirusInteraction.cs memiliki variabel 'public string topikVirus'
        // Ini adalah desain yang lebih baik daripada hardcoding topik
        string topik = "Ransomware"; // Placeholder
        // if (virus.TryGetComponent<VirusInteraction>(out var virusInfo))
        // {
        //     topik = virusInfo.topikVirus;
        // }

        // Memulai permintaan AI (Panduan Hari 6)
        // Menggunakan callback OnQuizLoaded (Sukses) dan OnQuizLoadFailed (Gagal)
        aiManager.RequestQuizFromAI(topik, OnQuizLoaded, OnQuizLoadFailed);
    }

    /// <summary>
    /// Dipanggil oleh UIManager saat pemain mengklik tombol jawaban.
    /// Memvalidasi jawaban dan memperbarui skor.
    /// </summary>
    /// <param name="selectedIndex">Index tombol yang diklik (0=A, 1=B, 2=C, 3=D).</param>
    public void OnAnswerSelected(int selectedIndex)
    {
        if (currentKuis == null) return; // Pengaman jika kuis belum dimuat

        // Konversi index (int) ke string jawaban ("A", "B", "C", "D")
        // Sesuai dengan format QuizData.jawaban_benar
        string[] answerMap = { "A", "B", "C", "D" };
        string selectedAnswer = "X"; // Default
        
        if (selectedIndex >= 0 && selectedIndex < answerMap.Length)
        {
            selectedAnswer = answerMap[selectedIndex];
        }

        // Validasi jawaban menggunakan AIManager
        bool isCorrect = aiManager.ValidasiJawaban(currentKuis, selectedAnswer);

        if (isCorrect)
        {
            // Logika jika jawaban BENAR
            currentSkor += skorPerJawabanBenar;
            
            // Beri tahu virus untuk memainkan animasi "Tertangkap"
            // if (virusAktif != null)
            // {
            //     virusAktif.GetComponent<VirusInteraction>().Caught();
            // }
            Debug.Log("Virus Tertangkap! (Panggil animasi di sini)");
        }
        else
        {
            // Logika jika jawaban SALAH
            // if (virusAktif != null)
            // {
            //     virusAktif.GetComponent<VirusInteraction>().Escape();
            // }
            Debug.Log("Virus Kabur! (Panggil animasi di sini)");
        }

        // Tampilkan panel hasil (BENAR/SALAH) dan skor baru
        uiManager.ShowResult(isCorrect, currentSkor);
        
        // Polesan (Panduan Hari 8): Highlight tombol
        uiManager.HighlightButton(selectedIndex, isCorrect);
        
        // Kosongkan kuis saat ini setelah selesai
        currentKuis = null;
    }

    #endregion

    #region Callback AI (Async)

    /// <summary>
    /// Callback jika permintaan AI SUKSES.
    /// </summary>
    private void OnQuizLoaded(QuizData kuisFromAI)
    {
        uiManager.ShowLoading(false); // Sembunyikan loading
        currentKuis = kuisFromAI;      // Simpan kuis LIVE
        
        // Tampilkan panel kuis dengan data dari AI
        uiManager.ShowKuisPanel(currentKuis); 
    }

    /// <summary>
    /// Callback jika permintaan AI GAGAL (Rencana B).
    /// </summary>
    private void OnQuizLoadFailed(string error)
    {
        Debug.LogError($"AI Gagal ({error}), beralih ke Rencana B: Kuis Lokal.");
        
        uiManager.ShowLoading(false); // Sembunyikan loading

        // RENCANA B: Muat kuis lokal dari AIManager
        // string topik = "Ransomware"; // Dapatkan topik dari virusAktif
        QuizData kuisLokal = aiManager.LoadQuizLokal(); // Menggunakan overload random
        
        currentKuis = kuisLokal;       // Simpan kuis LOKAL
        uiManager.ShowKuisPanel(currentKuis); // Tampilkan panel kuis
    }

    #endregion
}