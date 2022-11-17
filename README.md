**Speech recognition functional demo application for Windows designed for usage inside VR virtual desktop.**

**Note**: the application can also be used outside VR virtual desktop, however it is built with VR in mind.

**IMPORTANT NOTE: If you share this application and you want to include your Google application credentials which are found in your user profile folder, the people who gain access to your share can incur charges on your Google account!**

![SpeechRecognizer usage demo](https://raw.githubusercontent.com/dan-mirescu/Static/main/SpeechRecognizer/SpeechRecognizer_usage_example.gif)

# Why?
While speech recognition already exists in Windows, it did not exist for my native language. Additionally, I liked the quality of speech recognition that the Speech-to-Text service from Google offers. Therefore, I decided to create a desktop application that will help me to type text more easily while using VR virtual desktop.

# Limitations (for now)
 - Only two languages are available: Romanian and English (US). New languages can be easily added on request or by pulling the code yourself and adding them.
- Visual design is not the most appealing but I aimed for functionality / for an MVP

# Important note
The speech recognition service is offered by Google and it is not 100% free. You need to configure the Speech-to-Text service in Google Cloud with a Google account and use the credentials generated from there with this application. Generally, Google offers a certain amount of free minutes of service per month and low prices per minute after the free minutes elapse. See the pricing details here: https://cloud.google.com/speech-to-text/pricing

# How to use the application?
Requirements: Google account and Google cloud project with billing enabled

Before opening the application, you need to configure the Google Speech-To-Text service. This will be a one-time operation as your service credentials will be saved in your user profile folder.

- Go to https://cloud.google.com/speech-to-text/docs/before-you-begin and follow the steps until “Set your authentication environment variable”. At this point you should have a JSON file containing the necessary service credentials.
- Open the application and browse to the JSON file or drag and drop it on top of the window. Then, click on "Use provided file".

After you have set up your service credentials you can open the application.
When the application opens, a red “microphone” button is displayed on top of all the windows.

![SpeechRecognizer button](https://raw.githubusercontent.com/dan-mirescu/Static/main/SpeechRecognizer/SpeechRecognizer_button.png)

Right click the button for settings:

![SpeechRecognizer settings](https://raw.githubusercontent.com/dan-mirescu/Static/main/SpeechRecognizer/SpeechRecognizer_settings.png)

- **ro-RO, en-US**: the language to be used for the speech recognition. Note: the application is in a functional demo stage and these are the available languages. New languages can be easily added on request or by pulling the code yourself and adding them. 
- **Auto send keys**: whether the recognized text to be auto-typed from the keyboard when the speech recognition ends
- **Auto copy to clipb**: whether the recognized text should be auto-placed in the clipboard when the speech recognition ends

The settings are applied instantly when changing them.

## How to use speech recognition
The usual flow:

1.	Select any kind of text input area in Windows
2.	Click the application “microphone” button
3.	Wait until the message “Speak now” is displayed
4.	Speak
5.	When the application detects that you stopped, it will show you the recognized text and auto-fill it in the select text input area. Auto-fill happens if “Auto send keys” is checked in the settings.

# How to build the application
Requirements: Visual Studio, Google account and Google cloud project with billing enabled

1. Clone the repository on your local machine
2. Open SpeechRecognizer.sln in Visual Studio
3. Build the solution. NuGet packages should be restored as part of this operation
4. Obtain Google application credentials for the Speech-to-Text service. Please refer to [the beginning of "How to use the application" section](#how-to-use-the-application) for details on how to obtain the credentials.
5. Run the application.

Note: I sometimes had an issue with a missing grpc_csharp_ext.x86.dll when testing a build from scratch. It seems that the Grpc.Core package sometimes does not copy the native library to the application build folder. This happened when the application got stuck in "wait..." mode.
To check if this is the case:
1. Start debugging the application in Visual Studio
2. Inside Exception Settings in Visual Studio, put a checkmark near "Common Language Runtime Exceptions"
3. Start voice recognition
4. If there is an issue with the missing dll, an exception is thrown at this moment.

Possible solutions: Reference the dll explicitly from the SpeechRecognizer project or add the dll to the project and set it to be copied to the build folder.
