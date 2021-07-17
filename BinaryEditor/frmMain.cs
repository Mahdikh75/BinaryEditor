using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace BinaryEditor
{
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
        }
        bool booltxtText;
        bool booltxtBinary;
        OpenFileDialog ofd = null;

        public void methodOpen(Form frm, RichTextBox txtBox, RichTextBox txtBox2)
        {

            FileStream fs;
            BinaryReader br;

            frm.Enabled = false;
            frm.Cursor = Cursors.WaitCursor;
            frm.Text = "Binary File Editor. (Please wait...)";
            txtBox.Clear();
            txtBox2.Clear();

            fs = new FileStream(ofd.FileName, FileMode.Open);

            toolpbState.Maximum = (int)fs.Length * 2;
            toolpbState.Value = 0;
            br = new BinaryReader(fs);
            StringBuilder sb2 = new StringBuilder();
            char cr;
            int i = 0;

            while (i < fs.Length)
            {
                cr = (char)br.ReadByte();
                if (cr != '\0')
                {
                    sb2.Append(cr.ToString());
                }
                i++;
                toolpbState.Value++;
            }
            br.Close();
            fs.Close();

            fs = new FileStream(ofd.FileName, FileMode.Open);
            br = new BinaryReader(fs);
            byte[] bt = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();

            StringBuilder sb = new StringBuilder();
            i = 0;
            while (i < bt.Length)
            {
                if (i == bt.Length - 1)
                {
                    sb.Append(bt[i].ToString());
                }
                else sb.Append(bt[i].ToString() + " ");
                i++;
                toolpbState.Value++;
            }
            txtBox.Text = sb.ToString();
            txtBox2.Text = sb2.ToString();
            frm.Cursor = Cursors.Default;
            frm.Text = "Binary File Editor";
            frm.Enabled = true;

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            ofd.Filter = "All File's (*.*)|*.*";
            ofd.Title = "Open File";
            

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                methodOpen(this, txtBinary, txtText);
            }
        }
        //
        //  Save Button_Click
        //
        private void btnSave_Click(object sender, EventArgs e)
        {
            cms.Show();
            cms.Left = MousePosition.X;
            cms.Top = MousePosition.Y;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toollblState.Text = "State read byte's of selected file:";
            booltxtText = false;
            booltxtBinary = true;
        }

        private void BinSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Format 'Save with this format might be saved bad file!'|*.*|Text Format|*.*";
            sfd.FileName = "*.*";
            sfd.Title = "Save Files Binary Editor Box Dialog";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 1)
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    string[] txtSplited = txtBinary.Text.Split(' ');
                    byte[] bt = new byte[txtSplited.Length];
                    for (int i = 0; i < txtSplited.Length; i++)
                    {
                        bt[i] = byte.Parse(txtSplited[i]);
                    }
                    bw.Write(bt);
                    bw.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(txtBinary.Text);
                    sw.Close();
                    fs.Close();
                }
            }
        }

        private void TextSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Format 'Save with this format might be saved bad file!'|*.*|Text Format|*.*";
            sfd.FileName = "*.*";
            sfd.Title = "Save Files Text Editor Box Dialog";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 1)
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    
                    bw.Write(txtText.Text);
                    bw.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(txtText.Text);
                    sw.Close();
                    fs.Close();
                }
            }
        }

        private void mnubtnNew_Click(object sender, EventArgs e)
        {
            txtBinary.Text = "";
            txtText.Text = "";
            toolpbState.Value = 0;
            txtBinary.Focus();
        }

        private void mnubtnCut_Click(object sender, EventArgs e)
        {
            if (booltxtBinary)
            {
                txtBinary.Cut();
            }
            else if (booltxtText)
            {
                txtText.Cut();
            }
        }

        private void mnubtnCopy_Click(object sender, EventArgs e)
        {
            if (booltxtBinary)
            {
                txtBinary.Copy();
            }
            else if (booltxtText)
            {
                txtText.Copy();
            }
        }

        private void mnubtnPaste_Click(object sender, EventArgs e)
        {
            if (booltxtText)
            {
                txtText.Paste();
            }
            else if (booltxtBinary)
            {
                txtBinary.Paste();
            }
        }

        private void txtBinary_Click(object sender, EventArgs e)
        {
            booltxtBinary = true;
            optBinEditor.Checked = true;
            booltxtText = false;
            optTxtEditor.Checked = false;
        }

        private void txtText_Click(object sender, EventArgs e)
        {
            booltxtBinary = false;
            optBinEditor.Checked = false;
            booltxtText = true;
            optTxtEditor.Checked = true;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtBinary.SelectedText=="" && txtText.SelectedText=="")
            {
                mnubtnCopy.Enabled = false;
                mnubtnCut.Enabled = false;
            }
            else
            {
                mnubtnCopy.Enabled = true;
                mnubtnCut.Enabled = true;
            }
        }

        private void mnubtnSelectAll_Click(object sender, EventArgs e)
        {
            if (booltxtText)
            {
                txtText.SelectAll();
            }
            else if (booltxtBinary)
            {
                txtBinary.SelectAll();
            }
        }

        private void mnuSaveSaveBoth_Click(object sender, EventArgs e)
        {
            BinAndTxtSave_Click(sender, e);
        }

        private void BinAndTxtSave_Click(object sender, EventArgs e)
        {
            BinSave_Click(sender, e);
            TextSave_Click(sender, e);
        }

        private void detectURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtText.DetectUrls)
            {
                txtText.DetectUrls = false;
            }
            else
            {
                txtText.DetectUrls = true;
            }   
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            txtText.ScrollBars = RichTextBoxScrollBars.ForcedHorizontal;
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            txtText.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtText.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
        }
        private void verticalyScrolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBinary.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
        }
        private void hoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBinary.ScrollBars = RichTextBoxScrollBars.ForcedHorizontal;
        }
        private void bothScrollBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBinary.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
        }
        private void saveFileexeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
