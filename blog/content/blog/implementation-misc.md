+++
title = "Implementation - Miscellaneous"
date = "2026-03-15T16:00:00+01:00"
+++

At this point I went from being behind schedule to ahead of schedule. Both locomotion and interaction elements were functioning as intended, with little bugs. So I got to a rapid-fire session of implementing bells and whistles.

## Player surrogate
The reference game does not have an in-game representation of the player. This was proving to be a problem with collecting coins, for example. Making the coins smaller and being able to levitate means you didn't know whether you will hit the given target, or whether you were even on the ground.

So I added a bunny.

![A bunny was deployed](/Screenshot-2026-02-06-231549.png)

The bunny was chosen because ~~it was my spirit animal~~ the motion that I ended up making when moving around was rotating my wrists. It was a way to move around fast without making a lot of motion, which in game appeared as if you're jumping -- a bunny hop.

## Giraffe "Signpost"
The decision to include a giraffe in the game was made way early. As an indication that you should be scaling the world, I intended to include something like a height sign that you see in amusement parks ("You must be at least this tall to ride"). The name "Too big to ride" also originates here.

<img src="/Pullen1.jpg" width="40%">

<sub>*image source: http://www.signergy.us/wp-content/uploads/2014/11/Pullen1.jpg. For illustration purpose only.*</sub>


At first I was looking for a height signpost that explicitly contains a giraffe. But when searching through the Unity Asset Store revealed nothing of sort, I decided to use just good ol' giraffe.
