INCLUDE globals.ink

-> main

===main===
Hi, dear. Have you finished cleaning the house? #speaker:Mama

*[What are you making?]
    ->askedAboutDinner
*[Told papa it's dinner time]
    ->toldPapaItsDinner

===askedAboutDinner===
Dinner, of course. #speaker:Mama

Yeah, but you’re wrapping it in banana leaves! You always do that when you’re packing something! #speaker:You

Haha, how sharp. I’m simply packing breakfast ahead as well! #speaker:Mama

Oh! Okay! What’s for dinner, mama? #speaker:You

Fried tilapia and tortang talong, dear. #speaker:Mama

Okay! Yummy! #speaker:You

I would ask you to help me but you should be cleaning the dog house. So run along now. Tell your father dinner is almost ready. #speaker:Mama

Okay! #speaker:You
~TalkedToMom = true

-> END

===toldPapaItsDinner===
~TalkedToMom = true
{ ToldDadAboutDinner == true: 
Alright, dear. Set up the table. Let’s eat. #speaker:Mama
~ HouseZoneTasksFinished = true 
- else: Hmm, don't lie to me dear. Go on, tell papa it's dinner time. #speaker:Mama
} 



-> END

