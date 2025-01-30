using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MiniMiner;

public class MiniMinerJson
{
    public int Difficulty { get; set; }
    public required BlockDto Block { get; set; }

    public string GetCorrectHashString()
    {
        Block.Nonce = 0;
        var bytes = Block.GetHash();
        while (!CheckDifficultyWithResult(Difficulty, bytes))
        {
            Block.Nonce += 1;
            bytes = Block.GetHash();
        }
        
        var sBuilder = new StringBuilder();

        foreach (var t in bytes)
        {
            sBuilder.Append(t.ToString("x2"));
        }

        return sBuilder.ToString();
    }
    
    private bool CheckDifficultyWithResult(int difficulty, byte[] hash)
    {
        if (difficulty > hash.Length) return false;
        for (var i = 0; i * 8 < difficulty; i++)
        {
            var bits = hash[i];
            var restBits = difficulty - i * 8;
            for (var j = (8 < restBits ? 8 : restBits); j > 0; j--)
            {
                if ((bits & (1 << j - 1)) != 0) return false;
            }
        }
        return true;
    }
}

public class BlockDto
{
    public required object Data { get; set; }
    public int? Nonce { get; set; }

    public byte[] GetHash()
    {
        using var sha256Hash = SHA256.Create();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var jsonString = JsonSerializer.Serialize(this, options);
        return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(jsonString));
    }
}