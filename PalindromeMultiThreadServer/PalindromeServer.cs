using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PalindromeMultiThreadServer
{
	internal static class PalindromeServer
	{
		internal static int MaxThreads = 0;
		internal static int ThreadCounter = 0;
		internal static TcpListener server = null;
		internal static IPAddress IP = IPAddress.Parse("127.0.0.1");
		internal static int Port = 9595;
		public static void Connection(IPAddress IP, int Port, int maxThreads)
		{
			try
			{
				ThreadPool.SetMaxThreads(1, maxThreads);
				TcpListener server = new TcpListener(IP, Port);
				server.Start();

				while (true)
				{
					Console.WriteLine("Ожидание соединения...");
					ThreadPool.QueueUserWorkItem(ClientProcessing, server.AcceptTcpClient());

				}
			}
			catch (SocketException e)
			{
				Console.WriteLine("SocketException {0}", e);
			}

			finally
			{
				server.Stop();
			}
			Console.WriteLine("\n Нажмите Enter");
			Console.Read();
		}

		public static bool PalyndromeOrNot(string str)
		{
			string temp = string.Empty;
			StringBuilder sbNoSpace = new StringBuilder(string.Empty);
			StringBuilder sbForReverse = new StringBuilder(string.Empty);
			bool f = false;


			for (int i = str.Length - 1; i >= 0; i--)
			{

				if (str[i] != ' ')
				{
					sbForReverse.Append(str[i].ToString().ToLower());

				}
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] != ' ')
				{
					sbNoSpace.Append(str[i].ToString().ToLower());
				}
			}
			if (sbNoSpace.ToString() == sbForReverse.ToString())
				f = true;
			else
				f = false;
			Thread.Sleep(3000);
			return f;
		}
		static void ClientProcessing(object client_obj)
		{

			byte[] bytes = new byte[1024];
			String data = null;
			TcpClient client = client_obj as TcpClient;

			data = null;

			NetworkStream stream = client.GetStream();
			int i;
			
			while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
			{
				data = System.Text.Encoding.Unicode.GetString(bytes, 0, i);
				if (ThreadCounter < MaxThreads)
				{
					ThreadCounter++;
					if (PalyndromeOrNot(data))
					{
						byte[] msg = System.Text.Encoding.Unicode.GetBytes("Palindrome");  //??? (ASCII or UTF8)
						stream.Write(msg, 0, msg.Length);
					}
					else
					{
						byte[] msg = System.Text.Encoding.Unicode.GetBytes("NotPalindrome");  //??? (ASCII or UTF8)
						stream.Write(msg, 0, msg.Length);
					}

				}
				else
				{
					byte[] msg = System.Text.Encoding.Unicode.GetBytes("Exception");  //??? (ASCII or UTF8)
					stream.Write(msg, 0, msg.Length);
				}
				ThreadCounter--;
			}
			client.Close();
		}
		internal static void Stop()
		{
			server.Stop();
		}
	}
}
