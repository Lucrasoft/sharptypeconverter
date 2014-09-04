sharptypeconverter
==================

Convert C# source to Typescript.

Sharptypeconverter is a project that tries to convert c# code to typescript code by parsing the c# to a syntax tree and emitting it as typescript

For the parsing the parser from NRefactory is used.  https://github.com/icsharpcode/NRefactory 
After that the parsed tree is read and a typescript file is build from here.

Check out the current functionality on the github page: http://lucrasoft.github.io/sharptypeconverter/ 

Current Status: 

 - Files can be converted individually or by batch with correct internal dependancies
 - Basic syntax and statements are parsed correctly.
 - Method /constructor overloading is handled through helper function (only works for different parameter count)

Todo:

 - Fix code crash at files without a namespace
 - resolve external dependancies and create typescript definition files to include with the output so the output code can build correctly
 - Try to build the output as initial validation.
 - Improve method/constructor overloading.
 - Test for bugs and for syntax types not yet implemented. 
 - Add helper class to output to keep track of type information during runtime

github page UI:

 - Code highlighting
 - Multiple sample codes.
 - Better texbox formatting
 - upload of batch of files to be converted and presented as dowload afterwards 
 
Think of more Todo points.







