# 08 - Cinemachine + Timeline

Scene: `Scenes/08 Cinemachine Timeline`

## Cinemachine: cameras without code

Cinemachine makes smart cameras. The real Camera gets a **Cinemachine Brain**; you place
**Cinemachine Cameras** that each describe a shot, and the Brain blends between them.

1. **GameObject > Cinemachine > 2D Camera**. It adds a Brain to Main Camera for you.
2. Select the new camera. Drag **Player** into **Tracking Target**.
3. Play. The camera follows.
4. Open the **Position Composer** component and play with it while the game runs:
   - **Damping:** how lazily it catches up. Higher = smoother.
   - **Dead Zone:** the player can move this much before the camera does.
   - **Lookahead:** the camera leans toward where you're heading.
5. Screen shake: add a **Cinemachine Impulse Listener** extension to the camera
   (**Add Extension** dropdown), then put a **Cinemachine Impulse Source** on anything that should
   shake the screen and call **Generate Impulse** from a UnityEvent (lesson 02).
6. Confine it: make an empty object with a **Polygon Collider 2D** around the level, add the
   **Cinemachine Confiner 2D** extension and drag the collider in.

## Timeline: cutscenes

Timeline is a video editor inside Unity: tracks for animation, audio, turning objects on/off, and
camera cuts.

1. Make an empty GameObject called **Intro**.
2. **Window > Sequencing > Timeline**, then click **Create** and save `Intro.playable`.
3. Drag **Player** into the Timeline window > **Add Activation Track**. Make its bar start at
   1 second: the player "spawns" after a beat.
4. Make a second Cinemachine Camera zoomed out on the whole level (**Lens > Orthographic Size** 10).
   In Timeline: **+ > Cinemachine Track**, bind it to Main Camera's Brain, right-click the track >
   **Add Cinemachine Shot**, one for the wide camera then one for the follow camera. Overlap them
   to blend.
5. Play. On the **Playable Director** component, **Play On Awake** starts it automatically.

Compare with `Scenes/Finished/08 Cinemachine Timeline (Finished)` (follow camera already set up).

Next: [09 - Shaders](09-Shaders.md)
