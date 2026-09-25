# 00 - Git + Setup

Git is how a team shares one project. Everyone has a full copy, and Git keeps track of every change
so you can see who did what and undo mistakes.

## Words you'll hear

- **Repo (repository):** the project folder plus its whole history.
- **Clone:** download a repo to your computer.
- **Commit:** a saved snapshot of your changes with a short message ("added jump animation").
- **Push:** send your commits up to GitHub so the team gets them.
- **Fetch / Pull:** get everyone else's commits.
- **Branch:** your own line of work that doesn't touch everyone else's until you merge it.
- **Merge conflict:** two people changed the same thing. Scenes are the worst for this.

## Install

1. **Unity Hub:** https://unity.com/download
2. In Unity Hub > **Installs** > **Install Editor**, pick **Unity 6.3 LTS** (6000.3.23f1).
   Add the **WebGL** module if you want to put games on itch.io later.
3. **GitHub Desktop:** https://desktop.github.com, then sign in (make a free GitHub account if needed).

## Get the project

1. GitHub Desktop > **File > Clone repository > URL**.
2. Paste `https://github.com/JoshWright22/unity-tools-workshop` and pick a folder. Clone.
3. Unity Hub > **Projects > Add > Add project from disk**, pick the folder you just cloned.
4. Open it. The first import takes a few minutes (it's building the `Library` folder).
5. In the Project window open `Assets/Workshop/Scenes/01 Basics`.

## Make it yours

1. GitHub Desktop > **Current Branch > New Branch**, name it after yourself (`aspen-workshop`).
2. Do the lessons. Every so often, go to GitHub Desktop: you'll see every file you changed.
3. Type a short summary at the bottom left, **Commit to aspen-workshop**, then **Publish branch** / **Push**.

## Rules that save jam teams

- **Fetch, then Pull, before you start working.** Every time.
- **Commit small and often.** "Added coin prefab" beats one giant commit at 3 AM.
- **Never edit the same scene as someone else at the same time.** Say it in Discord first.
- **Build in your own test scene,** turn your stuff into prefabs, then drop the prefabs into the main scene.
- **Don't commit the `Library` folder.** The `.gitignore` in this repo already skips it. Unity rebuilds it.
- **Always commit `.meta` files.** They're how Unity keeps track of which file is which.

Next: [01 - Basics](01-Basics.md)
