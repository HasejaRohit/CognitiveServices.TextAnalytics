using Azure;
using System;
using System.Globalization;
using Azure.AI.TextAnalytics;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Net.Http;

namespace TextAnalyticsQuickstart
{
    class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("Add the key to this part from Azure  portal");
        private static readonly Uri endpoint = new Uri("add the endpoint details for Text Analytics from Azure portal");
        
        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);

            string inputText = @"The Altroz DCA works absolutely fine as a daily driver. The 6-speed dual clutch automatic is refined, it goes through the gears with a level of smoothness that we haven’t seen in any Tata vehicle yet and it brings out the best of that naturally aspirated 1.2-litre engine but and there is a big but. Things get a bit demotivating as soon as you start pushing this drivetrain. For instance, the gearshifts at high RPMS are nowhere as seamless as they are when you are just cruising along. And because this three-cylinder engine is not a free-revving engine, you can feel it holding back the dual clutch automatic as it takes its own sweet time to spin up to high RPMs and vice versa. ";
           

           SentimentAnalysisExample(client,inputText);
           LanguageDetectionExample(client,inputText);
           EntityRecognitionExample(client,inputText);
           EntityLinkingExample(client,inputText);
           KeyPhraseExtractionExample(client,inputText);

            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }



        

        static void SentimentAnalysisExample(TextAnalyticsClient client, string input)
        {
            string inputText = input;
            DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText);
            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");

            

            foreach (var sentence in documentSentiment.Sentences)
            {
                Console.WriteLine($"\tText: \"{sentence.Text}\"");
                Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                Console.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");
            }
        }

        static void LanguageDetectionExample(TextAnalyticsClient client, string input)
        {
           
            DetectedLanguage detectedLanguage = client.DetectLanguage(input);
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }

        static void EntityRecognitionExample(TextAnalyticsClient client, string input)
        {
            var response = client.RecognizeEntities(input);
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
                Console.WriteLine($"\t\tScore: {entity.ConfidenceScore:F2}\n");
            }
        }

        static void EntityLinkingExample(TextAnalyticsClient client, string input)
        {
            var response = client.RecognizeLinkedEntities(input);
            Console.WriteLine("Linked Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tName: {entity.Name},\tID: {entity.DataSourceEntityId},\tURL: {entity.Url}\tData Source: {entity.DataSource}");
                Console.WriteLine("\tMatches:");
                foreach (var match in entity.Matches)
                {
                    Console.WriteLine($"\t\tText: {match.Text}");
                    Console.WriteLine($"\t\tScore: {match.ConfidenceScore:F2}\n");
                }
            }
        }

        static void KeyPhraseExtractionExample(TextAnalyticsClient client,string input)
        {
            var response = client.ExtractKeyPhrases(input);

            // Printing key phrases
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }
    }
}