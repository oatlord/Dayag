INCLUDE globals.ink

-> main

===main===
Stop! #speaker:Soldier 1

You quickly stop in your tracks, terrified but trying to look normal. #speaker:Narrator

 What are you doing here? This is Kempei Tai territory. #speaker:Soldier 1
 
 Only those with important business are allowed to roam the streets here.
 
 {HasLetterFromTanaka == true: *[Show the note.] ->showNote}
 {HasHelpedHideo == true: *[Show the tag.] ->showTag}
 *[...]
    -> noResponse

-> END

===noResponse===
What? Got nothing, kid? Get lost! #speaker:Soldier 1

-> END

===showNote==
Hm. #speaker:Soldier 1

He takes the note from you. 

Let’s see. Authorization to move through the military supply road.

Who do you work for?

*[Furukawa Plantation Company]
    -> forFurukawa
*[Mr. Ernesto]
    -> forMrErnesto

-> END

===forFurukawa==
Huh, sounds about right. We’ve been needing the supplies anyway. 

Alright, you’re good to go. Don’t make any mess and leave when you’re done.

-> END

===forMrErnesto===
Huh, that “puppet” of a Filipino? 

Hahaha, don’t try to fool me, little boy, he doesn’t have the right to be putting his name all over these letters when you can't even fund your own farms! 

He has a boisterous laugh, before he turns serious.
You’re not fooling us. 

Hey! (turns to a nearby soldier) Help me take this kid to the damn cells!

-> END

===showTag===
Woah, hold on.

What?

I recognize the tag they’re wearing. It’s… one of ours.

What? No way? This kid? You’re dreaming. (turns to the kid) Give me that! (snatches the tag)

I’m telling you! It’s from our military!

Hmph. Guess the Hukbalahap did him in before he could get to us. 

Those sloppy rebel Filipino guerilla fighters only know how to strike like mice. Only sloppy soldiers like Hideo can get outdone by them.

Still, we need to discuss this with the General. We can only send him off respectfully.

Yeah. Hey, kid, we ain’t sure what to do with ya really, so sit tight for now and don’t move.

We’re gonna talk to the General about this.

-> END