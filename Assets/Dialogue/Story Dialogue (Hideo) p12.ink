INCLUDE globals.ink

-> main

{TalkedToHideo == true: -> talkedToHideo | -> main}

=== main ===
    He breathes laboriously, trying to scramble away from you.#speaker:Narrator
    
    What’s wrong…?#speaker:You
    
    He’s clutching the wound on his shoulder defensively.#speaker:Narrator
    
    What will you do?#speaker:Narrator
    *[Help the soldier]
        ->choice1
        
    *[Do not help the soldier]
        ->choice2
    
        
=== choice1 ===
~ TalkedToHideo = true
    The man looks like he needs a drink and something to bandage his wound with. #speaker: Narrator
    
    -> END
    
=== choice2 ===
~ TalkedToHideo = true
    You left.#speaker:Narrator
    -> END
    
=== talkedToHideo===
    The man doesn't have much to say. He doesn't look like he has the energy to, if he had anything to say anyway. #speaker:Narrator

    -> END