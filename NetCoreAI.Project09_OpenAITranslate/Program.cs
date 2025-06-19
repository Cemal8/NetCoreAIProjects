using Newtonsoft.Json;
using System.Net;
using System.Text;

class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Lütfen çevirmmek istediğiniz metni giriniz:");
        string inputText=Console.ReadLine();

        string apiKey = "";
        string translatedText = await TranslateTextToEnglish(inputText, apiKey);

        if (!string.IsNullOrEmpty(translatedText))
        {
            Console.Write($"Çeviri (İngilizce): {translatedText}");
        }
        else
        {
            Console.Write("Beklenmeyen bir hata oluştu.");
        }
    }
    private static async Task<string> TranslateTextToEnglish(string text,string apiKey)
    {
    
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new {role="system",content="You are a helpful translator."},
                    new {role="user",content=$"Please translate this text to English: {text}"}
                }
            };

            string jsonBody=JsonConvert.SerializeObject(requestBody);
            var content= new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API Response:");
                Console.WriteLine(responseString);

                dynamic responseObject=JsonConvert.DeserializeObject(responseString);
                string translation = responseObject.choices[0].message.content;

                return translation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                return null;
            }
        }
    }
}
