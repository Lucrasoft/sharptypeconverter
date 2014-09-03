sharptypeconverter
==================

Convert C# source to Typescript.

Sharptypeconverter is a project that tries to convert c# code to typescript code by parsing the c# to a syntax tree and emitting it as typescript

For the parsing the parser from NRefactory is used.  https://github.com/icsharpcode/NRefactory 
After that the parsed tree is read and a typescript file is build from here.

Check out the current functionality on the github page: http://lucrasoft.github.io/sharptypeconverter/ 

Current Status: 

 - Files can be converted individually.
 - Basic syntax and statements are parsed correctly.
 - Method /constructor overloading is handled through helper function (only works for different parameter count)

Todo:

 - Uploading of multiple files, so converter can resolve internal dependencies correctly.
 - Same for console app.
 - Fix code crash at files without a namespace
 - resolve external dependancies and create typescript definition files to include with the output so the output code can build correctly
 - Try to build the output as initial validation.
 - Improve method/constructor overloading.
 - Test for bugs and for syntax types not yet implemented. 

github page UI:

 - Code highlighting
 - Multiple sample codes.
 - Better texbox formatting
 - 
Think of more Todo points.







