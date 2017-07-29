using System;
using System.Windows.Forms;
using System.Drawing;
using System.ServiceModel;
using System.Configuration;
using System.IO;
using FileTransferring;
using CRYBlowFish;
namespace prosnap
{
    [ServiceContract(CallbackContract = typeof(IChatService))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Openup(string filename, string usersent);
        [OperationContract(IsOneWay = true)]
        void Join(string memberName);
        [OperationContract(IsOneWay = true)]
        void Leave(string memberName);
        [OperationContract(IsOneWay = true)]
        void SendMessage(string memberName, string message, string request, string usersent);
    }

    public interface IChatChannel : IChatService, IClientChannel
    {
    }
    public partial class prosnap1 : Form, IChatService
    {
        String file;
        private const string myKey = "A";
      //  private long m_originalLength;
        Boolean closee;
      //  static int k;
        public static Form1 f;
        public String zipfile;
        ChooseMethod frmMethod;
        Boolean booladd = false;
        public delegate void UserJoined(string name);
        private delegate void UserSendMessage(string name, string message, string request, string usersent);
        private delegate void UserLeft(string name);
        private delegate void UserOpenup(string filename, string usersent);

        TextBox txtUserName = new TextBox();
        //ChatServer.ChatServer c;
        public static event UserJoined NewJoin;
        private static event UserSendMessage MessageSent;
        private static event UserLeft RemoveUser;
        private static event UserOpenup Openupuser;

        String temppath = ConfigurationSettings.AppSettings["filesreceived"];
        String ImagePath = ConfigurationSettings.AppSettings["fileslist"];
      //  Bitmap b;
        public string userName;
        public IChatChannel channel;
        public DuplexChannelFactory<IChatChannel> factory;
        public prosnap1()
        {
            InitializeComponent();
        }
        public prosnap1(string userName)
        {
            this.userName = userName;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            String[] imagetype = { "*.bmp", "*.gif", "*.jpg", "*.tif" };



            string[] filePaths = Directory.GetFiles(ConfigurationSettings.AppSettings["zipfiles"]);

            foreach (string filePath in filePaths)
                File.Delete(filePath);

            // TODO: This line of code loads data into the 'imageSecurityDataSet.FileDetails' table. You can move, or remove it, as needed.
            this.fileDetailsTableAdapter.Fill(this.imageSecurityDataSet.FileDetails);
            DirectoryInfo Folder;
            FileInfo[] Images=null;
            Folder = new DirectoryInfo(ConfigurationSettings.AppSettings["fileslist"]);

            foreach (String imgpat in imagetype)
            {
                Images = Folder.GetFiles(imgpat);

                for (int x = 0; x < Images.Length; x++)
                {
                    listBox2.Items.Add(Images[x].Name);

                }
            }
            Imageload();
            NewJoin += new UserJoined(ChatClient_NewJoin);
            MessageSent += new UserSendMessage(ChatClient_MessageSent);
            RemoveUser += new UserLeft(ChatClient_RemoveUser);
            Openupuser += new UserOpenup(ChatClient_openuser);

            txtUserName.Text = Convert.ToString(ConfigurationSettings.AppSettings["Name"]);
            channel = null;
            this.userName = txtUserName.Text.Trim();
            InstanceContext context = new InstanceContext(
                new prosnap1(txtUserName.Text.Trim()));
            factory =
                new DuplexChannelFactory<IChatChannel>(context, "ChatEndPoint");
            channel = factory.CreateChannel();
            IOnlineStatus status = channel.GetProperty<IOnlineStatus>();
            status.Offline += new EventHandler(Offline);
            status.Online += new EventHandler(Online);
            try
            {
                channel.Open();
            }
            catch (CommunicationException )
            {
                MessageBox.Show("Server Offline", "Server Error", MessageBoxButtons.OK);
                Environment.Exit(0);
            }

            channel.Join(this.userName);
            f = new Form1();
            f.ButtonClicked += new EventHandler(ob_ButtonClicked);
            f.Visible = false;
            f.Show();
            f.Hide();






        }

