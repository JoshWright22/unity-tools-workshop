# 06 - Yarn Spinner

Scene: `Scenes/06 Yarn Spinner`

[Yarn Spinner](https://yarnspinner.dev) lets writers write dialogue like a screenplay: lines, choices
and branches, in plain text files. No code, no giant node graphs.

It's already installed here. In your own project: Package Manager > **+** > **Install package from git URL**
> `https://github.com/YarnSpinnerTool/YarnSpinner-Unity.git#current`.

## Read the script

Open `Dialogue/Shopkeeper.yarn` in any text editor (VS Code has a Yarn Spinner extension with
highlighting and a graph view).

```
title: Start
---
Shopkeeper: What do you want to know?
-> What is Yarn Spinner?
    <<jump WhatIsYarn>>
-> Can I have some coins?
    <<jump Coins>>
===
```

- `title:` names a **node**. `---` starts it, `===` ends it.
- `Name: text` is a line of dialogue.
- `->` is a choice. Indented lines under it run if the player picks it.
- `<<jump Node>>` goes to another node.
- `<<set $variable to true>>` and `<<if $variable>>` remember things between conversations.
- `<<give_coins 3>>` is a **command**: it runs code in the game (see `Scripts/WorkshopCommands.cs`).

## Hook it up

1. **GameObject > Yarn Spinner > Dialogue System**. That adds a Dialogue Runner plus the UI.
2. Select the new **Dialogue System**. Drag `Dialogue/Workshop` (the Yarn Project) into **Yarn Project**.
3. Make sure **Auto Start** is off (we'll start it by talking to the NPC).
4. Select **Shopkeeper** > **Add Component > Talker**. Drag the Dialogue System into **Dialogue Runner**.
5. Play. Walk up to the shopkeeper and press **E**. Click to advance, click a choice.

## Write your own

1. Change what the shopkeeper says in `Shopkeeper.yarn`, save, press Play again.
2. Add a choice that jumps to a new node you write.
3. Make a second `.yarn` file in the Dialogue folder. The Yarn Project picks up every `.yarn` file
   in the folder automatically. Give it a node called `Guard`, add another NPC with a Talker,
   and set its **Start Node** to `Guard`.

Heads up: `#` starts a tag in Yarn (used for localization and metadata), so don't use it in lines.

Compare with `Scenes/Finished/06 Yarn Spinner (Finished)`.

Next: [07 - ScriptableObjects](07-ScriptableObjects.md)
