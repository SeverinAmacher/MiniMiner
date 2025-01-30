using System.Net.Http.Headers;
using System.Text.Json;
using MiniMiner;

var httpClient = new HttpClient();
var response = await httpClient.GetAsync("https://hackattic.com/challenges/mini_miner/problem?access_token=80772ea280f4e067&playground=1");
var jsonString = await response.Content.ReadAsStringAsync();

var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var minerObject = JsonSerializer.Deserialize<MiniMinerJson>(jsonString, options);

if (minerObject is null)
{
    Console.WriteLine("Json can not be deserialized.");
    return;
}

Console.WriteLine(minerObject.Difficulty);
Console.WriteLine(minerObject.GetCorrectHashString());
Console.WriteLine($"Nonce: {minerObject.Block.Nonce}");
var resultJsonString = JsonSerializer.Serialize(new { data = minerObject.Block.Data, nonce = minerObject.Block.Nonce }, options);
var httpContent = new StringContent(resultJsonString, MediaTypeHeaderValue.Parse("application/json"));
var result = await httpClient.PostAsync("https://hackattic.com/challenges/mini_miner/solve?access_token=80772ea280f4e067&playground=1", httpContent);
Console.WriteLine($"Status: {result.StatusCode}, Message: {await result.Content.ReadAsStringAsync()}");