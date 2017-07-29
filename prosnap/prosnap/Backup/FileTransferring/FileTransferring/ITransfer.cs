using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;

namespace FileTransferring
{
    public enum InviteRusult
    {
        Ok, Cancel, Busy
    }

    public enum Action
    {
        Invite, Ok, Busy, Cancel
    }

    [DataContract]
    public class FilesTransfer
    {
        [DataMember]
        public Action Action { get; set; } // The action of the packet
        [DataMember]
        public string Name { get; set; } //Client name
        [DataMember]
        public string Id { get; set; } //Identification
        [DataMember]
        public List<Transfer> Files { get; set; } //the list of the file

        public FilesTransfer()
        {
            Files = new List<Transfer>();
        }

        public FilesTransfer(Action action)
            : this()
        {
            Action = action;
        }
    }

    [DataContract]
    public class Transfer
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public long Length { get; set; }
        [DataMember]
        public string Catalog { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string FullPath { get; set; }


        public static explicit operator FileTransferring.TransferNamespace.Transfer(Transfer transfer)
        {
            return new FileTransferring.TransferNamespace.Transfer()
            {
                Catalog = transfer.Catalog,
                FullPath = transfer.FullPath,
                Id = transfer.Id,
                Length = transfer.Length,
                Name = transfer.Name
            };
        }
    }

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ITransfer
    {
        [OperationContract]
        IEnumerable<object> SendQuery(string ip, FilesTransfer transfer, int sport);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class TransferClass : ITransfer
    {
        AutoResetEvent tcontinue = new AutoResetEvent(false);
        TProgress form;

        public IEnumerable<object> SendQuery(string ip, FilesTransfer transfer, int sport)
        {
            int port = 0;
            switch (transfer.Action)
            {
                case Action.Invite:
                    {
                        port = HelpClass.GetAvailablePort();
                        if (MessageBox.Show(string.Format("Accept {0} files from {1}", transfer.Files.Count, transfer.Name), "Files", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ThreadStart start = delegate
                            {
                                form = new TProgress(null, port);
                                form.sport = sport;
                                form.Show();
                                form.GetThread = new Thread(new ParameterizedThreadStart(form.GetInvoke));
                                form.GetThread.Start(transfer);
                            };
                            HelpClass.Form.BeginInvoke(start);


                            break;
                        }
                        else return new List<object>() { (int)InviteRusult.Cancel, 0 };

                    }
                default:
                    {
                        return new List<object>() { (int)InviteRusult.Busy, 0 };
                    }
            }
            return new List<object>() { (int)InviteRusult.Ok, port };
        }
    }
}
