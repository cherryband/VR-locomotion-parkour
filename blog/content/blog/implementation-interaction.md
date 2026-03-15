+++
title = "Implementation - Interaction"
date = "2026-03-15T15:00:00+01:00"
+++
The reference interaction was simply "grabbing" the object with the side trigger and manipulating it directly. It seemed that I was able to use it unmodified...

![Screenshot of far-away button](/a235df7b92db722768922fda9f608967.png)

... if I was able to reach the start button.

Possibly due to the fact that scaling the world also moves the terrain (to keep it centered from the player's POV), changing the world scale also weirdly translated into linear motion of the UI. I was able to use this fact to solve the first interaction task without modifying anything, but once I got to the 2nd task, whatever combination of scale and position it is, it was impossible to reach the button like the first one. So the testing was over, I had to change stuff.

## Development blunders

Intending to making it a world in miniature UI, I made it follow the left hand. But for whatever reason, it didn't.

A significant effort was made debugging, because something was not right. The issue manifested in a lot of different ways. In Unity, you can assign a GameObject to a script by dragging it to the data field in the Unity Editor, instead of finding it "programmatically". This didn't work; it resulted in `NullReferenceError`. I tried changing the GameObject hierarchy. It didn't work. I tried editing *something* on the scene to make the changes appear. It didn't work.

Turns out I was editing the wrong scene.

Even though these problems wasn't at all technically complex and it was completely my fault, it's worth mentioning as it had major influence in the development of the game. Having no prior Unity experience, developing within this constraint for the majority of my develpment felt like I was working with a broken, buggy development platform. The reason I was able to push through was that despite the difficulties, if I worked on it enough, there was always an answer. The answer in this case, however, came a bit too late: I had a week left before my final presentation to implement the locomotion technique and make other changes.

## Implementing Rotation
As mentioned in the [previous post](intro-motivation), at this point I have given up the world in miniature concept and tried implementing simpler ways to do it. I've decided to basically borrow the locomotion technique and apply it to interaction.

Implementing translation (moving) of the object was straightforward: just plug in the locomotion script and modify it. The issue was rotation. I didn't gain much from implementing locomotion, since I decided to skip it for the time being.

I came across [this handy-dandy function](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Quaternion.SetFromToRotation.html) of Unity's Quaternion class which, given a 'before' and 'after' rotation, returns a Quaternion that represents the rotational difference of the two vectors.

```
rotationOffset = Quaternion.FromToRotation(startPos2-startPos1, pos2-pos1);
```

This was exactly what I needed, since it fits in perfectly with my "preview changes live and commit" architecture. I can set the current rotation as the start value, apply the rotation in tandem to get both live feedback and later to apply the rotation and set it as the baseline.

<table>
    <tr>
        <td>
            <video width=100% controls>
                <source src="/a56369285992bb965f94e27c4052632e.mp4" type="video/mp4">
            </video>
        </td>
       <td>
            <video width=100% controls>
                <source src="/78cc13c21adb8fe45c80a96ea4a14e0d.mp4" type="video/mp4">
            </video>
       </td>
    </tr>
</table>

However, it didn't work the first time. It did rotate, but weirdly. The video on the right was a test implementation where I used the rotation of one controller directly, since I suspected maybe I was using `FromToRotation` function incorrectly. And what I got in both cases was somehow the rotation was applied... "backwards". That's because it was.

The Quarternion [operator *](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Quaternion-operator_multiply.html) states that combining rotation is not commutative, meaning `a*b != b*a`. What I wasn't aware of, though, was that I have to apply my desired additional rotation *first* and the current rotation second, rather than the other way around.

```csharp
correct:
    selectedObj.transform.rotation = rotationOffset * startRotation;
incorrect:
    selectedObj.transform.rotation = startRotation * rotationOffset;
```

Once that was fixed it was smooth sailing.
