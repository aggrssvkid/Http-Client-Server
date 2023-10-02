using Client;
using Server;
using System;
using System.Net;
using System.Text.Json;

namespace Client_Server
{
    public class ServerStandart
    {
        public ServerStandart(HttpListener listener) => Listener = listener; // constructor
        private HttpListener Listener { get; set; } // Объект, имитирующий сервер

        public void Start()  // Start Server
        {
            Listener.Prefixes.Add("http://localhost:8080/"); // Адрес и порт прослушивания, на котором ждем запросы клиента
            Listener.Start();
            Console.WriteLine("Server start listening...");
        }
        public void Stop() => Listener.Close(); // Stop Server

        public HttpListenerContext StartListenRequests() => Listener.GetContext(); // w8 for request

        // Получаем данные, которые запостил клиент и сохраняем их в модельку.
        public RequestJsonModel? ReadRequestDatas(HttpListenerContext context)
        {
            var request = context.Request; // Получаем объект, сформированный в результате запроса клиента.

            /* Считываем данные из потока, в который записал данные клиент.*/
            Stream stream = request.InputStream;
            StreamReader reader = new StreamReader(stream);
            string clientDatas = reader.ReadToEnd();
            /* Распаковываем полученый набор байтов в JSON формат и возвращаем модельку в вызывающий код */
            var dataModel = JsonSerializer.Deserialize<RequestJsonModel>(clientDatas);
            return dataModel;
        }
        /* Ответ на "POST" запрос */
        public void SendPostResponse(HttpListenerContext context, int status)
        {
            /* Формируем ответ с помощью "context.Response" свойства */
            HttpListenerResponse response = context.Response;
            if (status > 0)
                response.StatusCode = (int) HttpStatusCode.OK;
            else
                response.StatusCode = (int) HttpStatusCode.BadRequest;
            Stream outputStream = response.OutputStream; // поток, в который записываем ответ
            StreamWriter writer = new StreamWriter(outputStream); // обЪект, контролирующий запись ответа клиенту

            /* Просто передаем статус, обозначающий, успешно ли мы добавили данные в бд */
            writer.WriteLine();
            writer.Flush();
            writer.Close();
        }

        /* Ответ на "GET" запрос */
        public void SendGetResponse(HttpListenerContext context, string password)
        {
            HttpListenerResponse response = context.Response;
            Stream outputStream = response.OutputStream; // поток, в который записываем ответ
            StreamWriter writer = new StreamWriter(outputStream); // обЪект, контролирующий запись ответа клиенту

            /* Отправляем последний добавленный пароль, записывая его в поток обмена данными между сервером и клиентом*/
            writer.Write(password);
            writer.Flush();
            writer.Close();
        }

        public void SendOptionsResponse(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            /* Добавляем хеадеры, поясняющие клиенту, какую инфу может обработать сервер */
            response.Headers.Add("Allow: GET, POST");
            response.ContentType = "application/json;charset=UTF-8";
            Stream outputStream = response.OutputStream; // поток, в который записываем ответ
            StreamWriter writer = new StreamWriter(outputStream); // обЪект, контролирующий запись ответа клиенту

            /* Отправляем хеадеры. Более ничего в поток данных записывать не надо.*/
            writer.WriteLine();
            writer.Flush();
            writer.Close();
            Console.WriteLine("Options request handled!");
        }
    }
}
