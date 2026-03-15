---
title: "Introduction & Motivation"
slug: intro-motivation
date: 2026-03-15T12:00:00+01:00
---

# Introduction


As a part of the Interaction in Virtual and Augmented Reality(IARVR) I was tasked to create a parkour game in virtual reality with my own locomotion (moving around in the world) and interaction (interacting with virtual objects) methods. 

The project was done in Unity environment with Meta VR SDK (formerly Oculus VR), and a suitable head mounted display (in my case, Meta Quest 2) was provided as a part of the course. The project duration was roughly 6 months (2025-10 - 2026-03), including a separate mini-project in the form of reverse classroom, intended to help onboard students into the developing environment. The finished game was evaluated by 3 people including myself for speed and accuracy of the actions performed, and other quantitative/qualitative measurements.

---

# Motivation

Although I was at first sceptical I would complete the class successfully due to unfamiliarity of the problem domain and relative lack of ideas compared to peers, the structure of the course has given me enough guidance and assurance.

I was encouraged to seek out _real life examples_ to draw inspirations from, including apps and games on the supplied headset. A lot of creative freedom was given to students. Among the ideas discussed (and later actually implemented by others) for locomotion were those such as gravitational force fields, a donkey lured by a carrot on a stick, or [yourself sitting on top of a person steered by pulling their "hairs" around](https://en.wikipedia.org/wiki/Ratatouille_(film)). From this I have decided to simply come up with ideas and refine them into something workable.

---

# Problem statement & Solution

## Locomotion

I have wanted the locomotion to be based on physical motion, as I believed that is the most intuitive and direct way of moving around. The problem is **the mismatch of size between the virtual and physical world that you are allowed to move in.**

![First idea: roll the controller](../diagram1.png)

In this context my initial idea was a metaphorical hamster ball, mapped to the rotation of the controller. This idea was not much explored, and, if I were to do this course again, the one I would like to attempt. Practical limitations I've immediately identified was that it is hard to spin a controller unbounded fast enough to move distances and it risked damaging it. If I were to pursue this in the future the best path would be to design a 3D-printable sphere with the inside cut out to encapsulate the controller, and solving the tracking issue arising from the occlusion of the controller would be the challenge.

![Second idea: what if you just went](../diagram2.png)

Another locomotion technique I have envisioned was a concept of *restricted turning*. Due to my room layout I was restricted to mainly linear motion, back and forth. To be able to explore the world without indirect motion (e.g., via joystick), and to overcome the size mismatch problem, the idea was to decouple headset rotation from in-world rotation. The in-world rotation is dictated by where you are in it and how much rotation is necessary at that moment, eliminating the 2D component.

Aside from the foreseeable issue of motion sickness, and the awkwardness of being limited to an invisible wall (in terms of VR), it became clear that my viewing angle will be locked, and I had no clear distinction between turning the head to see things or to move in that direction. This idea was also abandoned.

---

Another idea that I ended up pursuing was a combination of existing ideas. It derived from the idea of increasing the speed of in-world movement in relation to the actual movement, essentially giving users a "superspeed". Due to motion sickness it was suggested that the world be shrinked down instead of increasing users' speed. This results in a relatively understandable way of locomotion that allows you to move faster than in real life.

![Third idea: I'm not small, you are](../diagram3.png)

From this I have come up with a idea to let the world size controlled by the user. The physical distance covered will stay the same, but with a shrunken down world the user effectively gains speed. This idea also offered additional opportunities to 'gameify' the experience, e.g., requiring users to be a certain size to progress or complete certain tasks. The idea was presented and received positive feedback.

## Interaction

Interaction was the harder part to come up with ideas. The problem was again the limited range of motion. To get around this I have decided initially to implement something called "World in Miniature": a secondary, miniature world that you can affect with, the changes of which is translated back into the main world. I think of it as essentially a dollhouse that's linked to "real" (in this case real-er) objects. The inspiration came from an application called "Theatre Elsewhere" which employed such a system for its UI.

<video width=60% autoplay loop muted>
    <source src="../277a464cd9d51a15ba191ba04e67b5ec.mp4" type="video/mp4">
</video>

---

# Idea Developments

From feedback, implementation details and practical limitations the concept has evolved and was adapted throughout the course of the project.

## Locomotion
Oculus headset wants to stay within a constrained boundary; a boundary is either stationary (usually while seated) or drawn over fixed physical location. Within the boundary there should be no obstacles, meaning it is inconvenient to try to use the majority of the room as a boundary, as anything above ground must be cleared away.

For my development I was able to designate a small area of around 2-3 square meters. Walking through the entire map, even with scaling, is inconvenient at this scale, so it required an alternative way of moving the player. 

It has come down to 2 techniques: simulated walking, where the player moves their body up and down as if they are walking which translates into forward motion, or world grabbing, essentially pushing and pulling the environment towards or away from you. I have chosen world grabbing, as the grabbing action can also be used with scaling, thereby increasing familiarity.

## Interaction
I was not truly happy with the idea of World in Miniature for this project as it felt disconnected to the locomotion method. There was no compelling reason I *should* manipulate an object through a miniature instead of directly, especially when I'm allowed to roam freely unbounded by the limits of physical space. In an effort to make it more interesting in the proposal I have suggested a mixture of miniature and direct interaction, to which I got a feedback that direct interaction is already a different form of interaction.

Another problem was the reality of interacting with the miniature itself. For this project I have decided to exclusively support controller inputs, as its positioning is more accurate and it offers more types of inputs (hand tracking offers pinch detection, meanwhile each controller has a thumb stick, 3 buttons and 2 analogue triggers). **How 'miniature' can the miniature be if it's meant to be interacted with a controller that is the size of a palm?**

Initially I have come up with ideas such as creating an implement such as a tong. But ultimately such conceptual problems and implementation constraints, such as having a mirrored set of objects in different coordinate systems, coupled with time constraints have forced me to abandon the idea and get back to the drawing board.

During consultation it was suggested that I take existing manipulation methods from locomotion and apply it to interaction as well, which made a lot of sense. The user again does not need to learn a different concept of movement. Objects can be directly manipulated without the user being in proximity. And I was able to reuse the code and save time for other aspects of the map.
