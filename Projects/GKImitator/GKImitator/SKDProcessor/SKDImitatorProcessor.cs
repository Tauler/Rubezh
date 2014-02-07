﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using XFiresecAPI;
using System.Diagnostics;
using GKImitator.ViewModels;
using GKProcessor;

namespace GKImitator.Processor
{
	public class SKDImitatorProcessor
	{
		public bool IsConnected { get; set; }
		int Port;
		Socket serverSocket;
		byte[] byteData = new byte[64];
		public List<SKDImitatorJournalItem> JournalItems { get; set; }
		public int LastJournalNo { get; set; }

		public SKDImitatorProcessor(int port)
		{
			Port = port;
			IsConnected = true;
			JournalItems = new List<SKDImitatorJournalItem>();
			JournalItems.Add(new SKDImitatorJournalItem());
			LastJournalNo = 0;
		}

		public void Start()
		{
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
			serverSocket.Bind(ipEndPoint);

			IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
			EndPoint epSender = (EndPoint)ipeSender;

			serverSocket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);
		}

		void OnReceive(IAsyncResult ar)
		{
			IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
			EndPoint epSender = (EndPoint)ipeSender;
			serverSocket.EndReceiveFrom(ar, ref epSender);

			if (IsConnected)
			{
				var bytes = CreateAnswer();
				if (bytes != null)
				{
					var sendBytes = new List<byte>();
					sendBytes.Add(byteData[0]);
					sendBytes.AddRange(bytes);
					byte[] message = sendBytes.ToArray();

					serverSocket.BeginSendTo(message, 0, message.Length, SocketFlags.None, epSender, new AsyncCallback(OnSend), epSender);
				}
			}

			serverSocket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);
		}

		void OnSend(IAsyncResult ar)
		{
			serverSocket.EndSend(ar);
		}

		List<byte> CreateAnswer()
		{
			var result = new List<byte>();
			switch (byteData[0])
			{
				case 1: // Информация об устройстве
					result.Add(9);
					return result;
				case 2: // Индекс последней записи
					result.AddRange(JournalItems.LastOrDefault().ToBytes());
					return result;
				case 3: // Чтение конкретной записи
					var no = BytesHelper.SubstructInt(byteData.ToList(), 1);
					var journalItem = JournalItems.FirstOrDefault(x=>x.No == no);
					if (journalItem != null)
					{
						result.AddRange(journalItem.ToBytes());
					}
					return result;
			}
			return null;
		}

		public static List<byte> ToBytes(short shortValue)
		{
			return BitConverter.GetBytes(shortValue).ToList();
		}

		public static List<byte> IntToBytes(int intValue)
		{
			return BitConverter.GetBytes(intValue).ToList();
		}
	}
}