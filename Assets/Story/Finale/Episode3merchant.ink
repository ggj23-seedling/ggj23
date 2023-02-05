=== Episode3merchant ===
I'm back and as you can see the boss of the merchants approches you.
"Hello, what a pleasure to see you again! Since your arrival we all made a lot of progresses, so I will share some resources with you." 
+   {cooperation >= 0} Thank you
~ resources = resources +4
"Take these resources, grow well and we will met again." #pause
-> Episode4life
+   {cooperation > 1} It's great!
~ resources = resources +8
"You can say it, live long and prosper!" #pause
-> Episode4life
+   {cooperation > 2} Wonderful!
"We still are in debt with you, thank you again." #pause
-> Episode4life