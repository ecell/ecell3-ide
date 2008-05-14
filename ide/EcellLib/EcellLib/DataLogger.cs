using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using EcellCoreLib;

namespace EcellLib
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
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key of the "EcellObject"</param>
        /// <param name="l_type">The type of the "EcellObject"</param>
        /// <param name="l_propName">The property name of the "EcellObject"</param>
        /// <param name="l_logValueList">The list of the "LogValue" of the property name</param>
        public LogData(
            string l_modelID,
            string l_key,
            string l_type,
            string l_propName,
            List<LogValue> l_logValueList
            )
        {
            this.m_modelID = l_modelID;
            this.m_key = l_key;
            this.m_type = l_type;
            this.m_propName = l_propName;
            this.m_logValueList = l_logValueList;
        }

        /// <summary>
        /// get/set m_model
        /// </summary>
        public string model
        {
            get { return this.m_modelID; }
            // set { this.m_modelID = value; }
        }

        /// <summary>
        /// get/set m_key
        /// </summary>
        public string key
        {
            get { return this.m_key; }
            // set { this.m_key = value; }
        }

        /// <summary>
        /// get/set m_type
        /// </summary>
        public string type
        {
            get { return this.m_type; }
            // set { this.m_type = value; }
        }

        /// <summary>
        /// get/set m_propName
        /// </summary>
        public string propName
        {
            get { return this.m_propName; }
            // set { this.m_propName = value; }
        }

        /// <summary>
        /// get/set m_logValueList
        /// </summary>
        public List<LogValue> logValueList
        {
            get { return this.m_logValueList; }
            // set { this.m_logValueList = value; }
        }
    }

    /// <summary>
    /// Stores the logger policy.
    /// </summary>
    public struct LoggerPolicy
    {
        /// <summary>
        /// The action when the HDD is full
        /// </summary>
        public int m_diskFullAction;
        /// <summary>
        /// The maximum HDD space
        /// </summary>
        public int m_maxDiskSpace;
        /// <summary>
        /// The reload interval
        /// </summary>
        public double m_reloadInterval;
        /// <summary>
        /// The reload step count
        /// </summary>
        public int m_reloadStepCount;
        /// <summary>
        /// The default action when the HDD is full
        /// </summary>
        public const int s_diskFullAction = 0;
        /// <summary>
        /// The default maximum HDD space
        /// </summary>
        public const int s_maxDiskSpace = 0;
        /// <summary>
        /// The default reload interval
        /// </summary>
        public const double s_reloadInterval = 0.0;
        /// <summary>
        /// The default reload step count
        /// </summary>
        public const int s_reloadStepCount = 1;

        /// <summary>
        /// Creates the new "LoggerPolicy" instance with some parameters.
        /// </summary>
        /// <param name="l_reloadStepCount">The reload step count</param>
        /// <param name="l_reloadInterval">The reload interval</param>
        /// <param name="l_diskFullAction">The action when the HDD is full</param>
        /// <param name="l_maxDiskSpace">The maximum HDD space</param>
        public LoggerPolicy(
            int l_reloadStepCount,
            double l_reloadInterval,
            int l_diskFullAction,
            int l_maxDiskSpace
            )
        {
            if (l_reloadStepCount < 0)
            {
                l_reloadStepCount = s_reloadStepCount;
            }
            this.m_reloadStepCount = l_reloadStepCount;
            if (l_reloadInterval < 0.0)
            {
                l_reloadInterval = s_reloadInterval;
            }
            this.m_reloadInterval = l_reloadInterval;
            //
            // Puts the reload step count ahead of the reload interval.
            //
            if (l_reloadStepCount == 0 && l_reloadInterval == 0.0)
            {
                l_reloadStepCount = s_reloadStepCount;
            }
            switch (l_diskFullAction)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    l_diskFullAction = s_diskFullAction;
                    break;
            }
            this.m_diskFullAction = l_diskFullAction;
            if (l_maxDiskSpace < 0)
            {
                l_maxDiskSpace = s_maxDiskSpace;
            }
            this.m_maxDiskSpace = l_maxDiskSpace;
        }
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
        /// <param name="l_time">The simulation time</param>
        /// <param name="l_value">The value of the data</param>
        /// <param name="l_avg">The average value</param>
        /// <param name="l_min">The minimum value</param>
        /// <param name="l_max">The maximun value</param>
        public LogValue(
            double l_time,
            double l_value,
            double l_avg,
            double l_min,
            double l_max
            )
        {
            this.m_time = l_time;
            this.Value = l_value;
            this.m_avg = l_avg;
            this.m_min = l_min;
            this.m_max = l_max;
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
        /// <summary>
        /// ResourceManager for PropertyEditor.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResLib));

        /// <summary>
        /// Creates a new "Ecd" instance with no argument.
        /// </summary>
        public Ecd()
        {
        }

        /// <summary>
        /// Creates the "ecd" formatted file.
        /// </summary>
        /// <param name="l_savedDirName">The saved directory name.</param>
        /// <param name="l_logData">The list of the "LogData"</param>
        /// <param name="l_saveType">The type of saved file.</param>
        public void Create(string l_savedDirName, LogData l_logData, String l_saveType)
        {
            try
            {
                //
                // Initializes.
                //
                if (l_savedDirName == null || l_savedDirName.Length <= 0)
                {
                    return;
                }
                else if (l_logData == null)
                {
                    return;
                }

                //
                // Sets the file name.
                //
                string l_fileName =
                    l_logData.type + Constants.delimiterUnderbar +
                    l_logData.key + Constants.delimiterUnderbar +
                    l_logData.propName;
                l_fileName = l_fileName.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                l_fileName = l_fileName.Replace(Constants.delimiterColon, Constants.delimiterUnderbar);
                l_fileName = l_savedDirName + Constants.delimiterPath +
                    l_fileName + Constants.delimiterPeriod + l_saveType;
                //
                // Checks the old model file.
                //
                if (File.Exists(l_fileName))
                {
                    string l_date
                        = File.GetLastAccessTime(l_fileName).ToString().Replace(
                            Constants.delimiterColon, Constants.delimiterUnderbar);
                    l_date = l_date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                    l_date = l_date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
                    string l_destFileName
                        = Path.GetDirectoryName(l_fileName) + Constants.delimiterPath
                        + Constants.delimiterUnderbar + l_date + Constants.delimiterUnderbar + Path.GetFileName(l_fileName);
                    File.Move(l_fileName, l_destFileName);
                }
                //
                // Saves the "LogData".
                //
                StreamWriter l_writer = null;
                try
                {
                    l_writer = new StreamWriter(
                            new FileStream(l_fileName, FileMode.CreateNew), System.Text.Encoding.UTF8);
                    //
                    // Writes the header.
                    //
                    l_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerData + Constants.delimiterColon + Constants.delimiterSpace +
                        l_logData.type + Constants.delimiterColon +
                        l_logData.key + Constants.delimiterColon +
                        l_logData.propName
                        );
                    int l_headerColumn = 0;
                    if (Double.IsNaN(l_logData.logValueList[0].avg) &&
                        Double.IsNaN(l_logData.logValueList[0].min) &&
                        Double.IsNaN(l_logData.logValueList[0].max)
                        )
                    {
                        l_headerColumn = 2;
                    }
                    else
                    {
                        l_headerColumn = 5;
                    }
                    l_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerSize + Constants.delimiterColon + Constants.delimiterSpace +
                        l_headerColumn + Constants.delimiterSpace +
                        l_logData.logValueList.Count
                        );
                    l_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerLabel + Constants.delimiterColon + Constants.delimiterSpace +
                        Constants.headerTime + Constants.delimiterTab +
                        Constants.headerValue + Constants.delimiterTab +
                        Constants.headerAverage + Constants.delimiterTab +
                        Constants.headerMinimum.ToLower() + Constants.delimiterTab +
                        Constants.headerMaximum.ToLower()
                        );
                    l_writer.WriteLine(
                        Constants.delimiterSharp + Constants.headerNote + Constants.delimiterColon + Constants.delimiterSpace
                        );
                    l_writer.WriteLine(
                        Constants.delimiterSharp
                        );
                    string l_separator = "";
                    for (int i = 0; i < 22; i++)
                    {
                        l_separator += Constants.delimiterHyphen;
                    }
                    l_writer.WriteLine(
                        Constants.delimiterSharp + l_separator
                        );
                    l_writer.Flush();
                    //
                    // Writes the "LogData".
                    //
                    double l_oldTime = -1.0;
                    foreach (LogValue l_logValue in l_logData.logValueList)
                    {
                        if (l_oldTime == l_logValue.time)
                        {
                            continue;
                        }
                        if (Double.IsNaN(l_logValue.avg) &&
                            Double.IsNaN(l_logValue.min) &&
                            Double.IsNaN(l_logValue.max)
                            )
                        {
                            l_writer.WriteLine(
                                l_logValue.time + Constants.delimiterTab +
                                l_logValue.value
                                );
                        }
                        else
                        {
                            l_writer.WriteLine(
                                l_logValue.time + Constants.delimiterTab +
                                l_logValue.value + Constants.delimiterTab +
                                l_logValue.avg + Constants.delimiterTab +
                                l_logValue.min + Constants.delimiterTab +
                                l_logValue.max
                                );
                        }
                        l_oldTime = l_logValue.time;
                    }
                }
                finally
                {
                    if (l_writer != null)
                    {
                        l_writer.Close();
                    }
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception(
                    m_resources.GetString(MessageConstants.ErrCreEcd) + "[" + l_logData.model + "] {" + l_ex.ToString() + "}");
            }
        }
    }
}
