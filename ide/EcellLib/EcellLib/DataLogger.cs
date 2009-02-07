using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using EcellCoreLib;
using Ecell.Exceptions;

namespace Ecell
{

    /// <summary>
    /// Property object to manage the information of saved logger.
    /// </summary>
    public class SaveLoggerProperty
    {
        private string m_fullPath = "";
        private double m_start = 0.0;
        private double m_end = 0.0;
        private string m_dirName = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        public SaveLoggerProperty()
        {
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="path">the full path.</param>
        /// <param name="start">start time.</param>
        /// <param name="end">end time.</param>
        /// <param name="dir">output directory name.</param>
        public SaveLoggerProperty(string path, double start, double end, string dir)
        {
            m_fullPath = path;
            m_start = start;
            m_end = end;
            m_dirName = dir;
        }

        /// <summary>
        /// get/set the full path of logger.
        /// </summary>
        public string FullPath
        {
            get { return this.m_fullPath; }
            set { this.m_fullPath = value; }
        }

        /// <summary>
        /// get/set the start time of logger.
        /// </summary>
        public double Start
        {
            get { return this.m_start; }
            set { this.m_start = value; }
        }

        /// <summary>
        /// get/set the end time of logger.
        /// </summary>
        public double End
        {
            get { return this.m_end; }
            set { this.m_end = value; }
        }

        /// <summary>
        /// get/set the output directory name of logger.
        /// </summary>
        public string DirName
        {
            get { return this.m_dirName; }
            set { this.m_dirName = value; }
        }
    }

    /// <summary>
    /// Stores the simulation results.
    /// </summary>
    public class LogData
    {
        /// <summary>
        /// The model ID
        /// </summary>
        private string m_modelID = null;
        /// <summary>
        /// The key of the "EcellObject"
        /// </summary>
        private string m_key = null;
        /// <summary>
        /// The type of the "EcellObject"
        /// </summary>
        private string m_type = null;
        /// <summary>
        /// The property name of the "EcellObject"
        /// </summary>
        private string m_propName = null;
        /// <summary>
        /// The flag whether this log is loaded.
        /// </summary>
        private bool m_isLoaded = false;
        /// <summary>
        /// The list of the "LogValue" of the property name
        /// </summary>
        private List<LogValue> m_logValueList = null;

        /// <summary>
        /// Creates the new "LogData" instance without any parameter.
        /// </summary>
        private LogData()
        {
        }

        /// <summary>
        /// Creates the new "LogValue" instance with some parameters.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key of the "EcellObject"</param>
        /// <param name="type">The type of the "EcellObject"</param>
        /// <param name="propName">The property name of the "EcellObject"</param>
        /// <param name="logValueList">The list of the "LogValue" of the property name</param>
        public LogData(
            string modelID,
            string key,
            string type,
            string propName,
            List<LogValue> logValueList
            )
        {
            this.m_modelID = modelID;
            this.m_key = key;
            this.m_type = type;
            this.m_propName = propName;
            this.m_logValueList = logValueList;
        }

        /// <summary>
        /// get m_model
        /// </summary>
        public string model
        {
            get { return this.m_modelID; }
        }

        /// <summary>
        /// get m_key
        /// </summary>
        public string key
        {
            get { return this.m_key; }
        }

        /// <summary>
        /// get m_type
        /// </summary>
        public string type
        {
            get { return this.m_type; }
        }

        /// <summary>
        /// get m_propName
        /// </summary>
        public string propName
        {
            get { return this.m_propName; }
        }

        /// <summary>
        /// get m_logValueList
        /// </summary>
        public List<LogValue> logValueList
        {
            get { return this.m_logValueList; }
        }

