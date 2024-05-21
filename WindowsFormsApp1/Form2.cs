using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows.Forms;
using static WindowsFormsApp1.Form1;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        DPrintUser dpu;
        string pass = "";
        public Form2(DPrintUser dPrintUser)
        {
            InitializeComponent();
            nameUser.Text = "Talakutskaya Anastasiya";
            paswordUser.Text = "";
            dpu = dPrintUser;
        }

        public class User
        {
            public string name;
            public string password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            User user = new User() { name = nameUser.Text, password = pass };
            string url = "http://192.168.88.30:3002/auth/login";
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            string json = JsonConvert.SerializeObject(user);
            string result = webClient.UploadString(url, json);

            if (result != null)
            {
                dpu(result);
                Close();
            }
        }

        private void paswordUser_TextChanged(object sender, EventArgs e)
        {
            pass += paswordUser.Text[paswordUser.Text.Length - 1];
            paswordUser.Text = paswordUser.Text.Replace(pass[pass.Length - 1], '*');
            paswordUser.SelectionStart = pass.Length;
        }
    }
}
