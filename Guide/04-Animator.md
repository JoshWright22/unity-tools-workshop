# 04 - The Animator

Scene: `Scenes/04 Animator` (uses the character you sliced in lesson 03)

Animators, this is your part of Unity. Two pieces:

- **Animation clips:** what moves. Frames, positions, colors, anything with a keyframe.
- **Animator Controller:** a state machine that picks which clip plays. Code never plays clips
  directly; it only sets **parameters**, and your state machine decides what they mean.

## 1. Make the clips

Quickest way:

1. In the Project window, open the sliced **Idle** sheet and select every frame (click the first,
   Shift+click the last).
2. Drag them onto **Player** in the Hierarchy. Save the clip as `Idle.anim` in a new `MyAnimation` folder.
3. Unity just made a clip, an **Animator Controller**, and added an **Animator** component to the Player.
   Press Play: it's idling.

Now the Run and Jump clips:

1. Select Player, open **Window > Animation > Animation** (Ctrl+6).
2. Clip dropdown (top left) > **Create New Clip** > `Run.anim`.
3. Select all **Run** frames and drag them onto the timeline.
4. Speed: the **⋮** menu > **Show Sample Rate**, set it to **20** (Pixel Adventure is drawn for 20 fps).
5. Same for `Jump.anim` (1 frame) and `Fall.anim` (1 frame).

You can keyframe more than sprites: hit the red **record** button and change scale, color or position.
A little squash on landing goes a long way.

## 2. The state machine

1. **Window > Animation > Animator**.
2. **Parameters** tab > **+**:
   - **Float** `Speed`
   - **Bool** `Grounded` (tick it so it starts true)
3. Drag `Run`, `Jump` and `Fall` into the grid if they aren't there. Idle is orange = the default state.
4. Right-click **Idle > Make Transition** > click **Run**. Select the arrow:
   - Untick **Has Exit Time**, set **Transition Duration** to 0 (pixel art shouldn't blend).
   - **Conditions > +** > `Speed` **Greater** `0.1`
5. Run back to Idle: `Speed` **Less** `0.1`.
6. **Any State** to Jump: `Grounded` **false**. Jump back to Idle: `Grounded` **true**.
   Same settings: no exit time, 0 duration.

## 3. Feed it parameters

1. Select Player > **Add Component > Player Animator**. It sets `Speed` and `Grounded` every frame
   and flips the sprite to face where you're walking.
2. Play. Run around, jump.

## 4. Animation events

Events call a function on a specific frame: footsteps, hitboxes, particles.

1. Add Component > **Footsteps**.
2. Open the Run clip in the Animation window, click a frame where a foot lands.
3. Click **Add Event** (the little marker button), pick **Footstep()** in the Inspector.
4. Play and watch the Console print `step`. Drag a sound into Footsteps' **Sound** slot for real audio.

## Try

- Split Jump into Jump (going up) and Fall (going down). You'll need a `VelocityY` float from
  your programmer. That's a real request you can make now.
- Blend Trees: for 3D or 8-direction movement, one state that blends clips by a parameter.

Compare with `Scenes/Finished/04 Animator (Finished)` (it uses the placeholder frames).

Next: [05 - Tilemap](05-Tilemap.md)
