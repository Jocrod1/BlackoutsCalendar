using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackoutsCalendar
{
    public partial class NewBlackout : Form
    {
        public Calendario CalendarioPrincipal;
        public ListOfBlackouts Blackouts;
        public string path;
        public int index;
        public string Tiporegistro = "Nuevo";

        public NewBlackout()
        {
            InitializeComponent();
        }

        private void NewBlackout_Load(object sender, EventArgs e)
        {
            LblTipoRegistro.Text = Tiporegistro + " Registro";

            if (Tiporegistro == "Nuevo") {
                BtnRegistrar.Text = "Registrar";
            }
            else if (Tiporegistro == "Editar") {
                BtnRegistrar.Text = "Editar";
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            Blackout b = new Blackout();
            DateTime bBeg = DTBlackout.Value;
            b.BlackoutBeginning = new DateTime(bBeg.Year, bBeg.Month, bBeg.Day, bBeg.Hour, bBeg.Minute, 0);
            DateTime bEnd = DTEndBlackout.Value;
            b.Ending = new DateTime(bEnd.Year, bEnd.Month, bEnd.Day, bEnd.Hour, bEnd.Minute, 0);
            if (Tiporegistro == "Nuevo")
            {
                Blackouts.Blackouts.Add(b);

            }
            else if (Tiporegistro == "Editar") {
                Blackouts.Blackouts[index] = b;
            }
            Blackouts.Blackouts = Blackouts.Blackouts.OrderBy(x => x.BlackoutBeginning).ToList();
            JsonLoader<ListOfBlackouts>.UpdateData(Blackouts, path);
            CalendarioPrincipal.Blackouts = Blackouts;
            CalendarioPrincipal.Limpiar();
            CalendarioPrincipal.Llenar();
            Close();
        }

        private void NewBlackout_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


    }
}
