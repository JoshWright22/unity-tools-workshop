# 02 - UnityEvents: hook things up without code

Scene: `Scenes/02 UnityEvents`

A **UnityEvent** is a list in the Inspector: "when this happens, do these things." You already know one:
a UI Button's **On Click** list. The trick is having a programmer write small, reusable scripts that
expose events, and then anyone can wire up what happens.

`Scripts/Touched.cs` is one of those. When the Player walks into it, it fires **On Touched**.

## Build a pickup

1. Play the scene first. A/D to move, Space to jump. The coin does nothing.
2. Select **Coin (make me work)**.
3. **Add Component > Circle Collider 2D**. Check **Is Trigger** (triggers detect overlap but don't block).
4. **Add Component > Touched**.
5. In **On Touched** click **+**:
   - Drag the **Coin** itself into the object slot.
   - Function: **GameObject > SetActive (bool)**, leave the box **unchecked**. (Hides the coin.)
6. Click **+** again:
   - Drag **Score** from the Hierarchy into the slot.
   - Function: **ScoreCounter > Add (int)**, type `1`.
7. Play. Grab the coin. Watch the counter.
8. Make it a prefab, then place a row of them.

## Why the Player works

The player has the **Player** tag (top of the Inspector). `Touched` only reacts to things tagged
Player. It also needs a **Rigidbody 2D**, because in Unity at least one of the two things touching needs one.

## Try

- Add an **Audio Source** to the coin, then another On Touched entry calling **AudioSource > Play**.
  (Tip: turn the coin off *last* in the list, or put the sound on a different object.)
- Make a "door": a wall that hides when you touch a switch.

Compare with `Scenes/Finished/02 UnityEvents (Finished)`.

Next: [03 - Asset Store](03-AssetStore.md)
