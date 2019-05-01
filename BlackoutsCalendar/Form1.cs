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

namespace BlackoutsCalendar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
