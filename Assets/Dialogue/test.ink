INCLUDE globals.ink

{ NameOfChoice == "": -> main | -> already_chose }

-> main

=== main ===
Hello, world! #speaker:TestNPC

This is line 2.
*[Seriously?]
    -> choice1
*[Nice.]
    -> choice2

=== choice1 ===
~ NameOfChoice = "Choice 1"
Yep.

Choice 1 picked!. #speaker:MC
-> END

=== choice2 ===
~ NameOfChoice = "Choice 2"
Aw.

Choice 2 picked!. #speaker:MC
-> END

=== already_chose ===
You've already chosen this choice: {NameOfChoice}
-> END