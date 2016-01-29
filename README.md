# SharpImgur
C# Imgur SDK

##What is this?
This is C# wrapper around Imgur APIs. This SDK was developed with lurking in mind. So, currently it works only for non authenticated calls. It's easily possible to implement rest of the APIs though. That is on the project roadmap. Pull requests welcome.  
The SDK uses commercial Mashape APIs for Imgur. So, it's okay to use this SDK for commercial applications.

##Is it being used anywhere?
There's this Imgur app called [Monocle Giraffe](https://github.com/akshay2000/MonocleGiraffe/tree/master). It uses this SDK.

##How do I use it?
Clone the repo and open the project in Visual Studio. It will complain that file `Secrets.json` does not exist. Create that file with following content.

    {
      "Client_Id": "your-client-id",
      "Mashape_Key": "your-mashape-key"
    }
    
You can get those credentials from Imgur and Mashape respectively.
