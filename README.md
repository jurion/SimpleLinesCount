# Async SimpleLines count

**Applies to Visual Studio 2019 and newer**

This is a simple extention for line counting in a vlock of c# code

Clone the repo to test out in Visual Studio 2019 yourself.

![Quick Info](art/QuickInfo.png)

This project is heavily based on Microsoft SDK Examples here : 
https://github.com/microsoft/VSSDK-Extensibility-Samples/tree/master/AsyncQuickInfo

**Ignored lines**

this extension doesn’t count the following lines : 
* Empty line
* Commented (//) line
* Line containing only “{“ or “}” (or “{//” or  “}//”)


Unfortunately for now multiline comments are counted. (/* */)


**TODOs :** 

Add a page in the options.

Use codeLens instead of quickInfo

Use Roslyn to parse actual code tree
