INCLUDE ../globals.ink

Hello there! #speaker:Ayah #portrait:Ayah #layout:right #audio:default
-> main

=== main ===
How are you feeling today?
+ [Happy]
    That makes me feel <color=\#F8FF30>happy</color> as well! #portrait:Putra #layout:left #audio:default
+ [Sad]
    Oh, well that makes me <color=\#5B81FF>sad</color> too. #portrait:Putra #layout:left #audio:default
    
- Don't trust him, he's <b><color=\#FF1E35>not</color></b> a real doctor! #speaker:Ibu #portrait:Ibu #layout:right #audio:coba_2


Well, do you have any more questions? #speaker:Ayah #portrait:Ayah #layout:rigt #audio:coba_1
+ [Yes]
    -> main
+ [No]
    Goodbye then!

    -> END