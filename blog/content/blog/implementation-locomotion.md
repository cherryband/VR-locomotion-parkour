+++
title = "Implementation - Locomotion"
date = "2026-03-15T14:00:00+01:00"
+++

# Implementing Movement
The reference implementation already had its own implementation of the movement. It worked similarly but not quite the same as I intended. The most crucial difference is that while the motion of the controller dictates the distance, the direction towards which the player will move solely depends on the orientation of the headset. For example, you could swing your arm back and forward and the player will only move forward, instead of going back and forth along with the controller movement. For this project I wanted to mave the controller motion decide both the magnitude and the direction of movement. This means you can reverse without looking back, or move sideways.

It took me a couple of tries to land on a working prototype, but it came relatively quickly. The architecture I've landed on here became the blueprint for basically every other aspect of control method in this project.

<details>
  <summary>Detailed description of the mechanism</summary>
  1. A global variable of the state is kept (e.g., `scaleOffset`, `startPosition`, ...). This variable is only updated *after* the adjustment, when the user lets go of the control.

```csharp
public class LocomotionTechnique : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float scaleOffset = 1f;
    (...)
}
```

  2. When the user presses the trigger, a boolean such as `isIndexTriggerDown` flips state. The initial state of the controller (position, rotation, etc.) is taken while doing so.

```csharp
void Update()
{
    if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
    {
        Vector3 pos1 = OVRInput.GetLocalControllerPosition(leftController);
        Vector3 pos2 = OVRInput.GetLocalControllerPosition(rightController);
        if (!isIndexTriggerDown)
        {
            isIndexTriggerDown = true;
            startPos1 = pos1;
            startPos2 = pos2;
        }
    }
    (...)
}
```

  3. A delta variable tracks the difference between the current state and the state recorded in step 2.

```csharp
if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
{
    (...)
    scaleDelta = (pos1 - pos2).magnitude - (startPos1 - startPos2).magnitude;
    positionOffset = (startPos1 - pos1) + (startPos2 - pos2);
}
```

  4. When user lets go of the trigger, state variables are updated and delta variables are zeroed.
  
```csharp
(...)
else if (leftTriggerValue < 0.95f && rightTriggerValue < 0.95f)
{
    if (isIndexTriggerDown)
    {
        isIndexTriggerDown = false;
        scaleOffset *= 1 + scaleDelta*scaleGain;
        startPosition += positionOffset*translationGain/scaleOffset;
    }
    positionOffset = Vector3.zero;
}
```

  5. Position and other transformations of GameObjects are updated every time, independent of user input. This is enabled by the decoupling of previous state and current ongoing modification. The calculations here are the same as what happens in step 4.

```csharp
void Update()
{
    (...)
    float curScale = scaleOffset * (1 + scaleDelta*scaleGain);
    if (curScale < 0.01f) curScale = 0.01f;
    transform.position = startPosition*curScale + positionOffset*translationGain;
}
```

  (`curScale` need to be applied to the previous position as it depends on the world scale and world scale also depends on input.)
</details>

# Implementing Scaling
With the basic structure out of the way, the challenge was experimenting with how to incorporate the differential. For  It needed to work across a range of different scales, meaning it had to work intuitively, for example, across 0.1x, 1x, or 10x scale. It has to be noted that `scaleDelta` is a *percentage difference* of distance between the controllers. This means the value will be close to 0 most of the time, and it can be negative as well as positive.

Here are some of the methods I have tried:

1. Add the `scaleDelta` directly to world scale (`currentScale = Math.E**(worldScale+scaleDelta);`). This worked, but because it was an addition it was effectively non-linear. The enlarging of the world was particularly slow due to the nature of the value.
2. Apply the [exponential function](https://en.wikipedia.org/wiki/Exponential_function) (`currentScale = Math.E**(worldScale+scaleDelta);`). This was an experiment to introduce non-linearity, considering I need more transition on the large values and less on the smaller, lower than 1 values. But it ended up failing miserably.
![diagram of exponential growth](/diagram6.png)
In particular, once the world scale went above 1, which was easy to do without being aware of, due to the design of refreshing scaleOffset to be the world scale once the player lets go of the controller, it was impossible to scale it back down to 1 or lower. I have looked into taming the function somehow to make it work, but ultimately decided that the solution doesn't need to be this complex.
3. Multiplying the world scale by `1+scaleDelta` (`currentScale = worldScale * (1+scaleDelta);`). This has resolved the issue of non-linearity and the result felt like I had adequate control over the world. While seemingly obvious in hindsight, the complication was that I could not simply multiply the `scaleDelta` to world scale, since it is a difference of percentage. Adding 1 has fixed the problem.

# Attempts at implementing rotation
The reference map contains a twisted path, and is designed so that the player has to spin 360 degrees overall to clear the map. This meant some scenes had to be viewed at a certain angle. The reference locomotion solves this by making the player rotate as it moves, therefore eventually aligning to the desired orientation.

In the spirit of direct manipulation, my method of rotation would have been manipulated in the same vein as the position and scale. The problem arises from the fact that it had to be rotated from the perspective of the player. Directly manipulating transform values -- what I've been doing for both scale and position -- means that I will have to calculate not only the rotation, but the resulting location offset myself so that the rotation appears anchored to the player.

The second problem was determining the angle of rotation itself. At first I tried to solve it as a vector problem; Using the initial and current positions of the controllers, I tried using a formula to get the angle between those vectors.

![diagram](/diagram5.png)

This worked only partially. The problem was that I am only getting the short angles. Practically this meant I can rotate only counter-clockwise, and the motion in the other direction is mirrored. Even after knowing this problem I had no better idea to acquire the angle, so this feature was put into low priority.

(If you're wondering, Quarternions have a helper functions that I could have used, I've tried them. You will see in the next section why that didn't work out.)

# Result
<video width=75% controls muted>
    <source src="/5c03344569260e10b05d53a3245239f9.mp4" type="video/mp4">
</video>

Notice that the effects such as halo and particles are not scaled so they look extremely large. You may also notice that the start button for the interaction task appears far away and had to be brought forward by scaling the world back up, which is where the next implementation starts.
