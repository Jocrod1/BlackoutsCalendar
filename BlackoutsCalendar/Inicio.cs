using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace BlackoutsCalendar
{
    public partial class Inicio : Form
    {

        public ListOfBlackouts Blackouts;

        public string JsonString;

        public string Path;

        ListOfBlackouts VoidBlackouts;

        public Inicio()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Inicio_DragEnter);
            this.DragDrop += new DragEventHandler(Inicio_DragDrop);
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            VoidBlackouts = new ListOfBlackouts();
            VoidBlackouts.Blackouts.Add(new Blackout
            {
                BlackoutBeginning = new DateTime(2019, 5, 2, 4, 22, 0),
                Ending = new DateTime(2019, 5, 2, 8, 22, 0)
            });
            VoidBlackouts.Blackouts.Add(new Blackout
            {
                BlackoutBeginning = new DateTime(2019, 5, 1, 1, 22, 0),
                Ending = new DateTime(2019, 5, 1, 5, 22, 0)
            });
        }

        void Inicio_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Inicio_DragDrop(object sender, DragEventArgs e)
        {

            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string file in FileList) Path = file;
            try
            {
                Blackouts = JsonLoader<ListOfBlackouts>.LoadData(Path);
                Blackouts.Blackouts = Blackouts.Blackouts.OrderBy(x => x.BlackoutBeginning).ToList();
                JsonLoader<ListOfBlackouts>.UpdateData(Blackouts, Path);
                MessageBox.Show("The Json has linked Succesfully!");
                Calendario Frm = new Calendario();
                Frm.Blackouts = Blackouts;
                Frm.Path = Path;
                Frm.Show();
                Hide();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                string name = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the file");

                if (name == "") {
                    return;
                }

                string PathOfCreation = AppDomain.CurrentDomain.BaseDirectory + name + ".json";

                if (File.Exists(PathOfCreation))
                {
                    MessageBox.Show("Theres already another file with the same name");
                    return;
                }


                String Jsonstring = JsonConvert.SerializeObject(VoidBlackouts);
                using (FileStream fs = File.Create(PathOfCreation))
                {
                    Byte[] Info = new UTF8Encoding(true).GetBytes(Jsonstring);
                    fs.Write(Info, 0, Info.Length);
                }

                MessageBox.Show("The " + name + ".json file has been created in  " +
                                                AppDomain.CurrentDomain.BaseDirectory);

                Blackouts = JsonLoader<ListOfBlackouts>.LoadData(PathOfCreation);
                Calendario Frm = new Calendario();
                Frm.Blackouts = Blackouts;
                Frm.Path = PathOfCreation;
                Frm.Show();
                Hide();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.flaticon.es/autores/smashicons");
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void PanelTop_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        }

    public class JsonLoader<T>
    {
        public static T LoadData(string FilePath)
        {
            try
            {
                string JsonString = File.ReadAllText(FilePath);
                T Item = JsonConvert.DeserializeObject<T>(JsonString);

                return Item;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return default(T);
            }
        }

        public static void UpdateData(T Data, string FilePath)
        {
            try
            {
                String Jsonstring = JsonConvert.SerializeObject(Data);
                File.WriteAllText(FilePath, Jsonstring);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
