# Goal
I want to apply some of the data structures and algorithms I've recently learned about from the Stanford course.  In particular, I want to implement a graph and use it in a meaningful way.  In order to do so, I need some interesting data -- the more the better.  Since the "Bacon Number" is a popular thing and I have a good source of parseable movie data out there, I've decided to create a graph where the nodes are actors/actresses and the edges represent having been cast together in a movie.  Using Dijkstra's shortest path algorithm, it should be possible to easily determine the "bacon number" between any two actors or actresses.  

# Step One: Obtaining a good source of data
I found a service called [OMDb](http://www.omdbapi.com/) that exposes a REST API for movie data.  It uses IMDb as its source, and allows donors to download a monthly database dump containing over a million movie entries.  The dump has tab delimited rows so parsing is trivial.  If you'd like a copy of the data I used for this project, go to the aforementioned OMBd link and make a donation!

# Step Two: A touch of Preprocessing
The first step to doing anything with the data is to get it in a format that's easy to work with.  To accomplish I created a simple class to represent a movie entry containing a handful of properties -- Id, Title, Year, and Cast.  I parse each line and determine if it has enough data to be useful.  Some movie entries didn't list a cast or a year, so those were discarded. 
