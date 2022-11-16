**Speech recognition functional demo application for Windows designed for usage inside VR virtual desktop.**

**Note**: the application can also be used outside VR virtual desktop, however it is built with VR in mind.

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

Before opening the application, you need to configure the Google Speech-To-Text service. This will be a one-time operation as your service credentials will be saved in the application’s folder.

- Go to https://cloud.google.com/speech-to-text/docs/before-you-begin and follow the steps until “Set your authentication environment variable”. At this point you should have a JSON file containing the necessary service credentials.
- Place the JSON file in the SpeechRecognizer application folder, with the `google-application-credentials.json` filename.

After you have saved your service credentials you can open the application.
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
2. Obtain Google application credentials for the Speech-to-Text service. Please refer to [the beginning of "How to use the application" section](#how-to-use-the-application) for details on how to obtain the credentials.
3. Save the credentials inside the SpeechRecognizer Visual Studio project folder (the folder containing the SpeechRecognizer.csproj file) in a file named `google-application-credentials.json`
4. Open SpeechRecognizer.sln in Visual Studio
5. Set SpeechRecognizer as startup project
6. Build the solution. NuGet packages should be restored as part of this operation
7. Now the application is ready to run.
