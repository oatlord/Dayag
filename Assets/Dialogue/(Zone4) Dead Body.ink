INCLUDE globals.ink

->start

=== start ===
{PlayerPickedUpBandages: 
    -> doneWithBody
-else:
    ->main
}

=== main ===
Before you is a body, face down to the ground. It doesn't seem as though they're from the Kempei Tai. #speaker:Narrator

You can't confirm though, since you’re too small and weak to turn him around. 

He’s holding a gun in his hand and a pool of dried blood has bloomed under him.

    *[Leave the body alone]
        You leave the body alone. #speaker:Narrator
        -> DONE
    * {ChoseToHelpHideo} [Take a piece of cloth]
            You take a piece of cloth from the body, carefully tearing it off. It's dusty, but it will do. #speaker:Narrator
            ~PlayerPickedUpBandages = true
            -> DONE
 
 ===doneWithBody===
 You gaze at the body again, cold in its own blood. Some fight must have happened here. #speaker:Narrator
 
 -> END