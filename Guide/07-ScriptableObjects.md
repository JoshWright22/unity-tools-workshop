# 07 - ScriptableObjects: data you can edit

Scene: `Scenes/07 ScriptableObjects`

A **ScriptableObject** is a file that holds data: an enemy's stats, an item, a recipe, a level's
settings. The programmer writes the *shape* of the data once. After that, anyone can make as many
as they want and balance the game without touching code.

## Look at one

1. Click `Data/Enemy_Slime`. It's a form: name, health, speed, damage, color, size.
2. Play the scene. The slime reads everything from that file (`Scripts/Enemy.cs`).
3. While playing, change the slime's **Speed** and **Tint** in the data file. It updates live.
4. Stop. **The change stayed.** ScriptableObject edits are saved, unlike component edits in Play mode.
   Handy for balancing, and easy to do by accident. Git has your back.

## Make your own enemy

1. Right-click the `Data` folder > **Create > Workshop > Enemy Data**. Name it `Enemy_Bat`.
2. Fill it in.
3. Drag `Prefabs/Enemy` into the scene, then drag `Enemy_Bat` into its **Data** slot.
4. Play.

## Why bother

- One enemy prefab, many enemy types. Fix a bug once and every enemy gets it.
- Designers own the numbers. Programmers own the behavior.
- Works for anything with stats: weapons, cards, upgrades, dialogue speakers, levels.

Compare with `Scenes/Finished/07 ScriptableObjects (Finished)`.

Next: [08 - Cinemachine + Timeline](08-Cinemachine-Timeline.md)
