INCLUDE globals.ink

{CheckHouseFirst == true: -> NotHere | ->AlreadyChecked}

=== AlreadyChecked ===
No one is here except me... #speaker:You

I have to find them.

-> END

=== NotHere ===
They're not here... #speaker:You

It was real, they were really taken... but why?

My parents aren't bad.

I need to find them...

I can't live with just me here.

I'm going to Mintal.

~LeaveHouse = true
~CheckHouseFirst = false
~GoToMintal = true

-> END

