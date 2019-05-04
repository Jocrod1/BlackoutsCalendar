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

        public NewBlackout()
        {
            InitializeComponent();
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
            b.BlackoutBeginning = DTBlackout.Value;
            b.Ending = DTEndBlackout.Value;
            Blackouts.Blackouts.Add(b);
            JsonLoader<ListOfBlackouts>.UpdateData(Blackouts, path);
            CalendarioPrincipal.Blackouts = Blackouts;
            Close();
        }

        private void NewBlackout_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
