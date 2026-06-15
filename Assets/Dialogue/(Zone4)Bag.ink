INCLUDE globals.ink

-> start

===start===
{PlayerPickedUpWater: 
    -> doneWithBag
- else: 
    -> main
    }
-> END

===main===
    The bag is awfully familiar, looking like the shoulder bag Kuya Lee always had on his person.#speaker:Narrator 
    It’s thrown on the floor haphazardly and left behind, its contents spilling out. 
    
    You notice a canister of water and what looks like a round of bullets. 

    *[Leave it alone]
        You leave the bag alone.#speaker:Narrator
        -> DONE
    * {ChoseToHelpHideo} [Take water]
        You take the canister of water.#speaker:Narrator
        ~PlayerPickedUpWater = true
        -> DONE
-> END

===doneWithBag===
    The bag has nothing left for you.#speaker:Narrator
    -> END