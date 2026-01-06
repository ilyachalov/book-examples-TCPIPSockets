using System;     // для Console, Exception
using System.Net; // для IPHostEntry, Dns, IPAddress

class IPAddressExample
{
    static void PrintHostInfo(string host)
    {
        try
        {
            IPHostEntry hostInfo;

            // попытка получить данные DNS о заданном имени хоста или IP-адресе
            //hostInfo = Dns.Resolve(host); // метод устарел
            hostInfo = Dns.GetHostEntry(host);

            // вывести в консоль первичное имя хоста
            Console.WriteLine("\tКаноническое имя: " + hostInfo.HostName);

            // вывести в консоль список IP-адресов этого хоста:
            Console.Write("\tIP-адреса:        ");
            foreach (IPAddress ipaddr in hostInfo.AddressList)
            {
                Console.Write(ipaddr.ToString() + " ");
            }
            Console.WriteLine();

            // вывести в консоль список псевдонимов этого хоста
            Console.Write("\tПсевдонимы:       ");
            foreach (string alias in hostInfo.Aliases)
            {
                Console.Write(alias + " ");
            }
            Console.WriteLine();
        }
        catch (Exception)
        {
            Console.WriteLine("\tНе могу получить данные о хосте: " + host + "\n");
        }
    }

    static void Main(string[] args)
    {
        // получить и вывести в консоль информацию о локальном хосте
        try
        {
            Console.WriteLine("Локальный хост:");
            string localHostName = Dns.GetHostName();
            Console.WriteLine("\tИмя хоста:        " + localHostName);
            PrintHostInfo(localHostName);
            Console.WriteLine();
        }
        catch (Exception)
        {
            Console.WriteLine("Не могу получить данные о локальном хосте!\n");
        }

        // получить и вывести в консоль информацию о хостах, заданных
        // в командной строке
        foreach (string arg in args)
        {
            Console.WriteLine(arg + ":");
            PrintHostInfo(arg);
        }
    }
}
