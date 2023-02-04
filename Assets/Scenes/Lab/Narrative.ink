INCLUDE Episode1.inkl
VAR aggressivity = 10
VAR has_attacked = false

-> Intro

= Intro
{has_attacked: There was a nasty attack last night.|No violence last night. Good news.}
{has_attacked: -> Revenge}
Once upon a time…
…and they lived happily ever after.
+   And then they divorced.
    It was a long and painful process.
    -> Divorce
+   But something awful happened…
    …an alien attack!
- To be continued. #pause
Continuation.
~ aggressivity = aggressivity + 1
Continuation!
{aggressivity > 10:
    The aggressivity was {aggressivity}.
  - else:
    The aggressivity was 10 or lower.
}
+   {aggressivity < 5} Peace and love!
+   Open door with key
+   Go south
- Whatever. #pause
-> Episode1

= Revenge
NativeLeader "We will kill you!"
You "Oh please, you are ridiculous."
-> DONE

= Divorce
They had to find an agreement for the children.
-> DONE