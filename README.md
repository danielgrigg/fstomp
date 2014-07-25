fstomp
======

An F# Stomp protocol parser and formatter.


# Why?

If you're working with the [Stomp](http://stomp.github.io) protocol and 
.NET you don't have few (good) options.  Basically you've got Apache 
ActiveMQ, which is fine if you're of the 'big enterprise frameworks ftw' 
variety but if not, you've got fstomp!


# Getting started

Just open FStomp/FStomp.sln in Visual Studio 2013 and build the solution.  
Alternatively build via msbuild.  I haven't tested xbuild to see 
if that works yet.

FStomp produces a single FStomp.dll that you can incorporate easily 
in the usual way.  

Nuget builds will be available very soon.

If you don't have Visual Studio 2013, checkout the great 
[FSharp.org](http://fsharp.org/use/windows/) which lists some options.


## Running Tests

The tests are written in [fsunit](https://github.com/fsharp/FsUnit), 
(2 days before I discovered the cool 
[unquote project](https://code.google.com/p/unquote/). To run them, 
just run them as you'd run normal NUnit tests.


## Future work

+ Extend fstomp beyond parsing/formatting and into a general purpose
client library or even better a type provider. Because 
types are your friends.

+ Port to Mono

Copyright(C) 2014, Daniel Casimir Grigg

[Daniel's Home](http://danielgrigg.github.io)
