using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib
{
    /// <summary>
    /// Class to manage the constant value.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// default project ID.
        /// </summary>
        public const string defaultPrjID = "project";
        /// <summary>
        /// default comment.
        /// </summary>
        public const string defaultComment = "comment";
        /// <summary>
        /// DM directory name
        /// </summary>
        public const string DMDirName = "DMs";
        /// <summary>
        /// Reserved extension for DM's.
        /// </summary>
        public const string dmFileExtension = ".dll";
        /// <summary>
        /// Reserved extension for plugins.
        /// </summary>
        public const string pluginFileExtension = ".dll";
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
        /// <summary>
        /// File extention of the action file.
        /// </summary>
        public const string extActionFile = "Action File(*.xml)|*.xml";
        /// <summary>
        /// File extention of the model file.
        /// </summary>
        public const string extEmlFile = "Model File(*.eml)|*.eml|all(*.*)|*.*";
        /// <summary>
        /// File extention of the script file.
        /// </summary>
        public const string extEssFile = "Script File(*.ess)|*.ess|all(*.*)|*.*";
        /// <summary>
        /// File extention of the window setting file.
        /// </summary>
        public const string extWinSetFile = "Window Setting File(*.xml)|*.xml|all(*.*)|*.*";
        /// <summary>
        /// Reserved the name of file..
        /// </summary>
        public const string fileProject = "project.info";
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
        /// Reserved XML path name for DefalutParameter.
        /// </summary>
        public const string parameterKey = "DefaultParameter";
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
        /// Reserved XML path name for comment.
        /// </summary>
        public const string textComment = "Comment";
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
        /// Reserved XML path name for key.
        /// </summary>
        public const string xpathKey = "Key";
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
        /// Reserved XML path name for Process.
        /// </summary>
        public const string xpathProcess = "Process";
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
        public const string xpathSimulation = "Parameters";
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
        public const string xpathVRL = EcellProcess.VARIABLEREFERENCELIST;
        /// <summary>
        /// Reserved XML path name for xml.
        /// </summary>
        public const string xpathXml = "xml";
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
        Loaded = 1,
        /// <summary>
        /// Project status (simulation is running).
        /// </summary>
        Running = 2,
        /// <summary>
        /// Project status (simulation is suspend).
        /// </summary>
        Suspended = 3,
        /// <summary>
        /// Project status (simulation is running in step mode).
        /// </summary>
        Stepping = 4
    }
}
