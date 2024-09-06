using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace StoryCreator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string StoryContent { get; set; }
        public string Title { get; set; }

        [BindProperty]
        public StoryDetails Details { get; set; } = new StoryDetails();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnPost()
        {
            string endpoint = "https://aplix-openai-dyplast.openai.azure.com/";
            string key = "45234f12cfb04eeab9338669a7151bb2";
            string deploymentName = "Aplix-gpt-4o";


            OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

            string prompt = $"Escríbame una historia llamada {Details.Title} en un tono {Details.Tone} sobre un " +
                $"{Details.Animal} llamado {Details.Name} que vive en un lugar llamado {Details.Environment}.";

            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.System, "Usted es un asistente de IA que ayuda con lo solicitado."),
                    new ChatMessage(ChatRole.User, prompt)
                }
            };

            // Send request to Azure OpenAI model
            ChatCompletions chatCompletionsResponse = client.GetChatCompletions(deploymentName, chatCompletionsOptions);

            StoryContent = chatCompletionsResponse.Choices[0].Message.Content;
        }
    }

    public class StoryDetails
    {
        public string Title { get; set; }
        public string Tone { get; set; }
        public string Name { get; set; }
        public string Animal { get; set; }
        public string Environment { get; set; }

    }
}