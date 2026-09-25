# Unity Tools Workshop

VGDC Systems Workshop: **Unity for non-programmers.** Artists, animators, writers and designers
welcome.

## The workshop: Coin Gate (90 minutes)

Build a small platformer from a starter scene: paint the level, animate the player, place coins and
slimes, write the gatekeeper's dialogue, and ship it with Git. No code.

**Start here: [Guide/Coin-Gate.md](Guide/Coin-Gate.md)**

| Step | Tool |
|------|------|
| Setup | Git, GitHub Desktop, Unity Hub |
| Paint the level | Tilemap, colliders |
| Follow camera | Cinemachine |
| Animate the player | Animation window, Animator |
| Coins and the flag | Prefabs, UnityEvents |
| Slimes | ScriptableObjects |
| The gatekeeper | Yarn Spinner |
| Hit flash | Shaders, materials |
| Ship it | Commit, push, play each other's |

Scene: `Assets/Workshop/Scenes/00 Coin Gate`. The finished version is in `Scenes/Finished/`.

## Deep dives (after the workshop)

One lesson per tool, each with a starter scene and a finished one.

| # | Lesson | You'll learn |
|---|--------|--------------|
| 00 | [Git + setup](Guide/00-Git-Setup.md) | GitHub Desktop, cloning, commit/push/pull, team rules |
| 01 | [Basics](Guide/01-Basics.md) | GameObjects, components, the Inspector, reading a script, prefabs |
| 02 | [UnityEvents](Guide/02-UnityEvents.md) | Hooking up a pickup with no code |
| 03 | [Asset Store](Guide/03-AssetStore.md) | Getting free assets, importing, slicing sprite sheets |
| 04 | [Animator](Guide/04-Animator.md) | Clips, the state machine, parameters, animation events |
| 05 | [Tilemap](Guide/05-Tilemap.md) | Tile palettes, painting levels, tilemap colliders |
| 06 | [Yarn Spinner](Guide/06-YarnSpinner.md) | Branching dialogue, variables, commands |
| 07 | [ScriptableObjects](Guide/07-ScriptableObjects.md) | Game data as files designers can edit |
| 08 | [Cinemachine + Timeline](Guide/08-Cinemachine-Timeline.md) | Follow cameras, screen shake, cutscenes |
| 09 | [Shaders](Guide/09-Shaders.md) | Materials, shader properties, Shader Graph |
| 10 | [Visual Scripting](Guide/10-VisualScripting.md) | Logic with nodes instead of code |

## Setup

- **Unity 6000.3.23f1** (Unity 6.3 LTS) through Unity Hub. Other 6.3 versions should open it fine.
- **GitHub Desktop** (or any Git client).
- Clone, open in Unity Hub, wait for the first import.

Controls: A/D or arrows to move, Space to jump, E to talk, H hides the step list.

## What's in here

```
Assets/Workshop/
  Art/          placeholder sprites and tiles
  Scripts/      small scripts, one job each
  Shaders/      sprite shaders: flash, wave, dissolve
  Dialogue/     .yarn files + the Yarn Project
  Scenes/       00 Coin Gate + the deep-dive lessons, Finished/ versions
  Prefabs/ Tiles/ Data/ Animation/ Materials/
```

Packages: URP 2D, Input System, Tilemap, Cinemachine 3, Timeline, Shader Graph, Visual Scripting,
TextMesh Pro, [Yarn Spinner](https://yarnspinner.dev).

**For whoever runs the workshop:** `Workshop > Rebuild Lessons` regenerates every scene, prefab and
asset from `Assets/Workshop/Editor/WorkshopBuilder.cs`. It overwrites the scenes, so don't run it on a
copy where people have started.

Asset Store packs (like Pixel Adventure 1 in lesson 03) aren't included, because their license doesn't
allow re-sharing them in a public repo. Everyone downloads their own copy.
