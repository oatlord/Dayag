INCLUDE globals.ink

->start

=== start ===
{TalkedToHideo: 
    -> talkedToHideo
- else: 
    -> main
}
-> END


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
~ ChoseToHelpHideo = true
    The man looks like he needs a drink and something to bandage his wound with. #speaker: Narrator
    
    -> END
    
=== choice2 ===
~ TalkedToHideo = true
    The man gazes at you weakly, but he says nothing more. His breath is getting more and more laborious by the second, but you get up and leave.#speaker:Narrator
    -> END
    
=== talkedToHideo ===
    // The man doesn't have much to say. He doesn't look like he has the energy to, if he had anything to say anyway. #speaker:Narrator
    // {HasHelpedHideo: He does, however, look up at you with thankful eyes. For now, that keeps you going.} #speaker:Narrator
    {HasHelpedHideo:
        He looks up at you with thankful eyes and says nothing else. #speaker:Narrator
        -else: 
    The man doesn't have much to say. He doesn't look like he has the energy to, if he had anything to say anyway. #speaker:Narrator
    }
    
    *[Leave]
        You decide against saying any more to this man.
        -> END
    * {!HasHelpedHideo && ChoseToHelpHideo && PlayerPickedUpWater && PlayerPickedUpBandages} [Assist the soldier]
        ->assistHideo
    
=== assistHideo ===
    You drop a cloth and some water in front of the injured man. #speaker:Narrator
    
    You take his injured shoulder, and he lets you, and with your little hands although inexperienced, you wrap it in the makeshift bandage to soak up the blood. #speaker:Narrator
    
    Then, you help him take gentle sips of water.#speaker:Narrator

    His mouth then moves, but you don’t quite understand what he said. #speaker:Narrator
    
    It seems he’s speaking Japanese, which is a language you don't quite understand fluently. #speaker:Narrator
    
    You nod anyway and get up to leave, but before you can leave, he takes your wrist and quickly places something in your hand. #speaker:Narrator
    
    My tag…#speaker:Hideo
    
    It seems like a dogtag that many military men wear, engraved with his initials.#speaker:Narrator
    
    You thank him in your language before heading on your way.#speaker:Narrator
    
    ~ HasHelpedHideo = true
    ~ TalkedToHideo = true

    -> END