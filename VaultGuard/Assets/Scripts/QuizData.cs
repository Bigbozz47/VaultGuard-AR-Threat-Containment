using System.Collections.Generic;

/// <summary>
/// Data structure untuk menyimpan informasi satu pertanyaan kuis keamanan siber
/// Class ini digunakan untuk serialisasi/deserialisasi data JSON dari local backup maupun AI API
/// </summary>
[System.Serializable]
public class QuizData
{
    /// <summary>Teks pertanyaan kuis</summary>
    public string pertanyaan;
    
    /// <summary>Pilihan jawaban A</summary>
    public string pilihan_a;
    
    /// <summary>Pilihan jawaban B</summary>
    public string pilihan_b;
    
    /// <summary>Pilihan jawaban C</summary>
    public string pilihan_c;
    
    /// <summary>Pilihan jawaban D</summary>
    public string pilihan_d;
    
    /// <summary>Jawaban yang benar (A, B, C, atau D)</summary>
    public string jawaban_benar;
    
    /// <summary>Topik virus untuk pertanyaan ini (Phishing, Ransomware, Trojan, dll)</summary>
    public string topik_virus;
    
    /// <summary>Tingkat kesulitan pertanyaan (mudah, sedang, sulit) - untuk ekspansi future</summary>
    public string tingkat_kesulitan;
}

/// <summary>
/// Container untuk menyimpan koleksi quiz data
/// Digunakan untuk parsing file JSON backup yang berisi multiple quiz
/// </summary>
[System.Serializable]
public class QuizDatabase
{
    /// <summary>List semua quiz yang tersedia di database lokal</summary>
    public List<QuizData> daftar_quiz;
}
