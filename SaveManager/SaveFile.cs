using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SaveManager
{
    [Serializable]
    public class SaveFile
    {
        private string path = "";
        public string username = "";
        public string password = "";
        public string streamlabsPath = @"D:\Streamlabs Chatbot\";
        //public string streamlabsPath = @"C:\Users\18diyann\Desktop\Test Ordner\";
        public List<int> currentInstalled = new List<int>();

        public SaveFile(string _path)
        {
            path = _path;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                SaveFile sf = (SaveFile)bf.Deserialize(File.Open(path, FileMode.Open));
                streamlabsPath = sf.streamlabsPath;
                currentInstalled = sf.currentInstalled;
            }
            catch { }
        }

        public void Save()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(File.Open(path, FileMode.Create), this);
            }
            catch { }
        }
    }
}
