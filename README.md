# SimpleLoggerForDotNet
If you just want to write a simple log file in .net/C# projects you are in the right place. Easy to set up and use

How to use

Methods are simply called statically (``` Log.<Methodname>``` ), no instance required

(1) Download the binary package (or compile yourself if you want)

(2) Add a reference to the DLL

(3) Call ``` Log.Init```  somewhere on program start up

(3.1) At minimum you provide a file path where to save the log file as string argument in ``` Log.Init``` 

(3.2) optional: if you want to use file size limiting feature, pass a maximum file size (bytes) in ``` Log.Init ```  and call ``` Log.Maintenance```  e.g. on start up or on program exit

(3.3) optional: if you want to use logging in multiple threads, pass true as "synchronized" parameter in ``` Log.Init ``` 

(3.4) optional: Set a log pattern. By default ``` {datetime}\t{type}\t{message}```  is used. You can use these variables (with {braces} to set your own pattern by calling Log.SetPattern

(4)  log something by calling on of these

```     
        Log.Info("string")         
        Log.Error("string")         
        Log.Error(<instance of System.Exception>)         
        Log.Critical("string") 
        Log.Critical(<instance of System.Exception>) 
        Log.Warning("string") 
        Log.Debug("string") 
 ```       
