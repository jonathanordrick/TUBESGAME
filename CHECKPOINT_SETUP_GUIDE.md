# 🎮 PANDUAN SETUP SISTEM CHECKPOINT 2x LIVES

## ✅ CHECKLIST SETUP LENGKAP

### 1. PLAYER SETUP

- [ ] **Tag**: "Player"
- [ ] **Components**:
  - [ ] Rigidbody2D (Body Type = Dynamic)
  - [ ] Collider2D (BoxCollider2D atau sejenisnya)
  - [ ] Animator (dengan trigger "hurt" dan "die")
  - [ ] **Scripts**:
    - [ ] `PlayerHealth.cs`
    - [ ] `respawn.cs`
    - [ ] `PlayerMovement.cs`

### 2. CHECKPOINT SETUP

- [ ] **GameObject**: "Checkpoint"
- [ ] **Components**:
  - [ ] Collider2D dengan **Is Trigger = TRUE**
  - [ ] Script `Checkpoint.cs`
- [ ] **Inspector Settings**:
  - [ ] Checkpoint Effect (opsional)
  - [ ] Checkpoint Sound (opsional)

### 3. ENEMY/DAMAGE DEALER SETUP

- [ ] **Tag**: "Enemy" atau "Spikes" atau "DeathZone"
- [ ] **Collider2D**: untuk deteksi collision
- [ ] **Script**: Yang memanggil `PlayerHealth.ChangeHealth(-damage)`

### 4. SCENE "ENDGAME" SETUP

- [ ] **Buat Scene**: File → New Scene → Save as "EndGame"
- [ ] **UI Elements**:
  - [ ] Canvas
  - [ ] Text "Game Over"
  - [ ] Button "Restart" → OnClick: EndGameManager.RestartGame()
  - [ ] Button "Main Menu" → OnClick: EndGameManager.LoadMainMenu()
  - [ ] Button "Quit" → OnClick: EndGameManager.QuitGame()
- [ ] **Script**: `EndGameManager.cs` di GameObject
- [ ] **Build Settings**: Tambahkan scene ke Build Settings

### 5. UI LIVES DISPLAY (OPSIONAL)

- [ ] **Canvas** di scene game
- [ ] **Text/TextMeshPro**: untuk menampilkan "Lives: 2/2"
- [ ] **Images**: array icon hidup (hearts, dll)
- [ ] **Script**: `LivesDisplay.cs`

## 🔧 KONFIGURASI INSPECTOR

### PlayerHealth.cs

- **Max Health**: 10 (atau sesuai kebutuhan)
- **Max Lives**: 2
- **Respawn Delay**: 1
- **End Game Scene Name**: "EndGame"

### respawn.cs

- **Respawn Delay**: 1
- **Death Layer**: Set layer objek mematikan
- **Death Effect**: Prefab efek kematian (opsional)
- **Respawn Effect**: Prefab efek respawn (opsional)

### Checkpoint.cs

- **Checkpoint Effect**: Prefab efek checkpoint aktif (opsional)
- **Checkpoint Sound**: Audio clip suara checkpoint (opsional)

## 🎯 CARA KERJA SISTEM

1. **Player Start**: 2 lives, health penuh
2. **Terkena Damage**: Health berkurang, animasi "hurt"
3. **Health = 0**:
   - Lives berkurang 1
   - Jika lives > 0: Respawn di checkpoint dengan health penuh
   - Jika lives = 0: Load scene "EndGame"
4. **Checkpoint**: Saat player menyentuh, update posisi respawn

## 🧪 TESTING

1. **Test Checkpoint**:
   - Player menyentuh checkpoint → Console: "Checkpoint activated at: (x,y)"
2. **Test Damage**:
   - Player terkena damage → Health berkurang
   - Health = 0 → Player mati, respawn di checkpoint
3. **Test Lives**:
   - Mati 1x → Respawn dengan health penuh
   - Mati 2x → Respawn dengan health penuh
   - Mati 3x → Load scene "EndGame"

## 🐛 TROUBLESHOOTING

### Player tidak respawn di checkpoint:

- Cek apakah checkpoint memiliki collider dengan "Is Trigger = TRUE"
- Cek tag Player = "Player"
- Cek Console untuk error messages

### Scene EndGame tidak load:

- Pastikan scene "EndGame" ada di Build Settings
- Cek nama scene di PlayerHealth → End Game Scene Name

### UI Lives tidak update:

- Pastikan LivesDisplay script terpasang
- Pastikan referensi PlayerHealth sudah benar

## 📝 SCRIPT CALLS UNTUK DAMAGE

Untuk memberikan damage ke player dari enemy/trap:

```csharp
// Di script enemy/trap
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ChangeHealth(-1); // Kurangi 1 health
        }
    }
}
```

Semua sistem sudah siap! 🚀
