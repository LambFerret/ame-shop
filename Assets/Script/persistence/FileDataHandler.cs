using System;
using System.IO;
using Script.persistence.data;
using UnityEngine;

namespace Script.persistence
{
    public class FileDataHandler
    {
        private const string EncryptionCodeWord = "{I.私.c.안.は.녕.h.ㅏ.エ.b.세.i.ン.n.I.요.ジ.n.ニ.g.ア.e.で.n.す.i.e.u.r}";
        private readonly string _dataDirPath;
        private readonly string _dataFileName;
        private readonly bool _useEncryption;

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _useEncryption = useEncryption;
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (_useEncryption) dataToLoad = EncryptDecrypt(dataToLoad);
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }

            return loadedData;
        }

        public void Save(GameData data)
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption) dataToStore = EncryptDecrypt(dataToStore);

                using FileStream stream = new FileStream(fullPath, FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
                modifiedData += (char)(data[i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);

            return modifiedData;
        }
    }
}