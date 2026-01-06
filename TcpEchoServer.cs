using System;             // для ArgumentException, Console, Environment
using System.Net;         // для IPAddress
using System.Net.Sockets; // для TcpListener, SocketException, TcpClient,
                          // NetworkStream

class TcpEchoServer
{
    private const int BUFSIZE = 32; // размер буфера для получения данных

    static void Main(string[] args)
    {
        // проверить: может быть один необязательный аргумент
        // если другое количество, прервать программу с ошибкой
        // <Порт> — первый (необязательный) аргумент командной строки, args[0]
        if (args.Length > 1)
        {
            throw new ArgumentException("Параметры: [<Порт>]");
        }

        int servPort = (args.Length == 1) ? int.Parse(args[0]): 7;

        TcpListener listener = null;

        try
        {
            // создать слушающий сокет для приема соединений от клиентов
            listener = new TcpListener(IPAddress.Any, servPort);
            listener.Start();
        }
        catch (SocketException se)
        {
            Console.WriteLine(se.ErrorCode + ": " + se.Message);
            Environment.Exit(se.ErrorCode);
        }
        
        byte[] rcvBuffer = new byte[BUFSIZE]; // буфер для получения данных
        int bytesRcvd;                        // счетчик полученных байтов

        // бесконечный цикл для приема и обслуживания соединений
        for (;;)
        {
            TcpClient client = null;
            NetworkStream netStream = null;

            try
            {
                // получить соединение от клиента
                client = listener.AcceptTcpClient();
                netStream = client.GetStream();
                Console.Write("Обработка соединения от клиента – ");

                // получать байты, пока клиент не закроет соединение, на это
                // укажет возвращенное значение 0 (т.е. получено 0 байтов)
                int totalBytesEchoed = 0;
                while ((bytesRcvd = netStream.Read(rcvBuffer, 0, rcvBuffer.Length)) > 0)
                {
                    netStream.Write(rcvBuffer, 0, bytesRcvd);
                    totalBytesEchoed += bytesRcvd;
                }
                Console.WriteLine("получено и возвращено {0} байтов.",
                                  totalBytesEchoed);

                // закрыть поток и сокет, соединение от клиента обработано!
                netStream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                netStream.Close();
            }
        }
    }
}