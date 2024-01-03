INCLUDE ../globals.ink

Hello there! #speaker:Ayah #portrait:Ayah #layout:right 
-> main

=== main ===
How are you feeling today?
+ [Happy]
    That makes me feel <color=\#F8FF30>happy</color> as well! #portrait:Ayah #layout:right 
+ [Sad]
    Oh, well that makes me <color=\#5B81FF>sad</color> too. #portrait:Ayah #layout:right 
    
- Don't trust him, he's <b><color=\#FF1E35>not</color></b> a real doctor! #speaker:Putra #portrait:Putra #layout:left 

Well, do you have any more questions? #speaker:Ayah #portrait:Ayah #layout:right 
+ [Yes]
    -> main
+ [No]
    Goodbye then!
    -> END