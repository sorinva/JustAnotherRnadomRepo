using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoldyCloud
{
    public partial class PrezentationForm : Form
    {



        public PrezentationForm()
        {
            InitializeComponent();
        }




       public  static Google.Apis.Drive.v2.Data.File curFolder;
       private BusinessLayer Business = new BusinessLayer();


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Business.Login();
            AccountInfo info = Business.GetAccountInfo();
            textBoxAccount.Text = "ID: " + info.ID + ", " + "Name: " + info.Name + ", " + "Email: " + info.Email + ", " + "Country: " + info.Country;

            label1.Visible = true;
            label2.Visible = true;
            textBoxAccount.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBoxElement.Visible = true;
            listBoxCloud.Visible = true;
            buttonList.Visible = true;
            buttonUpload.Visible = true;
            buttonDownload.Visible = true;
            buttonShare.Visible = true;
            buttonCopy.Visible = true;
            buttonMove.Visible = true;
            buttonDelete.Visible = true;
            button1.Visible = true;
            
        }





        List<Element> RootElements = new List<Element>();

        private void buttonList_Click(object sender, EventArgs e)
        {
            listBoxCloud.Items.Clear();
            RootElements = Business.GetElements();

            foreach (Element element in RootElements)
                if (element.IsRoot == 1)
                {
                    listBoxCloud.Items.Add(element.Name);
                }
        }





        private void buttonUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
          
           
            Business.UploadFile(dialog.FileName,curFolder.Id);

            listBoxCloud.Items.Clear();
            RootElements = Business.GetElements();

            foreach (Element element in RootElements)
                if (element.IsRoot == 1)
                {
                    listBoxCloud.Items.Add(element.Name);
                }
            
        }




        private void buttonDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;



            Business.SaveFile(dialog.FileName, listBoxCloud.SelectedItem.ToString());

        }





        private void listBoxCloud_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCloud.SelectedItem == null)
                return;

            string name = listBoxCloud.SelectedItem.ToString();

            foreach (Element element in RootElements)
                if (element.Name == name)
                {
                    textBoxElement.Text = "Name: " + element.Name + "\r\nSize: " + element.Size + 
                        "\r\nCreatedDate: " + element.Created + "\r\nModifiedDate: " + element.Modified +
                        "\r\nExtension: " + element.Extension + "\r\nIsPublic: " + element.IsPublic +
                        "\r\nIsShared: " + element.IsShared + "\r\nRevisions: " + element.Revisions;
                    break;
                }


        }





        private void listBoxCloud_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            string curItem = listBoxCloud.SelectedItem.ToString();
            Google.Apis.Drive.v2.Data.File fFile = Business.getFile(curItem);
            if (fFile.MimeType.ToString() == "application/vnd.google-apps.folder")
            {
                curFolder = fFile;
                listBoxCloud.Items.Clear();
                foreach (var file in Business.elem)
                {
                    if (file.Parents.Count != 0)
                    {
                        if (file.Parents[0].Id == fFile.Id)
                            listBoxCloud.Items.Add(file.Title);
                    }
                }

            }


        }





        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esti sigur ca vrei stergerea elementului?") != DialogResult.OK)
                return;

            Google.Apis.Drive.v2.Data.File fFile;
            fFile = Business.getFile(listBoxCloud.SelectedItem.ToString());
            Business.DeleteFile(fFile.Id);

            listBoxCloud.Items.Clear();
            RootElements = Business.GetElements();

            foreach (Element element in RootElements)
                if (element.IsRoot == 1)
                {
                    listBoxCloud.Items.Add(element.Name);
                }


        }

        private void textBoxElement_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            listBoxCloud.Items.Clear();
            Google.Apis.Drive.v2.Data.File fFile = Business.getFile(curFolder.Title);
            if (fFile.Parents[0].IsRoot == true)
                foreach (var file in Business.elem)
                {
                    if (file.Parents.Count != 0)
                    {
                        if (file.Parents[0].IsRoot == true)
                            listBoxCloud.Items.Add(file.Title);
                    }
                }
            else
            {
                curFolder = Business.getFilebyID(fFile.Parents[0].Id);
                foreach (var file in Business.elem)
                {
                    if (file.Parents[0].Id == curFolder.Id)
                        listBoxCloud.Items.Add(file.Title);

                }
            }
        
        }

        private void textBoxAccount_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonShare_Click(object sender, EventArgs e)
        {
      //      Business.getFile(listBoxCloud.SelectedItem.ToString());
            Google.Apis.Drive.v2.Data.File fFile;
            if(textBox1.Text.ToString() == "")
                MessageBox.Show("Nu este introdus e-mailul user-ului");

            fFile = Business.getFile(listBoxCloud.SelectedItem.ToString());
            Business.InsertPermission(fFile.Id, textBox1.Text.ToString(), "user", "reader");

            listBoxCloud.Items.Clear();
            RootElements = Business.GetElements();

            foreach (Element element in RootElements)
                if (element.IsRoot == 1)
                {
                    listBoxCloud.Items.Add(element.Name);
                }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Google.Apis.Drive.v2.Data.File fFile = Business.getFile(listBoxCloud.SelectedItem.ToString());

            Business.CopyFile(fFile.Id, listBoxCloud.SelectedItem.ToString() + "COPIE");

            listBoxCloud.Items.Clear();
            RootElements = Business.GetElements();

            foreach (Element element in RootElements)
                if (element.IsRoot == 1)
                {
                    listBoxCloud.Items.Add(element.Name);
                }
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            string parrentID = string.Empty;

            Google.Apis.Drive.v2.Data.File fFile = Business.getFile(textBox2.Text.ToString());

           

            Google.Apis.Drive.v2.Data.File fileToInsert = Business.getFile(listBoxCloud.SelectedItem.ToString());

            foreach (ParentReference parinte in fileToInsert.Parents)
            {
                parrentID = parinte.Id;
            }

            Business.RemoveParrent(parrentID, fileToInsert.Id);


            if (fFile.MimeType == "application/vnd.google-apps.folder")
            {
                Business.insertFileIntoFolder(fFile.Id, fileToInsert.Id);

            }
            else
            {
                MessageBox.Show("Nu a fost introdus un nume de folder in campul aferent");
            }

            listBoxCloud.Items.Clear();
            RootElements = Business.GetElements();

            foreach (Element element in RootElements)
                if (element.IsRoot == 1)
                {
                    listBoxCloud.Items.Add(element.Name);
                }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void PrezentationForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        





    }
}
