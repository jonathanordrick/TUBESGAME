# Respawn System Setup Guide

## Problem Fixed

The error "Scene 'GameOverScene' couldn't be loaded because it has not been added to the build settings" has been resolved.

## What Was Done

### 1. Updated Build Settings

- Added the `EndGame.unity` scene to the Build Settings in `ProjectSettings/EditorBuildSettings.asset`
- The scene is now properly registered and can be loaded

### 2. Improved Respawn Script

- **Forced scene name**: The `respawn.cs` script now forces `gameOverSceneName = "EndGame"` in both `Start()` and `OnValidate()` methods
- **Enhanced logging**: Added detailed debug logs to track exactly what's happening during scene transitions
- **Bulletproof scene name**: No matter what value is set in the Inspector, the script will always use "EndGame"

### 3. Created Helper Scripts

- **BuildSettingsHelper.cs**: Automatically adds missing scenes to Build Settings
- **FixBuildSettings.cs**: Unity Editor window (Tools > Fix Build Settings) for easy Build Settings management
- **RespawnSystemTester.cs**: Testing tools with keyboard shortcuts for debugging

## How to Use

### Manual Setup (if needed)

1. Open Unity
2. Go to File > Build Settings
3. Click "Add Open Scenes" or drag the EndGame scene from Assets/Scenes/EndGame.unity
4. Make sure EndGame.unity is in the list and enabled

### Automatic Setup

1. Add the `BuildSettingsHelper` script to any GameObject in your scene
2. Check the "Add Scenes To Build Settings" checkbox in the Inspector
3. Or use the Tools > Fix Build Settings menu

### Testing the System

1. Add the `RespawnSystemTester` script to any GameObject in your scene
2. Use these keyboard shortcuts during play:
   - **K**: Force kill the player (for testing)
   - **R**: Reset respawn count
   - **I**: Show current status in console

## Current System Behavior

### Respawn Limits

- Player can respawn up to **2 times**
- After 2 respawns, the next death triggers Game Over
- Game Over loads the **"EndGame"** scene

### Death Triggers

- Player health reaches 0 (from enemy attacks)
- Player falls below Y position -50 (falls off map)
- NOT triggered by ground collisions or undefined tags

### Scene Transitions

- All scene loading uses the central `SceneManagement.cs` script
- Fallback to Unity's SceneManager if SceneManagement component is missing

## Files Modified

### Core Scripts

- `/Assets/Scripts/Player/respawn.cs` - Main respawn logic with forced scene name
- `/Assets/Scripts/Player/PlayerHealth.cs` - Calls respawn only when health = 0
- `/Assets/Scripts/menuscreen/SceneManagement.cs` - Central scene management

### Helper Scripts (New)

- `/Assets/Scripts/BuildSettingsHelper.cs` - Auto-fix Build Settings
- `/Assets/Scripts/Editor/FixBuildSettings.cs` - Editor window for Build Settings
- `/Assets/Scripts/RespawnSystemTester.cs` - Testing and debugging tools

### Project Settings

- `/ProjectSettings/EditorBuildSettings.asset` - Added EndGame scene to Build Settings

## Troubleshooting

### If the error still occurs:

1. Check that EndGame.unity exists in `/Assets/Scenes/`
2. Open Build Settings (File > Build Settings) and verify EndGame is listed
3. Use the FixBuildSettings editor window (Tools > Fix Build Settings)
4. Check the Console for detailed debug logs from the respawn system

### If respawn doesn't work:

1. Make sure the player has both `respawn` and `PlayerHealth` components
2. Check that enemies are properly calling `PlayerHealth.ChangeHealth()`
3. Use the RespawnSystemTester to force test deaths

### If Game Over scene is wrong:

1. The respawn script automatically forces the scene name to "EndGame"
2. Check that EndGame.unity has the proper GameOverManager setup
3. Verify that SceneManagement component exists in the scene

## Debug Information

The system now provides extensive debug logging:

- Respawn attempts and remaining count
- Scene loading status and method used
- Component availability and health status
- Build Settings verification

All debug output appears in the Unity Console during play.
