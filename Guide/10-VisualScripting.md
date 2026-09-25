# 10 - Visual Scripting

Scene: `Scenes/10 Visual Scripting`

Unity Visual Scripting is logic as boxes and wires. It's the same Unity underneath: same components,
same events, same everything, just no typing.

## Make the square spin (again, without code)

1. Select **Square**. **Add Component > Script Machine**.
2. **Source: Graph**, click **New**, save it as `SpinGraph`. Click **Edit Graph**.
3. You'll see **On Start** and **On Update** events already there.
4. Right-click empty space, search **Rotate**, pick **Transform: Rotate (X Angle, Y Angle, Z Angle)**.
5. Connect **On Update**'s arrow to Rotate's arrow (the arrows are the order things happen in).
6. Type `1` in **Z Angle**. Play.

## Add a knob

1. In the graph's **Blackboard** (left) > **Object** tab, add a variable: `Speed`, type **Float**, value `90`.
2. Drag it into the graph (a **Get Variable** node).
3. Add **Get Delta Time** (search "delta time") and a **Multiply** node: Speed x Delta Time.
4. Plug the result into **Z Angle**.
5. Play and change Speed in the Script Machine's **Variables** component. That's the same idea as
   `Spinner.cs` from lesson 01.

## When to use it

- **Good for:** doors, triggers, simple puzzles, UI flow, trying an idea fast.
- **Not good for:** big systems. Graphs get messy quickly and are hard to merge in Git.
- A good split on a team: programmers make small custom nodes and components,
  designers wire them together.

If a node you expect is missing: **Edit > Project Settings > Visual Scripting > Regenerate Nodes**.

That's the workshop. Go make something for the jam.
