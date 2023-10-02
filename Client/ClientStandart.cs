using System;
using System.Text.Json;

namespace Client
{
    public class ClientStandart
    {
        public ClientStandart(HttpClient httpClient, string uri) // конструктор класса
        {
            Client = httpClient;
            Uri = uri;
        }
        public HttpClient Client { get; set; } // Класс .Net, в котором много полезных функций для реализации Http общения
        public string Uri { get; set; } // Адрес сервера, к которому держим путь

        // Постим юзера (id and passowrd)
        async public Task<int> PostDatas(int id, string password)
        {
            RequestJsonModel model = new RequestJsonModel(id, password);
            string jsonString = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(jsonString);
            HttpResponseMessage response = await Client.PostAsync(Uri, content);
            if (response.IsSuccessStatusCode)
                Console.WriteLine("Success!");
            else
                Console.WriteLine("Failure!");
            return (int)response.StatusCode;
        }
        // Get запрос, в которм мы получаем последний добавленный пароль на сервер
        async public Task<string?> GetLastPasswod()
        {
            var response = await Client.GetAsync(Uri);
            var message = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode == false)
                return null;
            return message;
        }
        // "Options" запрос, на проверку хеадеров сервера, чтобы понять, смогут ли клиент и сервер общаться в одинаковом формате
        async public Task<int> GetServerOptions(HttpRequestMessage request, ClientStandart client)
        {
            HttpResponseMessage response = await client.Client.SendAsync(request);
            var headers = response.Content.Headers;
            int checker = 0; // Если станет равным 2 после следующего цикла, то информация по проверяемым хедерам присутствует на сервере
            foreach (var header in headers)
            {
                foreach (var value in header.Value)
                    Console.WriteLine(value);
                if (header.Key == "Allow" || header.Key == "Content-Type")
                {
                    checker++;
                    if (client.CheckServerOptions(header.Key, header.Value) == false)
                    {
                        checker = -1;
                        break;
                    }
                }
            }
            if (checker == 2)
                Console.WriteLine("Good! Server can handle requests!");
            else
                Console.WriteLine("Failure! Server can't handle some requests! Be careful!");
            return checker;
        }

        // Вспомогательная функция для проверки хеадеров
        private bool CheckServerOptions(string header, IEnumerable<string> param)
        {
            if (header == "Allow")
            {
                Console.WriteLine("Check Allow");
                if (param.Contains("GET") == false || param.Contains("POST") == false)
                    return false;
                else
                    Console.WriteLine("Allow OK!");
            }
            else if (header == "Content-Type")
            {
                Console.WriteLine("Check Content-type");
                if (param.Contains("application/json; charset=UTF-8") == false)
                    return false;
                else
                    Console.WriteLine("Content-Type ok!");
            }
            return true;
        }
    }
}
