# 03 - The Asset Store

Scene: `Scenes/03 Asset Store`

The Unity Asset Store has thousands of free models, sprites, sounds and tools. Great for jams,
prototypes, and placeholder art while your artists work on the real thing.

We'll grab **Pixel Adventure 1** by Pixel Frog: free, with four animated characters, terrain tiles,
fruit and traps.

## Get it

1. Go to https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360
2. Sign in with your Unity account (same one as Unity Hub) and click **Add to My Assets**.
3. Back in Unity: **Window > Package Management > Package Manager**.
4. In the left sidebar pick **My Assets**. Find **Pixel Adventure 1**.
5. **Download**, then **Import**. Leave everything checked, click **Import**.
6. It shows up in the Project window as `Assets/Pixel Adventure 1`.

## Slice a sprite sheet

Animations usually come as one image with every frame in a row: a **sprite sheet**. Unity needs to
know where each frame is.

1. Open `Pixel Adventure 1/Assets/Main Characters/` and pick a character (Ninja Frog, Mask Dude,
   Pink Man or Virtual Guy).
2. Click `Idle (32x32)`. In the Inspector set:
   - **Sprite Mode:** Multiple
   - **Pixels Per Unit:** 32
   - **Filter Mode:** Point (no filter), so pixel art stays crisp
   - **Compression:** None
   - **Apply**
3. Click **Open Sprite Editor**. **Slice** (top left) > **Type: Grid By Cell Size** > **32 x 32** > **Slice**.
   **Apply** (top right), close.
4. Click the little arrow on the sheet in the Project window. Every frame is its own sprite now.
5. Do the same for `Run (32x32)`, `Jump (32x32)` and `Fall (32x32)`.

Tip: select all four sheets at once and set the import settings together. Slicing is one at a time.

## Use it

1. Select **Player** in the scene.
2. Drag the first Idle frame into the Sprite Renderer's **Sprite** slot.
3. Play. You're the frog now (it doesn't animate yet, that's next lesson).

## Licenses matter

- Asset Store assets use the **Asset Store EULA**: use them in your games, but you can't re-upload
  the files themselves. That's why they aren't in this repo.
- For a team repo, ask your lead whether to commit imported packs or have everyone import them.
- Always check the license before shipping. Credit the artist even when it isn't required.

Next: [04 - Animator](04-Animator.md)
