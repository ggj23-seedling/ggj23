=== Episode3merchant ===
I'm back and as you can see the boss of the merchants approches you.
Merchant "Hello, what a pleasure to see you again!"
Merchant "Since your arrival we all made a lot of progresses, so I will share some resources with you." 
+   {cooperation >= 0} Thank you
~ resources = resources +4
Merchant "Take these resources, grow well and we will met again." #pause
-> Episode4life
+   {cooperation > 1} It's great!
~ resources = resources +8
Merchant "You can say it, live long and prosper!" #pause
-> Episode4life
+   {cooperation > 2} Wonderful!
Merchant "We still are in debt with you, thank you again." #pause
-> Episode4life