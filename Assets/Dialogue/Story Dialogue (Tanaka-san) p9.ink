INCLUDE globals.ink

{TalkedToTanaka == true: -> talkedToTanaka | -> main}

=== main ===

    (looking around nervously) Quick. You’re the kid, aren’t you? (he takes out a slip of paper from his pocket and hands it to you)#speaker:Tanaka-san
    
    I don’t have a lot of time, but if you take this with you, the guards won’t question where you’re goin’, okay?
    
    Take this, and if any guards come by and ask you why you’re here, say your parents work for the Furukawa Plantation Company. 
    
    "Furukawa Plantation Company", say that.
    
    They’ll mistake you for an errand boy.
    
    But they don’t work for the Furukawa Plantation Company, they work for Mister Ernesto…#speaker:You
    
    It’s all the same kid. #speaker:Tanaka-san
    
    That’s how Filipinos get your land to begin with. It’s all funded by us Davao-kuo. 
    
    Furukawa, Mr. Ernesto’s; one and the same. We call it… “pakyaw”. Makes getting land a lot easier for you and me both.
    
    But— who am I kidding- that ain’t any of your concern. Just take the note or don’t.
    
    *[Take the note]
        ~ HasLetterFromTanaka = true
        ->choice1
        
    *[Refuse the note]
        ~ HasLetterFromTanaka = false
        ->choice2
        
        
    === choice1 ===
        Good luck out there, kid.#speaker:Tanaka-san
        
        Where are you going, Mr. Tanaka?#speaker:You
        
        Somewhere away from the Kempei Tai. It isn’t safe for me out here. Ha, maybe I’ll take up on that evacuation order to go to Tamugan instead!#speaker:Tanaka-san
        
        Aren’t they Japanese too? Why wouldn’t you feel safe around them?#speaker:You
        
        Haha! The Kempei Tai are a different monster, kid, even if we’re both Japanese. #speaker:Tanaka-san
        
        They’re from the mainland, and I’m an Okinawan; in their eyes, we’re one and the same too. Just islanders lower in worth than them.#speaker:Tanaka-san

        (he looks up and realizes some guards are getting closer) Kid, I’ve gotta scram. 
        
        (Tanaka looks at you with a look of apprehension, and he frowns) I’ll see you when I see you.
        
        O-okay…#speaker:You
        
        Alright, that's "Furukawa Plantation Company", got it? That's what you tell 'em. #speaker:Tanaka-san 
        
        Anyways, good luck out there, kid. I’ll see you when I see you.#speaker:Tanaka-san 
        
        ~ TalkedToTanaka = true
        
        -> END
        
        
    === choice2 ===
        
        Have it your way, kid, but lemme tell ya, the Kempei Tai are a different monster for both you and me.#speaker:Tanaka-san
        
        O-okay…#speaker:You
        
        Good luck out there, kid. I’ll see you when I see you.#speaker:Tanaka-san
        
        ~ TalkedToTanaka = true
        -> END
        
===talkedToTanaka===
Shoo, kid! I gotta scram! #speaker:Tanaka-san

-> END
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        