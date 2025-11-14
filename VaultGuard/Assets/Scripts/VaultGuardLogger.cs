using UnityEngine;

/// <summary>
/// Custom logger untuk VaultGuard project dengan format yang konsisten
/// Memudahkan debugging dengan menambahkan prefix module dan color coding
/// </summary>
public static class VaultGuardLogger
{
    private const string PREFIX = "[VaultGuard]";
    
    /// <summary>
    /// Log pesan informasi standar
    /// </summary>
    /// <param name="module">Nama module/class yang memanggil log (misal: "AIManager")</param>
    /// <param name="message">Pesan yang akan di-log</param>
    public static void Log(string module, string message)
    {
        Debug.Log($"{PREFIX}[{module}] {message}");
    }
    
    /// <summary>
    /// Log pesan warning (ditandai dengan emoji warning)
    /// </summary>
    /// <param name="module">Nama module/class yang memanggil log</param>
    /// <param name="message">Pesan warning</param>
    public static void LogWarning(string module, string message)
    {
        Debug.LogWarning($"{PREFIX}[{module}] ⚠️ {message}");
    }
    
    /// <summary>
    /// Log pesan error (ditandai dengan emoji cross)
    /// </summary>
    /// <param name="module">Nama module/class yang memanggil log</param>
    /// <param name="message">Pesan error</param>
    public static void LogError(string module, string message)
    {
        Debug.LogError($"{PREFIX}[{module}] ❌ {message}");
    }
    
    /// <summary>
    /// Log pesan success (ditandai dengan emoji checkmark)
    /// </summary>
    /// <param name="module">Nama module/class yang memanggil log</param>
    /// <param name="message">Pesan success</param>
    public static void LogSuccess(string module, string message)
    {
        Debug.Log($"{PREFIX}[{module}] ✅ {message}");
    }
}
