using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    //192.168.88.30:3000/task
    public partial class Form1 : Form
    {
        static string constr = "Server=192.168.88.30;Database=Components;User Id=root;Password=root;";
        SqlConnection connect = new SqlConnection(constr);
        string selectComp = "Select * From Computers;";
        string selectProc = "Select * From Processors;";
        string selectVid = "Select * From Videoadapters;";
        string selectMemory = "Select * From Memory;";
        string insertComp = "INSERT INTO Computers(name, processor_id, videoadapter_id, memory_id) VALUES (@name, @proc, @vid, @memory);";
        SqlCommand command;

        Result res;

        public void GetComputersList()
        {
            listComp.Items.Clear();
            command = new SqlCommand(selectComp, connect);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Computers comp = new Computers
                            (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
                        listComp.Items.Add(comp);
                    }
                }
            }
        }
        public void GetProcessoresList()
        {
            nameProc.Items.Clear();
            command = new SqlCommand(selectProc, connect);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Processores proc = new Processores
                            (reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetInt32(3));
                        nameProc.Items.Add(proc);
                    }
                }
            }
        }
        public void GetVideoadapterList()
        {
            nameVid.Items.Clear();
            command = new SqlCommand(selectVid, connect);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Videoadapter vid = new Videoadapter
                            (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3));
                        nameVid.Items.Add(vid);
                    }
                }
            }
        }
        public void GetMemoryList()
        {
            nameMemory.Items.Clear();
            command = new SqlCommand(selectMemory, connect);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Memory memory = new Memory
                            (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                        nameMemory.Items.Add(memory);
                    }
                }
            }
        }
        public void PrintUser(string user)
        {
            if (user != "")
            {
                res = JsonConvert.DeserializeObject<Result>(user);
                button1.Visible = false;
                panel1.Visible = true;
                label5.Text += "\n" + res.name;
                pictureBox1.ImageLocation = res.avatar;
            }
        }

        public class Processores
        {
            public int id;
            public string name;
            public double frequency;
            public int cores;

            public Processores(int id, string name, double frequency, int cores)
            {
                this.id = id;
                this.name = name;
                this.frequency = frequency;
                this.cores = cores;
            }

            public override string ToString()
            {
                return name;
            }
        }
        public class Videoadapter
        {
            public int id;
            public string name;
            public int memory_size;
            public double frequency;

            public Videoadapter(int id, string name, int memory_size, double frequency)
            {
                this.id = id;
                this.name = name;
                this.memory_size = memory_size;
                this.frequency = frequency;
            }

            public override string ToString()
            {
                return name;
            }
        }
        public class Memory
        {
            public int id;
            public string name;
            public int memory_size;

            public Memory(int id, string name, int memory_size)
            {
                this.id = id;
                this.name = name;
                this.memory_size = memory_size;
            }

            public override string ToString()
            {
                return name;
            }
        }
        public class Computers
        {
            public int id;
            public string name;
            public int processor_id;
            public int videoadapter_id;
            public int memory_id;

            public Computers(int id, string name, int processor_id, int videoadapter_id, int memory_id)
            {
                this.id = id;
                this.name = name;
                this.processor_id = processor_id;
                this.videoadapter_id = videoadapter_id;
                this.memory_id = memory_id;
            }

            public override string ToString()
            {
                return name;
            }
        }
        public class Result
        {
            public string jwt;
            public string name;
            public string avatar;
        }

        public delegate void DPrintUser(string user);

        private void button2_Click(object sender, EventArgs e)
        {
            if (res != null)
            {
                Processores p = (Processores)nameProc.SelectedItem;
                Videoadapter v = (Videoadapter)nameVid.SelectedItem;
                Memory m = (Memory)nameMemory.SelectedItem;
                command = new SqlCommand(insertComp, connect);
                command.Parameters.Add(new SqlParameter("@name", nameComp.Text));
                command.Parameters.Add(new SqlParameter("@proc", p.id));
                command.Parameters.Add(new SqlParameter("@vid", v.id));
                command.Parameters.Add(new SqlParameter("@memory", m.id));
                if (command.ExecuteNonQuery() > 0)
                    GetComputersList();
            }
            else
                MessageBox.Show("Войдите для добавления компьютера");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            DPrintUser dpu = PrintUser;
            Form2 form2 = new Form2(dpu);
            form2.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            panel1.Visible = false;
            label5.Text = "Пользователь:";
            pictureBox1.ImageLocation = "";
            res = null;
        }
        private void listComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (res != null)
            {
                Computers comp = (Computers)listComp.SelectedItem;
                if (comp != null)
                {
                    nameComp.Text = comp.name;
                    nameProc.SelectedIndex = comp.processor_id - 1;
                    nameVid.SelectedIndex = comp.memory_id - 1;
                    nameMemory.SelectedIndex = comp.videoadapter_id - 1;
                }
            }
            else
                MessageBox.Show("Войдите для просмотра детальной информации о компьютере");
        }

        public Form1()
        {
            InitializeComponent();
            connect.Open();
            GetComputersList();
            GetProcessoresList();
            GetVideoadapterList();
            GetMemoryList();
        }
        ~Form1()
        {
            connect.Close();
        }
    }
}
