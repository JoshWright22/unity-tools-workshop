# Coin Gate: build a game in 90 minutes

Scene: `Assets/Workshop/Scenes/00 Coin Gate`

Collect 5 coins, dodge the slimes, pay the gatekeeper, and touch the flag. By the end you'll have
used Git, Tilemaps, Cinemachine, the Animator, UnityEvents, prefabs, ScriptableObjects, Yarn Spinner
and shaders, and you won't have written a line of code.

Stuck? Open `Scenes/Finished/00 Coin Gate (Finished)` and compare.

| Time | Step | Tool |
|------|------|------|
| 10 min | [0. Setup](#0-setup-10-min) | Git, Unity Hub |
| 15 min | [1. Paint the level](#1-paint-the-level-15-min) | Tilemap |
| 5 min | [2. Follow camera](#2-follow-camera-5-min) | Cinemachine |
| 20 min | [3. Animate the player](#3-animate-the-player-20-min) | Animator |
| 10 min | [4. Coins and the flag](#4-coins-and-the-flag-10-min) | Prefabs, UnityEvents |
| 10 min | [5. Slimes](#5-slimes-10-min) | ScriptableObjects |
| 15 min | [6. The gatekeeper](#6-the-gatekeeper-15-min) | Yarn Spinner |
| 5 min | [7. Hit flash](#7-hit-flash-5-min) | Shaders |
| 10 min | [8. Ship it](#8-ship-it-10-min) | Git |

Controls: A/D or arrows to move, Space to jump, E to talk, H hides the step list in the Game view.

---

## 0. Setup (10 min)

1. Install **Unity Hub**, then **Unity 6.3 LTS** (6000.3.23f1) from Hub > Installs.
2. Install **GitHub Desktop** and sign in.
3. GitHub Desktop > **File > Clone repository > URL** >
   `https://github.com/JoshWright22/unity-tools-workshop`
4. Unity Hub > **Projects > Add > Add project from disk** > the cloned folder. Open it
   (the first import takes a few minutes).
5. GitHub Desktop > **Current Branch > New Branch** > your name. Your work stays on your branch.
6. In Unity's Project window open `Assets/Workshop/Scenes/00 Coin Gate`.

Quick tour: **Hierarchy** (what's in the scene), **Scene view** (move things: W move, E rotate,
R scale), **Inspector** (the components on what you selected), **Project** (all the files), **Play**.

Everything in a scene is a **GameObject**. Components on it (Sprite Renderer, Collider, scripts) give it
behavior. Click the Player and look at its components.

## 1. Paint the level (15 min)

The floor is there but you'll fall right through it, and there's nothing to jump on.

1. **Window > 2D > Tile Palette**. **Create New Palette** > name it `Coin Gate` > save it in a new
   `Palettes` folder.
2. Drag the `Workshop/Tiles` folder into the palette window.
3. Make sure **Active Tilemap** is **Level**. Pick the red brick tile and press **B** (brush).
4. Paint platforms between the start and the gatekeeper. You can jump about **2 tiles** high, so
   keep each platform at most 2 tiles above the last. Shift+click erases.
5. Make it solid: select **Grid > Level**, then:
   - **Add Component > Tilemap Collider 2D**, set **Composite Operation** to **Merge**.
   - **Add Component > Composite Collider 2D**. It adds a **Rigidbody 2D**: set **Body Type** to **Static**.
6. **Play.** Run and jump around. Tweak **Speed** and **Jump Force** on the Player while playing.
   (Play mode changes reset when you stop, so write down the numbers you like.)

## 2. Follow camera (5 min)

1. **GameObject > Cinemachine > 2D Camera**.
2. Select it and drag **Player** into **Tracking Target**.
3. Play. Open **Position Composer** and try **Damping** (how lazy the camera is) and
   **Dead Zone** (how far you can move before it follows).

## 3. Animate the player (20 min)

1. Select **Player**. **Window > Animation > Animation** (dock it next to the Game tab).
2. Click **Create**, save `Idle.anim` in a new `MyAnimation` folder.
3. Drag `Art/player_idle` onto the timeline. Hit the red **record** button, move to 0:30, set the
   Player's **Scale Y** to `0.94`, move to 1:00, set it back to `1`. That's a breathing idle.
   Stop recording.
4. Clip dropdown > **Create New Clip** > `Walk.anim`. Drag `player_walk1` and `player_walk2` onto the
   timeline. Drag `player_walk1` in again at the end so it loops cleanly. Set the speed with
   **⋮ > Show Sample Rate** (try 8).
5. One more: `Jump.anim` with just `player_walk2`.
6. **Window > Animation > Animator.** Unity made a controller with your clips.
   - **Parameters > +**: Float `Speed`, Bool `Grounded` (tick it).
   - Right-click **Idle > Make Transition** > **Walk**. Click the arrow: untick **Has Exit Time**,
     **Transition Duration** 0, **Conditions** `Speed` Greater `0.1`.
   - **Walk > Idle**: `Speed` Less `0.1`.
   - **Any State > Jump**: `Grounded` false. **Jump > Idle**: `Grounded` true. Same settings.
7. Player > **Add Component > Player Animator** (feeds it Speed and Grounded and flips the sprite).
8. Play.

Bonus: **Add Component > Footsteps**, then in the Walk clip click a frame and **Add Event** > `Footstep`.
The Console prints `step` every time it fires.

## 4. Coins and the flag (10 min)

1. Click `Prefabs/Coin` and look at its **Touched** component. **On Touched** has two entries:
   `Collectible.Collect` (adds to the score) and `GameObject.SetActive` unchecked (hides the coin).
   That's a **UnityEvent**: "when this happens, do these things," set up in the Inspector.
2. Drag the Coin prefab into the scene about 7 times, on your platforms and along the floor.
   Change the prefab once and every coin changes.
3. Now wire one yourself. Select **Flag** > its **On Touched** > **+**. Drag **Game** into the slot,
   pick **GameState > Win ()**.
4. Play. Grab coins (counter top right).

## 5. Slimes (10 min)

Slimes read their stats from **ScriptableObjects**: data files designers edit without code.

1. Drag `Prefabs/Enemy` onto the floor, then drag `Data/Enemy_Slime` into its **Data** slot.
2. Play. Touch it: you get sent back to the start.
3. Right-click the `Data` folder > **Create > Workshop > Enemy Data**. Make your own
   (fast and tiny? slow and huge?) and put it on a second enemy.
4. Change a data file while playing. The change **sticks** after you stop, unlike component edits.

## 6. The gatekeeper (15 min)

The gate costs 5 coins. The gatekeeper's lines are in `Dialogue/Gatekeeper.yarn`, written in
[Yarn Spinner](https://yarnspinner.dev).

1. **GameObject > Yarn Spinner > Dialogue System**.
2. Drag `Dialogue/Workshop` (the Yarn Project) into its **Yarn Project** slot.
3. Select **Gatekeeper** > **Add Component > Talker**. Drag the Dialogue System into
   **Dialogue Runner**, and set **Start Node** to `Gatekeeper`.
4. Play. Talk to him with **E** before you have 5 coins, then after.
5. Open `Gatekeeper.yarn` in any text editor:
   - `Name: text` is a line. `->` is a choice. `<<if coins() >= 5>>` checks the score.
   - `<<take_coins 5>>` and `<<open_gate>>` are commands that run game code.
   - Rewrite his lines, add a choice, save, and play again. (Don't use `#` in lines, it's a tag.)
6. Pay him, watch the gate dissolve, touch the flag. **You win.**

## 7. Hit flash (5 min)

The gate dissolves because its material uses a **shader**. Let's make getting hit flash white.

1. Select **Player** > Sprite Renderer > **Material** > pick `Sprite Flash`.
2. Play and touch a slime.
3. Select `Materials/Gate Dissolve` and change its **Edge Color**. Pay the gatekeeper again to see it.
4. Curious how it works? `Guide/09-Shaders.md` walks through building the flash in **Shader Graph**.

## 8. Ship it (10 min)

1. **File > Save** (Ctrl+S).
2. GitHub Desktop: every file you changed is listed. Write a summary ("Coin Gate: my level"),
   **Commit to your-branch**, then **Publish branch**.
3. Swap seats with someone and play their level. Beat their time.

Rules that save jam teams: **Pull before you start.** **Commit small and often.**
**Never edit the same scene as someone else at the same time.** Build in your own scene, share prefabs.

---

## Done early?

- **Asset Store:** get the free Pixel Adventure 1 pack and swap in a real character (`Guide/03-AssetStore.md`).
- **Timeline:** an intro cutscene that pans over the level (`Guide/08-Cinemachine-Timeline.md`).
- **Visual Scripting:** make the flag spin with nodes (`Guide/10-VisualScripting.md`).
- **Screen shake:** Cinemachine Impulse when you get hit (`Guide/08-Cinemachine-Timeline.md`).
- Each tool has its own deep-dive lesson in `Guide/`.
