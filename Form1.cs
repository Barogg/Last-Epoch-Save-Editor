using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Last_Epoch_Save_Editor
{
    public partial class Form1 : Form
    {
        string filePath = "";
        string characterName = "";
        JObject obj;
        int level;
        int chosenMastery;
        int charClass;
        private Dictionary<string, List<Specialization>> classesAndSpecializations;
        private Dictionary<string, List<Classes>> classes;

        public Form1()
        {
            InitializeComponent();
        }

        public class Specialization
        {
            public int ID { get; set; }
            public string Name { get; set; }
            // Add other properties if needed
        }

        public class Classes
        {
            public int ID { get; set; }
            public string Name { get; set; }
            // Add other properties if needed
        }



        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";

            classesAndSpecializations = new Dictionary<string, List<Specialization>>
            {
              { "ClassName0", new List<Specialization>
                    {
                        new Specialization { ID = 1, Name = "Beastmaster" },
                        new Specialization { ID = 2, Name = "Shaman" },
                        new Specialization { ID = 3, Name = "Druid" },
                    }
                },

                { "ClassName1", new List<Specialization>
                    {
                        new Specialization { ID = 1, Name = "Sorcerer" },
                        new Specialization { ID = 2, Name = "Spellblade" },
                        new Specialization { ID = 3, Name = "Runemaster" },
                    }
                },

                { "ClassName2", new List<Specialization>
                    {
                        new Specialization { ID = 1, Name = "Void Knight" },
                        new Specialization { ID = 2, Name = "Forge Guard" },
                        new Specialization { ID = 3, Name = "Paladin" },
                    }
                },

                { "ClassName3", new List<Specialization>
                    {
                        new Specialization { ID = 1, Name = "Necromancer" },
                        new Specialization { ID = 2, Name = "Lich" },
                        new Specialization { ID = 3, Name = "Warlock" },
                    }
                },

                { "ClassName4", new List<Specialization>
                    {
                        new Specialization { ID = 1, Name = "Blade Dancer" },
                        new Specialization { ID = 2, Name = "Marksman" },
                        new Specialization { ID = 3, Name = "Falconer" },
                    }
                },
                // Add other classes and their specializations
            };
            classes = new Dictionary<string, List<Classes>>
            {
              { "Class", new List<Classes>
                    {
                        new Classes { ID = 0, Name = "Primalist" },
                        new Classes { ID = 1, Name = "Mage" },
                        new Classes { ID = 2, Name = "Sentinel" },
                        new Classes { ID = 3, Name = "Acolyte" },
                        new Classes { ID = 4, Name = "Rogue" },
                    }
                },
                // Add other classes and their specializations
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileToolStripMenuItem.Enabled = true;
                filePath = openFileDialog1.FileName;

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string json = sr.ReadToEnd();
                    // Remove the extra text at the beginning of the file
                    json = json.Substring(5);

                    obj = JObject.Parse(json);

                    characterName = (string)obj["characterName"];
                    level = obj["level"].Value<int>();
                    chosenMastery = obj["chosenMastery"].Value<int>();
                    charClass = obj["characterClass"].Value<int>();
                }

                textBox1.Text = characterName;
                textBox2.Text = level.ToString();
                checkBox1.Checked = obj["hardcore"].Value<bool>();
                checkBox2.Checked = obj["masochist"].Value<bool>();
                checkBox3.Checked = obj["soloChallenge"].Value<bool>();

                string className = "ClassName" + charClass; // Get the class name from the class number

                if (classesAndSpecializations.TryGetValue(className, out var specializations))
                {
                    // Now you have the specializations for the class
                    // You can add them to the ComboBox like this:
                    comboBox2.Items.Clear(); // Clear the ComboBox items first
                    comboBox2.Text = "";
                    foreach (var specialization in specializations)
                    {
                        comboBox2.Items.Add(specialization.Name);
                    }
                    comboBox2.SelectedIndex = chosenMastery - 1;
                }
                else
                {
                    // The class was not found in the dictionary
                }

                if (classes.TryGetValue("Class", out var classes1))
                {
                    // Now you have the specializations for the class
                    // You can add them to the ComboBox like this:
                    comboBox1.Items.Clear(); // Clear the ComboBox items first
                    foreach (var classes2 in classes1)
                    {
                        comboBox1.Items.Add(classes2.Name);
                    }
                    comboBox1.SelectedIndex = charClass;
                }
                else
                {
                    // The class was not found in the dictionary
                }
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            obj = JObject.Parse(File.ReadAllText(filePath).Substring(5));
            obj["characterName"] = textBox1.Text;
            obj["level"] = int.Parse(textBox2.Text);
            obj["chosenMastery"] = comboBox2.SelectedIndex + 1;
            obj["characterClass"] = comboBox1.SelectedIndex;
            obj["hardcore"] = checkBox1.Checked;
            obj["masochist"] = checkBox2.Checked;
            obj["soloChallenge"] = checkBox3.Checked;
            obj["soloCharacterChallenge"] = checkBox3.Checked;
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write("EPOCH");
                sw.Write(obj.ToString(Formatting.None));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string json = "";
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                return;
            }

            // Remove the "EPOCH" text at the beginning of the file
            json = json.Substring(5);
            // Parse the JSON data
            obj = JObject.Parse(json);

            // Create an array of the desired timeline difficulty unlocks
            JArray timelineDifficultyUnlocksArray = new JArray();

            // Add each timeline difficulty unlock to the array
            for (int i = 1; i <= 10; i++)
            {
                JObject timelineDifficultyUnlock = new JObject();
                timelineDifficultyUnlock["timelineID"] = i;
                timelineDifficultyUnlock["progress"] = new JArray(0);
                timelineDifficultyUnlocksArray.Add(timelineDifficultyUnlock);
            }

            // Set the "timelineDifficultyUnlocks" property to the new array
            obj["timelineDifficultyUnlocks"] = timelineDifficultyUnlocksArray;

            // Write the modified JSON data back to the file
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                // Add the "EPOCH" text back to the beginning of the file
                sw.Write("EPOCH");
                sw.Write(obj.ToString(Formatting.None));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string json = "";
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                return;
            }

            // Remove the "EPOCH" text at the beginning of the file
            json = json.Substring(5);

            // Parse the JSON data
            obj = JObject.Parse(json);

            // Create an array of the desired parameters
            string[] waypointScenesArray = { "Z20", "Z30", "Z40", "A04", "Z50", "A10", "A30", "A45", "A60TR", "A60", "A70", "A90", "B10", "B20", "B25", "B33", "B7S10", "B40", "B1S40", "B40TR", "B50", "B60", "B80", "EoT", "C10", "C20", "C30", "C40", "C50", "C60", "C70", "D05", "Arena_1_Forest", "MonolithHub", "D20", "D30", "D35", "D40", "D60", "E10", "E20", "E20TR", "E30", "E40", "E50", "E60", "E80", "E90", "F10", "F40", "F1S10", "F50", "F70", "F80", "F90", "F100", "F110", "G40", "G60", "G70", "G80", "G90", "G96", "G93", "D05TR", "G110", "H10", "H40", "H50", "H70", "H80", "H100", "H110", "H120", "R4Q10", "G2S10", "Z32", "H50", "H50", "Bazaar", "Observatory", "Z32", /* add the rest of the parameters here */ };

            // Create a new JArray with the desired parameters
            JArray waypointScenes = new JArray();

            // Add each parameter to the JArray using a loop
            foreach (string scene in waypointScenesArray)
            {
                waypointScenes.Add(scene);
            }

            // Set the "unlockedWaypointScenes" property to the new JArray
            obj["unlockedWaypointScenes"] = waypointScenes;
            obj["portalUnlocked"] = true;


            // Write the modified JSON data back to the file
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                // Add the "EPOCH" text back to the beginning of the file
                sw.Write("EPOCH");
                sw.Write(obj.ToString(Formatting.None));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string json = "";
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                return;
            }


            // Remove the "EPOCH" text at the beginning of the file
            json = json.Substring(5);

            // Parse the JSON data
            obj = JObject.Parse(json);

            // Remove everything between the "[]" characters for the "nodeIDs" property
            obj["savedCharacterTree"]["nodeIDs"] = new JArray();
            obj["savedCharacterTree"]["nodePoints"] = new JArray();

            try
            {
                // Write the modified JSON data back to the file
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    // Add the "EPOCH" text back to the beginning of the file
                    sw.Write("EPOCH");

                    // Keep the JSON data on a single line
                    sw.Write(obj.ToString(Formatting.None));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file to disk. Original error: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            charClass = comboBox1.SelectedIndex;
            string className = "ClassName" + charClass; // Get the class name from the class number

            if (classesAndSpecializations.TryGetValue(className, out var specializations))
                {
                comboBox2.Items.Clear();
                foreach (var specialization in specializations)
                {
                    comboBox2.Items.Add(specialization.Name);
                }
                comboBox2.SelectedIndex = chosenMastery - 1;
            }
        }
    }
}
