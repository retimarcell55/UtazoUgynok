using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TravellingSalesmans
{
    class FileManager
    {
        private const string BASE_FOLDER_LOCATION = @"Configurations";

        public FileManager()
        {

        }

        public Graph readGraphFromFile(String path)
        {
            int lines = File.ReadAllLines(path).Length;
            int[,] matrix = new int[lines, lines];

            using (StreamReader sr = new StreamReader(path))
            {
                for (int i = 0; i < lines; i++)
                {
                    String[] data = sr.ReadLine().Split(' ');
                    for (int j = 0; j < lines; j++)
                    {
                        matrix[i, j] = int.Parse(data[j]);
                    }
                }
            }

            return new Graph(matrix);
        }

        public AgentManager readAgentsFromFile(String path)
        {

            AgentManager agentManager = new AgentManager();

            using (StreamReader sr = new StreamReader(path))
            {
                String[] data = sr.ReadLine().Split(' ');
                for (int i = 0; i < data.Length; i++)
                {
                    agentManager.Agents.Add(new Agent(i, int.Parse(data[i])));
                }
            }

            return agentManager;

        }

        public void saveConfiguration(Configuration configuration)
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            var binary = "";

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    xmlSerializer.Serialize(writer, configuration);
                    xml = sw.ToString();
                }
            }

            string path = BASE_FOLDER_LOCATION + @"\" + configuration.Name + ".xml";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    sw.WriteLine(xml);
            }

        }

        public Configuration loadConfiguration(string name)
        {
            string path = BASE_FOLDER_LOCATION + @"\" + name + ".xml";

            Configuration configuration = null;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                configuration = (Configuration)xmlSerializer.Deserialize(fs);
            }

            return configuration;

        }
    }
}
