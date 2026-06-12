INCLUDE globals.ink

{ ShowedTheTag == true or ShowedTheLetter == true: -> talkedToSoldiers | -> main }

// -> main

===main===
Stop! #speaker:Soldier 1 #cgImage:0

You quickly stop in your tracks, terrified but trying to look normal. #speaker:Narrator#cgImage:0

 What are you doing here? This is Kempei Tai territory. #speaker:Soldier 1#cgImage:0
 
 Only those with important business are allowed to roam the streets here. #speaker:Soldier 1#cgImage:0
 
 * {HasLetterFromTanaka} [Show the note.] ->showNote
 * {HasHelpedHideo} [Show the tag.] ->showTag
 *[...]
    -> noResponse

-> END

===noResponse===
What? Got nothing, kid? Get lost! #speaker:Soldier 1#cgImage:0

-> END

===showNote==
Hm. #speaker:Soldier 1#cgImage:0

He takes the note from you. #speaker:Narrator#cgImage:0

Let’s see. Authorization to move through the military supply road.#speaker:Soldier 1#cgImage:0

Who do you work for?#cgImage:0

*[Furukawa Plantation Company]
    -> forFurukawa
*[Mr. Ernesto]
    -> forMrErnesto

-> END

===forFurukawa==
~ ShowedTheLetter = true
~ AnsweredCorrectly = true

Huh, sounds about right. We’ve been needing the supplies anyway. #speaker:Soldier 1#cgImage:0

Alright, you’re good to go. Don’t make any mess and leave when you’re done.#cgImage:0



-> END

===forMrErnesto===
~ ShowedTheLetter = true
~ AnsweredCorrectly = false
Huh, that “puppet” of a Filipino? #speaker:Soldier 1#cgImage:0

Hahaha, don’t try to fool me, little boy, he doesn’t have the right to be putting his name all over these letters when you can't even fund your own farms! #cgImage:0

He has a boisterous laugh, before he turns serious.#speaker:Narrator#cgImage:0

You’re not fooling us. #speaker:Soldier 1#cgImage:0

Hey!#cgImage:0

The soldier turns to his nearby comrade. #speaker:Narrator#cgImage:0

Help me take this kid to the damn cells!#speaker:Soldier 1#cgImage:0

- #moveToScene:Ending 1

-> END

===showTag===

Woah, hold on.#speaker:Soldier 2#cgImage:0

What?#speaker:Soldier 1#cgImage:0

I recognize the tag they’re wearing. It’s… one of ours.#speaker:Soldier 2#cgImage:0

What? No way? This kid? You’re dreaming. #speaker:Soldier 1#cgImage:0

He turns to you.#speaker:Narrator#cgImage:0

Give me that! #speaker:Soldier 1#cgImage:0

The soldier snatches the tag from you.#speaker:Narrator#cgImage:0

I’m telling you! It’s from our military!#speaker:Soldier 2#cgImage:0

Hmph. Guess the Hukbalahap did him in before he could get to us.#speaker:Soldier 1#cgImage:0

Those sloppy rebel Filipino guerilla fighters only know how to strike like mice. Only sloppy soldiers like Hideo can get outdone by them.#cgImage:0

Still, we need to discuss this with the General. We can only send him off respectfully.#speaker:Soldier 1#cgImage:0

Yeah. Hey, kid, we ain’t sure what to do with ya really, so sit tight for now and don’t move.#speaker:Soldier 2#cgImage:0

We’re gonna talk to the General about this.#cgImage:0

~ ShowedTheTag = true

-> END

===talkedToSoldiers===
Scram, kid! #cgImage:0#speaker:Soldier 1

-> END