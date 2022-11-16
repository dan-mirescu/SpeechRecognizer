using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechRecognizer
{
    public delegate void SpeechResultData(List<string> speechResultData);
    public delegate void Notify();

    class StreamingMicSpeechRecognizer
    {
        public event SpeechResultData IncomingSpeechResultData;
        public event Notify ReadyToSpeak;
        public event SpeechResultData RecognitionEnded;

        protected virtual void OnIncomingSpeechResultData(List<string> speechResultData)
        {
            IncomingSpeechResultData?.Invoke(speechResultData);
        }

        protected virtual void OnReadyToSpeak()
        {
            ReadyToSpeak?.Invoke();
        }

        protected virtual void OnRecognitionEnded(List<string> speechResultData)
        {
            RecognitionEnded?.Invoke(speechResultData);
        }

        public async Task<object> StreamingMicRecognizeAsync(string languageCode, CancellationToken cancellationToken)
        {
            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();
            // Write the initial request with the config.
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding =
                            RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = languageCode,
                        },
                        InterimResults = true,
                        SingleUtterance = true
                    }
                });

            var recognitionEnded = false;

            // Print responses as they arrive.
            var printResponses = Task.Run(async () =>
            {
                var lastBestTranscripts = new List<string>();
                var responseStream = streamingCall.GetResponseStream();
                while (await responseStream.MoveNextAsync())
                {
                    StreamingRecognizeResponse response = responseStream.Current;

                    if (response.SpeechEventType == StreamingRecognizeResponse.Types.SpeechEventType.EndOfSingleUtterance)
                    {
                        recognitionEnded = true;
                        break;
                    }

                    var bestTranscripts = new List<string>();

                    foreach (StreamingRecognitionResult result in response.Results)
                    {
                        var bestAlternative = result.Alternatives.OrderByDescending(a => a.Confidence).First();
                        bestTranscripts.Add(bestAlternative.Transcript);

                        //Console.WriteLine(bestAlternative.Transcript);


                        //foreach (SpeechRecognitionAlternative alternative in result.Alternatives)
                        //{
                        //    Console.WriteLine(alternative.Transcript);
                        //}
                    }

                    lastBestTranscripts = bestTranscripts;
                    OnIncomingSpeechResultData(bestTranscripts);
                }

                recognitionEnded = true;
                return lastBestTranscripts;
            });
            // Read from the microphone and stream to API.
            object writeLock = new object();
            bool writeMore = true;
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
            waveIn.DataAvailable +=
                (object sender, NAudio.Wave.WaveInEventArgs args) =>
                {
                    lock (writeLock)
                    {
                        if (!writeMore)
                        {
                            return;
                        }

                        streamingCall.WriteAsync(
                            new StreamingRecognizeRequest()
                            {
                                AudioContent = Google.Protobuf.ByteString
                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                            }).Wait();
                    }
                };

            if(!cancellationToken.IsCancellationRequested)
            {
                waveIn.StartRecording();
                OnReadyToSpeak();
                Console.WriteLine("Speak now.");
                //await Task.Delay(TimeSpan.FromSeconds(seconds));
                // Stop recording and shut down.

                while (!recognitionEnded && !cancellationToken.IsCancellationRequested) {; }

                waveIn.StopRecording();
                lock (writeLock)
                {
                    writeMore = false;
                }
            }

            await streamingCall.WriteCompleteAsync();
            var speechResultData = await printResponses;
            OnRecognitionEnded(speechResultData);
            return 0;
        }

    }
}
