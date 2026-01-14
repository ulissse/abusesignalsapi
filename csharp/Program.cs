using System.Net.Http.Headers;
using System.Text.Json;

if (args.Length < 2)
{
    Console.WriteLine("Usage: dotnet run -- <domain-or-url> <api-key> [--explain]");
    return;
}

var input = args[0];
var apiKey = args[1];
var explain = args.Length >= 3 && args[2].Equals("--explain", StringComparison.OrdinalIgnoreCase);

string domain = input;

// Very small normalization: if user passes a URL, extract host.
if (Uri.TryCreate(input, UriKind.Absolute, out var uri) && !string.IsNullOrWhiteSpace(uri.Host))
    domain = uri.Host;

var baseUrl = "https://abusesignalsapi.analyses-web.com";
var url = $"{baseUrl}/abuse-signals?domain={Uri.EscapeDataString(domain)}" + (explain ? "&explain=1" : "");

using var http = new HttpClient();
http.DefaultRequestHeaders.Add("X-API-Key", apiKey);

var resp = await http.GetAsync(url);
var body = await resp.Content.ReadAsStringAsync();

Console.WriteLine($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}");
Console.WriteLine();

try
{
    using var doc = JsonDocument.Parse(body);
    var pretty = JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(pretty);
}
catch
{
    Console.WriteLine(body);
}
