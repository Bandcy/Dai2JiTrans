using RomLibrary;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dai2JiTrans
{
    public partial class TBLManagerForm : Form
    {
        private const string TBL_FILE_FILTER = "TBL file|*.tbl";

        public TBLManagerForm()
        {
            InitializeComponent();
        }

        public TBLManagerForm(string file)
            : this()
        {
            CurrentFile = file;
        }

        private TBL _TBL;
        public TBL TBL
        {
            get { return _TBL; }
            set { _TBL = value; }
        }

        private string _CurrentFile;
        public string CurrentFile
        {
            get { return _CurrentFile; }
            set
            {
                _CurrentFile = value;
                LoadTBL();
            }
        }

        public HexValuePair[] DataSource
        {
            get { return (HexValuePair[])grdTBL.DataSource; }
            set { grdTBL.DataSource = value; }
        }

        private void OpenTBL()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = TBL_FILE_FILTER;
            dialog.ShowDialog();

            CurrentFile = dialog.FileName;
        }

        private void LoadTBL()
        {
            if (string.IsNullOrEmpty(CurrentFile)
                || !File.Exists(CurrentFile))
            {
                DataSource = null;
            }
            else
            {
                TBL = new TBL(CurrentFile);
                DataSource = TBL
                    .Select(t => new HexValuePair(t.Key, t.Value))
                    .ToArray();
            }
        }

        private void SaveTBL(string file)
        {
            TBL = new TBL();
            DataSource.ToList()
                .ForEach(hv => TBL.Add(hv.Hex, hv.Value));

            TBL.WriteFile(file);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentFile = null;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTBL();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTBL();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTBL(CurrentFile);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = TBL_FILE_FILTER;
            dialog.ShowDialog();

            string file = dialog.FileName;
            if (string.IsNullOrEmpty(file))
                return;

            SaveTBL(file);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class HexValuePair
    {
        public HexValuePair() { }
        public HexValuePair(string hex, string value)
            : this()
        {
            Hex = hex;
            Value = value;
        }

        public string Hex { get; set; }
        public string Value { get; set; }
    }
}
