=== Episode6war ===
I have advised you, these desperate yellings are the result of the war.
Also in this little environment is difficult to stay in peace, what do you think about this war?
+ I'm crying
Poor little seed, maybe life is too hard for you, could you resist?
+ + Yes
+ + No
- - We'll see...
+ I'm not ok with it
Clear but will you proceed, right?
+ + Yes!
+ + Yes...
- - Perfect
+ I'm a little disturbed
Clear, but will you proceed, right?
+ + Yes!
+ + Yes...
- - Perfect
+ I'm the lord of war
Ohoh, we have a big boy here!
+ I'm fine, thanks
You are the smart guy of the gang, isn't it?
+ + The smartest in the world
+ + Not really...
- - If you say so...
- Interesting, in the meantime you are convoked for a small talk by a mean presence with a loudly voice.
Small mean presence "Come here, inhabitant, now it is your turn to speak with me. 
Your intentions in this world become clearer going further in time but I want to hear from you a little speech."
+   {hostility >= 0} I'm here to rule above all and I have stolen a lot of resources
~ resources = resources +80
~ hostility = hostility +1
Small mean presence "You have convinced me, conquest everything! I have prepared for you this big pack of resources, have fun!" #pause
-> Episode7earthquake
+   {cooperation > 1} I'm here to expand, trade, make deals, live, nothing weird
~ hostility = hostility -1 
~ resources = resources +20
Small mean presence "Not exactly my expectation but I'm ok with this intent, so take this abandoned resources." #pause
-> Episode7earthquake
+   {resources > 100} I don't know what do you desire from me exactly, I want only to accumulate resources
~ resources = resources +8
Small mean presence "Seems that you have only one thing in your head, take this little abandoned resources and leave" #pause
-> Episode7earthquake