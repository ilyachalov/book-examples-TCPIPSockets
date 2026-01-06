using System;             // для string, ArgumentException, byte, int, Exception
using System.Text;        // для Encoding
using System.Net.Sockets; // для TcpClient, NetworkStream

class TcpEchoClient
{
    static void Main(string[] args)
    {
        // проверить количество аргументов, их должно быть 2 или 3
        // если другое количество, прервать программу с ошибкой
        // <Сервер> — первый аргумент командной строки, args[0]
        // <Слово> — второй аргумент командной строки, args[1]
        // <Порт> — третий (необязательный) аргумент командной строки, args[2]
        if (args.Length < 2 || args.Length > 3)
        {
            throw new ArgumentException("Параметры: <Сервер> <Слово> [<Порт>]");
        }

        // имя сервера или IP-адрес
        string server = args[0];

        // преобразовать второй аргумент командной строки в байты
        args[1] += "\n"; // для тестирования на сайте tcpbin.com
        byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);

        // использовать заданный порт (если указан), иначе — порт 7
        int servPort = (args.Length == 3) ? int.Parse(args[2]) : 7;

        TcpClient client = null;
        NetworkStream netStream = null;

        try
        {
            // создать сокет, присоединенный к заданному серверу
            // через заданный порт
            client = new TcpClient(server, servPort);

            Console.WriteLine("Подключился к серверу... отправляю ему строку...");

            netStream = client.GetStream();

            // отправить строку в вышеуказанной кодировке (ASCII) на сервер
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            Console.WriteLine("Отправлено {0} байтов на сервер...", byteBuffer.Length);

            int totalBytesRcvd = 0; // Всего байтов получено на данный момент
            int bytesRcvd = 0;      // Получено байтов при последнем чтении

            // получить строку-ответ от сервера,
            // в данном случае предполагается, что сервер вернет копию (эхо)
            // строки, которую мы ему отправили выше
            while (totalBytesRcvd < byteBuffer.Length)
            {
                if ((bytesRcvd = netStream.Read(byteBuffer, totalBytesRcvd,
                     byteBuffer.Length - totalBytesRcvd)) == 0)
                {
                    Console.WriteLine("Соединение закрыто преждевременно.");
                    break;
                }
                totalBytesRcvd += bytesRcvd;
            }
            Console.WriteLine("Получено {0} байтов от сервера: {1}",
                    totalBytesRcvd,
                    Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            netStream.Close();
            client.Close();
        }
    }
}