using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TravellingSalesmen
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

            string path = BASE_FOLDER_LOCATION + @"\" + configuration.Name + ".xml";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, configuration);
            }

        }

        public Configuration loadConfiguration(string name)
        {
            string path = BASE_FOLDER_LOCATION + @"\" + name + ".xml";

            Configuration configuration = null;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                configuration = (Configuration)formatter.Deserialize(fs);
            }
            return configuration;

        }
    }
}
