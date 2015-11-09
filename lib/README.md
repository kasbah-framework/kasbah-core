# Submodules

Ideally we wouldn't need to use submodules as NuGet packages would 
be the preferred way of referencing external libraries.  However 
there may be instances where this won't work.  Exceptions should 
be listed in this file with a short description as to why.

## Exceptions

 * *StackExchange.Redis* - the library as compiled and distributed
   on NuGet does function correctly on Mono.  There were only
   only very minor tweaks required to make it work.  Until it is
   fixed on the NuGet version, a specific copy will need to be
   used.

