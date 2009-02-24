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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell
{
    /// <summary>
    /// Class to manage the constant value.
    /// </summary>
    public class Constants
    {
        #region Group name
        /// <summary>
        /// 
        /// </summary>
        public const string groupDebug = "debug";
        /// <summary>
        /// 
        /// </summary>
        public const string groupCompile = "compile";
        #endregion

        #region Default Settings
        /// <summary>
        /// Reserved the name of file..
        /// </summary>
        public const string fileProject = "project.info";
        /// <summary>
        /// Reserved the name of file..
        /// </summary>
        public const string fileProjectXML = "project.xml";
        /// <summary>
        /// Reserved the file name of window setting.
        /// </summary>
        public const string fileWinSetting = "window.config";
        /// <summary>
        /// Reserved the file name of window setting list.
        /// </summary>
        public const string fileWinSettingList = "settinglist.conf";
        /// <summary>
        /// Reserved the file name of startup html.
        /// </summary>
        public const string fileStartupHTML = "startup";
        /// <summary>
        /// The default process name
        /// </summary>
        public const string DefaultProcessName = "MassActionFluxProcess"; // "BisectionRapidEquilibriumProcess";
        /// <summary>
        /// The default stepper name
        /// </summary>
        public const string DefaultStepperName = "FixedODE1Stepper";

        #endregion

        #region File Extentions
        /// <summary>
        /// Reserved extension for DM's.
        /// </summary>
        public const string WildCard = "*";
        /// <summary>
        /// Reserved extension for DM's.
        /// </summary>
        public const string FileExtDM = ".dll";
        /// <summary>
        /// Reserved extension for plugins.
        /// </summary>
        public const string FileExtPlugin = ".dll";
        /// <summary>
        /// The extention of EML file.
        /// </summary>
        public const string FileExtEML = ".eml";
        /// <summary>
        /// The extention of LEML file.
        /// </summary>
        public const string FileExtLEML = ".leml";
        /// <summary>
        /// The extention of SBML file.
        /// </summary>
        public const string FileExtSBML = ".sbml";
        /// <summary>
        /// The extention of XML file.
        /// </summary>
        public const string FileExtXML = ".xml";
        /// <summary>
        /// The extention of XML file.
        /// </summary>
        public const string FileExtINFO = ".info";
        /// <summary>
        /// The extention of csv file.
        /// </summary>
        public const string FileExtCSV= ".csv";
        /// <summary>
        /// The extention of BMP file.
        /// </summary>
        public const string FileExtBMP = ".bmp";
        /// <summary>
        /// The extention of Png file.
        /// </summary>
        public const string FileExtPNG = ".png";
        /// <summary>
        /// The extention of JPG file.
        /// </summary>
        public const string FileExtJPG = ".jpg";
        /// <summary>
        /// The extention of gif file.
        /// </summary>
        public const string FileExtGIF = ".gif";
        /// <summary>
        /// The extention of png file.
        /// </summary>
        public const string FileExtSVG = ".svg";
        /// <summary>
        /// The extention of BackUp file.
        /// </summary>
        public const string FileExtBackUp = ".bak";
        /// <summary>
        /// The extention of source file.
        /// </summary>
        public const string FileExtSource = ".cpp";
        /// <summary>
        /// The extention of HTML file.
        /// </summary>
        public const string FileExtHTML = ".html";
        #endregion

        #region File Filters
        /// <summary>
        /// File Filter for the action file.
        /// </summary>
        public const string FilterActionFile = "Action File (*.xml)|*.xml";
        /// <summary>
        /// File Filter for the csv file.
        /// </summary>
        public const string FilterCSVFile = "CSV File (*.csv)|*.csv|all(*.*)|*.*";
        /// <summary>
        /// 
        /// </summary>
        public const string FilterECDFile = "Log File(*.ecd)|*.ecs|all(*.*)|*.*";
        /// <summary>
        /// File Filter for the window setting file.
        /// </summary>
        public const string FilterDMFile = "C++ Source (*.cpp)|*.cpp|all(*.*)|*.*";
        /// <summary>
        /// File Filter for the model file.
        /// </summary>
        public const string FilterEmlFile = "E-Cell Model (*.eml)|*.eml|all(*.*)|*.*";
        /// <summary>
        /// File Filter for the SBML file.
        /// </summary>
        public const string FilterSBMLFile = "SBML (*.sbml)|*.sbml|all(*.*)|*.*";
        /// <summary>
        /// File Filter for the script file.
        /// </summary>
        public const string FilterEssFile = "E-Cell Script Files (*.ess)|*.ess|all(*.*)|*.*";
        /// <summary>
        /// File Filter for the window setting file.
        /// </summary>
        public const string FilterWinSetFile = "Window Settings (*.xml)|*.xml|all(*.*)|*.*";
        /// <summary>
        /// File Filter for the zip file.
        /// </summary>
        public const string FilterZipFile = "Zip Archive (*.zip)|*.zip";
        /// <summary>
        /// File Filter for the zip file.
        /// </summary>
        public const string FilterSVGFile = "SVG File - Scalable Vector Graphics (*.svg)|*.svg";
        /// <summary>
        /// File Filter for the zip file.
        /// </summary>
        public const string FilterImageFile = "BMP File - Windows Bitmap (*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg|GIF File(*.gif)|*.gif|PNG File(*.png)|*.png";

        #endregion

        #region Delimiters
        /// <summary>
        /// Reserved char for comma.
        /// </summary>
        public const string delimiterComma = ",";
        /// <summary>
        /// Reserved char for colon.
        /// </summary>
        public const string delimiterColon = ":";
        /// <summary>
        /// Reserved char for equal.
        /// </summary>
        public const string delimiterEqual = "=";
        /// <summary>
        /// Reserved char for Hyphen.
        /// </summary>
        public const string delimiterHyphen = "-";
        /// <summary>
        /// Reserved char for Path.
        /// </summary>
        public const string delimiterPath = "/";
        /// <summary>
        /// Reserved char for period.
        /// </summary>
        public const string delimiterPeriod = ".";
        /// <summary>
        /// Reserved char for semi colon.
        /// </summary>
        public const string delimiterSemiColon = ";";
        /// <summary>
        /// Reserved char for sharp.
        /// </summary>
        public const string delimiterSharp = "#";
        /// <summary>
        /// Reserved char for space.
        /// </summary>
        public const string delimiterSpace = " ";
        /// <summary>
        /// Reserved char for delimiter.
        /// </summary>
        public const string delimiterTab = "\t";
        /// <summary>
        /// Reserved char for ubderbar,
        /// </summary>
        public const string delimiterUnderbar = "_";
        /// <summary>
        /// Reserved char for wild card.
        /// </summary>
        public const string delimiterWildcard = "*";

        #endregion

        #region Regstry Keys
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE ANALYSIS".
        /// </summary>
        public const string registryAnalysisDirKey = "E-CELL IDE ANALYSIS";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE BASE".
        /// </summary>
        public const string registryBaseDirKey = "E-CELL IDE BASE";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE DM".
        /// </summary>
        public const string registryDMDirKey = "E-CELL IDE DM";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE PLUGIN".
        /// </summary>
        public const string registryPluginDirKey = "E-CELL IDE PLUGIN";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE LANG".
        /// </summary>
        public const string registryLang = "E-CELL IDE LANG";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE STATICDEBUG PLUGIN".
        /// </summary>
        public const string registryStaticDebugDirKey = "E-CELL IDE STATICDEBUG PLUGIN";
        /// <summary>
        /// Reserved the registry key of startup.
        /// </summary>
        public const string registryStartup = "E-CELL IDE STARTUP";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE TMP".
        /// </summary>
        public const string registryTmpDirKey = "E-CELL IDE TMP";
        /// <summary>
        /// Reserved XML path name for key of environment.
        /// </summary>
        public const string registryEnvKey = "Environment";
        /// <summary>
        /// Reserved XML path name for key of Software.
        /// </summary>
        public const string registrySWKey = "software\\KeioUniv\\E-Cell IDE";
        /// <summary>
        /// Reserved XML path name for key of Software.
        /// </summary>
        public const string registrySW2Key = "software\\KeioUniv";
        /// <summary>
        /// Reserved the key of registry for E-CELL WINDOW SETTING.
        /// </summary>
        public const string registryWinSetDir = "E-CELL IDE WINDOW SETTING";

        #endregion

        #region XML Path
        /// <summary>
        /// Reserved XML path name for file header.
        /// </summary>
        public const string xPathEcellProject = "ECellProject";
        /// <summary>
        /// Reserved XML path name for file header.
        /// </summary>
        public const string xPathFileHeader1 = "ECell-IDE configuration file.";
        /// <summary>
        /// Reserved XML path name for file header.
        /// </summary>
        public const string xPathFileHeader2 = "Automatically generated file. DO NOT modify!";
        /// <summary>
        /// Reserved XML path name for action.
        /// </summary>
        public const string xpathAction = "Action";
        /// <summary>
        /// Reserved XML path name for activity.
        /// </summary>
        public const string xpathActivity = "Activity";
        /// <summary>
        /// Reserved XML path name for class.
        /// </summary>
        public const string xpathClass = "class";
        /// <summary>
        /// Reserved XML path name for class name.
        /// </summary>
        public const string xpathClassName = "ClassName";
        /// <summary>
        /// Reserved XML path name for ECD.
        /// </summary>
        public const string xpathEcd = "ecd";
        /// <summary>
        /// Reserved XML path name for CSV.
        /// </summary>
        public const string xpathCsv = "csv";
        /// <summary>
        /// Reserved XML path name for DM.
        /// </summary>
        public const string xpathDM = "dm";
        /// <summary>
        /// Reserved XML path name for EML.
        /// </summary>
        public const string xpathEml = "eml";
        /// <summary>
        /// Reserved XML path name for Expression.
        /// </summary>
        public const string xpathExpression = "Expression";
        /// <summary>
        /// Reserved XML path name for FireMethod.
        /// </summary>
        public const string xpathFireMethod = "FireMethod";
        /// <summary>
        /// Reserved XML path name for Fixed.
        /// </summary>
        public const string xpathFixed = "Fixed";
        /// <summary>
        /// Reserved XML path name for ID.
        /// </summary>
        public const string xpathID = "ID";
        /// <summary>
        /// Reserved XML path name for initial condition.
        /// </summary>
        public const string xpathInitialCondition = "InitialCondition";
        /// <summary>
        /// Reserved XML path name for Interval.
        /// </summary>
        public const string xpathInterval = "Interval";
        /// <summary>
        /// Reserved XML path name for IsEpsilonChecked.
        /// </summary>
        public const string xpathIsEpsilonChecked = "IsEpsilonChecked";
        /// <summary>
        /// Reserved XML path name for K (in MassFluxActionProcess).
        /// </summary>
        public const string xpathK = "k";
        /// <summary>
        /// Reserved XML path name for key.
        /// </summary>
        public const string xpathKey = "Key";
        /// <summary>
        /// Reserved XML path name for Variable.
        /// </summary>
        public const string xpathLog = "Log";
        /// <summary>
        /// Reserved XML path name for LoggerPolicy.
        /// </summary>
        public const string xpathLoggerPolicy = "LoggerPolicy";
        /// <summary>
        /// Reserved XML path name for Model.
        /// </summary>
        public const string xpathModel = "Model";
        /// <summary>
        /// Reserved XML path name for MolarConc.
        /// </summary>
        public const string xpathMolarConc = "MolarConc";
        /// <summary>
        /// Reserved XML path name for MolarConc.
        /// </summary>
        public const string xpathMolarActivity = "MolarActivity";
        /// <summary>
        /// Reserved XML path name for Name.
        /// </summary>
        public const string xpathName = "Name";
        /// <summary>
        /// Reserved XML path name for NumberConc.
        /// </summary>
        public const string xpathNumberConc = "NumberConc";
        /// <summary>
        /// Reserved XML path name for Prm.
        /// </summary>
        public const string xpathPrm = "Prm";
        /// <summary>
        /// Reserved XML path name for Parameters.
        /// </summary>
        public const string xpathParameters = "Parameters";
        /// <summary>
        /// Reserved XML path name for Process.
        /// </summary>
        public const string xpathProcess = "Process";
        /// <summary>
        /// Reserved XML path name for Process.
        /// </summary>
        public const string xpathText = "Text";
        /// <summary>
        /// Reserved XML path name for Project.
        /// </summary>
        public const string xpathProject = "Project";
        /// <summary>
        /// Reserved XML path name for Property.
        /// </summary>
        public const string xpathProperty = "property";
        /// <summary>
        /// Reserved XML path name for Result.
        /// </summary>
        public const string xpathResult = "Result";
        /// <summary>
        /// Reserved XML path name for Size.
        /// </summary>
        public const string xpathSize = "Size";
        /// <summary>
        /// Reserved XML path name for Simulation.
        /// </summary>
        public const string xpathSimulation = "Simulation";
        /// <summary>
        /// Reserved XML path name for Space.
        /// </summary>
        public const string xpathSpace = "Space";
        /// <summary>
        /// Reserved XML path name for Step.
        /// </summary>
        public const string xpathStep = "Step";
        /// <summary>
        /// Reserved XML path name for StepInterval.
        /// </summary>
        public const string xpathStepInterval = "StepInterval";
        /// <summary>
        /// Reserved XML path name for Stepper.
        /// </summary>
        public const string xpathStepper = "Stepper";
        /// <summary>
        /// Reserved XML path name for StepperID.
        /// </summary>
        public const string xpathStepperID = "StepperID";
        /// <summary>
        /// Reserved XML path name for System.
        /// </summary>
        public const string xpathSystem = "System";
        /// <summary>
        /// Reserved XML path name for Type.
        /// </summary>
        public const string xpathType = "Type";
        /// <summary>
        /// Reserved XML path name for Value.
        /// </summary>
        public const string xpathValue = "Value";
        /// <summary>
        /// Reserved XML path name for Variable.
        /// </summary>
        public const string xpathVariable = "Variable";
        /// <summary>
        /// Reserved XML path name for VariableReferenceList.
        /// </summary>
        public const string xpathVRL = "VariableReferenceList";

        #endregion

        /// <summary>
        /// default project ID.
        /// </summary>
        public const string defaultPrjID = "project";
        /// <summary>
        /// default comment.
        /// </summary>
        public const string defaultComment = "comment";
        /// <summary>
        /// default comment.
        /// </summary>
        public const string defaultSimParam = "DefaultParameter";
        /// <summary>
        /// DM directory name
        /// </summary>
        public const string DMDirName = "DMs";
        /// <summary>
        /// Parameter directory name
        /// </summary>
        public const string ParameterDirName = "Parameters";
        /// <summary>
        /// tmp directory name
        /// </summary>
        public const string TmpDirName = "tmp";
        /// <summary>
        /// Reserved the header of average.
        /// </summary>
        public const string headerAverage = "avg";
        /// <summary>
        /// Reserved the number of header.
        /// </summary>
        public const string headerColumn = "5";
        /// <summary>
        /// Reserved the header of data.
        /// </summary>
        public const string headerData = "DATA";
        /// <summary>
        /// Reserved the header of label.
        /// </summary>
        public const string headerLabel = "LABEL";
        /// <summary>
        /// Reserved the header of  max.
        /// </summary>
        public const string headerMaximum = "Max";
        /// <summary>
        /// Reserved the header of min.
        /// </summary>
        public const string headerMinimum = "Min";
        /// <summary>
        /// Reserved the header of note.
        /// </summary>
        public const string headerNote = "NOTE";
        /// <summary>
        /// Reserved the header of Size.
        /// </summary>
        public const string headerSize = "SIZE";
        /// <summary>
        /// Reserved the header of Time.
        /// </summary>
        public const string headerTime = "t";
        /// <summary>
        /// Reserved the header of tolerable.
        /// </summary>
        public const string headerTolerable = "Tolerable";
        /// <summary>
        /// Reserved the header of value.
        /// </summary>
        public const string headerValue = "value";
        /// <summary>
        /// Reserved the message of analysis.
        /// </summary>
        public const string messageAnalysis = "Analysis";
        /// <summary>
        /// Reserved the message of debug.
        /// </summary>
        public const string messageDebug = "Debug";
        /// <summary>
        /// Reserved the message of simulation.
        /// </summary>
        public const string messageSimulation = "Simulation";
        /// <summary>
        /// Property name of ReadVariableList.
        /// </summary>
        public const string propReadVariableList = "ReadVariableList";
        /// <summary>
        /// Property name of ProcessList.
        /// </summary>
        public const string propProcessList = "ProcessList";
        /// <summary>
        /// Property name of SystemList.
        /// </summary>
        public const string propSystemList = "SystemList";
        /// <summary>
        /// Property name of WriteProcessList.
        /// </summary>
        public const string propWriteVariableList = "WriteVariableList";

        /// <summary>
        /// Reserved XML path name for comment.
        /// </summary>
        public const string textComment = "Comment";
        /// <summary>
        /// Reserved XML path name for comment.
        /// </summary>
        public const string textDate = "Date";
        /// <summary>
        /// Reserved XML path name for default stepper.
        /// </summary>
        public const string textKey = "DefaultStepper";
        /// <summary>
        /// Reserved XML path name for parameter of simulation.
        /// </summary>
        public const string textParameter = "SimulationParameter";
        /// <summary>
        /// Reserved XML path name for the type of double.
        /// </summary>
        public const string typeDouble = "double";
        /// <summary>
        /// Reserved XML path name for the type of int.
        /// </summary>
        public const string typeInt = "int";
        /// <summary>
        /// Reserved XML path name for the type of list.
        /// </summary>
        public const string typeList = "list";
        /// <summary>
        /// Reserved XML path name for string.
        /// </summary>
        public const string typeString = "string";
    }

    /// <summary>
    /// Class to manage the status of analysis.
    /// </summary>
    public enum AnalysisStatus
    {
        /// <summary>
        /// Analysis status (analysis is not running)
        /// </summary>
        Initialized = 0,
        /// <summary>
        /// Analysis status (this analysis is running).
        /// </summary>
        Running = 1,
        /// <summary>
        /// Analysis status (this analysis is already done).
        /// </summary>
        Completed = 2
    }

    /// <summary>
    /// Class to manage the status of project.
    /// </summary>
    public enum ProjectStatus
    {
        /// <summary>
        /// Project status (no project has been loaded yet,)
        /// </summary>
        Uninitialized = 0,
        /// <summary>
        /// Projct status (project is loaded with the simulation stopped)
        /// </summary>
        Loading = 1,
        /// <summary>
        /// Projct status (project is loaded with the simulation stopped)
        /// </summary>
        Loaded = 2,
        /// <summary>
        /// Project status (simulation is running).
        /// </summary>
        Running = 3,
        /// <summary>
        /// Project status (simulation is suspend).
        /// </summary>
        Suspended = 4,
        /// <summary>
        /// Project status (simulation is running in step mode).
        /// </summary>
        Stepping = 5,
        /// <summary>
        /// Project status (Refresh).
        /// </summary>
        Refresh = 6,
        /// <summary>
        /// Project status(project is in analysis).
        /// </summary>
        Analysis = 7
    }

    /// <summary>
    /// SimulationStatus
    /// </summary>
    public enum SimulationStatus
    {
        /// <summary>
        /// The waiting flag of the simulation
        /// </summary>
        Wait = 0,
        /// <summary>
        /// The running flag of the simulation
        /// </summary>
        Run = 1,
        /// <summary>
        /// The suspending flag of the simulation
        /// </summary>
        Suspended = 2
    };

    /// <summary>
    /// Availability of Redo/Undo
    /// </summary>
    public enum UndoStatus
    {
        /// <summary>
        /// Both undo and redo are available.
        /// </summary>
        UNDO_REDO,
        /// <summary>
        /// Only undo is available.
        /// </summary>
        UNDO_ONLY,
        /// <summary>
        /// Only redo is available.
        /// </summary>
        REDO_ONLY,
        /// <summary>
        /// Both undo and redo are NOT available.
        /// </summary>
        NOTHING
    }

    /// <summary>
    /// FileType
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Index of FolderIcon on TreeNode.
        /// </summary>
        Folder = 0,
        /// <summary>
        /// Index of ProjectIcon on TreeNode.
        /// </summary>
        Project = 1,
        /// <summary>
        /// Index of ModelIcon on TreeNode.
        /// </summary>
        Model = 2
    }

    /// <summary>
    /// Message type.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 
        /// </summary>
        Information = 1,
        /// <summary>
        /// 
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 
        /// </summary>
        Error = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum StatusBarMessageKind
    {
        /// <summary>
        /// 
        /// </summary>
        Generic,
        /// <summary>
        /// 
        /// </summary>
        QuickInspector
    }
    /// <summary>
    /// 
    /// </summary>
    public class MenuConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemFile = "MenuItemFile";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemEdit = "MenuItemEdit";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemLayout = "MenuItemLayout";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemView = "MenuItemView";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemRun = "MenuItemRun";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemSetup = "MenuItemSetup";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemTools = "MenuItemTools";
        /// <summary>
        /// 
        /// </summary>
        public const string MenuItemHelp = "MenuItemHelp";
    }
}