        /// <summary>
        /// get / set the flag.
        /// </summary>
        public bool IsLoaded
        {
            get { return this.m_isLoaded; }
            set { this.m_isLoaded = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DiskFullAction
    {
        /// <summary>
        /// 
        /// </summary>
        Terminate = 0,
        /// <summary>
        /// 
        /// </summary>
        Overwrite = 1
    }

    /// <summary>
    /// Stores the logger policy.
    /// </summary>
    public class LoggerPolicy: ICloneable
    {
        /// <summary>
        /// The action when the HDD is full
        /// </summary>
        private DiskFullAction m_diskFullAction = DiskFullAction.Terminate;
        /// <summary>
        /// The maximum HDD space
        /// </summary>
        private int m_maxDiskSpace = 0;
        /// <summary>
        /// The reload interval
        /// </summary>
        private double m_reloadInterval = 0;
        /// <summary>
        /// The reload step count
        /// </summary>
        private int m_reloadStepCount = 1;
        /// <summary>
        /// 
        /// </summary>
        public DiskFullAction DiskFullAction
        {
            get { return m_diskFullAction; }
            set { m_diskFullAction = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaxDiskSpace
        {
            get { return m_maxDiskSpace; }
            set { m_maxDiskSpace = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double ReloadInterval
        {
            get { return m_reloadInterval; }
            set { m_reloadInterval = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ReloadStepCount
        {
            get { return m_reloadStepCount; }
            set { m_reloadStepCount = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new LoggerPolicy(this);
        }

        /// <summary>
        /// Creates the new "LoggerPolicy" instance with some parameters.
        /// </summary>
        /// <param name="reloadStepCount">The reload step count</param>
        /// <param name="reloadInterval">The reload interval</param>
        /// <param name="diskFullAction">The action when the HDD is full</param>
        /// <param name="maxDiskSpace">The maximum HDD space</param>
        public LoggerPolicy(
            int reloadStepCount,
            double reloadInterval,
            DiskFullAction diskFullAction,
            int maxDiskSpace
            )
        {
            Debug.Assert(reloadStepCount >= 0);
            Debug.Assert(reloadInterval >= 0.0);
            Debug.Assert(reloadStepCount != 0 || reloadInterval != 0.0);
            Debug.Assert(maxDiskSpace >= 0);

            this.m_reloadStepCount = reloadStepCount;
            this.m_reloadInterval = reloadInterval;
            this.m_diskFullAction = diskFullAction;
            this.m_maxDiskSpace = maxDiskSpace;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rhs"></param>
        public LoggerPolicy(LoggerPolicy rhs)
        {
            this.m_reloadStepCount = rhs.m_reloadStepCount;
            this.m_reloadInterval = rhs.m_reloadInterval;
            this.m_diskFullAction = rhs.m_diskFullAction;
            this.m_maxDiskSpace = rhs.m_maxDiskSpace;
        }
        /// <summary>
        /// 
        /// </summary>
        public LoggerPolicy() { }
    }

    /// <summary>
    /// Stores the log of the simulation.
    /// </summary>
    public class LogValue
    {
        /// <summary>
        /// The average value
        /// </summary>
        private double m_avg = double.NaN;
        /// <summary>
        /// The maximun value
        /// </summary>
        private double m_max = double.NaN;
        /// <summary>
        /// The minimum value
        /// </summary>
        private double m_min = double.NaN;
        /// <summary>
        /// The simulation time
        /// </summary>
        private double m_time = double.NaN;
        /// <summary>
        /// The value of the data
        /// </summary>
        private double Value = double.NaN;

        /// <summary>
        /// Creates the new "LogValue" instance without any parameter.
        /// </summary>
        private LogValue()
        {
        }

        /// <summary>
        /// Creates the new "LogValue" instance with some parameters.
        /// </summary>
        /// <param name="time">The simulation time</param>
        /// <param name="value">The value of the data</param>
        /// <param name="avg">The average value</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximun value</param>
        public LogValue(
            double time,
            double value,
            double avg,
            double min,
            double max
            )
        {
            this.m_time = time;
            this.Value = value;
            this.m_avg = avg;
            this.m_min = min;
            this.m_max = max;
        }

        /// <summary>
        /// get/set m_time
        /// </summary>
        public double time
        {
            get { return this.m_time; }
            // set { this.m_time = value; }
        }

        /// <summary>
        /// get/set Value
        /// </summary>
        public double value
        {
            get { return this.Value; }
            //set { this.Value = value; }
        }

        /// <summary>
        /// get/set m_avg
        /// </summary>
        public double avg
        {
            get { return this.m_avg; }
            // set { this.m_avg = value; }
        }

        /// <summary>
        /// get/set m_min
        /// </summary>
        public double min
        {
            get { return this.m_min; }
            // set { this.m_min = value; }
        }

        /// <summary>
        /// get/set m_max
        /// </summary>
        public double max
        {
            get { return this.m_max; }
            // set { this.m_max = value; }
        }
    }

        /// <summary>
    /// Treats the "ecd" formatted file.
    /// </summary>
    internal class Ecd
    {
        private double m_oldTime;
        private StreamWriter m_writer = null;
        /// <summary>
        /// Creates a new "Ecd" instance with no argument.
        /// </summary>
        public Ecd()
        {
            m_oldTime = -1.0;            
        }

        public void Close()
        {
            if (m_writer != null)
                m_writer.Close();
        }

        public void Append(string savedDirName, LogData logData, SaveType saveType,
            double startTime, double endTime)
        {
            string fileName = null;
            string fileExtension = "";
            string splitter = "";

            if (saveType == SaveType.CSV)
            {
                fileExtension = ".csv";
                splitter = Constants.delimiterComma;
            }
            else
            {
                fileExtension = ".ecd";
                splitter = Constants.delimiterTab;
            }

            try
            {
                //
                // Initializes.
                //
                if (savedDirName == null || savedDirName.Length <= 0)
                {
                    return;
                }
                else if (logData == null)
                {
                    return;
                }

                //
                // Sets the file name.
                //
                fileName =
                    logData.type + Constants.delimiterUnderbar +
                    logData.key + Constants.delimiterUnderbar +
                    logData.propName;
                fileName = fileName.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                fileName = fileName.Replace(Constants.delimiterColon, Constants.delimiterUnderbar);
                fileName = savedDirName + Constants.delimiterPath +
                    fileName + fileExtension;            

                    //
                    // Writes the "LogData".
                    //
                    foreach (LogValue logValue in logData.logValueList)
                    {
                        if (m_oldTime == logValue.time)
                        {
                            continue;
                        }
                        double ltime;
                        double.TryParse(logValue.time.ToString(), out ltime);
                        if (startTime > ltime ||
                            endTime < ltime)
                            continue;
                        if (Double.IsNaN(logValue.avg) &&
                            Double.IsNaN(logValue.min) &&
                            Double.IsNaN(logValue.max)
                            )
                        {
                            m_writer.WriteLine(
                                logValue.time + splitter +
                                logValue.value
                                );
                        }
                        else
                        {
                            m_writer.WriteLine(
                                logValue.time + splitter +
                                logValue.value + splitter +
                                logValue.avg + splitter +
                                logValue.min + splitter +
                                logValue.max
                                );
                        }

                }
            }
            catch (Exception ex)
            {
                if (m_writer != null)
                {
                    m_writer.Close();
                    m_writer = null;
                }
                throw new EcellException(String.Format(MessageResources.ErrCreFile,
                    new object[] { fileName }), ex);
            }
        }

        /// <summary>
        /// Creates the "ecd" formatted file.
        /// </summary>
        /// <param name="savedDirName">The saved directory name.</param>
        /// <param name="logData">The list of the "LogData"</param>
        /// <param name="saveType">The type of saved file.</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void Create(string savedDirName, LogData logData, SaveType saveType,
            double startTime, double endTime)
        {
            string fileName = null;
            string fileExtension = "";
            string splitter = "";

            if (saveType == SaveType.CSV)
            {
                fileExtension = ".csv";
                splitter = Constants.delimiterComma;
            }
            else
            {
                fileExtension = ".ecd";
                splitter = Constants.delimiterTab;
            }

            try
            {
                //
                // Initializes.
                //
                if (savedDirName == null || savedDirName.Length <= 0)
                {
                    return;
                }
                else if (logData == null)
                {
                    return;
                }

                //
                // Sets the file name.
                //
                fileName =
                    logData.type + Constants.delimiterUnderbar +
                    logData.key + Constants.delimiterUnderbar +
                    logData.propName;
                fileName = fileName.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                fileName = fileName.Replace(Constants.delimiterColon, Constants.delimiterUnderbar);
                fileName = savedDirName + Constants.delimiterPath +
                    fileName + fileExtension;
                //
                // Checks the old model file.
                //
                if (File.Exists(fileName))
                {
                    string date
                        = File.GetLastAccessTime(fileName).ToString().Replace(
                            Constants.delimiterColon, Constants.delimiterUnderbar);
                    date = date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                    date = date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
                    string destFileName
                        = Path.GetDirectoryName(fileName) + Constants.delimiterPath
                        + Constants.delimiterUnderbar + date + Constants.delimiterUnderbar + Path.GetFileName(fileName);
                    File.Move(fileName, destFileName);
                }
                //
                // Saves the "LogData".
                //

                    m_writer = new StreamWriter(
                            new FileStream(fileName, FileMode.CreateNew), System.Text.Encoding.UTF8);
                    //
                    // Writes the header.
                    //
                    m_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerData + Constants.delimiterColon + Constants.delimiterSpace +
                        logData.type + Constants.delimiterColon +
                        logData.key + Constants.delimiterColon +
                        logData.propName
                        );
                    int headerColumn = 0;
                    if (Double.IsNaN(logData.logValueList[0].avg) &&
                        Double.IsNaN(logData.logValueList[0].min) &&
                        Double.IsNaN(logData.logValueList[0].max)
                        )
                    {
                        headerColumn = 2;
                    }
                    else
                    {
                        headerColumn = 5;
                    }
                    m_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerSize + Constants.delimiterColon + Constants.delimiterSpace +
                        headerColumn + Constants.delimiterSpace +
                        logData.logValueList.Count
                        );
                    m_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerLabel + Constants.delimiterColon + Constants.delimiterSpace +
                        Constants.headerTime + splitter +
                        Constants.headerValue + splitter +
                        Constants.headerAverage + splitter +
                        Constants.headerMinimum.ToLower() + splitter +
                        Constants.headerMaximum.ToLower()
                        );
                    m_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerNote + Constants.delimiterColon + Constants.delimiterSpace
                        );
                    m_writer.WriteLine(
                        Constants.delimiterSharp
                        );
                    string separator = "";
                    for (int i = 0; i < 22; i++)
                    {
                        separator += Constants.delimiterHyphen;
                    }
                    m_writer.WriteLine(
                        Constants.delimiterSharp + separator
                        );
                    m_writer.Flush();
                    //
                    // Writes the "LogData".
                    //
                    foreach (LogValue logValue in logData.logValueList)
                    {
                        if (m_oldTime == logValue.time)
                        {
                            continue;
                        }
                        double ltime;
                        double.TryParse(logValue.time.ToString(), out ltime);
                        if (startTime > ltime ||
                            endTime < ltime)
                            continue;
                        if (Double.IsNaN(logValue.avg) &&
                            Double.IsNaN(logValue.min) &&
                            Double.IsNaN(logValue.max)
                            )
                        {
                            m_writer.WriteLine(
                                logValue.time + splitter +
                                logValue.value
                                );
                        }
                        else
                        {
                            m_writer.WriteLine(
                                logValue.time + splitter +
                                logValue.value + splitter +
                                logValue.avg + splitter +
                                logValue.min + splitter +
                                logValue.max
                                );
                        }
                        m_oldTime = logValue.time;
                }
            }
            catch (Exception ex)
            {
                if (m_writer != null)
                {
                    m_writer.Close();
                    m_writer = null;

                }
                throw new EcellException(String.Format(MessageResources.ErrCreFile,
                    new object[] { fileName }), ex);
            }
        }

        static public LogData LoadSavedLogData(string fileName)
        {
            string type  = "";
            string key = "";
            string prop = "";
            string line = "";
            char splitter = '\t';
            List<LogValue> valueList = new List<LogValue>();
            StreamReader strread = new StreamReader(fileName);
            string ext = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(ext) && ext.ToLower().Equals(".csv"))
            {
                splitter = ',';
            }

            while ((line = strread.ReadLine()) != null)
            {
                if (line.StartsWith(Constants.delimiterSharp))
                {
                    if (line.StartsWith(Constants.delimiterSharp + Constants.headerData))
                    {
                        string[] ele = line.Split(new char[] { ':' });
                        type = ele[1].Replace(" ", "");
                        key = ele[2].Replace(" ", "") + Constants.delimiterColon + ele[3].Replace(" ", "");
                        prop = line.Replace(ele[0], "").Replace(" ", "");
                    }
                    continue;
                }

                string[] points = line.Split(new char[] { splitter });
                if (points.Length < 5) continue;
                double time = Convert.ToDouble(points[0]);
                double value = Convert.ToDouble(points[1]);
                double ave = Convert.ToDouble(points[2]);
                double min = Convert.ToDouble(points[3]);
                double max = Convert.ToDouble(points[4]);
                valueList.Add(new LogValue(time, value, ave, min, max));
            }

            strread.Close();
            LogData d = new LogData("", key, type, prop, valueList);
            d.IsLoaded = true;

            return d;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum SaveType
    {
        /// <summary>
        /// 
        /// </summary>
        ECD = 0,
        /// <summary>
        /// 
        /// </summary>
        CSV = 1
    }
}
