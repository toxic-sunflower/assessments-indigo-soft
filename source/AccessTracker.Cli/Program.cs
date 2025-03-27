// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;

var url = args[0];
var count = int.Parse(args[1]);
var random = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));

Console.Clear();
Console.WriteLine($"Using API URL: {url}");
Console.WriteLine($"Generating {count}: events");

using var client = new HttpClient();

client.BaseAddress = new Uri(url);

for (var i = 0; i < count; i++)
{
    var (userId, ipAddress) = GenerateRecord(random);
    await SendRecordAsync(client, url!, userId, ipAddress);
}

Console.WriteLine("Done.");

return;


static (int, string) GenerateRecord(Random random)
{
    var userId = random.Next();
    var ipAddress = $"192.168.{random.Next(1, 255)}.{random.Next(1, 255)}";
    
    return (userId, ipAddress);
}

static async Task SendRecordAsync(
    HttpClient client,
    string url,
    long userId,
    string ipAddress)
{
    var json = JsonSerializer.Serialize(new {userId, ipAddress});

    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PostAsync("/api/events", content);

    Console.WriteLine(response.IsSuccessStatusCode
        ? $"Sent: {json}"
        : $"Error ({(int)response.StatusCode}): {response.ReasonPhrase}");
}