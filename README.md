Steam Screenshot Manager
========================

With Steam's new "Save an uncompressed copy outside of Steam" option for screenshots, Steam simply 
dumps them all in a single folder. *Steam Screenshot Manager* helps manage external screenshots by 
automatically moving screenshots into a subfolder with the name of the game.

Current functionality
---------------------

- Retrieve the name of game based on it's App ID;
- Move screenshots based on its filename into a subfolder;
- User interface to allow naming non-Steam games.

Missing functionality
---------------------

- A user interface to show progress;
- Cache game ID and names in a file (currently, it is only cached in memory);
- Automatically detect the external screenshots folder from Steam's settings;
- Continuously monitor the folder for new screenshots and move them immediately.
