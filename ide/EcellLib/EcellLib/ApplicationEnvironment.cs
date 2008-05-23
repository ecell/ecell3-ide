using System;
using System.Collections.Generic;
using System.Text;
using EcellLib.Message;
using EcellLib.Job;

namespace EcellLib
{
    public class ApplicationEnvironment
    {
        private DataManager m_dManager;
        private PluginManager m_pManager;
        private MessageManager m_mManager;
        private ActionManager m_aManager;
        private CommandManager m_cManager;
        private IJobManager m_jManager;

        public DataManager DataManager
        {
            get { return m_dManager; }
        }

        public PluginManager PluginManager
        {
            get { return m_pManager; }
        }

        public MessageManager MessageManager
        {
            get { return m_mManager; }
        }

        public ActionManager ActionManager
        {
            get { return m_aManager; }
        }

        public CommandManager CommandManager
        {
            get { return m_cManager; }
        }

        public IJobManager JobManager
        {
            get { return m_jManager; }
            set { m_jManager = value; }
        }

        public ApplicationEnvironment()
        {
            m_dManager = new DataManager(this);
            m_mManager = new MessageManager(this);
            m_pManager = new PluginManager(this);
            m_aManager = new ActionManager(this);
            m_jManager = new JobManager(this);
            m_cManager = new CommandManager(this);
        }
    }
}
