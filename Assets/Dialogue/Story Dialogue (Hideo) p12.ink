VAR HasHelped = false

-> main

=== main ===

    (breathing laboriously, trying to scramble away from the MC)#speaker:Hideo
    
    What’s wrong…?#speaker:MC
    
    (he’s clutching the wound on his shoulder defensively, inching away from you)#speaker:Hideo
    
    What will you do?#speaker:Narrator
    *[Help the soldier]
        ~ HasHelped = true
        ->choice1
        
    *[Do not help the soldier]
        ~ HasHelped = false
        ->choice2
        
    === choice1 ===
        The MC will be tasked with obtaining water and a makeshift bandage.#speaker:Narrator
        
        Hideo: My tag…#speaker:Hideo
        
        Tag Aquired!#speaker:Narrator
        
        -> END
        
        
    === choice2 ===
        
        You left.#speaker:Narrator
        -> END