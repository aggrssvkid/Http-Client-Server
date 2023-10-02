using Client_Server;
using Server;
using System.Net;

DataBaseImitator imitator = new DataBaseImitator(); // объект, имитирующий базу данных.
ServerStandart server = new ServerStandart(new HttpListener()); // создали объект, имитирующий сервер
server.Start(); // запустили

while (true)
{
    // Сервер начинает слушать входящие запросы и соответствующе реагировать на запрос
    var context = server.StartListenRequests();
    Console.WriteLine("Someone Connected!");

    if (context.Request.HttpMethod.ToUpper() == "POST") // Прищел Post запрос
    {
        var datas = server.ReadRequestDatas(context);
        if (datas != null && datas.Id >= 0 && datas.Password.Length > 0 && datas.Password.Length <= 10)
        {
            imitator.DataBase.Add(datas); // Данные, которые скинул на сервер клиент, добавляем в бд.
            server.SendPostResponse(context, 1); // Отправляем сообщение, что данные успешно добавлены.
            Console.WriteLine($"Successfuly added: {imitator.DataBase.Last().Id} with password: {imitator.DataBase.Last().Password}");
        }
        else
            server.SendPostResponse(context, -1); // Ошибка в добавлении данных
    }
    else if (context.Request.HttpMethod.ToUpper() == "GET") // Пришел Get Запрос
    {
        var dataBase = imitator.DataBase;
        if (dataBase.Count == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            server.SendGetResponse(context, "");
        }
        else
        {
            string pass = dataBase.Last().Password;
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            server.SendGetResponse(context, pass);
        }
        Console.WriteLine("Data sent.");
    }
    else if (context.Request.HttpMethod.ToUpper() == "OPTIONS") // Обрабатываем "Options" запрос
        server.SendOptionsResponse(context);
}
