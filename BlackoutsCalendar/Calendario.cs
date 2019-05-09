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
using System.Globalization;

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

        public string Path;

        public ListOfBlackouts Blackouts;

        public DateTime Desde;
        public DateTime Hasta;

        public int dia;
        public void Limpiar() {
            DGFechas.Rows.Clear();

        }

        public void LLenardg() {
            for (int i = 0; i < Blackouts.Blackouts.Count; i++) {

                Blackout Boff = Blackouts.Blackouts[i];

                int day = (int)Boff.BlackoutBeginning.DayOfWeek;
                int id = chart1.Series[0].Points.AddXY(day, Blackouts.Blackouts[i].BlackoutBeginning.Hour);
                CultureInfo ci = new CultureInfo("ES-ES");
                DayOfWeek DiaIng = Blackouts.Blackouts[i].BlackoutBeginning.DayOfWeek;
                chart1.Series[0].Points[id].AxisLabel = ci.DateTimeFormat.GetDayName(DiaIng);


                int ide = chart2.Series[0].Points.AddXY(i, (Boff.Ending - Boff.BlackoutBeginning).TotalHours);
                chart2.Series[0].Points[ide].AxisLabel = Boff.BlackoutBeginning.ToLongDateString();



                Blackout Bout = Blackouts.Blackouts[i];

                TimeSpan span = (Bout.Ending - Bout.BlackoutBeginning);
                string diff = string.Format("{0} Horas, {1} Minutos", span.Hours, span.Minutes);
                DGFechas.Rows.Add(new object[] {false,
                                                            "0",
                                                            Bout.BlackoutBeginning.ToLongDateString(), 
                                                            Bout.BlackoutBeginning.ToString("hh:mm tt"), 
                                                            Bout.Ending.ToString("hh:mm tt"),
                                                            diff});
            }
        }

        public void Llenar() {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();

            DateTime? latest = Blackouts.Blackouts.Max(r => r.BlackoutBeginning);
            DateTime? Firstest = Blackouts.Blackouts.Min(r => r.BlackoutBeginning);
            DateTime ad = (DateTime)Firstest;
            DateTime bd = (DateTime)latest;

            if (Desde >= ad) {
                ad = Desde;
            }
            if (Hasta <= bd) {
                bd = Hasta;
            }

            for (int i = 0; i < Blackouts.Blackouts.Count; i++)
            {

                Blackout Boff = Blackouts.Blackouts[i];

                if ((Boff.BlackoutBeginning >= ad && Boff.BlackoutBeginning <= bd) &&
                            (dia == 7 || dia == (int)Boff.BlackoutBeginning.DayOfWeek))
                {
                    int day = (int)Boff.BlackoutBeginning.DayOfWeek;
                    int id = chart1.Series[0].Points.AddXY(day, Blackouts.Blackouts[i].BlackoutBeginning.Hour);
                    CultureInfo ci = new CultureInfo("ES-ES");
                    DayOfWeek DiaIng = Blackouts.Blackouts[i].BlackoutBeginning.DayOfWeek;
                    chart1.Series[0].Points[id].AxisLabel = ci.DateTimeFormat.GetDayName(DiaIng);


                    int ide = chart2.Series[0].Points.AddXY(i, (Boff.Ending - Boff.BlackoutBeginning).TotalHours);
                    chart2.Series[0].Points[ide].AxisLabel = Boff.BlackoutBeginning.ToLongDateString();
                }
            }

            int j = 0;
            for (DateTime date = ad; date.Date <= bd; date = date.AddDays(1))
            {
                bool EA = false;

                for (int i = 0; i < Blackouts.Blackouts.Count; i++)
                {


                    Blackout Bout = Blackouts.Blackouts[i];
                    DateTime a = new DateTime(date.Year, date.Month, date.Day);
                    DateTime b = new DateTime(Bout.BlackoutBeginning.Year,
                                                Bout.BlackoutBeginning.Month,
                                                Bout.BlackoutBeginning.Day);

                    if (!(dia == 7 || dia == (int)a.DayOfWeek))
                    {
                        EA = true;
                        break;
                    }

                    if (a == b)
                    {
                        Blackout Boff = Bout;
                        TimeSpan span = (Boff.Ending - Boff.BlackoutBeginning);
                        string diff = string.Format("{0} Horas, {1} Minutos", span.Hours, span.Minutes);
                        DGFechas.Rows.Add(new object[] {false,
                                                            i.ToString(),
                                                            date.ToLongDateString(), 
                                                            Boff.BlackoutBeginning.ToString("hh:mm tt"), 
                                                            Boff.Ending.ToString("hh:mm tt"),
                                                            diff});
                        j++;
                        int ef = i + 1;
                        if (ef >= Blackouts.Blackouts.Count())
                        {
                            EA = true;
                            break;
                        }
                        Bout = Blackouts.Blackouts[ef];
                        b = new DateTime(Bout.BlackoutBeginning.Year,
                                            Bout.BlackoutBeginning.Month,
                                            Bout.BlackoutBeginning.Day);
                        while (a == b)
                        {
                            Boff = Bout;
                            span = (Boff.Ending - Boff.BlackoutBeginning);
                            diff = string.Format("{0} Horas, {1} Minutos", span.Hours, span.Minutes);
                            DGFechas.Rows.Add(new object[] {false,
                                                            i.ToString(),
                                                            "", 
                                                            Boff.BlackoutBeginning.ToString("hh:mm tt"), 
                                                            Boff.Ending.ToString("hh:mm tt"),
                                                            ((Boff.Ending - Boff.BlackoutBeginning).TotalHours).ToString()});
                            j++;
                            ef++;
                            if (ef >= Blackouts.Blackouts.Count())
                            {
                                EA = true;
                                break;
                            }
                            Bout = Blackouts.Blackouts[ef];
                            b = new DateTime(Bout.BlackoutBeginning.Year,
                                                Bout.BlackoutBeginning.Month,
                                                Bout.BlackoutBeginning.Day);
                        }
                        EA = true;
                        break;
                    }

                }
                if (!EA && j > 1)
                {
                    DGFechas.Rows.Add(new object[] {false,
                                                    "",
                                                    date.ToLongDateString(), 
                                                    "Nada", 
                                                    "Nada",
                                                    "Nada"});
                }

            }
        }

        public Calendario()
        {
            InitializeComponent();
        }

        private void Calendario_Load(object sender, EventArgs e)
        {

            if (Blackouts.Blackouts.Count == 0)
                return;

            DateTime? latest = Blackouts.Blackouts.Max(r => r.BlackoutBeginning);
            DateTime? Firstest = Blackouts.Blackouts.Min(r => r.BlackoutBeginning);
            DateTime l = (DateTime)latest;
            DateTime f = (DateTime)Firstest;

            Desde = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, 0);
            Hasta = new DateTime(l.Year, l.Month, l.Day, l.Hour, l.Minute, 0);

            dia = 7;


            Llenar();
            DTDesde.Value = Desde;
            DTHasta.Value = Hasta;
            CBDias.SelectedIndex = dia;
            BtnEliminar.Enabled = checkBox1.Checked;
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
            if (BtnAlternar.Text == "Tabla") {
                BtnAlternar.Text = "Estadistica";
                chart1.SendToBack();
            }
            else if (BtnAlternar.Text == "Estadistica"){
                BtnAlternar.Text = "Duraciones";
                DGFechas.SendToBack();
            }
            else if (BtnAlternar.Text == "Duraciones") {
                BtnAlternar.Text = "Tabla";
                chart2.SendToBack();
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
            NewBlackout bout = new NewBlackout();
            bout.Tiporegistro = "Nuevo";
            bout.path = Path;
            bout.CalendarioPrincipal = this;
            bout.Blackouts = Blackouts;
            PopUpMbox(bout);
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

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (BtnActualizar.Text == "Actualizar") {
                BtnActualizar.Text = "ActualizarDG";
                Limpiar();
                LLenardg();
                Blackouts.Blackouts = Blackouts.Blackouts.OrderBy(x => x.BlackoutBeginning).ToList();
                JsonLoader<ListOfBlackouts>.UpdateData(Blackouts, Path);
            }
            else if (BtnActualizar.Text == "ActualizarDG") {
                BtnActualizar.Text = "Actualizar";
                Limpiar();
                Llenar();
                Blackouts.Blackouts = Blackouts.Blackouts.OrderBy(x => x.BlackoutBeginning).ToList();
                JsonLoader<ListOfBlackouts>.UpdateData(Blackouts, Path);
            }
        }

        private void DGFechas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DGFechas.Columns["Eliminar"].Index)
            {
                DataGridViewCheckBoxCell ChkEliminar = (DataGridViewCheckBoxCell)DGFechas.Rows[e.RowIndex].Cells["Eliminar"];
                ChkEliminar.Value = !Convert.ToBoolean(ChkEliminar.Value);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DGFechas.Columns[0].Visible = checkBox1.Checked;
            BtnEliminar.Enabled = checkBox1.Checked;
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;
                Opcion = MessageBox.Show("Realmente Desea Eliminar los Registros", "Blackout Calendar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Opcion == DialogResult.OK)
                {

                    foreach (DataGridViewRow row in DGFechas.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {

                            if (row.Cells[1].Value.ToString() != "")
                            {

                                int codigo = int.Parse(Convert.ToString(row.Cells[1].Value));

                                Blackouts.Blackouts.RemoveAt(codigo);
                                Blackouts.Blackouts = Blackouts.Blackouts.OrderBy(x => x.BlackoutBeginning).ToList();
                                JsonLoader<ListOfBlackouts>.UpdateData(Blackouts, Path);
                                Limpiar();
                                Llenar();

                                MessageBox.Show("Se Eliminó Correctamente el registro");
                            }
                            else {
                                MessageBox.Show("Los dias que no tienen registrado un apagon no se pueden eliminar");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void DGFechas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGFechas.Rows[e.RowIndex].Cells[1].Value.ToString() == "") {
                MessageBox.Show("No se puede editar un registro no existente");
                return;
            }
            NewBlackout bout = new NewBlackout();
            bout.Tiporegistro = "Editar";
            bout.index = int.Parse(Convert.ToString(DGFechas.Rows[e.RowIndex].Cells[1].Value));
            bout.DTBlackout.Value = Blackouts.Blackouts[bout.index].BlackoutBeginning;
            bout.DTEndBlackout.Value = Blackouts.Blackouts[bout.index].Ending;
            bout.path = Path;
            bout.CalendarioPrincipal = this;
            bout.Blackouts = Blackouts;
            PopUpMbox(bout);
        }

        private void DTDesde_ValueChanged(object sender, EventArgs e)
        {
            DateTime a = DTDesde.Value;
            DateTime? Firstest = Blackouts.Blackouts.Min(r => r.BlackoutBeginning);
            DateTime f = (DateTime)Firstest;

            Desde = new DateTime(a.Year, a.Month, a.Day, f.Hour, f.Minute, 0);
            Limpiar();
            Llenar();
        }

        private void DTHasta_ValueChanged(object sender, EventArgs e)
        {
            DateTime a = DTHasta.Value;
            DateTime? latest = Blackouts.Blackouts.Max(r => r.BlackoutBeginning);
            DateTime l = (DateTime)latest;

            Hasta = new DateTime(a.Year, a.Month, a.Day, l.Hour, l.Minute, 0);
            Limpiar();
            Llenar();
        }

        private void DTDesde_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void CBDias_SelectedIndexChanged(object sender, EventArgs e)
        {
            dia = CBDias.SelectedIndex;

            Limpiar();
            Llenar();
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
