# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity 6 (6000.3.8f1) 2D tile platformer built with URP. The primary scene is `Assets/Scenes/SampleScene.unity`. There are no automated tests; all testing is done by running the scene in the Unity Editor.

## Build & Run

- Open the project in Unity 6.0.3+
- Open `Assets/Scenes/SampleScene.unity`
- Press **Play** in the Unity Editor to run
- Build: **File → Build Settings → Build**

Controls: Arrow keys / WASD to move, Space to jump (double-jump supported), Left Mouse / Z to shoot, Escape to pause.

## Architecture

### Script Organization (`Assets/Scripts/`)

| Folder | Scripts | Responsibility |
|---|---|---|
| `Player/` | `PlayerController`, `PlayerHealth`, `PlayerShoot`, `PlayerCollector` | Movement, health/invincibility, projectile firing, item pickup |
| `Enemy/` | `EnemyController`, `EnemyHealth` | Patrol AI with edge/wall raycasts, health & damage |
| `Projectile/` | `Projectile` | Straight-line movement, collision with enemies/terrain |
| `Collectables/` | `Collectable` (base), `Coin` | Pickup animation/sound, coin → GameManager |
| `GameState/` | `GameManager`, `PauseMenu`, `WinScreen`, `LevelTimer` | Singleton state, pause/resume, win screen, HUD timer |
| `Level/` | `WinZone`, `HazardZone`, `CinemachineBoundaryPause`, `BoundaryAudioZone` | Win trigger, damage zones, camera transition freeze, zone music |
| `Platform/` | `MovingPlatform` | Oscillating platforms; players become children while riding |

### Key Design Conventions

- **SerializeField** for externally-assigned references (prefabs, transforms, audio clips, UI elements). Use `GetComponent` + `[RequireComponent]` for same-GameObject dependencies.
- **GameManager** is a singleton (`GameManager.Instance`). It does not persist across scenes (no `DontDestroyOnLoad`) — each scene reload reinitializes it.
- All scripts include a header comment block with `SETUP` and `USAGE` instructions. Keep this convention when adding new scripts.
- Ground detection uses `Physics2D.OverlapCircle` with a `groundLayer` mask. Enemy edge detection uses `Physics2D.Raycast` downward ahead of the enemy.
- Moving platforms parent the player on `OnCollisionEnter2D` and unparent on `OnCollisionExit2D` — the player must be tagged `"Player"`.

### Game Flow

1. Player spawns → collects coins (`Coin` → `GameManager.AddCoin()`) → defeats enemies (`Projectile` → `EnemyHealth`) → reaches `WinZone`
2. `WinZone` → `GameManager.TriggerWin()` → `WinScreen.Show(time, coins)`
3. Death → `PlayerHealth` → `GameManager.RespawnPlayer()` → scene reload

### Camera & Audio Zones

Cinemachine handles camera following and boundary transitions. `CinemachineBoundaryPause` freezes the game briefly (realtime) during transitions. `BoundaryAudioZone` crossfades background music when the player enters a zone; attach it to Cinemachine confiner boundary objects alongside an `AudioSource`.
