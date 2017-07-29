using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Configuration;
namespace FileTransferring
{
    public partial class Form1 : Form
    {
        DataTable files = new DataTable();
        ServiceHost Shost;
        String zipfile;
        String usersent;
        public event EventHandler ButtonClicked;
        public Form1()
        {
            InitializeComponent();
            files.Columns.Add(new DataColumn("Name", typeof(string)));
            files.Columns.Add(new DataColumn("Size", typeof(long)));
            files.Columns.Add(new DataColumn("Type", typeof(EDataType)));
            files.Columns.Add(new DataColumn("Path", typeof(string)) { Unique = true });

            CreateSocket(null);
            Load += new EventHandler(Form1_Load);
        }


        void Form1_Load(object sender, EventArgs e)
        {
            HelpClass.Form = this;
        }

        public bool IsValidIP(string addr)
        {
            //create our match pattern
            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (addr == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(addr, 0);
            }
            //return the results
            return valid;
        }




        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            files.Clear();
        }

        private void exitToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void CreateSocket(object listen)
        {
            int port = 7634;
            Uri tcpAdrs = new Uri("net.tcp://0.0.0.0:" + port.ToString() + "/ChatServer");
            Uri[] baseAdresses = { tcpAdrs };
            Shost = new ServiceHost(typeof(TransferClass), baseAdresses);
            NetTcpBinding bin = new NetTcpBinding(SecurityMode.None, false);
            bin.MaxBufferPoolSize = (int)67108864;
            bin.MaxBufferSize = (int)67108864;
            bin.MaxReceivedMessageSize = (int)67108864;
            bin.SendTimeout = TimeSpan.FromSeconds(15);
            bin.ReaderQuotas.MaxArrayLength = 67108864;
            bin.ReaderQuotas.MaxBytesPerRead = 67108864;
            bin.ReaderQuotas.MaxStringContentLength = 67108864;
            bin.MaxConnections = 2000;
            ServiceThrottlingBehavior throttle;
            throttle = Shost.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior();
                throttle.MaxConcurrentCalls = 1000;
                throttle.MaxConcurrentSessions = 1000;
                Shost.Description.Behaviors.Add(throttle);
            }
            Shost.AddServiceEndpoint(typeof(ITransfer), bin, "tcp");

            ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            Shost.Description.Behaviors.Add(mBehave);

            Shost.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexTcpBinding(),
                "net.tcp://0.0.0.0:" + (port - 1).ToString() + "/ChatServer/mex");
            try
            {
                Shost.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }



        public void callup(String zipfile, String usersent)
        {

            this.zipfile = zipfile;
            this.usersent = usersent;
            TransferNamespace.FilesTransfer transfer = new TransferNamespace.FilesTransfer() { Action = FileTransferring.TransferNamespace.Action.Invite };
            transfer.Files = new List<TransferNamespace.Transfer>();


            FileInfo file = new FileInfo(zipfile);
            TransferNamespace.Transfer t = new TransferNamespace.Transfer();
            t.Catalog = "<root>";
            t.Id = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            t.Name = file.Name;
            t.FullPath = file.FullName;
            t.Length = file.Length;
            transfer.Files.Add(t);
            transfer.Name = "karthik";
            String ip = ConfigurationSettings.AppSettings[usersent.ToString()].ToString();
            TransferNamespace.TransferClient client = new FileTransferring.TransferNamespace.TransferClient(HelpClass.ConfigureEndpoint(), new EndpointAddress("net.tcp://" + ip + ":7634/ChatServer/tcp"));
            try
            {
                client.Open();
            }
            catch { MessageBox.Show("Can not connect to the remote address!"); }
            TProgress progress = new TProgress(client, 0);
            progress.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            progress.BeginSend(transfer);
            try
            {
                progress.Show(this);



            }
            catch { }

        }
        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ButtonClicked != null)
                ButtonClicked(this, e);
        }
        private List<TransferNamespace.Transfer> RecurseFolder(string root, string folderName)
        {
            List<TransferNamespace.Transfer> result = new List<TransferNamespace.Transfer>();
            DirectoryInfo info = new DirectoryInfo(root + folderName);
            foreach (FileInfo file in info.GetFiles())
            {
                TransferNamespace.Transfer transfer = new TransferNamespace.Transfer();
                transfer.Catalog = folderName.Substring(1);
                transfer.Id = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                transfer.Length = file.Length;
                transfer.Name = file.Name;
                transfer.FullPath = file.FullName;
                result.Add(transfer);
            }
            foreach (DirectoryInfo dir in info.GetDirectories())
            {
                result.AddRange(RecurseFolder(root, folderName + @"\" + dir.Name));
            }
            return result;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }



    }

    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 65536;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public enum EDataType
    {
        File, Folder
    }
}
