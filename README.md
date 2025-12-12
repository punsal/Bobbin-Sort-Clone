# Bobbin Sort Clone

A deterministic stacking puzzle game developed as a **senior game developer case**.  
This project demonstrates clean architecture, data-driven level design, and decoupled UI using ScriptableEvents and observables.

---

## 🎮 Overview

Bobbin Sort is a **container sorting puzzle** where players move items between containers under movement constraints until all containers contain uniform items.

---

## 📺 Scenes

### 🕹 GameScene  
**Path:** `Assets/Scenes/GameScene`  
Main gameplay scene. Plays levels defined by `LevelDefinition` assets.  
Gameplay flow, UI orchestration, and persistence all happen here.

### 🧪 TestScene  
**Path:** `Assets/Scenes/TestScene`  
A dedicated test scene for verifying the **fail condition** (`HasMoves()`).

#### How to trigger level fail:
1. Run **TestScene**.
2. Execute the **single available valid move**.
3. After the move:
   - The level is **not won**.
   - No valid moves remain (`HasMoves()` returns `false`).
4. The level immediately fails — this confirms the failure logic works correctly.

---

## 📁 Project Structure

```

Assets/
├── Scenes/
│   ├── GameScene
│   └── TestScene
│
├── Gameplay/
│   ├── Level/              # Level loading and definitions
│   ├── Container/          # Container logic (SortContainer)
│   └── Item/               # Item & data (SortItem, SortItemData)
│
├── UI/
│   ├── UIManager.cs        # UI orchestrator
│   ├── UIView/             # View prefabs
│   └── HUD/                # Always-on status HUD
│
├── Events/                 # ScriptableEvent system and assets
├── Save/                   # SaveManager & SaveData
└── Common/                 # Observables, utility systems

```

---

## 🧠 Software Design

### ➤ Level System
- Prefab-based layouts (`3ContainerLevel`, `4ContainerLevel`, etc.) define container positions and camera settings.
- `LevelDefinition` assets hold **initial container content** data.
- `LevelLoader` instantiates the correct prefab and applies level data.
- `LevelDatabase` maintains all levels for looping.

---

### ➤ Gameplay Logic
- `LevelController` handles input + move execution + win/lose evaluation.
- `SortContainer` encapsulates container stack logic.
- `HasMoves()` checks for valid remaining moves (deterministic).

---

### ➤ UI Architecture
- Event-driven UI using `AScriptableEvent`.
- UIViews raise events (`NextLevel`, `Restart`) without referencing GameManager.
- `UIManager` listens for state events (`ShowWin`, `ShowLose`) and toggles views.
- HUD is always visible and driven by an observable level index.

Game systems do **not** directly reference UI systems.

---

### ➤ Save System
- Generic, JSON-based save using `PlayerPrefs`.
- Abstract `SaveData` with a unique key per save type.
- Current level index is persisted and restored on app launch.
- Easily extended with new save fields without changing the save manager.

---

## 📌 Notes for Reviewers

- Clear separation of responsibilities:
  - Gameplay, UI, events, and persistence are decoupled.
- `TestScene` explicitly verifies edge-case fail logic.
- All interactions are deterministic — no physics or randomness.
- Designer-friendly level definitions using ScriptableObjects.

---

## 🚀 Running the Project

1. Open `GameScene` to play or review core flow.
2. Open `TestScene` to validate the failure condition.
3. Level progress will resume from the last saved level automatically.

---

Thanks for reviewing this project!