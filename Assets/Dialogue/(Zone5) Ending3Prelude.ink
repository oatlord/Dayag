INCLUDE globals.ink

-> main

===main===
The soldiers began discussing with the General, and you can only wait patiently and watch. #speaker:Narrator#cgImage:1

You observe multiple makeshift jail cells, ducking your head as the cries and wales of hungry and mistreated Filipinos haunt you. #speaker:Narrator#cgImage:1

The soldier brings the tag you obtained from Hideo to the General.#speaker:Narrator#cgImage:1

General, that frail Filipino boy over by the entrance… He has a hold of this tag.#speaker:Soldier 1#cgImage:1

Hideo... Tsk. Of course.#speaker:General#cgImage:2

What should we do with the kid, General?#speaker:Soldier 1#cgImage:2

We can put him in jail like the rest of ‘em.#speaker:Soldier 2#cgImage:2

The two soldiers share a hearty laugh.#speaker:Narrator#cgImage:2

The General raises a hand to silence them.#speaker:Narrator#cgImage:2

What you should do is interrogate him further. Find where Hideo was last seen and send soldiers there after to retrieve him, copy?#speaker:General#cgImage:2

Yes General!#speaker:Soldier 1 & 2#cgImage:2

The two soldiers approach you again.#speaker:Narrator#cgImage:3

Wow, aren’t you a good kid, sat still when ordered to.#speaker:Soldier 2#cgImage:3

We are gonna ask you more questions, okay? Let’s start with this. #speaker:Soldier 1#cgImage:3

The soldier brings out the tag.#speaker:Narrator#cgImage:0

Where did you find this?#speaker:Soldier 1#cgImage:0

*[Ruined Plantation] -> ruinedPlantation
*[I Don’t Know] -> iDontKnow

-> END

===ruinedPlantation===
Did he just say the plantation?#speaker:Soldier 1#cgImage:0

Most probably. Did… Did Hideo give this to you?#speaker:Soldier 2#cgImage:0

*[Yes]
	Hideo did, huh? Then he’s-#speaker:Soldier 2#cgImage:0

	Probably already dead.#speaker:Soldier 1#cgImage:0

    Hey now, we can’t just rule him out. He’s probably there in need of help. #speaker:Soldier 2#cgImage:0

	Or, he forcefully took this tag from an already rotting Hideo.#speaker:Soldier 1#cgImage:0

    The soldier kneels down to your height and hangs the tag right in front of you. #speaker:Narrator#cgImage:0
	
	Right, that’s what you did, right? You killed Hideo, and Hideo is dead because of you? #speaker:Soldier 1#cgImage:0

->answeredQuestion

===iDontKnow===
Woah, calm down. That’s just a kid!#speaker:Soldier 2#cgImage:0

We’ve seen what those nasty Filipino rebellion fighters can do. Who knows, they might even be low enough to send a kid with a bomb strapped to his back to blow us all up? #speaker:Soldier#cgImage:0

Heh, wouldn’t put it above them.#speaker:Soldier 1#cgImage:0

Well, either way, he looks terrified. Weak and terrified. I doubt he did something to Hideo in that sorry state.#speaker:Soldier 2#cgImage:0

The soldiers continue arguing while you sit silently between them.#speaker:Narrator#cgImage:0

->answeredQuestion

===answeredQuestion===
No matter. This Filipino could’ve killed Hideo or left him for dead. He’s no saint. He knew what he had to do to survive.#speaker:Soldier 1#cgImage:0

He was hurt… in the plantation. I gave him water. I wrapped his shoulder.#speaker:You#cgImage:0

Hah! Listen to that. A Filipino helping one of us?#speaker:Soldier 1#cgImage:0

Soldier 2 studies you carefully. Behind them, the General finally speaks.#speaker:Narrator#cgImage:0

Enough.#speaker:General#cgImage:1

The soldiers immediately straighten.#speaker:General#cgImage:1

The tag belongs to Hideo of the 4th Division. We’ll send a patrol to the ruined plantation ASAP.#speaker:General#cgImage:1

Yes, General.#speaker:Soldier 2#cgImage:1

And the boy?#speaker:Soldier 1#cgImage:1

The General pauses for a moment.#speaker:Narrator#cgImage:1

Hold him here until we know the truth.#speaker:General#cgImage:1

-#moveToScene:Ending 3

-> END