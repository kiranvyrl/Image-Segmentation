using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Configuration;
namespace FileTransferring
{
    public partial class TProgress : Form
    {
        TransferNamespace.TransferClient clients { get; set; }
        public const int limit = 2;
        public int port;
        public int sport;
        public Socket socket;
        public EndPoint receivedFromEP = new IPEndPoint(IPAddress.Any, 4815);
        public Thread SendThread;
        public Thread GetThread;
        TransferNamespace.FilesTransfer CurrentTransfer { get; set; }
        public List<String> paths = new List<string>();
        AutoResetEvent connected = new AutoResetEvent(false);
        ManualResetEvent wait = new ManualResetEvent(true);
        public AutoResetEvent tcontinue = new AutoResetEvent(false);

        public TProgress(TransferNamespace.TransferClient client, int port)
        {
            InitializeComponent();
            this.clients = client;
            this.port = port;
        }

        public void GetInvoke(object obj)
        {
            try
            {
                string path = Convert.ToString(ConfigurationSettings.AppSettings["ReceivedFile"]);
                byte[] overthrow = new byte[0];
                FilesTransfer tr = (FilesTransfer)obj;
                ThreadStart callback = delegate
                {
                    progressBar1.Maximum = (int)tr.Files.Sum(t => t.Length) / 4;
                };
                progressBar1.Invoke(callback, null);
                TcpListener listener = new TcpListener(port);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                callback = delegate
                {
                    this.Text = string.Format("File Received from {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
                };
                this.Invoke(callback, null);
                client.ReceiveBufferSize = 65 * (int)Math.Exp(6 * System.Math.Log(10));
                foreach (TransferNamespace.Transfer t in tr.Files)
                {
                    if (t.Catalog != "<root>" && !Directory.Exists(path + t.Catalog))
                        Directory.CreateDirectory(path + t.Catalog);
                    FileStream stream = new FileStream(path + (t.Catalog == "<root>" ? "" : t.Catalog + "\\") + t.Name, FileMode.Create, FileAccess.Write);
                    ThreadStart todo = delegate
                    {
                        progressBar2.Value = 0;
                        progressBar2.Maximum = (int)t.Length;
                        textBox1.Text = t.Name;
                    };
                    this.Invoke(todo, null);
                    long getted = 0;
                    if (overthrow.Length > 0)
                    {
                        int gotted = 0;
                        if (overthrow.Length <= t.Length)
                        {
                            stream.Write(overthrow, 0, overthrow.Length);
                            gotted = overthrow.Length;
                        }
                        else
                        {
                            byte[] otherbuf = overthrow.Without(0, t.Length);
                            stream.Write(otherbuf, 0, otherbuf.Length);
                            overthrow = overthrow.Without(t.Length);
                            gotted = otherbuf.Length;
                        }
                        stream.Flush();
                        getted = gotted;
                        todo = delegate
                        {
                            try
                            {
                                progressBar1.Value += (int)gotted / 4;
                                progressBar2.Value += (int)gotted;
                            }
                            catch { }
                        };
                        this.Invoke(todo, null);
                    }
                    while (getted < t.Length)
                    {
                        byte[] buffer = new byte[client.Client.Available];
                        client.Client.Receive(buffer, SocketFlags.Partial);
                        getted += buffer.Length;
                        if (getted > t.Length)
                        {
                            long length = (long)getted - t.Length;
                            overthrow = buffer.Without(buffer.Length - length);
                            buffer = buffer.Without(0, buffer.Length - length);
                            getted -= length;
                        }
                        todo = delegate
                        {
                            try
                            {
                                progressBar1.Value += (int)buffer.Length / 4;
                                progressBar2.Value += (int)buffer.Length;
                            }
                            catch { }
                        };
                        this.Invoke(todo, null);
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Flush();
                    }
                    stream.Dispose();
                    client.Client.SendTo(new byte[1] { 1 }, new IPEndPoint((client.Client.RemoteEndPoint as IPEndPoint).Address, sport));
                }
                client.Close();
                callback = delegate
                {
                    textBox1.Text = "File Received.";
                };
                this.Invoke(callback, null);
            }
            catch { MessageBox.Show("Aborted"); }
        }

        void Connected(IAsyncResult ar)
        {
            connected.Set();
        }

        public void SendInvoke(object obj)
        {
            try
            {
                TransferNamespace.FilesTransfer ft = (TransferNamespace.FilesTransfer)(obj as List<object>)[0];
                ThreadStart ts = delegate
                {
                    progressBar1.Maximum = (int)(ft.Files.Sum(t => t.Length) / 4);
                };
                progressBar1.Invoke(ts, null);
                IPEndPoint sendTo = (IPEndPoint)(EndPoint)(obj as List<object>)[1];
                TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Any, sport));
                client.BeginConnect(sendTo.Address, sendTo.Port, new AsyncCallback(Connected), null);
                connected.WaitOne();    
                long sended = 0;
                foreach (TransferNamespace.Transfer t in ft.Files)
                {
                    using (FileStream stream = new FileStream(t.FullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                    {
                        ts = delegate
                        {
                            progressBar2.Value = 0;
                            progressBar2.Maximum = (int)t.Length;
                            textBox1.Text = t.Name;
                        };
                        this.Invoke(ts, null);
                        client.SendBufferSize = 65 * (int)Math.Exp(6 * System.Math.Log(10));
                        sended = 0;
                        stream.Seek(0, SeekOrigin.Begin);
                        while (true)
                        {
                            byte[] bytes;
                            if (sended >= stream.Length) break;
                            if (stream.Length - sended < 20000)
                                bytes = new byte[stream.Length - sended];
                            else
                                bytes = new byte[20000];
                            sended += bytes.Length;
                            ThreadStart asdasd = delegate
                            {
                                progressBar1.Value += (int)bytes.Length / 4;
                                progressBar2.Value += (int)bytes.Length;
                            };
                            this.Invoke(asdasd, null);
                            stream.Read(bytes, 0, bytes.Length);
                            client.Client.Send(bytes, SocketFlags.Partial);
                        }
                    }
                    client.Client.Receive(new byte[1]);
                }
                client.Close();
                ts = delegate
                {
                    textBox1.Text = "File Sent.";
                };
                this.Invoke(ts, null);
            }
            catch { MessageBox.Show("Aborted"); }
        }

        public void BeginSend(TransferNamespace.FilesTransfer transfer)
        {
            try
            {
                sport = HelpClass.GetAvailablePort();
                IEnumerable<object> r = clients.SendQuery(Dns.GetHostAddresses(Dns.GetHostName())[0].ToString(), transfer, sport);
                switch ((InviteRusult)r.First())
                {
                    case InviteRusult.Ok:
                        {
                            SendThread = new Thread(new ParameterizedThreadStart(SendInvoke));
                            SendThread.Start(new List<object>() { transfer, new IPEndPoint(IPAddress.Parse(clients.Endpoint.Address.Uri.Host), (int)r.Last()) });
                            this.Text = string.Format("File Transfering to {0}", clients.Endpoint.Address.Uri.Host);
                            break;
                        }
                    case InviteRusult.Cancel:
                        {
                            try
                            {
                                if (SendThread != null)
                                    SendThread.Abort();
                                if (GetThread != null)
                                    GetThread.Abort();
                                this.Close();
                            }
                            catch { }
                            break;
                        }
                    case InviteRusult.Busy:
                        {
                            transfer = null;
                            this.Close();
                            break;
                        }
                }
            }
            catch { }
        }


        public TransferNamespace.Transfer Converter(Transfer t)
        {
            return (TransferNamespace.Transfer)t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (SendThread != null)
                    SendThread.Abort();
                if (GetThread != null)
                    GetThread.Abort();

            }
            catch (Exception)
            {
            }
            finally
            {
                this.Close();
            }
        }

        private void TProgress_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
