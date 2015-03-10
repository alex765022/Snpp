using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace snpp
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("Please enter 2 arguments!");
			}
			else
			{

				const string myFile = "trace.log"; //TODO: get this from the user.  
				TextWriterTraceListener myTextListener = new TextWriterTraceListener(myFile);
				Trace.Listeners.Add(myTextListener);
				Trace.AutoFlush = true;

				string phoneNumber = args[0];
				string messageText = args[1];

				TcpClient client = new TcpClient
				{
					ReceiveTimeout = 50000
				};
				try
				{
					client.Connect("www.extel-gsm.com", 4444);
					NetworkStream stream = client.GetStream();
					StreamReader reader = new StreamReader(stream);
					StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(1251));
					string data = reader.ReadLine();
					bool flag = data != null && !data.Contains("220");
					Console.WriteLine(data);
					if (flag)
					{
						throw new Exception(data);
					}
					data = reader.ReadLine();
					if (data != null)
					{
						flag = !data.Contains("220");
						Console.WriteLine(data);
						if (flag)
						{
							throw new Exception(data);
						}
						writer.WriteLine("LOGIn login password");
						writer.Flush();
					}
					data = reader.ReadLine();
					if (data != null)
					{
						flag = !data.Contains("250");
						Console.WriteLine(data);
						if (flag)
						{
							throw new Exception(data);
						}
						writer.WriteLine("PAGEr " + phoneNumber);
						writer.Flush();
					}
					data = reader.ReadLine();
					if (data != null)
					{
						flag = !data.Contains("250");
						Console.WriteLine(data);
						if (flag)
						{
							throw new Exception(data);
						}
						writer.WriteLine("MESSAGE " + messageText);
						writer.Flush();
					}
					data = reader.ReadLine();
					if (data != null)
					{
						flag = !data.Contains("250");
						Console.WriteLine(data);
						if (flag)
						{
							throw new Exception(data);
						}
						writer.WriteLine("SEND");
						writer.Flush();
					}
					data = reader.ReadLine();
					if (data != null)
					{
						flag = !data.Contains("250");
						Console.WriteLine(data);
						if (flag)
						{
							throw new Exception(data);
						}
					}
					writer.WriteLine("QUIT");
					writer.Flush();
					Console.WriteLine(reader.ReadLine());
				}
				catch (Exception exception)
				{
					Trace.WriteLine(String.Format("{0:u} {1} Exception: {2}", DateTime.Now, phoneNumber, exception.Message));
					Console.WriteLine(exception);
				}
				finally
				{
					client.Close();
					Console.WriteLine("Port closed.");
				}
			}

		}
	}
}
