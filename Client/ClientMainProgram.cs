using Client;
using System.Net.Http;
using System.Text.Json;

ClientStandart client = new ClientStandart(new HttpClient(), "http://localhost:8080/");
Console.WriteLine("Enter http Method request (GET, POST, or OPTIONS)");
int id;
string method = Console.ReadLine();
if (method != null && method.ToUpper() == "POST") // Post request
{
    Console.WriteLine("Enter Id (integer number):");
    id = int.Parse(Console.ReadLine());
    Console.WriteLine("Enter Password (string):");
    string psw = Console.ReadLine();
    await client.PostDatas(id, psw);
}
else if (method != null && method.ToUpper() == "GET") // Get request
{
    string? password = await client.GetLastPasswod();
    if (password == null)
        Console.WriteLine("No passwords recorded. :(");
    else
        Console.WriteLine($"Last password: {password}");
}
else if (method != null && method.ToUpper() == "OPTIONS") // Options request
{
    HttpRequestMessage request = new(HttpMethod.Options, client.Uri);
    await client.GetServerOptions(request, client);
}
else
    Console.WriteLine("No methods choosed.");