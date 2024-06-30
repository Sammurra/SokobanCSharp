using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace libs
{
    public class DialogNode
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class DialogHandler
    {
        public List<DialogNode> DialogNodes { get; private set; }

        public void LoadDialog(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var dialogData = JsonConvert.DeserializeObject<Dictionary<string, List<DialogNode>>>(json);
            DialogNodes = dialogData["dialog"];
        }

        public void DisplayDialog()
        {
            foreach (var node in DialogNodes)
            {
                Console.Clear();
                Console.WriteLine(node.Text);
                Console.ReadKey(true);  // Wait for the user to press a key to proceed to the next dialog node
            }
        }
    }
}
