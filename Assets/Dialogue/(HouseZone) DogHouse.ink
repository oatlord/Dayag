INCLUDE globals.ink

{ ReceivedTaskFromDad == true: -> main | -> hasNotReceivedTaskFromDad}
{ CheckedOnDogHouse == true: ->alreadyCheckedDogHouse | ->main}

==main==
You enter the dog house momentarily to find something for your father. #speaker:Narrator

However, nothing is inside the dog house. #speaker:Narrator

It is quite snug, though. This dog house fits you perfectly. #speaker:Narrator

~CheckedOnDogHouse = true

-> END

===hasNotReceivedTaskFromDad===
It's a nice dog house. Your father built it some time ago. #speaker:Narrator

-> END

==alreadyCheckedDogHouse===
You've already checked before and found nothing. #speaker:Narrator

-> END

