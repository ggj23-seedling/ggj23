=== Episode2growth ===
Welcome back, as I can see you are growing fast, good for you! 
A near voice calls you.
You haven't heard this voice before and it seems very deep and solemn, as if coming from a spirit.
Voice "Newcomer, are you a good guy?"
+   {hostility > 1} Not really
~ resources = resources +2
Voice "At least you are sincere."
Voice "Take this 2 resources, I hope in a change of heart." #pause
-> Episode3merchant
+   {cooperation > 1} Yes, and I'm proud of it
~ hostility = hostility -1 
Voice "I see, I think that maybe you could have done something wrong without knowing it, so I can help you." #pause
-> Episode3merchant
+  I don't know
Voice "We will discover it in the future" #pause
-> Episode3merchant