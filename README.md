# WarKata
 
A couple notes about the implementation. 

Maybe I'm just tired (it's 4 AM), but I couldn't figure out a good way to unit test the `Game` class since each turn is fairly encapsulated and it wouldn't make sense to make any of the methods public.  Instead, I added a bunch of events so I can capture when things occur and check it against the current players' decks.

Since games are non-deterministic due to the shuffled decks, I got lazy and just repeated my unit tests 100x each (added `[Repeat(100)]` attribute) to make sure they hit as many scenarios as they could. I could have modified the `Game` class to take in a static deck of cards, but... it's 4 AM.
