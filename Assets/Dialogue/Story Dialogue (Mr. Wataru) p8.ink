INCLUDE globals.ink

{AskedWataruAllQuestions == true: ->askedAllQuestions | -> main}

=== main ===
    It’s you…! You’re safe…! #speaker:Wataru
    
    *[What happened to your family?]
        ->askAboutFamily
    -> END  
    
    ===askAboutFamily===
    Mr. Wataru…? What happened…? Where is Ms. Maria and Sakura? #speaker:You
    
    My wife and my daughter… they took them away… #speaker:Wataru
    
    They…? Is it the same people who came into our house? They took my parents away too…!#speaker:You
    
    Those wretched Kempei Tai… #speaker:Wataru
    
    Why…? They’re Japanese too, aren’t they? Why would they do such a thing? We’ve never harmed them…#speaker:You
    
    I fear I don’t quite know either… something about rounding up suspected rebels… #speaker:Wataru
    
    Rebels? My parents would never…#speaker:You
    
    That’s what we said too, but the Kempei Tai refuse to believe us. Those wretched masked men have been around eyeing us too… #speaker:Wataru
    
    *[Who are the Masked Men?]
    ->askAboutMaskedMen
    
    -> END
    
    ===askAboutMaskedMen===
    They’re called Makapilis. Funny enough, they’re Filipino. Just like you. But they’re no different from the Kempei Tai. #speaker:Wataru
    
    They report anyone who is suspicious of sympathizing with the guerrilla forces.
    
    Hukbalahap, the Americans… as long as you’re with them, they’ll take you to Little Tokyo.
    
    *["Little Tokyo"?]
    ->askAboutLittleTokyo
    
    -> END
    
    ===askAboutLittleTokyo===
    Well, you all call it Mintal, but it isn't the same as it used to be. #speaker:Wataru
    
    It was a bustling center for business and agriculture till… well, all of this happened.
    
    Now Little Tokyo has become a military base. All the resources go to them now; food and all
    
    It’s where they’ve probably taken everyone they’ve rounded up from here, too.
    
    ~ AskedWataruAllQuestions = true

-> END

===askedAllQuestions===
Stay safe, little one. #speaker:Wataru

*[Mr. Wataru's Family]
    ->askAboutFamily
*[Masked Men]
    ->askAboutMaskedMen
*[Little Tokyo]
    ->askAboutLittleTokyo

-> END
