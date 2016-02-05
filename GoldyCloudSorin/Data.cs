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



    public class AccountInfo
    {

        public string ID = string.Empty;
        public string Name = string.Empty;
        public string Country = string.Empty;
        public string Email = string.Empty;

    }



    public class Element
    {
        public int Type = 0; // 0 folder, 1 file etc
        public string Size = string.Empty;
        public string Name = string.Empty;
       // public DateTime Modified = DateTime.Now;
        public string Modified = string.Empty;
        public string Created = string.Empty;
        public string Extension = string.Empty;
       // public int Revisions = 0;
        public string Revisions = string.Empty;
        //public bool IsPublic = false;
        public string IsPublic = string.Empty;
        //public bool IsShared = false;
        public string IsShared = string.Empty;

        public int IsRoot = 0;
    }


}
