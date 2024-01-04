INCLUDE ../../globals.ink

Hello there! #speaker:Adik #portrait:Adik #layout:left #audio:default
-> main

=== main ===
How are you feeling today?
+ [Happy]
    
    That makes me feel <color=\#F8FF30>happy</color> as well! #portrait:Adik #POINTS:1
+ [Sad]
    Oh, well that makes me <color=\#5B81FF>sad</color> too. #portrait:Adik #POINTS:2
    
- Don't trust him, he's <b><color=\#FF1E35>not</color></b> a real doctor! #speaker:Putra  #portrait:Putra #layout:left #audio:default

~ playEmote("question")
Well, do you have any more questions? #speaker:Adik #portrait:Adik #layout:left #audio:default
+ [Yes]
    -> main 
+ [No]
    Goodbye then!
    
    -> END
