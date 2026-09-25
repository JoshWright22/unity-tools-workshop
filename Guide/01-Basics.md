# 01 - GameObjects, Scripts, Prefabs

Scene: `Scenes/01 Basics`

## The one idea

Everything in a scene is a **GameObject**. A GameObject is an empty container. **Components** stacked on
it give it behavior:

- **Transform:** where it is, how it's rotated, how big it is. Every GameObject has one.
- **Sprite Renderer:** what it looks like.
- **Collider 2D:** what it bumps into.
- **Rigidbody 2D:** makes it fall and get pushed around.
- **Scripts:** what it does. A script is just another component.

## The editor, quickly

- **Hierarchy** (left): everything in the scene.
- **Scene view** (middle): move things around. W = move, E = rotate, R = scale.
- **Inspector** (right): the components on whatever you selected, with all their knobs.
- **Project** (bottom): every file in the project.
- **Play button** (top): runs the game.

## Make something spin

1. Hierarchy > **+** > **2D Object > Sprites > Square**.
2. Change its **Color** in the Sprite Renderer.
3. **Add Component** > type `Spinner` > add it.
4. Press **Play**. Change **Speed** in the Inspector while it's running. Try a negative number.
5. Stop. Notice Speed went back. **Changes made in Play mode are thrown away.**
   To keep them: while playing, right-click the component header > **Copy Component**, stop,
   right-click again > **Paste Component Values**.

## Read the script

Double-click `Scripts/Spinner.cs`:

```csharp
public class Spinner : MonoBehaviour
{
    public float speed = 90f;  // a knob

    void Update()  // every frame
    {
        float step = speed * Time.deltaTime;
        transform.Rotate(0, 0, step);
    }
}
```

- `public` = shows up in the Inspector as a knob.
- `Update()` runs every frame. `Start()` (not used here) runs once at the beginning.
- `Time.deltaTime` keeps it the same speed on a fast or slow computer.

You don't need to write scripts to use Unity. You do need to know which knobs to ask your programmer
for: "can you make that a public field?"

## Prefabs

1. Drag your square from the Hierarchy into the `Prefabs` folder. It turns **blue**: it's a prefab now.
2. Drag the prefab into the scene four more times.
3. Double-click the prefab in the Project window to open it, change its color, go back.
   **Every copy changed.**
4. Change one copy's color in the scene. That's an **override**, just for that copy.
   The Inspector's **Overrides** dropdown can apply it to the prefab or revert it.

Compare with `Scenes/Finished/01 Basics (Finished)`.

Next: [02 - UnityEvents](02-UnityEvents.md)
