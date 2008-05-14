using System;
using System.Collections.Generic;
using System.Text;
using EcellLib.Message;
using EcellLib.Session;

namespace EcellLib
{
    public class ApplicationEnvironment
    {
        private DataManager m_dManager;
        private PluginManager m_pManager;
        private MessageManager m_mManager;
        private ActionManager m_aManager;
        private ISessionManager m_sManager;

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

        public ISessionManager SessionManager
        {
            get { return m_sManager; }
            set { m_sManager = value; }
        }

        public ApplicationEnvironment()
        {
            m_dManager = new DataManager(this);
            m_mManager = new MessageManager(this);
            m_pManager = new PluginManager(this);
            m_aManager = new ActionManager(this);
            m_sManager = new SessionManager(this);
        }
    }
}
