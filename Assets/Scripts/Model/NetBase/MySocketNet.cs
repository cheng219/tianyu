using System;
using System.Net.Sockets;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace st.net.NetBase
{
    public delegate void CommandDelegate(MySocketNet mynet, Pt pt);
    public delegate void NetEventHandler(object sender, EventArgs e);

    public class MySocketNet
    {
		#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern string GetIOSIPv6(string mHost, string mPort);
		#endif
        public Socket socket = null;
        public string server = "127.0.0.1";
        public uint port = 5000;

        private AsyncCallback connectCallback;
        private AsyncCallback receiveHeaderCallback;
		private AsyncCallback receiveBodyCallback;

        private byte[] headerBuf;
        private byte[] bodyBuf;
        private int readBytes;

        public const int HEADER_SIZE = 4;


        private byte[] send_buf = null;
        private int send_len = 0;
        public const int MAX_SEND_SIZE = 1024 * 256;

		private int PswCode = 0;

        public MySocketNet()
        {
        }

        public MySocketNet(string server, uint port)
        {
            this.server = server;
            this.port = port;
        }

        private void Init()
        {
            this.connectCallback = new AsyncCallback(this.ConnectCallback);
            this.receiveHeaderCallback = new AsyncCallback(this.ReceiveHeaderCallback);
			this.receiveBodyCallback = new AsyncCallback(this.ReceiveBodyCallback);
            this.headerBuf = new byte[HEADER_SIZE];

            this.send_buf = new byte[MAX_SEND_SIZE]; ;
        }

        public bool Connect()
        {
            if (socket != null)
                Close();
            Init();

			AddressFamily newAddressFamily = AddressFamily.InterNetwork;
			if (!GameCenter.instance.isDevelopmentPattern)
			{
				String newServerIp = "";
				getIPType(server, port.ToString(), out newServerIp, out newAddressFamily);
				if (!string.IsNullOrEmpty(newServerIp))
				{
					server = newServerIp;
				}
			}

			socket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Blocking = true;

            IPAddress serverIP = IPAddress.Parse(server);
            IPEndPoint serverhost = new IPEndPoint(serverIP, (int)port);


            try
            {
                this.socket.BeginConnect(serverhost, this.connectCallback, null);
            }
            catch (Exception e)
            {
				socket = null;
                OnConnectFailed(e,new System.EventArgs());
                return false;
            }
            return true;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);

                this.OnConnected(new System.EventArgs());
                Receive();
            }
            catch (Exception e)
            {
				socket = null;
                OnConnectFailed(e, new System.EventArgs());
            }      
        }

        public void Receive()
        {
            if ((socket != null) && socket.Connected)
            {
                this.BeginReceiveHeader(0);
            }
        }

        private void BeginReceiveHeader(int offset)
        {
            if (offset == 0)
                this.readBytes = 0;
            try
            {
                OnInfo("--------------------start BeginReceiveHeader");

                this.socket.BeginReceive(
                this.headerBuf,
                offset,
                this.headerBuf.Length - offset,
                SocketFlags.None,
                this.receiveHeaderCallback,
                null);
            }
            catch (Exception e)
            {
                OnError("BeginReceiveHeader:socket catch" + e.Message);
            }
        }

        private void ReceiveHeaderCallback(IAsyncResult ar)
        {
            try
            {
                OnInfo("--------------------start ReceiveHeaderCallback");

                if (!this.socket.Connected)
                {
                    OnError("ReceiveHeaderCallback socket not connected");
                    return;
                }

                int count = this.socket.EndReceive(ar);
                if (count == 0)
                {
                    OnError("ReceiveHeaderCallback:socket receive 0");
                    return;
                }

                this.readBytes += count;
                if (this.readBytes < this.headerBuf.Length)
                {
                    this.BeginReceiveHeader(this.readBytes);
                    return;
                }

                int dataSize = ByteAr_b.b2i(headerBuf);
				dataSize = dataSize ^ PswCode;
                int bodySize = dataSize - this.headerBuf.Length;
                this.bodyBuf = new byte[bodySize];
                this.BeginReceiveBody(0);
            }
            catch (Exception e)
            {
                OnError("ReceiveHeaderCallback:socket catch" + e.Message);
            }
        }

        private void BeginReceiveBody(int offset)
        {
            if (offset == 0) 
                this.readBytes = 0;
            try
            {
                OnInfo("--------------------start BeginReceiveBody");

                this.socket.BeginReceive(
                    this.bodyBuf,
                    offset,
                    this.bodyBuf.Length - offset,
                    SocketFlags.None,
                    this.receiveBodyCallback,
                    null);
            }
            catch (Exception e)
            {
                OnError("BeginReceiveBody:socket catch" + e.Message);
            }
        }

        private void ReceiveBodyCallback(IAsyncResult ar)
        {
            try
            {
                OnInfo("--------------------start ReceiveBodyCallback");

                if (!this.socket.Connected)
                    return;

                int count = this.socket.EndReceive(ar);


                if (count == 0)
                {
                    OnError("ReceiveBodyCallback:socket receive 0");
                    return;
                }

                this.readBytes += count;
                if (this.readBytes < this.bodyBuf.Length)
                {
                    this.BeginReceiveBody(this.readBytes);
                    return;
                }

                byte[] bytes_cmdid = new byte[2];
                byte[] bytes_seq = new byte[4];
                byte[] bytes_body = new byte[bodyBuf.Length - 6];
                ByteAr_b.PickBytes(this.bodyBuf, bytes_cmdid.Length, bytes_cmdid, 0);
                ByteAr_b.PickBytes(this.bodyBuf, bytes_seq.Length, bytes_seq, 2);
                ByteAr_b.PickBytes(this.bodyBuf, bytes_body.Length, bytes_body, bytes_cmdid.Length + bytes_seq.Length);

				if(ByteAr_b.b2us(bytes_cmdid) == 0xa10b)
					PswCode = ByteAr_b.b2i(bytes_body);
				else
				{
                    this.OnReceived(this.bodyBuf);
/*					Pt pt = ptMng.GetInstance().MakePt(ByteAr_b.b2us(bytes_cmdid),ByteAr_b.b2ui(bytes_seq), bytes_body);

	                if (pt == null)
	                    OnError("Receive unknow pt pt_id = " + ByteAr_b.b2us(bytes_cmdid));
	                else
	                {
	                    pt.seq = ByteAr_b.b2ui(bytes_seq);
                        this.OnReceived(pt);
	                }*/
				}

                this.BeginReceiveHeader(0);
            }
            catch (Exception e)
            {
                OnError("ReceiveBodyCallback:socket catch" + e.Message);
            }
        }

        public void Close()
        {
            try
            {
                OnInfo("--------------------start Close");

                if ((int)this.socket.Handle > 0)
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    this.socket = null;
                }
                this.OnClosed(new System.EventArgs());
            }
            catch (Exception e)
            {
                OnError("Close:socket catch" + e.Message);
            }
        }
       

        public void Send(Pt pt)
        {
            if (pt == null)
            {
                OnInfo("--------------------error pt");
                return;
            }

            byte[] data = pt.toBinary();
            OnInfo("--------------------start send:" + (data.Length + 10));

            if ((this.socket == null) || (!this.socket.Connected))
            {
                OnInfo("--------------------send,no net");
                return;
            }

			int allLen = data.Length + 10;
			allLen = allLen ^ PswCode;
			byte[] bytesLen = ByteAr_b.i2b(allLen);
            byte[] bytesPt = ByteAr_b.s2b(pt.GetID());
            byte[] bytesSeq = ByteAr_b.i2b(pt.seq);


            byte[] allData = new byte[data.Length + 10];
            Array.Copy(bytesLen, 0, allData, 0, bytesLen.Length);
            Array.Copy(bytesPt, 0, allData, bytesLen.Length, bytesPt.Length);
            Array.Copy(bytesSeq, 0, allData, bytesLen.Length + bytesPt.Length, bytesSeq.Length);
            Array.Copy(data, 0, allData, bytesLen.Length + bytesPt.Length + bytesSeq.Length, data.Length);

            lock (send_buf)
            {
                if (send_len == 0)
                {
                    allData.CopyTo(send_buf, send_len);
                    send_len = send_len + allData.Length;
                    BeginSend();
                }
                else
                {
                    allData.CopyTo(send_buf, send_len);
                    send_len = send_len + allData.Length;
                }
            }
        }
        private void BeginSend()
        {
            try
            {
                this.socket.BeginSend(
                send_buf, 0, send_len,
                SocketFlags.None,
                new AsyncCallback(this.SendCallback), null);
            }
            catch (Exception e)
            {
                OnError("BeginSend:socket catch" + e.Message);
            }
        }

        int sendBytes = 0;
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                sendBytes = this.socket.EndSend(ar);
           
