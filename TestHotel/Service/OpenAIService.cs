namespace BhoomiGlobalAPI.Service
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is missing or invalid.");
            }
        }


        public async Task<string> SendRequestAsync(string prompt)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                new { role = "user", content = prompt }
            },
                    max_tokens = 100
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("https://api.openai.com/v1/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    var errorCode = response.StatusCode.ToString();

                    // You can check for quota error (code: 429 or insufficient_quota) and handle accordingly
                    if (errorCode == "TooManyRequests" || errorDetails.Contains("insufficient_quota"))
                    {
                        throw new InvalidOperationException("You have exceeded your OpenAI API quota. Please check your plan and billing details.");
                    }

                    throw new HttpRequestException($"OpenAI API error: {errorCode} - {errorDetails}");
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error in OpenAIService: {ex.Message}", ex);
            }
        }


    }

}
