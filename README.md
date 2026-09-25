# Unity Tools Workshop

VGDC Systems Workshop: **Unity for non-programmers.** Artists, animators, writers and designers
welcome. You'll go from cloning a repo to hooking up art, animation, dialogue, levels and shaders
yourself, without waiting on a programmer.

Every lesson has a **starter scene** you set up yourself and a **finished scene** to compare against.
Steps also show up in the corner of the Game view (press **H** to hide them).

## Lessons

| # | Lesson | You'll learn |
|---|--------|--------------|
| 00 | [Git + setup](Guide/00-Git-Setup.md) | GitHub Desktop, cloning, opening the project, commit/push/pull |
| 01 | [Basics](Guide/01-Basics.md) | GameObjects, components, the Inspector, reading a script, prefabs |
| 02 | [UnityEvents](Guide/02-UnityEvents.md) | Hooking up a pickup with no code |
| 03 | [Asset Store](Guide/03-AssetStore.md) | Getting free assets, importing, slicing sprite sheets |
| 04 | [Animator](Guide/04-Animator.md) | Animation clips, the Animator state machine, parameters, animation events |
| 05 | [Tilemap](Guide/05-Tilemap.md) | Tile palettes, painting levels, tilemap colliders |
| 06 | [Yarn Spinner](Guide/06-YarnSpinner.md) | Writing branching dialogue, running it in game |
| 07 | [ScriptableObjects](Guide/07-ScriptableObjects.md) | Game data as files designers can edit |
| 08 | [Cinemachine + Timeline](Guide/08-Cinemachine-Timeline.md) | Follow cameras, screen shake, cutscenes |
| 09 | [Shaders](Guide/09-Shaders.md) | Materials, shader properties, Shader Graph |
| 10 | [Visual Scripting](Guide/10-VisualScripting.md) | Logic with nodes instead of code |

## Setup

- **Unity 6000.3.23f1** (Unity 6.3 LTS) through Unity Hub. Other 6.3 versions should open it fine.
- **GitHub Desktop** (or any Git client).
- Clone, open in Unity Hub, wait for the first import. Full steps in [Lesson 00](Guide/00-Git-Setup.md).

## Controls

A/D or arrows to move, Space to jump, E to talk, H to hide the lesson steps.

## What's in here

```
Assets/Workshop/
  Art/          placeholder sprites and tiles
  Scripts/      small scripts, one job each
  Shaders/      three sprite shaders (flash, wave, dissolve)
  Dialogue/     Shopkeeper.yarn + the Yarn Project
  Scenes/       starter scenes, plus Finished/ versions
  Prefabs/ Tiles/ Data/ Animation/ Materials/
```

Packages: URP 2D, Input System, Tilemap, Cinemachine 3, Timeline, Shader Graph, Visual Scripting,
[Yarn Spinner](https://yarnspinner.dev).

**For whoever runs the workshop:** `Workshop > Rebuild Lessons` regenerates every scene, prefab and
asset from `Assets/Workshop/Editor/WorkshopBuilder.cs`. It overwrites the scenes, so don't run it on a
copy where people have started lessons.

Asset Store packs (like Pixel Adventure 1 in lesson 03) aren't included, because their license doesn't
allow re-sharing them in a public repo. Everyone downloads their own copy, which is the lesson anyway.
