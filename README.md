sharptypeconverter
==================

Convert C# source to Typescript.

Sharptypeconverter is a project that tries to convert c# code to typescript code by parsing the c# to a syntax tree and emitting it as typescript

For the parsing the parser from NRefactory is used.  https://github.com/icsharpcode/NRefactory 
After that the parsed tree is read and a typescript file is build from here.

Current Status: 

Files can be converted individually.
Basic syntax and statements are parsed correctly.
method /constructor overloading is handled through helper function (only works for different parameter count)

Todo:

Uploading of multiple files, so converter can resolve internal dependencies correctly.
Improve method/constructor overloading.
Test for bugs and for syntax types not yet implemented. 
Think of more Todo points.






