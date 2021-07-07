# BattleShip

* Visual Studio solution.
* No front end or UI.
* Back end logic for a user.
* C# code for the game: https://en.wikipedia.org/wiki/Battleship_(game)
* Spent lot of time crafting the code.
* Implemented as an Event Sourcing (CQRS) pattern as I feel it's good approach in this case.
* Since lot time spent in coding, test cases are written around the core logic of loading ship on board.
* I feel there are lot of improvements can be done. There might be un-tested code having bugs.
* Memory just stores everything, no restrictions on how many games to store. Since its in memory when redeployed all the data will be lost.
* Number of ships alloted to game is set to one as it was easy to test, can be modified.
