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



    public class BusinessLayer
    {



        private DriveService Service;
        private static UserCredential Credential;
        public  string ApplicationName = "Sorin Drive API";
        public static About user;
        public  IList<Google.Apis.Drive.v2.Data.File> elem;
      


        public BusinessLayer()
        {
        }



        public void Login()
        {
            
            

           // creeaza si stocheaza obiectul credentiale
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart");
                Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.Drive},
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            //    Directory.Delete(credPath, true);
            }

            //creeaza serviciul 
            Service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = "Sorin Drive API",
            });

           

            
        }
        public AccountInfo GetAccountInfo()
        {
           AccountInfo info =new AccountInfo();
           user = Service.About.Get().Execute();
           info.ID = user.PermissionId;
           info.Name = user.User.DisplayName;
           info.Email = user.User.EmailAddress;
           info.Country = (new System.Globalization.RegionInfo(user.LanguageCode)).DisplayName;
          

            return info;
        }
        public List<Element> GetElements()
        {
            List<Element> elements = new List<Element>();
            FilesResource.ListRequest listRequest = Service.Files.List();
            listRequest.MaxResults = 100;
            elem = listRequest.Execute().Items;

            foreach (var file in elem)
            {
         //       if (file.Parents[0].IsRoot == true)
          //      {
                    Element element = new Element();
                    element.Name = file.Title;
                    element.Size = file.FileSize.ToString();
                    element.Modified = file.ModifiedDate.ToString();
                    element.Created = file.CreatedDate.ToString();
                    element.Extension = file.FileExtension;
                    element.Revisions = file.Version.ToString();

                    PermissionList permission = Service.Permissions.List(file.Id).Execute();
                    if(permission.Items.Count==1)
                    {
                        element.IsPublic = "Element privat";
                    }
                    else
                    {
                        element.IsPublic = "Elementul public";
                    }

                    if (file.Shared == true)
                    {
                        element.IsShared = "Element share-uit cu : ";
                        for (int i = 0; i < permission.Items.Count; i++)
                        { 
                            if(permission.Items[i].EmailAddress != null)
                            {
                                element.IsShared += permission.Items[i].EmailAddress + " ";
                            }

                        }

                    }
                    else
                    {
                        element.IsShared = "Element ne share-uit";
                    }
                    if (file.Parents[0].IsRoot == true)
                    {
                        element.IsRoot = 1;
                    }

                    elements.Add(element);
             //   }

            }

            //IList<Google.Apis.Drive.v2.Data.File> files = listFiles.show();
            //foreach (var file in files)
            //{
            //    Element element = new Element();
            //    element.Name = file.Title;
            //    element.Size = file.FileSize;
            //    element.Modified = file.ModifiedDate;
            //    elements.Add(element);
            //}
            /*
            Element element = new Element();
            element.Name = "FOLDER 1";
            elements.Add(element);

            element = new Element();
            element.Name = "FOLDER 2";
            elements.Add(element);

            element = new Element();
            element.Name = "file 1";
            elements.Add(element);

            element = new Element();
            element.Name = "file 2";
            elements.Add(element);

            element = new Element();
            element.Name = "file 3";
            elements.Add(element);
            */
            return elements;
        }

        public string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public  Google.Apis.Drive.v2.Data.File getFile(string getThisFile)
        {
            foreach (var item in elem)
            {
                if (getThisFile == item.Title)
                    return item;
            }
            return null;
        }

        public  Google.Apis.Drive.v2.Data.File getFilebyID(string Id)
        {
            foreach (var item in elem)
            {
                if (Id == item.Id)
                    return item;
            }
            return null;
        }

        public void UploadFile(string file, string _parent)
        {
            Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
            body.Title = System.IO.Path.GetFileName(file);
            body.Description = "File uploaded by Diamto Drive Sample";
            body.MimeType = GetMimeType(file);
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = _parent } };

            //// File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(file);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);


            FilesResource.InsertMediaUpload request = Service.Files.Insert(body, stream, GetMimeType(file));
            //request.Convert = true;   // uncomment this line if you want files to be converted to Drive format
            request.Upload();
         ///////   return request.ResponseBody;
        }

   



        public void SaveFile(string localName, string remoteName)
        {

            
            Google.Apis.Drive.v2.Data.File fFile = getFile(remoteName);

            if (!String.IsNullOrEmpty(fFile.DownloadUrl))
            {

                var x = Service.HttpClient.GetByteArrayAsync(fFile.DownloadUrl);
                byte[] arrBytes = x.Result;
                System.IO.File.WriteAllBytes(localName + "." + fFile.FileExtension.ToString(), arrBytes);
          //      return true;

            }
          //  else
          //  {
                // The file doesn't have any content stored on Drive.
             //  return false;
           // }
        }

        public  Permission InsertPermission(String fileId, String value,
     String type, String role)
        {
            Permission newPermission = new Permission();
            newPermission.Value = value;
            newPermission.Type = type;
            newPermission.Role = role;
            try
            {
                return Service.Permissions.Insert(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }

        public  void DeleteFile(String fileId)
        {
            //   try
            //    {
            Service.Files.Delete(fileId).Execute();
            //  }
            //  catch (Exception e)
            //   {
            //       Console.WriteLine("An error occurred: " + e.Message);
            //   }
        }

        public  Google.Apis.Drive.v2.Data.File CopyFile(String originFileId, String copyTitle)
        {
            Google.Apis.Drive.v2.Data.File copiedFile = new Google.Apis.Drive.v2.Data.File();
            copiedFile.Title = copyTitle;
       //     try
       //     {
                return Service.Files.Copy(copiedFile, originFileId).Execute();
                
       //     }
       //     catch (Exception e)
       //     {
       //         Console.WriteLine("An error occurred: " + e.Message);
       //     }
       //     return null;
        }

        public  ParentReference insertFileIntoFolder(String folderId, String fileId)
        {
            ParentReference newParent = new ParentReference();
            newParent.Id = folderId;
         //   try
        //    {
                return Service.Parents.Insert(newParent, fileId).Execute();
        //    }
       //     catch (Exception e)
       //     {
       //         Console.WriteLine("An error occurred: " + e.Message);
       //     }
      //      return null;
        }

        public void RemoveParrent(String folderId, String fileId)
        {
            Service.Parents.Delete(fileId, folderId).Execute();
        }
        


    }
}
