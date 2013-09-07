
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Note
{
    public class BinarySerialize
    {

       string strFile =Directory.GetCurrentDirectory()+"\\files.data";

       public void Serialize(Array allFiles)
        {
            using (FileStream fs = new FileStream(strFile, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, allFiles);
            }
        }

       public Array DeSerialize()
        {
            Array allFiles;
            using (FileStream fs = new FileStream(strFile, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                allFiles = (Array)formatter.Deserialize(fs);
            }
            return allFiles;
        }

    }
}
