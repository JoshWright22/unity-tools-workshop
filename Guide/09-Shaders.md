# 09 - Shaders

Scene: `Scenes/09 Shaders`

- A **shader** is a small program that decides the color of every pixel.
- A **material** is a shader plus its settings. Objects use materials, not shaders directly.
- One shader can have many materials: a red flash, a blue flash, a slow dissolve, a fast one.

## Use the ones here

`Shaders/` has three sprite shaders, and `Materials/` has a material for each.

1. Drag `Materials/Sprite Flash` onto the **Flash** sprite in the Scene view.
2. Select the material and drag **Flash Amount** from 0 to 1. That's a hit flash.
3. Do the same with **Sprite Wave** and **Sprite Dissolve** on the other two. Play with every slider.
4. Right-click a material > **Duplicate** to make a variation without breaking the original.

Open `Shaders/SpriteDissolve.shader` if you're curious. The whole effect is about five lines:
make noise, throw away pixels below the **Amount**, color the ones right at the edge.

## Build a flash in Shader Graph

Shader Graph is the same thing with nodes instead of code.

1. Project window: **Create > Shader Graph > URP > Sprite Unlit Shader Graph**. Name it `MyFlash`, open it.
2. In the **Blackboard** (left), click **+**:
   - **Texture2D** named `MainTex`. In the Node Settings set its **Reference** to `_MainTex`
     (this is how the Sprite Renderer's sprite gets in).
   - **Color** named `Flash Color` (white).
   - **Float** named `Flash Amount`, Mode **Slider** 0 to 1.
3. Drag all three into the graph. Add nodes (right-click > **Create Node** or press Space):
   - **Sample Texture 2D**: plug `MainTex` into its Texture input.
   - **Lerp**: A = Sample Texture's **RGBA**, B = `Flash Color`, T = `Flash Amount`.
4. Lerp output > **Base Color** on the Fragment block. Sample Texture's **A** > **Alpha**.
5. **Save Asset** (top left). Right-click the graph in the Project window > **Create > Material**.
6. Put the material on a sprite and drag Flash Amount.

## Try

- **Dissolve:** a **Simple Noise** node into a **Step** node with `Amount`, multiplied into Alpha.
- **Outline, glow, scrolling water, wind sway:** almost every sprite effect you've seen is a
  handful of these nodes.
- For a hit flash in game, ask your programmer to set `_FlashAmount` for a tenth of a second.
  Or animate the material property in an animation clip (lesson 04).

Compare with `Scenes/Finished/09 Shaders (Finished)`.

Next: [10 - Visual Scripting](10-VisualScripting.md)
