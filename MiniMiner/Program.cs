using System.Text.Json;
using MiniMiner;

var testString =
    "{\"difficulty\": 8, \"block\": {\"nonce\": null, \"data\": []}}";

var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var minerObject = JsonSerializer.Deserialize<MiniMinerJson>(testString, options);

if (minerObject is null)
{
    Console.WriteLine("Json can not be deserialized.");
    return;
}

Console.WriteLine(minerObject.GetCorrectHashString());
Console.WriteLine($"Nonce: {minerObject.Block.Nonce}");