        private void Imageload()
        {
            listView1.Bounds = new Rectangle(new Point(10, 10), new Size(400, 400));
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.AllowColumnReorder = true;
            listView1.CheckBoxes = false;
            listView1.FullRowSelect = true;
            listView1.GridLines = false;
            listView1.Sorting = SortOrder.Ascending;
            this.listView1.View = View.LargeIcon;

            this.imageList1.ImageSize = new Size(50, 50);

            int i = 0;
            ImageList imageListSmall = new ImageList();
            listView1.Columns.Add("Image List", -2, HorizontalAlignment.Left);
            String[] filesreceived = System.IO.Directory.GetFiles(temppath);
            foreach (String filename in filesreceived)
            {

                String filen = filename.Remove(0, 17);
                filen = filen.Remove(0, 1);
                ListViewItem item1 = new ListViewItem(filen, i);
                i++;
                listView1.Items.AddRange(new ListViewItem[] { item1 });
                imageListSmall.Images.Add(Bitmap.FromFile(filename));
                imageList1.Images.Add(Bitmap.FromFile(filename));
                listView1.LargeImageList = imageListSmall;
                this.listView1.LargeImageList = this.imageList1;
            }
        }


        void Online(object sender, EventArgs e)
        {
            ////rtbMessages.AppendText("\r\nOnline: " + this.userName);
        }

        void Offline(object sender, EventArgs e)
        {
            ////rtbMessages.AppendText("\r\nOffline: " + this.userName);
        }
        public void ChatClient_NewJoin(string name)
        {
            if (userName != name)
            {
                listBox1.Items.Add(name);
            }
            channel.SendMessage(this.userName, "", "", "");
        }

        public void ChatClient_openuser(string filename, string usersent)
        {
            channel.Openup(filename, usersent);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SearchListView();
        }

