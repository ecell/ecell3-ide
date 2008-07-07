using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Message;
using Ecell.Job;

namespace Ecell
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationEnvironment
    {
        private static readonly ApplicationEnvironment instance = new ApplicationEnvironment();

        private DataManager m_dManager;
        private PluginManager m_pManager;
        private MessageManager m_mManager;
        private ActionManager m_aManager;
        private CommandManager m_cManager;
        private IJobManager m_jManager;
        /// <summary>
        /// 
        /// </summary>
        public DataManager DataManager
        {
            get { return m_dManager; }
        }
        /// <summary>
        /// 
        /// </summary>
        public PluginManager PluginManager
        {
            get { return m_pManager; }
        }
        /// <summary>
        /// 
        /// </summary>
        public MessageManager MessageManager
        {
            get { return m_mManager; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ActionManager ActionManager
        {
            get { return m_aManager; }
        }
        /// <summary>
        /// 
        /// </summary>
        public CommandManager CommandManager
        {
            get { return m_cManager; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IJobManager JobManager
        {
            get { return m_jManager; }
            set { m_jManager = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private ApplicationEnvironment()
        {
            m_dManager = new DataManager(this);
            m_mManager = new MessageManager(this);
            m_pManager = new PluginManager(this);
            m_aManager = new ActionManager(this);
            m_jManager = new JobManager(this);
            m_cManager = new CommandManager(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApplicationEnvironment GetInstance()
        {
            return instance;
        }
    }
}
