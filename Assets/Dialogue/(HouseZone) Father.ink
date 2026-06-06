INCLUDE globals.ink

{CheckedOnDogHouse == true: ->checkedOnDogHouse | ->main}

==main==
Hey, how was school, kid? #speaker:Papa

It was okay. My classmate told me she’ll teach me how to make little paper stars! #speaker:You

Hmm? That’s awesome. #speaker:Papa

Say, kid, you can fit in the dog house, right? I think I dropped something in there. #speaker:Papa

I can get it for you! #speaker:You

Good kid. #speaker:Papa

~ReceivedTaskFromDad = true

-> END

===checkedOnDogHouse===
Papa, there was nothing in there! #speaker:You

Hmmm? Guess your dad’s just getting a bit more forgettable these days. #speaker:Papa

Oh, right, papa! Dinner is almost ready! #speaker:You

Guess that means we should head inside. You go on ahead. I’ll catch up later, alright? #speaker:Papa

Okay! #speaker:You

~ToldDadAboutDinner = true

-> END

