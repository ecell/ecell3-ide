//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    /// <summary>
    /// Form class to select the initial window setting.
    /// </summary>
    public partial class SelectWinSettingWindow : Form
    {
        /// <summary>
        /// The path of the initial window setting file.
        /// </summary>
        private string m_selectPath = null;
        /// <summary>
        /// Dictionary of the initial window setting.
        /// </summary>
        private Dictionary<int, WindowSetting> m_dicPath = new Dictionary<int, WindowSetting>();
        
        /// <summary>
        /// Constructor/
        /// </summary>
        public SelectWinSettingWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the list of window setting.
        /// </summary>
        private void LoadSetting()
        {
            int i = 1;
            string path = Util.GetWindowSettingDir();
            if (path == null)
            {
                throw new IgnoreException("Not found the registry key");
            }
            StreamReader reader = new StreamReader(
                (System.IO.Stream)File.OpenRead(path + "/settinglist.conf"));
            while (i <= 4)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                if (line.StartsWith("#")) continue;
                string[] ele = line.Split(new char[] { '\t' });
                WindowSetting s = new WindowSetting(
                    ele[0],
                    path + "/" + ele[0] + ".xml",
                    path + "/" + ele[0] + ".png",
                    ele[1]);
                m_dicPath.Add(i, s);
                i++;
            }
        }

        /// <summary>
        /// Layout the information of window setting.
        /// </summary>
        private void LayoutSetting()
        {
            foreach (int id in m_dicPath.Keys)
            {
                if (id == 1)
                {
                    SWSSetting1RadioButton.Tag = Convert.ToInt32(1);
                    SWSSetting1RadioButton.Text = m_dicPath[id].Name;
                    SWSNoteTextBox.Text = m_dicPath[id].Note;
                    SWSPictureBox.Image = Image.FromFile(m_dicPath[id].Image);
                }
                else if (id == 2)
                {
                    SWSSetting2RadioButton.Tag = Convert.ToInt32(2);
                    SWSSetting2RadioButton.Text = m_dicPath[id].Name;
                }
                else if (id == 3)
                {
                    SWSSetting3RadioButton.Tag = Convert.ToInt32(3);
                    SWSSetting3RadioButton.Text = m_dicPath[id].Name;
                }
                else if (id == 4)
                {
                    SWSSetting4RadioButton.Tag = Convert.ToInt32(4);
                    SWSSetting4RadioButton.Text = m_dicPath[id].Name;
                }
            }
        }

        /// <summary>
        /// Display this form.
        /// </summary>
        /// <returns>path of the selected window setting.</returns>
        public string ShowWindow()
        {
            LoadSetting();
            LayoutSetting();
            this.ShowDialog();
            return m_selectPath;
        }

        /// <summary>
        /// Event when SelectButton is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs.</param>
        private void ClickSWSSelectButton(object sender, EventArgs e)
        {
            if (SWSSetting1RadioButton.Checked) m_selectPath = m_dicPath[1].Path;
            else if (SWSSetting2RadioButton.Checked) m_selectPath = m_dicPath[2].Path;
            else if (SWSSetting3RadioButton.Checked) m_selectPath = m_dicPath[3].Path;
            else if (SWSSetting4RadioButton.Checked) m_selectPath = m_dicPath[4].Path;
            this.Close();
        }

        /// <summary>
        /// Event when the check of RadioButton is changed.
        /// </summary>
        /// <param name="sender">RadioButton.</param>
        /// <param name="e">EventArgs.</param>
        private void ChangePatternRadioBox(object sender, EventArgs e)
        {
            RadioButton r = sender as RadioButton;
            if (r == null) return;
            if (!r.Checked) return;
            Int32 id = (Int32)r.Tag;

            SWSNoteTextBox.Text = m_dicPath[id].Note;
            SWSPictureBox.Image = Image.FromFile(m_dicPath[id].Image);            
        }
    }

    /// <summary>
    /// Class the information of window setting.
    /// </summary>
    public class WindowSetting
    {
        /// <summary>
        /// The name of window setting.
        /// </summary>
        private string m_name;
        /// <summary>
        /// The file path of window setting.
        /// </summary>
        private string m_path;
        /// <summary>
        /// The note of window setting.
        /// </summary>
        private string m_note;
        /// <summary>
        /// The image file path of window setting.
        /// </summary>
        private string m_imagePath;

        /// <summary>
        /// Constructoru with initial parameters.
        /// </summary>
        /// <param name="name">The name of window setting.</param>
        /// <param name="path">The file path of window setting.</param>
        /// <param name="imagePath">The image file path of window setting.</param>
        /// <param name="note">The note of window setting.</param>
        public WindowSetting(string name, string path, string imagePath, string note)
        {
            m_name = name;
            m_path = path;
            m_imagePath = imagePath;
            m_note = note;
        }

        /// <summary>
        /// get the name of window setting.
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// get the file path of window setting.
        /// </summary>
        public string Path
        {
            get { return m_path; }
        }

        /// <summary>
        /// get the image file path of window setting.
        /// </summary>
        public string Image
        {
            get { return m_imagePath; }
        }

        /// <summary>
        /// get the note of window setting.
        /// </summary>
        public string Note
        {
            get { return m_note; }
        }
    }
}