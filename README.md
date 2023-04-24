# Turn-Based-Strategy-Prototype
An ancient unfinished 2D Unity project/blatant Advance Wars clone of mine from 2015.

If you just happen to be a Unity developer stumbling upon this repo looking for inspiration or code examples of a strategy game done in Unity, please feel free to browse 'assets/scripts/' where all the C# scripts live. Alternatively you can clone this repo and open it as a Unity project to look through yourself. Just know that my code is probably very bad and absolutely not the best way to do things. Especially be mindful that any of the packages I may have been using and even the built-in Unity libraries are likely outdated or deprecated.

## Description
This is one of my biggest and oldest game dev projects from back in the day, even though it never turned into a solid working game. At that time, I was addicted to a little game on the Game Boy Advance called Advance Wars. Back when I started getting serious about learning Unity, I decided to try and make a clone of Advance Wars.


![The map editor](https://i.imgur.com/L0qgthU.png)


This project was a learn as I go endeavor. By making this I learned a lot about fundamental concepts around OOP, polymorphism, encapsulation, saving and serialization, pathfinding algorithms, singleton patterns (and their potential for evil), and state machines, which the entity system and AI uses heavily.

This was originally conceived back when Unity was still in version 5. In those days, Unit's 2D features weren't as fleshed out. If you wanted a top-down 2D tilemap system, it had to be made scratch. Unity's input handlers and its (then new) UI system were pretty wonky and not super easy to use in my opinion.

![VS AI](https://i.imgur.com/uDno6IR.png)


Looking through this code many years later, it looks like a hot mess with some redeeming qualities here and there. A lot of things are public that shouldn't be public. There are a tremendous amount of things at huge risk for a null reference exception. To say that this is tightly coupled code would be an understatement. 

Only a couple of unit types are implemented - foot soldiers and tanks. There are 4 different terrain types - grass, roads, rivers, and ocean. These work pretty well in the map editor and they even auto-connect in a cohesive way, so you can create 4-way intersecting roads or rivers using 1 tile (if I recall, this involves some fun and whacky arithmetic). New map generation is unfortunately broken. If it wasn't, you COULD create a new map in the map editor, make it look pretty, fill it with units and buildings for up to 4 different teams (represented by red, green, blue, and yellow), and then play against the AI until all units are dead. Moving, targeting, attacking, and switching turns for a human or AI player all work. The AI utilizes pathfinding to move its units and it will seek out the highest value target to attack and it will end its turn after moving all of its units. While attacking and killing things works, the damage formula seems broken. Low health units can't hit as hard as high health units (as in Advance Wars), but tanks and infantry don't do appropriate amounts of damage. Saving and loading maps in the editor, and starting a new vs AI game from the "War Games" menu works as expected.

All in all, this project will live on Github as a reminder to myself of my own learning experiences. Maybe someday I will do something cool with it!