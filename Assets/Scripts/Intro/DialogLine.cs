using UnityEngine;
using System; // Penting untuk System.Serializable

[Serializable] // Ini agar kelas bisa ditampilkan dan diedit di Inspector
public class DialogLine
{
    [TextArea(3, 10)] // Membuat kolom teks lebih besar di Inspector
    public string text; // Teks untuk baris dialog ini
    public AudioClip typingSound; // AudioClip khusus untuk baris ini
    public float soundPlayDelay = 0.05f; // Jeda suara untuk baris ini (opsional, bisa sama semua)

    // === TAMBAHKAN BARIS INI ===
    public float delay = 0.05f; // Jeda waktu per karakter untuk baris ini
    // ===========================
}