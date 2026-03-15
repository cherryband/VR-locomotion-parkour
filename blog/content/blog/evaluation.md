---
title: "Evaluation & Feedback"
slug: evaluation
date: 2026-03-15T17:00:00+01:00
---

# Preliminary Feedback
During the final presentation there were short demo sessions where all of the course participants had a chance to play the game in their own headset. From this informal demo I was already able to get useful feedback:

* The perspective of looking down on the mini parkour map was interesting and unique.
* Having to constantly look down is fatiguing.
* The movement feels restricted; People were frustrated that they were moving so slowly and wanted to move faster.

While I shared some of the feedback, such as fatigue from having to look down, the feedback of feeling restricted came as a surprise to me. It seemed to indicate that I didn't communicate clearly enough that in order to speed up you needed to shrink the world down, or something else.

With this helpful information I designed an experiment to confirm this hypothesis.

# User Survey
I needed to do a user survey on 3 people including myself. Since people seemed to have problems moving around, I have decided to design an experiment in such a way that I can gradually introduce them to the game mechanics. If I were given more time and scale this could have been a comparison study, but extrapolating results from 3 people are already limited in itself.

The study was to be conducted as follows:

1. The participant first plays the game following all the rules. This includes collecting all coins, minimizing property damages as much as possible, positioning themselves in the optimal scale, and performing the T-shape rotation task to the best of their ability. The time of this is recorded.
2. The participant then completes a short survey over the experience.
3. The participant plays the game again, and this time they are encouraged to ignore the rules and complete the map as fast as possible. The interaction tasks were not skipped but rather allowed for some errors. The time is recorded.
4. The participant completes the same survey.

<details>
  <summary>Survey Questions</summary>

All questions (except Interaction-1 and the last open-end question) use 5-point Likert Scale.

* Locomotion
    1. I was able to move around and scale accurately.
    1. I was able to move around and scale quickly.
    1. I was able to move around without physical fatigue or uncomfortable postures.
* Interaction
    1. The size of T-shapes during the interaction tasks were in general...
        * small and far apart
        * large and close together
    1. I was able to move and rotate the object accurately.
    1. I was able to move and rotate the object quickly.
    1. I was able to move and rotate the object without physical fatigue or uncomfortable postures.
* If you can change one thing about the game, what will it be? (optional open-end question)

The intention of Interaction-1 was to confirm, generally, that the user has played the interaction part using the correct scale. "Small and far apart" can indicate that the world is too small.
</details>

The participants I was able to recruit were other students in the same course, meaning they had prior VR experience. They have participated in the study remotely using their own headsets.

## Results
Unfortunately the study has not worked out as planned, since both of the 2 people I recruited had failed to complete the map even once. Both of them have reported problems in locomotion. At first I expected a movement bug, specifically one that involves different kind of headset (students were randomly given a Quest 2/3/3S). But analysing the footage they have given me revealed that they were scaling too little, and staying too small. They were not gaining speed because **they were too small** relative to the world (which is how the game starts by design, to encourage scaling the world down).

Furthermore, I have realised I have not tested such a scenario, meaning there can be edge cases where the user travells "too far off the map." Because it's the world scaling relative to the user and player moving, not using the scaling function means that the actual coordinates travelled by the player will be much larger, even if you end up at the "same place". Participant 1 has reported a bug where they could not progress to a different level even after finishing the task, and participant 2 reported what looked like a grey wall cutting off the world, indicating they might have somehow reached the far side of the render distance.

<video width=60% autoplay loop muted>
    <source src="../VID-20260315-WA0003.mp4" type="video/mp4">
</video>

This scenario was not considered during the development, because... it was not a reasonable way to play. Player 1 has reported they had spent 5 minutes just to reach the 1st interaction area, meanwhile I had spent 4 minutes completing the *entire map* under the accuracy constraint (following all the rules). The scale at which they were moving in the screenshot reflected this: they had ample room to grow but they didn't, even when they knew how to scale.

---

After this I was able to get 2 more participants who *did* finish their runs and was able to submit the questionnaire. The questionnaire results generally converge into neutral or positive experience, but not much meaningful results were observed. 

The T-shape size question did lead to a finding: in the accuracy run, everybody selected "large and close together," while in the speed run only 1 out of 3 did. This means that when pressed for speed, users rather scale down less to preserve motion. This also means they are navigating a scaled-down world for the majority of time, which was not the case for the above 2 people who couldn't finish the map. In other words, they appear to have successfully learned the rules of the game.

Considering the results it may have been better to do a one-on-one interview to find out what led to such differences in behaviour, but due to time constraints it wasn't possible.

# Conclusion

My main goal was to create a locomotion & interaction techniques that were physical in nature while not being bounded by spatial limitations. In that sense, the project was able to meet its goal. The assumptions leading up to it, however, seems to be incorrect.

One of the principles I have used designing the mechanics was how intuitive it was, without concrete justification. It often is the case that as a creator you know your way around too much that there is a high barrier to entry. I have tried to mitigate that problem, but without **frequent, active involvement of others** this was not as effective. Reports from study participants reflect this: while they find the mechanic interesting and well balanced once they got to understand it, getting there seemed to be not as easy.

It may have also been beneficial to **get technical assistance** early on. I prefer to figure something out by myself, but by thinking with others the issues I had with basic operation may have been resolved earlier.

All in all, however, I started from not knowing anything about Unity, C#, and VR to being able to create a substantial work based on it. Even though the project was mostly modifying existing code, I rather see it as a blessing since it means I was able to focus on the business logic for the most part. The creative exercise of creating and refining an idea was also a part that I found helpful, since I started out with no prior experience in VR.
