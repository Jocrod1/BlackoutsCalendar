using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Calendar.NET;
using System.Runtime.InteropServices;

namespace BlackoutsCalendar
{
    public partial class Calendario : Form
    {


        public class TransparentPanel : Panel {
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                    return cp;
                }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                //base.OnPaintBackground(e);
            }
        }

        public ListOfBlackouts Blackouts
        {
            get;

            set
            {
                for (int i = 0; i < Blackouts.Blackouts.Count; i++)
                {

                    Blackout B = Blackouts.Blackouts[i];
                    var ce = new CustomEvent();
                    ce.IgnoreTimeComponent = false;
                    ce.EventText = "Blackout";
                    ce.Date = B.BlackoutBeginning;
                    ce.EventLengthInHours = (float)(B.Ending - B.BlackoutBeginning).TotalHours;
                    MessageBox.Show(((float)(B.Ending - B.BlackoutBeginning).TotalHours).ToString());
                    ce.RecurringFrequency = RecurringFrequencies.None;
                    ce.EventFont = new Font("Century Gothic", 12, FontStyle.Regular);
                    ce.Enabled = true;
                    calendar1.RemoveEvent(ce);

                }
                Blackouts = value;
                for (int i = 0; i < Blackouts.Blackouts.Count; i++)
                {

                    Blackout B = Blackouts.Blackouts[i];
                    var ce = new CustomEvent();
                    ce.IgnoreTimeComponent = false;
                    ce.EventText = "Blackout";
                    ce.Date = B.BlackoutBeginning;
                    ce.EventLengthInHours = (float)(B.Ending - B.BlackoutBeginning).TotalHours;
                    MessageBox.Show(((float)(B.Ending - B.BlackoutBeginning).TotalHours).ToString());
                    ce.RecurringFrequency = RecurringFrequencies.None;
                    ce.EventFont = new Font("Century Gothic", 12, FontStyle.Regular);
                    ce.Enabled = true;
                    calendar1.AddEvent(ce);
                }
                DGFechas.Rows.Clear();
                DGFechas.Columns.Clear();
                Llenar();
            }
        }

        public void Llenar() {
            DGFechas.Columns.Add("Fecha", "Fecha");
            DGFechas.Columns.Add("Hora Apagon", "Hora Apagón");
            DGFechas.Columns.Add("Hora Alumbron", "Hora Alumbrón");

            DateTime? latest = Blackouts.Blackouts.Max(r => r.BlackoutBeginning);
            DateTime? Firstest = Blackouts.Blackouts.Min(r => r.BlackoutBeginning);

            for (DateTime date = (DateTime)Firstest; date.Date <= (DateTime)latest; date = date.AddDays(1))
            {
                bool EA = false;

                for (int i = 0; i < Blackouts.Blackouts.Count; i++)
                {
                    if (date == (DateTime)Firstest)
                    {
                        Blackout B = Blackouts.Blackouts[i];
                        var ce = new CustomEvent();
                        ce.IgnoreTimeComponent = false;
                        ce.EventText = "Blackout";
                        ce.Date = B.BlackoutBeginning;
                        ce.EventLengthInHours = (float)(B.Ending - B.BlackoutBeginning).TotalHours;
                        MessageBox.Show(((float)(B.Ending - B.BlackoutBeginning).TotalHours).ToString());
                        ce.RecurringFrequency = RecurringFrequencies.None;
                        ce.EventFont = new Font("Century Gothic", 12, FontStyle.Regular);
                        ce.Enabled = true;
                        calendar1.AddEvent(ce);
                    }

                    Blackout Bout = Blackouts.Blackouts[i];
                    DateTime a = new DateTime(date.Year, date.Month, date.Day);
                    DateTime b = new DateTime(Bout.BlackoutBeginning.Year,
                                                Bout.BlackoutBeginning.Month,
                                                Bout.BlackoutBeginning.Day);

                    if (a == b)
                    {
                        Blackout Boff = Bout;
                        DGFechas.Rows.Add(new object[] { date.ToLongDateString(), 
                                                        Boff.BlackoutBeginning.ToString("hh:mm:ss"), 
                                                        Boff.Ending.ToString("hh:mm:ss") });
                        EA = true;
                        break;
                    }
                }
                if (!EA)
                {
                    DGFechas.Rows.Add(new object[] { date.ToLongDateString(), 
                                                        "Nada", 
                                                        "Nada"});
                }

            }
        }

        public Calendario()
        {
            InitializeComponent();
            TransparentPanel SP = new TransparentPanel();
            SP.BackColor = Color.Black;
            SP.Location = new Point(0, 0);
            SP.Name = "SP";
            SP.Size = new Size(100, 100);
            SP.TabIndex = 0;
            PrincipalPanel.Controls.Add(SP);
            SP.BringToFront();
        }

        private void calendar1_Load(object sender, EventArgs e)
        {

        }

        private void Calendario_Load(object sender, EventArgs e)
        {
            Llenar();
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void BtnAlternar_Click(object sender, EventArgs e)
        {
            if (BtnAlternar.Text == "Días") {
                calendar1.CalendarView = CalendarViews.Day;
                BtnAlternar.Text = "Tabla";
            }
            else if (BtnAlternar.Text == "Tabla") {
                calendar1.SendToBack();
                BtnAlternar.Text = "Meses";
            }
            else if (BtnAlternar.Text == "Meses") {
                calendar1.BringToFront();
                calendar1.CalendarView = CalendarViews.Month;
                BtnAlternar.Text = "Días";
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void Calendario_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SecundarioPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            
        }

        public void PopUpMbox(object f) {
            Enabled = false;
            Form shadow = new Form();
            shadow.MinimizeBox = false;
            shadow.MaximizeBox = false;
            shadow.ControlBox = false;

            shadow.Text = "";
            shadow.FormBorderStyle = FormBorderStyle.None;
            shadow.Size = Size;
            shadow.BackColor = Color.Black;
            shadow.Opacity = 0.3;
            shadow.Show();
            shadow.Location = Location;
            //shadow.Enabled = false;

            Form PopUp = f as Form;
            PopUp.FormClosing += (ss, ee) => { shadow.Close(); Enabled = true; };
            shadow.Click += (ss, ee) => { PopUp.Close(); };

            PopUp.StartPosition = FormStartPosition.Manual;
            PopUp.Location = new Point(shadow.Left + (shadow.Width - PopUp.Width) / 2,
                                        shadow.Top + (shadow.Height - PopUp.Height) / 2);
            PopUp.Show();
        }
    }

    public class ListOfBlackouts {
        public List<Blackout> Blackouts = new List<Blackout>();
    }

    public class Blackout {
        public DateTime BlackoutBeginning;
        public DateTime Ending;
    }

}
