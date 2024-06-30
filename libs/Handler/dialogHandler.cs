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
        public List<DialogChoice> Choices { get; set; }
        public int? NextId { get; set; }
    }

    public class DialogChoice
    {
        public string Text { get; set; }
        public int NextId { get; set; }
        public bool ExitGame { get; set; }  // Add this property to handle exiting the game
    }

    public class DialogHandler
    {
        private Dictionary<int, DialogNode> dialogNodes;

        public void LoadDialog(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var dialogData = JsonConvert.DeserializeObject<Dictionary<string, List<DialogNode>>>(json);
            dialogNodes = new Dictionary<int, DialogNode>();
            foreach (var node in dialogData["dialog"])
            {
                dialogNodes[node.Id] = node;
            }
        }

        public void DisplayDialog()
        {
            DisplayNode(dialogNodes[1]);
        }

        private void DisplayNode(DialogNode node)
        {
            Console.Clear();
            Console.WriteLine(node.Text);

            if (node.Choices != null && node.Choices.Count > 0)
            {
                for (int i = 0; i < node.Choices.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {node.Choices[i].Text}");
                }

                int choice = GetChoice(node.Choices.Count);
                if (node.Choices[choice].ExitGame)
                {
                    Console.WriteLine("Exiting game...");
                    Environment.Exit(0);
                }
                DisplayNode(dialogNodes[node.Choices[choice].NextId]);
            }
            else if (node.NextId.HasValue)
            {
                Console.ReadKey(true);
                DisplayNode(dialogNodes[node.NextId.Value]);
            }
            else
            {
                Console.ReadKey(true);
            }
        }

        private int GetChoice(int maxChoice)
        {
            int choice;
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                choice = keyInfo.KeyChar - '1';
            } while (choice < 0 || choice >= maxChoice);

            return choice;
        }
    }
}
