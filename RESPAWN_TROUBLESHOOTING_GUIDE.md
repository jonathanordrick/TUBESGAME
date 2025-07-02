# Respawn System Troubleshooting Guide

## Quick Fix for Immediate Death Issue

The most common cause of players dying immediately when the game starts is the **Death Layer configuration** in the `respawn.cs` script.

### Problem: Death Layer Set to "Everything" (-1)

If `deathLayer` is set to `-1` (Everything), the player will die when touching ANY object, including the ground.

### Solution: Configure Death Layer Properly

1. **Open Unity Editor**
2. **Select the Player GameObject**
3. **In the Inspector, find the "respawn" component**
4. **In the "Death Layer" field, set it to:**
   - `0` (Nothing) - Player only dies from tagged objects and falling below Y=-50
   - OR select specific layers for deadly objects (like "Hazards", "Spikes", etc.)

### Recommended Setup

#### Option 1: Tag-Based Death Detection (Recommended for beginners)

- Set `Death Layer` to `0` (Nothing)
- Use tags on deadly objects:
  - `DeathZone` - For pits, voids
  - `Spikes` - For spike traps
  - `Lava` - For lava/fire hazards
  - `Pit` - For bottomless pits

#### Option 2: Layer-Based Death Detection

- Create a new layer called "Hazards" (Layer 8)
- Set `Death Layer` to "Hazards" only
- Put all deadly objects on the "Hazards" layer

## Common Issues and Solutions

### 1. Player Dies on Spawn

**Symptoms:** Player dies immediately when game starts
**Causes:**

- Death Layer set to -1 (Everything)
- Ground/Platform objects tagged incorrectly
- Player spawning inside a deadly object

**Solutions:**

- Set Death Layer to 0 or specific hazard layers only
- Ensure ground objects are tagged as "Ground" or "Platform"
- Move player spawn point away from deadly objects

### 2. Player Doesn't Die from Hazards

**Symptoms:** Player walks through spikes/lava without dying
**Causes:**

- Hazard objects not properly tagged or layered
- Death Layer doesn't include the hazard layer
- Colliders on hazards not set as triggers

**Solutions:**

- Tag hazard objects with "DeathZone", "Spikes", "Lava", or "Pit"
- If using layers, add hazard layer to Death Layer mask
- Set hazard colliders as "Is Trigger" = true

### 3. Enemies Kill Player Instantly

**Symptoms:** Player dies in one hit from any enemy
**Causes:**

- Enemy objects tagged as death-causing tags
- Enemy layer included in Death Layer mask

**Solutions:**

- Don't tag enemies with "DeathZone", "Spikes", etc.
- Don't include enemy layer in Death Layer mask
- Enemies should use `PlayerHealth.ChangeHealth(-damage)` instead

### 4. Checkpoint Not Working

**Symptoms:** Player respawns at start position instead of checkpoint
**Causes:**

- Checkpoint not activated
- Checkpoint collider not set as trigger
- Player doesn't have "Player" tag

**Solutions:**

- Set checkpoint collider as "Is Trigger" = true
- Ensure player GameObject has "Player" tag
- Check console for checkpoint activation messages

### 5. Lives System Not Working

**Symptoms:** Player goes to EndGame immediately or has wrong number of lives
**Causes:**

- MaxLives set incorrectly in PlayerHealth
- EndGame scene name wrong
- PlayerHealth script not attached to player

**Solutions:**

- Set `Max Lives` to 2 in PlayerHealth component
- Set `End Game Scene Name` to exact scene name
- Ensure PlayerHealth script is on the Player GameObject

## Debug Information

### Console Messages to Look For

**Good Messages (System Working):**

```
Player spawned at: (x, y, z)
Initial checkpoint set to: (x, y, z)
Death layer configured: X - Player will die when touching objects on these layers or with deadly tags.
Ignoring collision with safe object: Ground (tag: Ground)
Checkpoint activated at: (x, y, z)
Player respawned at checkpoint with full health
```

**Warning Messages (Need Attention):**

```
Death layer is set to 0 (Nothing). Player will only die from tagged objects...
PlayerHealth not found! Using fallback respawn system.
respawn component not found on Player!
```

**Error Messages (Fix Immediately):**

```
Death layer is set to -1 (Everything)! This will cause player to die immediately.
Player GameObject not found!
PlayerHealth not found on Player GameObject!
```

### Manual Testing Steps

1. **Start the game**
2. **Check console for startup messages**
3. **Move player around - should not die from normal movement**
4. **Touch a checkpoint - should see "Checkpoint activated" message**
5. **Take damage from enemy - should lose health, not die instantly**
6. **Take enough damage to die - should respawn at checkpoint**
7. **Die again - should go to EndGame scene**

## Unity Inspector Setup Checklist

### Player GameObject Must Have:

- ✅ PlayerHealth script
- ✅ respawn script
- ✅ PlayerMovement script
- ✅ Rigidbody2D
- ✅ Collider2D
- ✅ Tag: "Player"

### PlayerHealth Settings:

- ✅ Max Health: 10 (or desired amount)
- ✅ Max Lives: 2
- ✅ Respawn Delay: 1
- ✅ End Game Scene Name: "EndGame" (exact scene name)

### respawn Settings:

- ✅ Respawn Delay: 1
- ✅ Death Layer: 0 or specific hazard layers only (NOT -1)

### Checkpoint Objects Must Have:

- ✅ Checkpoint script
- ✅ Collider2D with "Is Trigger" = true
- ✅ Positioned where you want respawn point

### Deadly Objects Must Have:

- ✅ Tag: "DeathZone", "Spikes", "Lava", or "Pit"
- ✅ Collider2D with "Is Trigger" = true
- ✅ OR be on layer included in Death Layer mask

### Enemy Objects Should Have:

- ✅ MeleeEnemy or similar combat script
- ✅ NOT tagged with deadly tags
- ✅ NOT on death layer
- ✅ Use damage system, not instant death

## Final Notes

- **The system is designed to be robust and prevent accidental deaths**
- **Most issues come from incorrect layer/tag configuration**
- **Check the Unity Console for helpful debug messages**
- **Test thoroughly in Unity Editor before building**

If you're still having issues after following this guide, check the console messages and compare your setup with the checklist above.
