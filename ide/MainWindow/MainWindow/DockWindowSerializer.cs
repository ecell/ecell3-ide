//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using WeifenLuo.WinFormsUI.Docking;
using Ecell.Exceptions;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Class to serialize the object.
    /// </summary>
    public partial class DockWindowSerializer
    {

        /// <summary>
        /// Version of config file.
        /// </summary>
        private const string ConfigFileVersion = "1.2";

        /// <summary>
        /// Save ECell window settings.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="filename"></param>
        /// <param name="isClosing"></param>
        public static void SaveAsXML(MainWindow window, string filename, bool isClosing)
        {
            DockPanel dockPanel = window.dockPanel;
            CheckUnsavableWindows(dockPanel, isClosing);
            CheckFilePath(filename);
            FileStream fs = null;
            XmlTextWriter xmlOut = null;
            try
            {
                // Create xml file
                fs = new FileStream(filename, FileMode.Create);
                xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

                // Use indenting for readability
                xmlOut.Formatting = Formatting.Indented;
                xmlOut.WriteStartDocument();

                // Always begin file with identification and warning
                xmlOut.WriteComment(Constants.xPathFileHeader1);
                xmlOut.WriteComment(Constants.xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement("Application");
                xmlOut.WriteAttributeString("Name", Application.ProductName);
                xmlOut.WriteAttributeString("Version", Application.ProductVersion);
                xmlOut.WriteAttributeString("ConfigFileVersion", ConfigFileVersion);

                // Form settings
                xmlOut.WriteStartElement("Form");
                xmlOut.WriteAttributeString("WindowState", window.WindowState.ToString());
                if (window.WindowState == FormWindowState.Maximized)
                {
                    xmlOut.WriteAttributeString("Top", window.RestoreBounds.Top.ToString());
                    xmlOut.WriteAttributeString("Left", window.RestoreBounds.Left.ToString());
                    xmlOut.WriteAttributeString("Height", window.RestoreBounds.Height.ToString());
                    xmlOut.WriteAttributeString("Width", window.RestoreBounds.Width.ToString());
                }
                else
                {
                    xmlOut.WriteAttributeString("Top", window.DesktopBounds.Top.ToString());
                    xmlOut.WriteAttributeString("Left", window.DesktopBounds.Left.ToString());
                    xmlOut.WriteAttributeString("Height", window.DesktopBounds.Height.ToString());
                    xmlOut.WriteAttributeString("Width", window.DesktopBounds.Width.ToString());
                }
                xmlOut.WriteEndElement();   //</Form>

                // DockPanel settings
                xmlOut.WriteStartElement("DockPanel");//	<DockPanel>
                xmlOut.WriteAttributeString("DockLeftPortion", dockPanel.DockLeftPortion.ToString());
                xmlOut.WriteAttributeString("DockRightPortion", dockPanel.DockRightPortion.ToString());
                xmlOut.WriteAttributeString("DockTopPortion", dockPanel.DockTopPortion.ToString());
                xmlOut.WriteAttributeString("DockBottomPortion", dockPanel.DockBottomPortion.ToString());
                xmlOut.WriteAttributeString("ActiveDocumentPane", dockPanel.Panes.IndexOf(dockPanel.ActiveDocumentPane).ToString());
                xmlOut.WriteAttributeString("ActivePane", dockPanel.Panes.IndexOf(dockPanel.ActivePane).ToString());
                
                // Contents
                xmlOut.WriteStartElement("Contents");//	<Contents>
                xmlOut.WriteAttributeString("Count", dockPanel.Contents.Count.ToString());
                foreach (DockContent content in dockPanel.Contents)
                {
                    xmlOut.WriteStartElement("Content");
                    xmlOut.WriteAttributeString("ID", dockPanel.Contents.IndexOf(content).ToString());
                    xmlOut.WriteAttributeString("Name", content.Name.ToString());
                    xmlOut.WriteAttributeString("AutoHidePortion", content.DockHandler.AutoHidePortion.ToString());
                    xmlOut.WriteAttributeString("IsHidden", content.DockHandler.IsHidden.ToString());
                    xmlOut.WriteAttributeString("IsFloat", content.DockHandler.IsFloat.ToString());
                    xmlOut.WriteEndElement();
                }
                xmlOut.WriteEndElement();//	</Contents>

                // Panes
                xmlOut.WriteStartElement("Panes");//	<Panes>
                xmlOut.WriteAttributeString("Count", dockPanel.Panes.Count.ToString());
                foreach (DockPane pane in dockPanel.Panes)
                {
                    xmlOut.WriteStartElement("Pane");//	<Panes>
                    xmlOut.WriteAttributeString("ID", dockPanel.Panes.IndexOf(pane).ToString());
                    xmlOut.WriteAttributeString("DockState", pane.DockState.ToString());
                    xmlOut.WriteAttributeString("ActiveContent", dockPanel.Contents.IndexOf(pane.ActiveContent).ToString());
                    xmlOut.WriteStartElement("Contents");//	<Contents>
                    xmlOut.WriteAttributeString("Count", pane.Contents.Count.ToString());
                    foreach (DockContent content in pane.Contents)
                    {
                        xmlOut.WriteStartElement("Content");//	<Content>
                        xmlOut.WriteAttributeString("ID", pane.Contents.IndexOf(content).ToString());
                        xmlOut.WriteAttributeString("Name", content.Name);
                        xmlOut.WriteAttributeString("RefID", dockPanel.Contents.IndexOf(content).ToString());
                        xmlOut.WriteEndElement();//	</Content>
                    }
                    xmlOut.WriteEndElement();//	</Contents>
                    xmlOut.WriteEndElement();//	</Pane>
                }
                xmlOut.WriteEndElement();//	</Panes>

                // DockWindows
                xmlOut.WriteStartElement("DockWindows");
                int dockWindowId = 0;
                foreach (DockWindow dw in dockPanel.DockWindows)
                {
                    xmlOut.WriteStartElement("DockWindow");
                    xmlOut.WriteAttributeString("ID", dockWindowId.ToString());
                    dockWindowId++;
                    xmlOut.WriteAttributeString("DockState", dw.DockState.ToString());
                    xmlOut.WriteAttributeString("ZOrderIndex", dockPanel.Controls.IndexOf(dw).ToString());
                    xmlOut.WriteStartElement("NestedPanes");
                    xmlOut.WriteAttributeString("Count", dw.NestedPanes.Count.ToString());
                    foreach (DockPane pane in dw.NestedPanes)
                    {
                        xmlOut.WriteStartElement("Pane");
                        xmlOut.WriteAttributeString("ID", dw.NestedPanes.IndexOf(pane).ToString());
                        xmlOut.WriteAttributeString("RefID", dockPanel.Panes.IndexOf(pane).ToString());
                        NestedDockingStatus status = pane.NestedDockingStatus;
                        xmlOut.WriteAttributeString("PrevPane", dockPanel.Panes.IndexOf(status.PreviousPane).ToString());
                        xmlOut.WriteAttributeString("Alignment", status.Alignment.ToString());
                        xmlOut.WriteAttributeString("Proportion", status.Proportion.ToString());
                        xmlOut.WriteEndElement();
                    }
                    xmlOut.WriteEndElement();//	</NestedPanes>
                    xmlOut.WriteEndElement();//	</DockWindow>
                }
                xmlOut.WriteEndElement();//	</DockWindows>

                // FloatWindows
                RectangleConverter rectConverter = new RectangleConverter();
                xmlOut.WriteStartElement("FloatWindows");
                xmlOut.WriteAttributeString("Count", dockPanel.FloatWindows.Count.ToString());
                foreach (FloatWindow fw in dockPanel.FloatWindows)
                {
                    xmlOut.WriteStartElement("FloatWindow");
                    xmlOut.WriteAttributeString("ID", dockPanel.FloatWindows.IndexOf(fw).ToString());
                    xmlOut.WriteAttributeString("Bounds", rectConverter.ConvertToInvariantString(fw.Bounds));
                    xmlOut.WriteAttributeString("ZOrderIndex", fw.DockPanel.FloatWindows.IndexOf(fw).ToString());
                    xmlOut.WriteStartElement("NestedPanes");
                    xmlOut.WriteAttributeString("Count", fw.NestedPanes.Count.ToString());
                    foreach (DockPane pane in fw.NestedPanes)
                    {
                        xmlOut.WriteStartElement("Pane");
                        xmlOut.WriteAttributeString("ID", fw.NestedPanes.IndexOf(pane).ToString());
                        xmlOut.WriteAttributeString("RefID", dockPanel.Panes.IndexOf(pane).ToString());
                        NestedDockingStatus status = pane.NestedDockingStatus;
                        xmlOut.WriteAttributeString("PrevPane", dockPanel.Panes.IndexOf(status.PreviousPane).ToString());
                        xmlOut.WriteAttributeString("Alignment", status.Alignment.ToString());
                        xmlOut.WriteAttributeString("Proportion", status.Proportion.ToString());
                        xmlOut.WriteEndElement();
                    }
                    xmlOut.WriteEndElement();//	</NestedPanes>
                    xmlOut.WriteEndElement();//	</FloatWindow>
                }
                xmlOut.WriteEndElement();	//	</FloatWindows>

                xmlOut.WriteEndElement();   //	</DockPanel>
                xmlOut.WriteEndElement();   //	</Application>

                //xmlOut.WriteEndDocument();
                xmlOut.WriteEndDocument();
            }
            finally
            {
                if (xmlOut != null) xmlOut.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// Close TracerWindow.
        /// </summary>
        private static void CloseUnSavableWindows(DockPanel dockPanel)
        {
            List<DockContent> list = new List<DockContent>();
            foreach (DockContent content in dockPanel.Contents)
            {
                if (!(content is EcellDockContent))
                    list.Add(content);
                else if (!((EcellDockContent)content).IsSavable)
                    list.Add(content);
            }
            if (list.Count <= 0)
                return;

            foreach (DockContent content in list)
                content.Close();

            dockPanel.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="isClosing"></param>
        private static void CheckUnsavableWindows(DockPanel dockPanel, bool isClosing)
        {
            string list = "";
            foreach (DockContent content in dockPanel.Contents)
            {
                if (!(content is EcellDockContent))
                    list += Environment.NewLine + " - " + content.Text;
                else if (!((EcellDockContent)content).IsSavable)
                    list += Environment.NewLine + " - " + content.Text;
            }

            if (string.IsNullOrEmpty(list) || isClosing)
                return;
            string msg = MessageResources.ConfirmUnsavableWindows + list;
            Util.ShowNoticeDialog(msg);
        }

        /// <summary>
        /// Check file path.
        /// </summary>
        private static void CheckFilePath(string filename)
        {
            string path = Path.GetDirectoryName(filename);
            if(string.IsNullOrEmpty(path))
                return;
            if (!Directory.Exists(path))
               Directory.CreateDirectory(path); 
        }

        /// <summary>
        /// Load ECell window settings.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="filename"></param>
        public static void LoadFromXML(MainWindow window, string filename)
        {
            DockPanel dockPanel = window.dockPanel;
            FileStream fs = null;
            XmlTextReader xmlIn = null;
            try
            {
                // Load XML file
                fs = new FileStream(filename, FileMode.Open);
                xmlIn = new XmlTextReader(fs);
                xmlIn.WhitespaceHandling = WhitespaceHandling.None;
                xmlIn.MoveToContent();

                // Check XML file
                CheckFileStatus(xmlIn);

                // load window settings
                WindowStateStruct windowState = LoadWindowState(xmlIn);
                // Load DockPanelStruct
                DockPanelStruct dockPanelStruct = LoadDockPanelStruct(xmlIn);
                // Load Contents
                ContentStruct[] contents = LoadContents(xmlIn);
                // Load Panes
                PaneStruct[] panes = LoadPanes(xmlIn);
                // Load DockWindows
                DockWindowStruct[] dockWindows = LoadDockWindows(xmlIn, dockPanel);
                // Load FloatWindows
                FloatWindowStruct[] floatWindows = LoadFloatWindows(xmlIn);
                // close file
                xmlIn.Close();

                dockPanel.SuspendLayout(true);
                CloseUnSavableWindows(dockPanel);
                // Set WindowSetting
                SetWindowStatus(window, windowState);

                // Set DockPanelLayout
                dockPanel.DockLeftPortion = dockPanelStruct.DockLeftPortion;
                dockPanel.DockRightPortion = dockPanelStruct.DockRightPortion;
                dockPanel.DockTopPortion = dockPanelStruct.DockTopPortion;
                dockPanel.DockBottomPortion = dockPanelStruct.DockBottomPortion;
                
                // Set DockWindow ZOrders
                SetDockWindowZOrder(dockPanel, dockWindows);

                // Create Contents
                List<DockContent> contentList = CreateContents(dockPanel, contents);

                // Create panes
                List<DockPane> paneList = CreatePanes(dockPanel, panes, contentList);

                // Assign Panes to DockWindows
                AssignPanes(dockPanel, panes, dockWindows, paneList);

                // Create float windows
                CreateFloatWindows(dockPanel, panes, floatWindows, paneList);

                // Create float windows for unrecorded contents
                CreateFloatWindowForUnrecordedContents(dockPanel, contentList, paneList);

                // sort IDockContent by its Pane's ZOrder
                int[] sortedContents = SortContents(dockPanel, contents, panes);

                // show non-document IDockContent first to avoid screen flickers
                for (int i = 0; i < contents.Length; i++)
                {
                    IDockContent content = contentList[sortedContents[i]];
                    if (content.DockHandler.Pane != null && content.DockHandler.Pane.DockState != DockState.Document)
                        content.DockHandler.IsHidden = contents[sortedContents[i]].IsHidden;
                }

                // after all non-document IDockContent, show document IDockContent
                for (int i = 0; i < contents.Length; i++)
                {
                    IDockContent content = contentList[sortedContents[i]];
                    if (content.DockHandler.Pane != null && content.DockHandler.Pane.DockState == DockState.Document)
                        content.DockHandler.IsHidden = contents[sortedContents[i]].IsHidden;
                }

                // Activate the Contents, Panes and DockWindows.
                for (int i = 0; i < panes.Length; i++)
                    if (panes[i].IndexActiveContent < 0 || panes[i].IndexActiveContent >= contentList.Count)
                        paneList[i].ActiveContent = null;
                    else
                        paneList[i].ActiveContent = contentList[panes[i].IndexActiveContent];
                if (dockPanelStruct.IndexActiveDocumentPane != -1)
                    paneList[dockPanelStruct.IndexActiveDocumentPane].Activate();
                if (dockPanelStruct.IndexActivePane != -1)
                    paneList[dockPanelStruct.IndexActivePane].Activate();
                dockPanel.ResumeLayout(true, true);

                CloseUnSavableWindows(dockPanel);
            }
            catch (Exception e)
            {
                foreach (DockContent content in dockPanel.Contents)
                {
                }

                Debug.Print(e.StackTrace);
                throw new EcellException("Failed to load file: " + filename, e);
            }
            finally
            {
                if (xmlIn != null) xmlIn.Close();
                if (fs != null) fs.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="contents"></param>
        /// <param name="panes"></param>
        /// <returns></returns>
        private static int[] SortContents(DockPanel dockPanel, ContentStruct[] contents, PaneStruct[] panes)
        {
            int[] sortedContents = null;
            if (contents.Length > 0)
            {
                sortedContents = new int[contents.Length];
                for (int i = 0; i < contents.Length; i++)
                    sortedContents[i] = i;

                for (int i = 0; i < contents.Length - 1; i++)
                {
                    for (int j = i + 1; j < contents.Length; j++)
                    {
                        DockPane pane1 = dockPanel.Contents[sortedContents[i]].DockHandler.Pane;
                        int ZOrderIndex1 = GetZOrderIndex(dockPanel, panes, pane1);
                        DockPane pane2 = dockPanel.Contents[sortedContents[j]].DockHandler.Pane;
                        int ZOrderIndex2 = GetZOrderIndex(dockPanel, panes, pane2); ;
                        if (ZOrderIndex1 > ZOrderIndex2)
                        {
                            int temp = sortedContents[i];
                            sortedContents[i] = sortedContents[j];
                            sortedContents[j] = temp;
                        }
                    }
                }
            }
            return sortedContents;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="contentList"></param>
        /// <param name="paneList"></param>
        private static void CreateFloatWindowForUnrecordedContents(DockPanel dockPanel, List<DockContent> contentList, List<DockPane> paneList)
        {
            foreach (DockContent content in dockPanel.Contents)
            {
                // Check Unrecorded content.
                bool isRecorded = false;
                foreach (DockContent recorded in contentList)
                {
                    if (content == recorded)
                    {
                        isRecorded = true;
                        break;
                    }
                }
                if (isRecorded)
                    continue;
                // Create new content.
                content.IsHidden = true;
                content.AutoHidePortion = 0.25;
                content.Pane = null;
                content.PanelPane = null;
                content.FloatPane = null;
                contentList.Add(content);
                DockPane pane = dockPanel.DockPaneFactory.CreateDockPane(content, DockState.Float, false);
                content.DockHandler.FloatPane = pane;
                content.IsFloat = true;
                paneList.Add(pane);
                FloatWindow fw = dockPanel.FloatWindowFactory.CreateFloatWindow(dockPanel, pane, content.Bounds);
                CheckWindowSize(fw);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="panes"></param>
        /// <param name="dockWindows"></param>
        /// <param name="paneList"></param>
        private static void AssignPanes(DockPanel dockPanel, PaneStruct[] panes, DockWindowStruct[] dockWindows, List<DockPane> paneList)
        {
            for (int i = 0; i < dockWindows.Length; i++)
            {
                DockWindow dw = dockPanel.DockWindows[dockWindows[i].DockState];
                for (int j = 0; j < dockWindows[i].NestedPanes.Length; j++)
                {
                    int indexPane = dockWindows[i].NestedPanes[j].IndexPane;
                    DockPane pane = paneList[indexPane];
                    int indexPrevPane = dockWindows[i].NestedPanes[j].IndexPrevPane;
                    DockPane prevPane = (indexPrevPane == -1) ? dw.NestedPanes.GetDefaultPreviousPane(pane) : paneList[indexPrevPane];
                    DockAlignment alignment = dockWindows[i].NestedPanes[j].Alignment;
                    double proportion = dockWindows[i].NestedPanes[j].Proportion;
                    pane.DockTo(dw, prevPane, alignment, proportion);
                    if (panes[indexPane].DockState == dw.DockState)
                        panes[indexPane].ZOrderIndex = dockWindows[i].ZOrderIndex;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="panes"></param>
        /// <param name="floatWindows"></param>
        /// <param name="paneList"></param>
        private static void CreateFloatWindows(DockPanel dockPanel, PaneStruct[] panes, FloatWindowStruct[] floatWindows, List<DockPane> paneList)
        {
            for (int i = 0; i < floatWindows.Length; i++)
            {
                FloatWindow fw = null;
                for (int j = 0; j < floatWindows[i].NestedPanes.Length; j++)
                {
                    int indexPane = floatWindows[i].NestedPanes[j].IndexPane;
                    DockPane pane = paneList[indexPane];
                    if (j == 0)
                    {
                        fw = dockPanel.FloatWindowFactory.CreateFloatWindow(dockPanel, pane, floatWindows[i].Bounds);
                        CheckWindowSize(fw);
                    }
                    else
                    {
                        int indexPrevPane = floatWindows[i].NestedPanes[j].IndexPrevPane;
                        DockPane prevPane = indexPrevPane == -1 ? null : paneList[indexPrevPane];
                        DockAlignment alignment = floatWindows[i].NestedPanes[j].Alignment;
                        double proportion = floatWindows[i].NestedPanes[j].Proportion;
                        pane.DockTo(fw, prevPane, alignment, proportion);
                        if (panes[indexPane].DockState == fw.DockState)
                            panes[indexPane].ZOrderIndex = floatWindows[i].ZOrderIndex;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="panes"></param>
        /// <param name="contentList"></param>
        /// <returns></returns>
        private static List<DockPane> CreatePanes(DockPanel dockPanel, PaneStruct[] panes, List<DockContent> contentList)
        {
            List<DockPane> paneList = new List<DockPane>();
            for (int i = 0; i < panes.Length; i++)
            {
                DockPane pane = null;
                for (int j = 0; j < panes[i].IndexContents.Length; j++)
                {
                    IDockContent content = contentList[panes[i].IndexContents[j]];
                    if (j == 0)
                        pane = dockPanel.DockPaneFactory.CreateDockPane(content, panes[i].DockState, false);
                    else if (panes[i].DockState == DockState.Float)
                        content.DockHandler.FloatPane = pane;
                    else
                        content.DockHandler.PanelPane = pane;
                }
                paneList.Add(pane);
            }
            return paneList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dockPanel"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private static List<DockContent> CreateContents(DockPanel dockPanel, ContentStruct[] contents)
        {
            List<DockContent> contentList = new List<DockContent>();
            for (int i = 0; i < contents.Length; i++)
            {
                IDockContent content = SetDockContent(dockPanel, contents[i]);
                if (content == null)
                    content = new DockContent();
                content.DockHandler.DockPanel = dockPanel;
                content.DockHandler.AutoHidePortion = contents[i].AutoHidePortion;
                content.DockHandler.IsHidden = contents[i].IsHidden;
                content.DockHandler.IsFloat = contents[i].IsFloat;

                contentList.Add((DockContent)content);
            }
            return contentList;
        }

        private static void SetDockWindowZOrder(DockPanel dockPanel, DockWindowStruct[] dockWindows)
        {
            int prevMaxDockWindowZOrder = int.MaxValue;
            for (int i = 0; i < dockWindows.Length; i++)
            {
                int maxDockWindowZOrder = -1;
                int index = -1;
                for (int j = 0; j < dockWindows.Length; j++)
                {
                    if (dockWindows[j].ZOrderIndex > maxDockWindowZOrder && dockWindows[j].ZOrderIndex < prevMaxDockWindowZOrder)
                    {
                        maxDockWindowZOrder = dockWindows[j].ZOrderIndex;
                        index = j;
                    }
                }

                dockPanel.DockWindows[dockWindows[index].DockState].BringToFront();
                prevMaxDockWindowZOrder = maxDockWindowZOrder;
            }
        }

        private static int GetZOrderIndex(DockPanel dockPanel, PaneStruct[] panes, DockPane pane)
        {
            if (pane == null || dockPanel.Panes.IndexOf(pane) >= panes.Length)
                return 0;
            else
                return panes[dockPanel.Panes.IndexOf(pane)].ZOrderIndex;
        }

        private static bool MoveToNextElement(XmlTextReader xmlIn)
        {
            if (!xmlIn.Read())
                return false;
            while (xmlIn.NodeType == XmlNodeType.EndElement)
            {
                if (!xmlIn.Read())
                    return false;
            }
            return true;
        }

        private static void CheckFileStatus(XmlTextReader xmlIn)
        {
            while (!xmlIn.Name.Equals("Application"))
            {
                if (!MoveToNextElement(xmlIn))
                    throw new EcellException(string.Format(MessageResources.ErrFailedToParse,"DockWindow settings"));
            }
            // version check
            string formatVersion = xmlIn.GetAttribute("ConfigFileVersion");
            if (formatVersion == null || !IsFormatVersionValid(formatVersion))
                throw new ArgumentException("Config file format Version error." + Environment.NewLine + "Current version is " + ConfigFileVersion);
        }

        private static bool IsFormatVersionValid(string formatVersion)
        {
            if (formatVersion == ConfigFileVersion)
                return true;

            //foreach (string s in CompatibleConfigFileVersions)
            //    if (s == formatVersion)
            //        return true;

            return false;
        }

        private static void CheckWindowSize(Form win)
        {
            if (win.Left < 0)
                win.Left = 0;
            if (win.Top < 0)
                win.Top = 0;
            if (win.Width > Screen.PrimaryScreen.WorkingArea.Width)
                win.Width = Screen.PrimaryScreen.WorkingArea.Width;
            if (win.Height > Screen.PrimaryScreen.WorkingArea.Height)
                win.Height = Screen.PrimaryScreen.WorkingArea.Height;
            if (win.Width + win.Left > Screen.PrimaryScreen.WorkingArea.Width)
                win.Left = Screen.PrimaryScreen.WorkingArea.Width - win.Width;
            if (win.Height + win.Top > Screen.PrimaryScreen.WorkingArea.Height)
                win.Top = Screen.PrimaryScreen.WorkingArea.Height - win.Height;
        }

        private static void SetWindowStatus(MainWindow window, WindowStateStruct windowState)
        {
            window.WindowState = windowState.WindowState;
            window.Left = windowState.Left;
            window.Top = windowState.Top;
            window.Width = windowState.Width;
            window.Height = windowState.Height;
            CheckWindowSize(window);
        }

        private static DockContent SetDockContent(DockPanel dockPanel, ContentStruct contentStruct)
        {
            // Get DockContent.
            DockContent content = null;
            foreach (DockContent cont in dockPanel.Contents)
                if (cont.Name.Equals(contentStruct.Name))
                    content = cont;
            if (content == null)
                return null;
            // Set parameter.
            content.Pane = null;
            content.PanelPane = null;
            content.FloatPane = null;
            content.IsHidden = contentStruct.IsHidden;
            content.IsFloat = contentStruct.IsFloat;
            content.AutoHidePortion = contentStruct.AutoHidePortion;
            
            return content;
        }

        #region XML Parser for DockContents.
        /// <summary>
        /// Parse WindowState.
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <returns></returns>
        private static WindowStateStruct LoadWindowState(XmlTextReader xmlIn)
        {
            while (!xmlIn.Name.Equals("Form"))
            {
                if (!MoveToNextElement(xmlIn))
                    throw new ArgumentException();
            }
            EnumConverter windowStateConverter = new EnumConverter(typeof(FormWindowState));
            WindowStateStruct windowState = new WindowStateStruct();
            windowState.WindowState = (FormWindowState)windowStateConverter.ConvertFrom(xmlIn.GetAttribute("WindowState"));
            windowState.Left = Convert.ToInt32(xmlIn.GetAttribute("Left"), CultureInfo.InvariantCulture);
            windowState.Top = Convert.ToInt32(xmlIn.GetAttribute("Top"), CultureInfo.InvariantCulture);
            windowState.Height = Convert.ToInt32(xmlIn.GetAttribute("Height"), CultureInfo.InvariantCulture);
            windowState.Width = Convert.ToInt32(xmlIn.GetAttribute("Width"), CultureInfo.InvariantCulture);
            return windowState;
        }
        /// <summary>
        /// Parse DockPanelState.
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <returns></returns>
        private static DockPanelStruct LoadDockPanelStruct(XmlTextReader xmlIn)
        {
            while (!xmlIn.Name.Equals("DockPanel"))
            {
                if (!MoveToNextElement(xmlIn))
                    throw new ArgumentException("No DockPanel.");
            }
            DockPanelStruct dockPanelStruct = new DockPanelStruct();
            dockPanelStruct.DockLeftPortion = Convert.ToDouble(xmlIn.GetAttribute("DockLeftPortion"), CultureInfo.InvariantCulture);
            dockPanelStruct.DockRightPortion = Convert.ToDouble(xmlIn.GetAttribute("DockRightPortion"), CultureInfo.InvariantCulture);
            dockPanelStruct.DockTopPortion = Convert.ToDouble(xmlIn.GetAttribute("DockTopPortion"), CultureInfo.InvariantCulture);
            dockPanelStruct.DockBottomPortion = Convert.ToDouble(xmlIn.GetAttribute("DockBottomPortion"), CultureInfo.InvariantCulture);
            dockPanelStruct.IndexActiveDocumentPane = Convert.ToInt32(xmlIn.GetAttribute("ActiveDocumentPane"), CultureInfo.InvariantCulture);
            dockPanelStruct.IndexActivePane = Convert.ToInt32(xmlIn.GetAttribute("ActivePane"), CultureInfo.InvariantCulture);
            return dockPanelStruct;
        }
        /// <summary>
        /// Parse DockContentState
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <returns></returns>
        private static ContentStruct[] LoadContents(XmlTextReader xmlIn)
        {
            MoveToNextElement(xmlIn);
            if (xmlIn.Name != "Contents")
                throw new ArgumentException("No DockContents.");

            int countOfContents = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture);
            ContentStruct[] contents = new ContentStruct[countOfContents];
            MoveToNextElement(xmlIn);
            for (int i = 0; i < countOfContents; i++)
            {
                int id = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture);
                if (xmlIn.Name != "Content" || id != i)
                    throw new ArgumentException();

                contents[i].Name = xmlIn.GetAttribute("Name");
                contents[i].AutoHidePortion = Convert.ToDouble(xmlIn.GetAttribute("AutoHidePortion"), CultureInfo.InvariantCulture);
                contents[i].IsHidden = Convert.ToBoolean(xmlIn.GetAttribute("IsHidden"), CultureInfo.InvariantCulture);
                contents[i].IsFloat = Convert.ToBoolean(xmlIn.GetAttribute("IsFloat"), CultureInfo.InvariantCulture);
                MoveToNextElement(xmlIn);
            }

            return contents;
        }
        /// <summary>
        /// Parse DockPaneState
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <returns></returns>
        private static PaneStruct[] LoadPanes(XmlTextReader xmlIn)
        {
            if (xmlIn.Name != "Panes")
                throw new ArgumentException("No DockPanes.");

            EnumConverter dockStateConverter = new EnumConverter(typeof(DockState));
            int countOfPanes = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture);
            PaneStruct[] panes = new PaneStruct[countOfPanes];
            MoveToNextElement(xmlIn);
            for (int i = 0; i < countOfPanes; i++)
            {
                int id = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture);
                if (xmlIn.Name != "Pane" || id != i)
                    throw new ArgumentException();

                panes[i].DockState = (DockState)dockStateConverter.ConvertFrom(xmlIn.GetAttribute("DockState"));
                panes[i].IndexActiveContent = Convert.ToInt32(xmlIn.GetAttribute("ActiveContent"), CultureInfo.InvariantCulture);
                panes[i].ZOrderIndex = -1;

                MoveToNextElement(xmlIn);
                if (xmlIn.Name != "Contents")
                    throw new ArgumentException();
                int countOfPaneContents = Convert.ToInt32(xmlIn.GetAttribute("Count"), CultureInfo.InvariantCulture);
                panes[i].IndexContentNames = new string[countOfPaneContents];
                panes[i].IndexContents = new int[countOfPaneContents];
                MoveToNextElement(xmlIn);
                for (int j = 0; j < countOfPaneContents; j++)
                {
                    int id2 = Convert.ToInt32(xmlIn.GetAttribute("ID"), CultureInfo.InvariantCulture);
                    if (xmlIn.Name != "Content" || id2 != j)
                        throw new ArgumentException();

                    panes[i].IndexContentNames[j] = xmlIn.GetAttribute("Name");
                    panes[i].IndexContents[j] = Convert.ToInt32(xmlIn.GetAttribute("RefID"), CultureInfo.InvariantCulture);
                    MoveToNextElement(xmlIn);
                }
            }

            return panes;
        }
        /// <summary>
        /// Parse DockWindowState
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <param name="dockPanel"></param>
        /// <returns></returns>
        private static DockWindowStruct[] LoadDockWindows(XmlTextReader xmlIn, DockPanel dockPanel)
        {
            if (xmlIn.Name != "DockWindows")
                throw new ArgumentException("No DockWindows.");

            EnumConverter dockStateConverter = new EnumConverter(typeof(DockState));
            EnumConverter dockAlignmentConverter = new EnumConverter(typeof(DockAlignment));
            int countOfDockWindows = dockPanel.DockWindows.Count;
            DockWindowStruct[] dockWindows = new DockWindowStruct[countOfDockWindows];
            MoveToNextElement(xmlIn);
            for (int i = 0; i < countOfDockWindows; i++)
            {
                int id = Convert.ToInt32(xmlIn.GetAttribute("ID"));
                if (xmlIn.Name != "DockWindow" || id != i)
                    throw new ArgumentException();

                dockWindows[i].DockState = (DockState)dockStateConverter.ConvertFrom(xmlIn.GetAttribute("DockState"));
                dockWindows[i].ZOrderIndex = Convert.ToInt32(xmlIn.GetAttribute("ZOrderIndex"));
                MoveToNextElement(xmlIn);
                if (xmlIn.Name != "DockList" && xmlIn.Name != "NestedPanes")
                    throw new ArgumentException();
                int countOfNestedPanes = Convert.ToInt32(xmlIn.GetAttribute("Count"));
                dockWindows[i].NestedPanes = new NestedPane[countOfNestedPanes];
                MoveToNextElement(xmlIn);
                for (int j = 0; j < countOfNestedPanes; j++)
                {
                    int id2 = Convert.ToInt32(xmlIn.GetAttribute("ID"));
                    if (xmlIn.Name != "Pane" || id2 != j)
                        throw new ArgumentException();
                    dockWindows[i].NestedPanes[j].IndexPane = Convert.ToInt32(xmlIn.GetAttribute("RefID"));
                    dockWindows[i].NestedPanes[j].IndexPrevPane = Convert.ToInt32(xmlIn.GetAttribute("PrevPane"));
                    dockWindows[i].NestedPanes[j].Alignment = (DockAlignment)dockAlignmentConverter.ConvertFrom(xmlIn.GetAttribute("Alignment"));
                    dockWindows[i].NestedPanes[j].Proportion = Convert.ToDouble(xmlIn.GetAttribute("Proportion"));
                    MoveToNextElement(xmlIn);
                }
            }

            return dockWindows;
        }
        /// <summary>
        /// Parse FloatWindowState
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <returns></returns>
        private static FloatWindowStruct[] LoadFloatWindows(XmlTextReader xmlIn)
        {
            if (xmlIn.Name != "FloatWindows")
                throw new ArgumentException("No FloatWindows");

            EnumConverter dockAlignmentConverter = new EnumConverter(typeof(DockAlignment));
            RectangleConverter rectConverter = new RectangleConverter();
            int countOfFloatWindows = Convert.ToInt32(xmlIn.GetAttribute("Count"));
            FloatWindowStruct[] floatWindows = new FloatWindowStruct[countOfFloatWindows];
            MoveToNextElement(xmlIn);
            for (int i = 0; i < countOfFloatWindows; i++)
            {
                int id = Convert.ToInt32(xmlIn.GetAttribute("ID"));
                if (xmlIn.Name != "FloatWindow" || id != i)
                    throw new ArgumentException();

                floatWindows[i].Bounds = (Rectangle)rectConverter.ConvertFromInvariantString(xmlIn.GetAttribute("Bounds"));
                floatWindows[i].ZOrderIndex = Convert.ToInt32(xmlIn.GetAttribute("ZOrderIndex"));
                MoveToNextElement(xmlIn);
                if (xmlIn.Name != "DockList" && xmlIn.Name != "NestedPanes")
                    throw new ArgumentException();
                int countOfNestedPanes = Convert.ToInt32(xmlIn.GetAttribute("Count"));
                floatWindows[i].NestedPanes = new NestedPane[countOfNestedPanes];
                MoveToNextElement(xmlIn);
                for (int j = 0; j < countOfNestedPanes; j++)
                {
                    int id2 = Convert.ToInt32(xmlIn.GetAttribute("ID"));
                    if (xmlIn.Name != "Pane" || id2 != j)
                        throw new ArgumentException();
                    floatWindows[i].NestedPanes[j].IndexPane = Convert.ToInt32(xmlIn.GetAttribute("RefID"));
                    floatWindows[i].NestedPanes[j].IndexPrevPane = Convert.ToInt32(xmlIn.GetAttribute("PrevPane"));
                    floatWindows[i].NestedPanes[j].Alignment = (DockAlignment)dockAlignmentConverter.ConvertFrom(xmlIn.GetAttribute("Alignment"));
                    floatWindows[i].NestedPanes[j].Proportion = Convert.ToDouble(xmlIn.GetAttribute("Proportion"));
                    MoveToNextElement(xmlIn);
                }
            }

            return floatWindows;
        }
        #endregion

        #region Content Structs
        /// <summary>
        /// Struct for WindowState.
        /// </summary>
        private struct WindowStateStruct
        {
            private FormWindowState m_windowState;
            public FormWindowState WindowState
            {
                get { return m_windowState; }
                set { m_windowState = value; }
            }
            private int m_left;

            public int Left
            {
                get { return m_left; }
                set { m_left = value; }
            }
            private int m_top;

            public int Top
            {
                get { return m_top; }
                set { m_top = value; }
            }
            private int m_width;

            public int Width
            {
                get { return m_width; }
                set { m_width = value; }
            }
            private int m_height;

            public int Height
            {
                get { return m_height; }
                set { m_height = value; }
            }
        }
        /// <summary>
        /// Struct for DockPanelState.
        /// </summary>
        private struct DockPanelStruct
        {
            private double m_dockLeftPortion;
            public double DockLeftPortion
            {
                get { return m_dockLeftPortion; }
                set { m_dockLeftPortion = value; }
            }

            private double m_dockRightPortion;
            public double DockRightPortion
            {
                get { return m_dockRightPortion; }
                set { m_dockRightPortion = value; }
            }

            private double m_dockTopPortion;
            public double DockTopPortion
            {
                get { return m_dockTopPortion; }
                set { m_dockTopPortion = value; }
            }

            private double m_dockBottomPortion;
            public double DockBottomPortion
            {
                get { return m_dockBottomPortion; }
                set { m_dockBottomPortion = value; }
            }

            private int m_indexActiveDocumentPane;
            public int IndexActiveDocumentPane
            {
                get { return m_indexActiveDocumentPane; }
                set { m_indexActiveDocumentPane = value; }
            }

            private int m_indexActivePane;
            public int IndexActivePane
            {
                get { return m_indexActivePane; }
                set { m_indexActivePane = value; }
            }
        }
        /// <summary>
        /// Struct for DockPaneState.
        /// </summary>
        private struct PaneStruct
        {
            private DockState m_dockState;
            public DockState DockState
            {
                get { return m_dockState; }
                set { m_dockState = value; }
            }

            private int m_indexActiveContent;
            public int IndexActiveContent
            {
                get { return m_indexActiveContent; }
                set { m_indexActiveContent = value; }
            }

            private int[] m_indexContents;
            public int[] IndexContents
            {
                get { return m_indexContents; }
                set { m_indexContents = value; }
            }

            private string[] m_indexContentNames;
            public string[] IndexContentNames
            {
                get { return m_indexContentNames; }
                set { m_indexContentNames = value; }
            }

            private int m_zOrderIndex;
            public int ZOrderIndex
            {
                get { return m_zOrderIndex; }
                set { m_zOrderIndex = value; }
            }
        }
        /// <summary>
        /// Struct for NestedPaneState.
        /// </summary>
        private struct NestedPane
        {
            private int m_indexPane;
            public int IndexPane
            {
                get { return m_indexPane; }
                set { m_indexPane = value; }
            }

            private int m_indexPrevPane;
            public int IndexPrevPane
            {
                get { return m_indexPrevPane; }
                set { m_indexPrevPane = value; }
            }

            private DockAlignment m_alignment;
            public DockAlignment Alignment
            {
                get { return m_alignment; }
                set { m_alignment = value; }
            }

            private double m_proportion;
            public double Proportion
            {
                get { return m_proportion; }
                set { m_proportion = value; }
            }
        }
        /// <summary>
        /// Struct for DockContentState.
        /// </summary>
        private struct ContentStruct
        {
            private string m_name;
            public string Name
            {
                get { return m_name; }
                set { m_name = value; }
            }

            private double m_autoHidePortion;
            public double AutoHidePortion
            {
                get { return m_autoHidePortion; }
                set { m_autoHidePortion = value; }
            }

            private bool m_isHidden;
            public bool IsHidden
            {
                get { return m_isHidden; }
                set { m_isHidden = value; }
            }

            private bool m_isFloat;
            public bool IsFloat
            {
                get { return m_isFloat; }
                set { m_isFloat = value; }
            }
        }
        /// <summary>
        /// Struct for DockWindowState.
        /// </summary>
        private struct DockWindowStruct
        {
            private DockState m_dockState;
            public DockState DockState
            {
                get { return m_dockState; }
                set { m_dockState = value; }
            }

            private int m_zOrderIndex;
            public int ZOrderIndex
            {
                get { return m_zOrderIndex; }
                set { m_zOrderIndex = value; }
            }

            private NestedPane[] m_nestedPanes;
            public NestedPane[] NestedPanes
            {
                get { return m_nestedPanes; }
                set { m_nestedPanes = value; }
            }
        }
        /// <summary>
        /// Struct for FloatWindowState.
        /// </summary>
        private struct FloatWindowStruct
        {
            private Rectangle m_bounds;
            public Rectangle Bounds
            {
                get { return m_bounds; }
                set { m_bounds = value; }
            }

            private int m_zOrderIndex;
            public int ZOrderIndex
            {
                get { return m_zOrderIndex; }
                set { m_zOrderIndex = value; }
            }

            private NestedPane[] m_nestedPanes;
            public NestedPane[] NestedPanes
            {
                get { return m_nestedPanes; }
                set { m_nestedPanes = value; }
            }

            public static Rectangle DefaultBounds = new Rectangle(100, 100, 300, 300);
        }
        #endregion
    }
}