        private void SearchListView()
        {
            Boolean testfile = false;
            foreach (String filename in listBox2.Items)
            {
                String extension = System.IO.Path.GetExtension(filename);
                String result = filename.Substring(0, filename.Length - extension.Length);
                extension = System.IO.Path.GetExtension(txtsearch.Text);
                String search = txtsearch.Text.Substring(0, txtsearch.Text.Length - extension.Length);
                if (search.ToLower() == result.ToLower())
                {
                    listBox2.SelectedItem = filename;
                    txtsetfile.Text = filename;
                    testfile = true;
                    break;
                }
            }
            if (testfile == false)
            {
                MessageBox.Show("Image Not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ChatClient_MessageSent(string name, string message, string request, string usersent)
        {
            if (!listBox1.Items.Contains(name) && txtUserName.Text != name)
            {
                listBox1.Items.Add(name);
            }
            if (txtUserName.Text == usersent)
            {
                booladd = true;
                this.fileDetailsTableAdapter.InsertQuery(name, message, request);
                this.fileDetailsTableAdapter.Fill(this.imageSecurityDataSet.FileDetails);
                dataGridView1.Refresh();
                booladd = false;
            }
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
                txtsetfile.Text = listBox2.SelectedItem.ToString();
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            String filename = ((System.Windows.Forms.ListView)(sender)).FocusedItem.Text;
            
            string filename1 = filename.Substring(filename.LastIndexOf(('\\')) + 1);
            filename = temppath + filename1;
            
            imagePanel1.Image = new Bitmap(filename);
            imagePanel1.Zoom = trackBar1.Value * 0.02f;


        }
        void ChatClient_RemoveUser(string name)
        {
            try
            {
                listBox1.Items.Remove(name);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            imagePanel1.Zoom = trackBar1.Value * 0.02f;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            //   c.btnStop_Click(sender, e);
            this.Close();
        }

        private void btnmaxmin_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void imagePanel1_MouseClick(object sender, MouseEventArgs e)
        {

            String filename;
            if (listView1.FocusedItem != null)
            {
                int index = listView1.FocusedItem.Index;
                if (index < (listView1.Items.Count - 1))
                {
                    filename = listView1.Items[(index + 1)].Text;
                    listView1.Items[(index + 1)].Focused = true;

                }
                else
                {
                    filename = listView1.Items[0].Text;
                    listView1.Items[0].Focused = true;
                }
                filename = temppath + filename;
                imagePanel1.Image = new Bitmap(filename);
                imagePanel1.Zoom = trackBar1.Value * 0.02f;
            }
        }

        private void txtsearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchListView();
            }
        }

        #region IChatService Members

        public void Join(string memberName)
        {
            if (NewJoin != null)
            {
                NewJoin(memberName);
            }
        }

        public new void Leave(string memberName)
        {
            if (RemoveUser != null)
            {
                RemoveUser(memberName);
            }
        }

        public void SendMessage(string memberName, string message, string request, string usersent)
        {
            if (MessageSent != null && message != "")
            {
                MessageSent(memberName, message, usersent, request);


            }
            else
            {
                MessageSent(memberName, message, usersent, request);
            }
        }
        public void Openup(string filename, string usersent)
        {
            if (txtUserName.Text != usersent)
            {

                txtUserName.Text = Convert.ToString(ConfigurationSettings.AppSettings["Name"]);
                if (usersent == txtUserName.Text)
                {

                }
                else
                {



                    System.Threading.Thread.Sleep(1000);

                    f.callup(filename, usersent);



                }

            }
        }
        void ob_ButtonClicked(object sender, EventArgs e)
        {
            String newzipfile = zipfile.Substring(zipfile.LastIndexOf('\\'), (zipfile.Length - zipfile.LastIndexOf('\\')));
            newzipfile = ConfigurationSettings.AppSettings["ReceivedFile"] + newzipfile;
            channel.SendMessage(this.txtUserName.Text, newzipfile, this.txtuserna.Text, "received");


            string[] filePaths = Directory.GetFiles(ConfigurationSettings.AppSettings["zipfiles"]);

            foreach (string filePath in filePaths)
                File.Delete(filePath);

            filePaths = Directory.GetFiles(ConfigurationSettings.AppSettings["Converted"]);
            foreach (string filePath in filePaths)
                File.Delete(filePath);

        }
        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                txtuserna.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            channel.SendMessage(txtUserName.Text, txtreqmessage.Text.Trim(), txtuserna.Text, "request");
            txtreqmessage.Clear();
            txtreqmessage.Select();
            txtreqmessage.Focus();
        }

        private void prosnap_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (channel != null)
                {
                    channel.Leave(this.userName);
                    listBox1.Items.Remove(userName);
                    channel.Close();
                }
                if (factory != null)
                {
                    factory.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtsetfile.Text.Trim()))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtsetfile, "Select the Image Name");
            }
            else if (string.IsNullOrEmpty(txtuserna.Text.Trim()))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtuserna, "Select the User Name");
            }
            else if (string.IsNullOrEmpty(txtrequser.Text.Trim()))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtrequser, "Select the  Requested User Name");
            }

            else
            {
               
                   
                    if((ConfigurationSettings.AppSettings[txtuserna.Text.ToString()])==null )
                    {
                         MessageBox.Show("Selected user name not matches!!!","Check?");
                         return;
                    }

                    
                    if((ConfigurationSettings.AppSettings[txtrequser.Text.ToString()])==null )
                    {
                         MessageBox.Show("Requested user name not matches!!!","Check?");
                         return;
                    }

                    if (!File.Exists(ImagePath + txtsetfile.Text.ToString()))
                    {
                        MessageBox.Show("File not exists!!!", "Check?");
                        return;
                    }
            
             
                errorProvider1.Clear();
                frmMethod = new ChooseMethod(txtsetfile.Text, txtUserName.Text, txtrequser.Text);
                frmMethod.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                frmMethod.ShowDialog();

                frmMethod.Refresh();
                if (closee != true)
                {
                    if (listBox1.Items.Contains(txtuserna.Text) && listBox1.Items.Contains(txtrequser.Text))
                        upload(zipfile, txtuserna.Text);
                    else
                    {
                        MessageBox.Show("User Offline", "Check!!!");
                        return;
                    }

                }
            }
        }
        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            closee = frmMethod.closee;
            if (closee != true)
            {
                this.zipfile = frmMethod.returnzipfile;
            }
        }
        void upload(String zipfile, String usersent)
        {
            channel.Openup(zipfile, usersent);
        }
        void decrypt(String filename)
        {
            long len;
            FileStream originalStream = File.OpenRead(filename);
            len = originalStream.Length;
            BlowFish alg = new BlowFish("04B915BA43FEB5B6");
            Byte[] buffer = new byte[originalStream.Length];
            originalStream.Read(buffer, 0, buffer.Length);
            originalStream.Close();
            Byte[] cbuffer = alg.Decrypt_ECB(buffer);

            if (filename.EndsWith(".xml"))
            {
                FileStream stream = new FileStream(ImagePath+@"temp.txt", FileMode.Create);
                stream.Write(cbuffer, 0, (int)cbuffer.Length); //Dangerous casting - Write in chunks.  
                stream.Close();

                String[] s = File.ReadAllLines(ImagePath+@"temp.txt");
                String t = "";
                foreach (char c in s[s.Length - 1])
                {
                    if (c == '\0')
                        break;
                    else
                        t += c;
                }
                s[s.Length - 1] = t;

                File.WriteAllLines(filename, s);
                File.Delete(ImagePath+@"temp.txt");

            }
            else
            {
                FileStream stream = new FileStream(filename, FileMode.Create);
                stream.Write(cbuffer, 0, (int)cbuffer.Length); //Dangerous casting - Write in chunks.  
                stream.Close();

            }
        }
        String[] callunzip(String zipFileName)
        {
            sbyte[] buf = new sbyte[1024];
            int len;
            java.io.FileInputStream fis = new java.io.FileInputStream(zipFileName);
            java.util.zip.ZipInputStream ziss = new java.util.zip.ZipInputStream(fis);
            java.util.zip.ZipEntry ze;
            String[] fileName = new String[10];
            int j = 0;
            while ((ze = ziss.getNextEntry()) != null)
            {
                fileName[j] = ze.getName().ToString();
                j++;
            }
            fis = new java.io.FileInputStream(zipFileName);
            java.util.zip.ZipInputStream zis = new java.util.zip.ZipInputStream(fis);
            String path = zipFileName.Substring(zipFileName.LastIndexOf("\\") + 1);
            path = path.Substring(0, path.LastIndexOf("."));
            String extension = System.IO.Path.GetExtension(path);
            path = path.Substring(0, path.LastIndexOf("."));
            int i = 0;
            while ((ze = zis.getNextEntry()) != null)
            {
                if (fileName[i] == ze.getName())
                {
                    // File name format in zip file is:
                    // folder/subfolder/filename
                    // Let's check...
                    int index = fileName[i].LastIndexOf('/');
                    if (index > 1)
                    {
                        string folder = fileName[i].Substring(0, index);
                        folder = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + folder;
                        DirectoryInfo di = new DirectoryInfo(folder);
                        // Create directory if not exists
                        if (!di.Exists)
                            di.Create();

                    }
                    java.io.FileOutputStream fos = new java.io.FileOutputStream(ConfigurationSettings.AppSettings["zipfiles"] + fileName[i]);
                    while ((len = zis.read(buf)) >= 0)
                    {
                        fos.write(buf, 0, len);
                    }
                    fos.close();

                }
                i++;
            }
            zis.close();
            fis.close();
            fileName[1] = fileName[1].Substring(fileName[1].LastIndexOf('/') + 1, fileName[1].Length - fileName[1].LastIndexOf('/') - 1);
            return fileName;
        }
        int callmethod(int num, String[] imagelocat)
        {
           // String temp;
            String seq = ConfigurationSettings.AppSettings["Name"];
            seq = ConfigurationSettings.AppSettings[seq + "key"];
            int[] sequence = { 0, 0, 0, 0 };
            sequence[0] = Convert.ToInt32(seq.Substring(0, 1));
            sequence[1] = Convert.ToInt32(seq.Substring(2, 1));
            sequence[2] = Convert.ToInt32(seq.Substring(4, 1));
            sequence[3] = Convert.ToInt32(seq.Substring(6, 1));

            try
            {

                if (num == sequence[0])
                {
                    int count = 2;
                    int tempcount = 0;
                    for (tempcount = 0; imagelocat[tempcount] != null; )
                    {
                        tempcount++;

                    }

                    if (count == tempcount - 1)
                    {
                        String files = callvertical(imagelocat[1]);
                        file = files;
                        return 1;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                else if (num == sequence[1])
                {
                    int count = 3;
                    int tempcount = 0;
                    for (tempcount = 0; imagelocat[tempcount] != null; )
                    {
                        tempcount++;

                    }

                    if (count == tempcount - 1)
                    {
                        String files1 = callhorizontal(imagelocat[1]);
                        file = files1;
                        return 1;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                else if (num == sequence[2])
                {
                    int count = 5;
                    int tempcount = 0;
                    for (tempcount = 0; imagelocat[tempcount] != null; )
                    {
                        tempcount++;

                    }

                    if (count == tempcount - 1)
                    {
                        String files2 = callquadtree(imagelocat);
                        file = files2;
                        return 1;
                    }
                    else
                    {


                        throw new Exception();
                    }


                }
                else if (num == sequence[3])
                {
                    int count = 1;
                    int tempcount = 0;
                    for (tempcount = 0; imagelocat[tempcount] != null; )
                    {
                        tempcount++;

                    }

                    if (count == tempcount - 1)
                    {
                        String files3 = callbox(imagelocat[1]);
                        file = files3;
                        return 1;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                else
                {
                    MessageBox.Show("Format Not Supported", "Invalid Formate", MessageBoxButtons.OK);
                    return 0;
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Access Denied", "Invalid Access", MessageBoxButtons.OK);
                return 0;
            }
        }

        String callvertical(String imagelocat)
        {

            string sFileName_open = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + imagelocat;
            string sFileName_Save1 = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + imagelocat;
            string sFileName_Save2 = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "N" + imagelocat;
            int code = sFileName_Save1.LastIndexOf('.');
            sFileName_Save1 = sFileName_Save1.Remove(code - 1, 1);
            int code1 = sFileName_Save2.LastIndexOf('.');
            sFileName_Save2 = sFileName_Save2.Remove(code1 - 1, 1);
            unshred c = new unshred(sFileName_open, sFileName_Save1);
            c = new unshred(sFileName_Save1, sFileName_Save2);
            c = null;
            return sFileName_Save2;

        }

        String callhorizontal(String imagelocat)
        {
            string sFileName_open = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + imagelocat;
            String temp = imagelocat;
            temp = temp.Remove(temp.LastIndexOf('.') - 1, 1);
            temp = temp.Insert(temp.LastIndexOf('.'), "1");
            string sFileName_Save1 = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + temp;
            temp = temp.Remove(temp.LastIndexOf('.') - 1, 1);
            string sFileName_Save2 = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + temp;
            string sFileName_Save3 = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "N" + temp;
            int code = sFileName_Save3.LastIndexOf('.');
            sFileName_Save3 = sFileName_Save3.Remove(code - 1, 1);
            unshred c = new unshred(sFileName_open, sFileName_Save1);
            c = new unshred(sFileName_Save1, sFileName_Save2);
            c = new unshred(sFileName_Save2, sFileName_Save3);
            c = null;
            return sFileName_Save3;
        }

        String callquadtree(String[] imagelocat)
        {
            string sFileName_open = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + imagelocat[5];
            string sFileName_Save = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "5" + imagelocat[5];
            unshred c = new unshred(sFileName_open, sFileName_Save, imagelocat);
            sFileName_open = sFileName_Save;
            sFileName_Save = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "4" + imagelocat[5];
            c = new unshred(sFileName_open, sFileName_Save, imagelocat);
            sFileName_open = sFileName_Save;
            sFileName_Save = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "3" + imagelocat[5];
            c = new unshred(sFileName_open, sFileName_Save, imagelocat);
            sFileName_open = sFileName_Save;
            sFileName_Save = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "2" + imagelocat[5];
            c = new unshred(sFileName_open, sFileName_Save, imagelocat);
            sFileName_open = sFileName_Save;
            sFileName_Save = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "1" + imagelocat[5];
            c = new unshred(sFileName_open, sFileName_Save, imagelocat);
            String temp = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]);
            sFileName_Save.Remove(0, temp.Length + 2);
            return sFileName_Save;
        }

        String callbox(String imagelocat)
        {
            string sFileName_open = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + imagelocat;
            string sFileName_Save = Convert.ToString(ConfigurationSettings.AppSettings["zipfiles"]) + "N" + imagelocat;
            int code = sFileName_Save.LastIndexOf('.');
            sFileName_Save = sFileName_Save.Remove(code - 1, 1);
            unshred c = new unshred(sFileName_open, sFileName_Save);
            c = null;
            return sFileName_Save;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (booladd == false)
            {
                if (e.RowIndex != -1)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "received")
                    {
                        String filena = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                        if (!File.Exists(filena))
                        {
                            MessageBox.Show("File Not Exists", "Check!!!");
                            return;
                        }
                        String[] path = callunzip(filena);
                        foreach (String s in path)
                        {
                            if (s != null)
                                decrypt(ConfigurationSettings.AppSettings["zipfiles"] + s);
                            else
                                break;
                        }
                        int code = 0;
                        int returnvalue;
                        String tempat="";
                        if (path[4] == null)
                        {

                            code = path[1].LastIndexOf('.');
                            code = Convert.ToInt32(path[1].Substring(code - 1, 1));

                            returnvalue = callmethod(code, path);
                            if (returnvalue != 0)
                            {
                                tempat = file;
                                tempat = tempat.Substring(tempat.LastIndexOf('\\'), tempat.Length - tempat.LastIndexOf('\\'));
                                tempat = tempat.Remove(0, 2);
                            }
                        }
                        else
                        {
                            code = filena.LastIndexOf('.');
                            int k = code - 1;
                            while (filena[k] != '.')
                            {
                                k--;
                            }
                            code = k;
                            code = Convert.ToInt32(filena.Substring(code - 1, 1));
                            returnvalue = callmethod(code, path);
                            if (returnvalue != 0)
                            {
                                tempat = path[5];
                                tempat = tempat.Remove(0, 1);
                            }


                        }

                        if (returnvalue == 1)
                        {

                            if (File.Exists(ConfigurationSettings.AppSettings["filesreceived"] + tempat))
                            {
                                FileExists fobj = new FileExists();
                                fobj.textBox1.Text = tempat;
                                fobj.ShowDialog();

                                if (fobj.newfile == true)
                                {
                                    tempat = fobj.newfilename;
                                    File.Copy(file, (ConfigurationSettings.AppSettings["filesreceived"] + tempat));
                                    file = ConfigurationSettings.AppSettings["filesreceived"] + tempat;
                                }
                                else
                                {
                                    returnvalue = 0;
                                }
                            }
                            else
                            {
                                File.Copy(file, ConfigurationSettings.AppSettings["filesreceived"] + tempat);
                                file = ConfigurationSettings.AppSettings["filesreceived"] + tempat;
                            }
                        }

                        if (returnvalue == 1)
                        {
                            ImageList imageListSmall = new ImageList();
                            String filen = file.Remove(0, ConfigurationSettings.AppSettings["filesreceived"].Length);

                            ListViewItem item1 = new ListViewItem(filen, listView1.Items.Count);
                            listView1.Items.AddRange(new ListViewItem[] { item1 });
                            imageListSmall.Images.Add(Bitmap.FromFile(file));
                            imageList1.Images.Add(Bitmap.FromFile(file));
                            listView1.LargeImageList = imageListSmall;
                            this.listView1.LargeImageList = this.imageList1;

                            File.Delete(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                            int slno = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                            fileDetailsTableAdapter.DeleteQuery(slno);
                            this.fileDetailsTableAdapter.Fill(this.imageSecurityDataSet.FileDetails);
                            dataGridView1.Refresh();
                        }
                    }
                    else
                    {
                        txtrequser.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    }
                }
            }
        }

        private void txtuserna_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