//            if (sendBytes == 0)
//            {
//                OnInfo("--------------------SendCallback 0");
//                OnError("SendCallback 0");
//                return;
//            }
                OnInfo("--------------------Sended:" + sendBytes);

                if (sendBytes == 0)
                {
                    OnInfo("--------------------SendCallback 0 continue send");
                    this.BeginSend();
                }
                else
                {
                    OnInfo("--------------------SendCallback continue send");
                
                    lock (send_buf)
                    {
                        send_len = (send_len - sendBytes < 0) ? 0 : send_len - sendBytes;
                        Array.Copy(send_buf, sendBytes, send_buf, 0, send_len);
                        if (send_len > 0)
                            this.BeginSend();
                    }
                }
            }
            catch (Exception e)
            {
                OnError("SendCallback:socket catch" + e.Message);
            }
        }
       
        public NetEventHandler Error;
        protected virtual void OnError(String err_str)
        {
            if (this.Error == null) return;
            NetErr err = new NetErr(err_str);
            this.Error(this, err);
        }

        public NetEventHandler Info;
        protected virtual void OnInfo(String info_str)
        {
            if (this.Info == null) return;
            NetErr err = new NetErr(info_str);
            this.Info(this, err);
        }

        public NetEventHandler Received;
        protected virtual void OnReceived(byte[] pt)
        {
            if (this.Received == null) return;
            this.Received(this, new PtMsg(pt));
        }
 /*       protected virtual void OnReceived(Pt pt)
        {
            if (this.Received == null) return;
            this.Received(this, new PtMsg(pt));
        }*/

        public NetEventHandler ConnectFailed;
        protected virtual void OnConnectFailed(Exception e, System.EventArgs eventArgs)
        {
            if (this.ConnectFailed == null) return;
			NetErr err = new NetErr("error" + e.Message);
            this.ConnectFailed(this, err);
        }

        public NetEventHandler Connected;
        protected virtual void OnConnected(System.EventArgs eventArgs)
        {
			PswCode = 0;
            if (this.Connected == null) return;
            this.Connected(this, eventArgs);
        }

        public NetEventHandler Closed;
        protected virtual void OnClosed(System.EventArgs e)
        {
            if (this.Closed == null) return;
            this.Closed(this, e);
        }

		private static string GetIPv6(string mHost, string mPort)
		{
			#if UNITY_IPHONE
			string mIPv6 = GetIOSIPv6(mHost, mPort);
			return mIPv6;
			#else
			return mHost + "&&ipv4";
			#endif
		}

		private void getIPType(String serverIp, String serverPorts, out String newServerIp, out AddressFamily mIPType)
		{
			mIPType = AddressFamily.InterNetwork;
			newServerIp = serverIp;
			try
			{
				string mIPv6 = GetIPv6(serverIp, serverPorts);
				//  NGUIDebug.Log("IPv6:" + mIPv6);
				if (!string.IsNullOrEmpty(mIPv6))
				{
					string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
					if (m_StrTemp != null && m_StrTemp.Length >= 2)
					{
						string IPType = m_StrTemp[1];
						if (IPType == "ipv6")
						{
							newServerIp = m_StrTemp[0];
							mIPType = AddressFamily.InterNetworkV6;
						}
					}
				}
			}
			catch (Exception e)
			{
				OnError("GetIPv6 error:" + e);
			}

		}
    }
}
