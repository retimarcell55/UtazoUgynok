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

        public CompleteGraph readGraphFromFile(String path)
        {
            int lines = File.ReadAllLines(path).Length;
            List<Vertex> vertices = new List<Vertex>();

            using (StreamReader sr = new StreamReader(path))
            {
                for (int i = 0; i < lines; i++)
                {
                    String[] data = sr.ReadLine().Split(' ');
                    vertices.Add(new Vertex(i, new Coordinate(int.Parse(data[0]), int.Parse(data[1]))));
                }
            }

            return new CompleteGraph(vertices);
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

            string path = BASE_FOLDER_LOCATION + @"\" + configuration.Name + ".txt";

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
            string path = BASE_FOLDER_LOCATION + @"\" + name + ".txt";

            Configuration configuration = null;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                configuration = (Configuration)formatter.Deserialize(fs);
            }
            return configuration;

        }

        public List<Configuration> loadConfigurations()
        {
            string[] fileNames = Directory.GetFiles(BASE_FOLDER_LOCATION, "*.txt")
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToArray();

            List<Configuration> configurations = new List<Configuration>();

            for (int i = 0; i < fileNames.Length; i++)
            {
                configurations.Add(loadConfiguration(fileNames[i]));
            }

            return configurations;
        }
    }
}
