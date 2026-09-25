# 05 - Tilemaps

Scene: `Scenes/05 Tilemap`

Tilemaps let you paint 2D levels with a brush instead of placing hundreds of objects.

- **Grid:** the parent. Sets the cell size.
- **Tilemap:** one layer you paint on (background, ground, decoration...).
- **Tile Palette:** your paint box.
- **Tiles:** assets that say which sprite goes in a cell (see `Tiles/`).

## Make a palette

1. **Window > 2D > Tile Palette**.
2. **Create New Palette**, name it `Workshop`, save it in a new `Palettes` folder.
3. Drag the whole `Tiles` folder into the palette window.

## Paint

1. In the palette, make sure **Active Tilemap** says **Level**.
2. Pick a tile, press **B** (brush) and paint in the Scene view.
   - **Shift** while painting erases. **G** is a box fill. **I** picks a tile from the scene.
3. Paint a floor, some platforms and two walls.

## Make it solid

1. Select **Grid > Level**. **Add Component > Tilemap Collider 2D**.
2. Play. It works, but every tile is its own box (look at the green outlines). Slow, and
   the player can snag on seams.
3. On the Tilemap Collider 2D set **Composite Operation: Merge**.
4. **Add Component > Composite Collider 2D**. It adds a Rigidbody 2D too. Set its **Body Type: Static**.
5. Now it's one smooth outline.
6. Select **Player**, set Rigidbody 2D **Gravity Scale** to `3` (it's 0 in the starter so you don't
   fall forever before painting). Play.

## With your Asset Store pack

Pixel Adventure has `Terrain (16x16)`. Slice it 16x16 with **Pixels Per Unit 16**, then drag the
**sheet** (not a Tiles folder) into a new palette. Unity makes the tile assets for you.

## Try

- A second Tilemap for background decoration: right-click Grid > **2D Object > Tilemap > Rectangular**,
  no collider, lower **Order in Layer**.
- **Rule Tiles** (Create > 2D > Tiles > Rule Tile): tiles that pick the right edge or corner sprite
  on their own.

Compare with `Scenes/Finished/05 Tilemap (Finished)`.

Next: [06 - Yarn Spinner](06-YarnSpinner.md)
