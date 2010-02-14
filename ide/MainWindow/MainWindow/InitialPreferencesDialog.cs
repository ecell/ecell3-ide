//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Form class to select the initial window setting.
    /// </summary>
    public partial class InitialPreferencesDialog : Form
    {
        /// <summary>
        /// The path of the initial window setting file.
        /// </summary>
        private string m_selectPath = null;

        /// <summary>
        /// The language code
        /// </summary>
        private CultureInfo m_lang = null;

        /// <summary>
        /// Dictionary of the initial window setting.
        /// </summary>
        private List<WindowSetting> m_dicPath = new List<WindowSetting>();
        private List<RadioButton> m_patternList = new List<RadioButton>();

        public CultureInfo Language
        {
            get { return m_lang; }
        }

        public string FilePath
        {
            get { return m_selectPath; }
        }

        /// <summary>
        /// Constructor/
        /// </summary>
        public InitialPreferencesDialog(bool isInitial)
        {
            InitializeComponent();
            string lang = Util.GetLanguage().TwoLetterISOLanguageName;
            switch (lang)
            {
                case "en": case "en-US":
                    SIEnglishRadioButton.Checked = true;
                    break;
                case "ja":
                    SIJapaneseRadioButton.Checked = true;
                    break;
                default:
                    SIAutoRadioButton.Checked = true;
                    break;
            }

            // Load WindowSettings
            if (isInitial)
                this.Text = MessageResources.TitleInitialSet;
            string dirStr = Util.GetWindowSettingDir();
            if (dirStr != null)
            {
                int i = 1;
                try
                {
                    StreamReader reader = new StreamReader(
                        (System.IO.Stream)File.OpenRead(
                            Path.Combine(
                                dirStr, Constants.fileWinSettingList)));
                    while (i <= 5)
                    {
                        string line = reader.ReadLine();
                        if (line == null) break;
                        if (line.StartsWith("#")) continue;
                        string[] ele = line.Split(new char[] { '\t' });
                        m_dicPath.Add(new WindowSetting(
                            ele[0],
                            Path.Combine(dirStr, ele[0] + Constants.FileExtXML),
                            Path.Combine(dirStr, ele[0] + Constants.FileExtPNG),
                            ele[1]));
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
                if (!isInitial)
                {
                    m_dicPath.Add(new WindowSetting(
                        "Current",
                        null, null, "not change."));
                }
            }

            // Load WindowSettings
            int id = 0;
            int curId = 0;
            if (!isInitial)
            {
                curId = m_dicPath.Count - 1;
            }
            foreach (WindowSetting d in m_dicPath)
            {
                RadioButton b = new RadioButton();
                b.Tag = (Int32)id;
                b.Text = d.Name;
                if (curId == id)
                {
                    b.Checked = true;
                    SWSNoteTextBox.Text = d.Note;
                    if (d.Image != null)
                    {
                        SWSPictureBox.Image = Image.FromFile(d.Image);
                    }
                    else
                    {
                        SWSPictureBox.Image = null;
                    }
                }
                b.CheckedChanged += new EventHandler(ChangePatternRadioBox);
                SWSPatternListLayoutPanel.Controls.Add(b, 0, id);
                m_patternList.Add(b);
                id++;
            }

            // Set WorkSpace
            string path = Util.GetBaseDir();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Event when SelectButton is clicked.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs.</param>
        private void ClickSWSSelectButton(object sender, EventArgs e)
        {
            foreach ( RadioButton r in m_patternList)
            {
                if (!r.Checked) continue;
                int id = (Int32)r.Tag;
                m_selectPath = m_dicPath[id].Path;
                break;
            }
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
            if (m_dicPath[id].Image != null)
                SWSPictureBox.Image = Image.FromFile(m_dicPath[id].Image);
            else
                SWSPictureBox.Image = null;
        }

        private void SIAutoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.InvariantCulture;
        }

        private void SIJapaneseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.GetCultureInfo("ja");
        }

        private void SIEnglishRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.GetCultureInfo("en-us");
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
