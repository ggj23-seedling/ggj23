=== Episode9knowledge ===
The time of the knowledge has come. 
Let's check your thoughts about the journey until now.
+   {hostility >= 0} I'm not a colonizer
I see.
~ resources = resources +80
~ cooperation = cooperation +1
+   {hostility > 2} I'm enjoying the fights
I see.
~ resources = resources +60
~ hostility = hostility +1
+   {hostility > 3} I'm into the war system!
I see.
~ resources = resources +40
- Your choice. ->step2
 =step2
+   {cooperation >= 0} I'm not a warlord
I see.
~ resources = resources +80
~ hostility = hostility -1
+   {cooperation > 2} I'm enjoying the dialogues
I see.
~ resources = resources +60
~ cooperation = cooperation +1
+   {cooperation > 3} I'm mastering the storyline!
I see.
~ resources = resources +40
- Your choice. ->step3
= step3
+   {resources >= 0} I'm not an accumulator
I see.
~ resources = resources +80
+   {resources > 200} I'm enjoying the resources system
I see.
~ resources = resources +60
+   {resources > 300} I'm rich!
I see.
~ resources = resources +40
- Your choice. #pause
-> Episode10equilibrium