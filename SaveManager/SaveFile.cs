using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public String version = "1.0.0";
        public string username = "";
        public string password = "";
        public string streamlabsPath = @"D:\Streamlabs Chatbot\test\";
        //public string streamlabsPath = @"C:\Users\18diyann\Desktop\Test Ordner\";
        public ObservableCollection<KeyValuePair<int, string>> currentInstalled = new ObservableCollection<KeyValuePair<int, string>>();
        public List<string> ratedScripts = new List<string>();
        public List<string> accountScripts = new List<string>();

        private string path = "";

        public SaveFile(string _path)
        {
            path = _path;
            try
            {
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    SaveFile sf = (SaveFile)bf.Deserialize(file);
                    streamlabsPath = sf.streamlabsPath;
                    username = sf.username;
                    password = sf.password;
                    version = sf.version;

                    if (sf.currentInstalled != null)
                        currentInstalled = sf.currentInstalled;

                    if (sf.ratedScripts != null)
                        ratedScripts = sf.ratedScripts;
                }
            }
            catch { Console.WriteLine("Could not load SaveFile!"); }
        }

        public void Save()
        {
            try
            {
                using (FileStream file = File.Open(path, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, this);
                }
            }
            catch (Exception ex) { Console.WriteLine("Could not save SaveFile!: " + ex.StackTrace); }
        }
    }
}
