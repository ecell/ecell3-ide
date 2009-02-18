using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell
{
    /// <summary>
    /// Abstract class for action.
    /// </summary>
    public abstract class UserAction
    {
        #region Fields
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        protected bool m_isAnchor = true;
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        protected bool m_isUndoable = true;

        /// <summary>
        /// ApplicationEnvironment
        /// </summary>
        protected ApplicationEnvironment m_env;
        #endregion

        #region Accessors
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        public bool IsAnchor
        {
            get { return m_isAnchor; }
        }
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        public bool IsUndoable
        {
            get { return m_isUndoable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            internal set { m_env = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Abstract function of UserAction.
        /// Execute action of this UserAction.
        /// </summary>
        public abstract void Execute();
        /// <summary>
        /// Abstract function of UserAction.
        /// Unexecute action of this UserAction.
        /// </summary>
        public abstract void UnExecute();

        #endregion
    }
    /// <summary>
    /// Action class to set Undo Anchor.
    /// </summary>
    public class AnchorAction : UserAction
    {
        /// <summary>
        /// 
        /// </summary>
        public AnchorAction()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AnchorAction: True";
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Execute()
        {
            m_env.PluginManager.RaiseRefreshEvent();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void UnExecute()
        {
            m_env.PluginManager.RaiseRefreshEvent();
        }
    }
    /// <summary>
    /// Action class to create the project.
    /// </summary>
    public class NewProjectAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The name of created project.
        /// </summary>
        private string m_prjName;
        /// <summary>
        /// The comment of created project.
        /// </summary>
        private string m_comment;
        private string m_prjPath;
        #endregion

        /// <summary>
        /// The constructor for NewProjectAction with initial parameters.
        /// </summary>
        /// <param name="prjName">the projectID.</param>
        /// <param name="comment">the project comment.</param>
        /// <param name="path">the path of this project.</param>
        public NewProjectAction(string prjName, string comment, string path)
        {
            m_prjName = prjName;
            m_comment = comment;
            m_prjPath = path;
            m_isUndoable = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NewProjectAction:" + m_prjName;
        }

        /// <summary>
        /// Execute to create the project using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.CreateNewProject(m_prjName, m_comment, m_prjPath, new List<string>());
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Do nothing.
        /// </summary>
        public override void UnExecute()
        {
        }
    }

    /// <summary>
    /// Action class to create the object.
    /// </summary>
    public class DataAddAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The added EcellObject.
        /// </summary>
        private EcellObject m_obj;
        #endregion

        /// <summary>
        /// The constructor for DataAddAction with initial parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isUndoable">The flag the action is undoable.</param>
        public DataAddAction(EcellObject obj, bool isUndoable)
        {
            m_obj = obj.Clone();
            m_obj.isFixed = true;
            m_isUndoable = isUndoable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DataAddAction:" + m_obj.ToString();
        }

        /// <summary>
        /// Execute to add the object using the information.
        /// </summary>
        public override void Execute()
        {
            if (m_obj == null) 
                return;
            m_env.DataManager.DataAdd(m_obj.Clone(), false, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// object will be deleted.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DataDelete(m_obj.Clone(), false, false);
        }
    }

    /// <summary>
    /// Action class to delete the object.
    /// </summary>
    public class DataDeleteAction : UserAction
    {
        #region Fields
        /// <summary>
        /// Deleted object.
        /// </summary>
        private EcellObject m_obj;
        #endregion

        /// <summary>
        /// The constructor for DataDeleteAction with initial parameters.
        /// </summary>
        /// <param name="obj">deleted object.</param>
        public DataDeleteAction(EcellObject obj)
        {
            m_obj = obj.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DataDeleteAction:" + m_obj.ToString();
        }
        /// <summary>
        /// Execute to delete the object using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.DataDelete(m_obj, false, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// An object will be deleted.
        /// </summary>
        public override void UnExecute()
        {
            if (m_obj == null)
                return;
            m_env.DataManager.DataAdd(m_obj.Clone(), false, false);
        }
    }

    /// <summary>
    /// Action class to change the properties of object.
    /// </summary>
    public class DataChangeAction : UserAction
    {
        #region Fields
        /// <summary>
        /// An object before changing.
        /// </summary>
        private EcellObject m_oldObj;
        /// <summary>
        /// An object after changing.
        /// </summary>
        private EcellObject m_newObj;
        #endregion

        /// <summary>
        /// The constructor for DataChangeAction with initial parameters.
        /// </summary>
        /// <param name="oldObj">An object before changing.</param>
        /// <param name="newObj">An object after changing.</param>
        public DataChangeAction(EcellObject oldObj, EcellObject newObj)
        {
            m_oldObj = oldObj.Clone();
            m_newObj = newObj.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DataChangeAction:" + m_isAnchor.ToString() + ", " + m_oldObj.ToString() + ", " + m_newObj.ToString();
        }

        /// <summary>
        /// Execute to change the object using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.DataChanged(m_oldObj.ModelID, m_oldObj.Key, m_oldObj.Type, m_newObj.Clone(), false, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// Changing will be aborted.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DataChanged(m_newObj.ModelID, m_newObj.Key, m_newObj.Type, m_oldObj.Clone(), false, false);
        }
    }

    /// <summary>
    /// Action class to load the project.
    /// </summary>
    public class LoadProjectAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The load project ID.
        /// </summary>
        private string m_prjID;
        /// <summary>
        /// The load project file.
        /// </summary>
        private string m_prjFile;
        #endregion

        /// <summary>
        /// The constructor for LoadProjectAction with initial parameters.
        /// </summary>
        /// <param name="prjID">The load project ID.</param>
        /// <param name="prjFile">The load project file.</param>
        public LoadProjectAction(string prjID, string prjFile)
        {
            m_prjID = prjID;
            m_prjFile = prjFile;
            m_isUndoable = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "LoadProjectAction:" + m_prjFile;
        }
        /// <summary>
        /// Execute to load the project using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.LoadProject(m_prjFile);
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            throw new EcellException("The method or operation is not implemented.");
        }
    }

    /// <summary>
    /// Action class to create stepper.
    /// </summary>
    public class AddStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter ID added the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The stepper object.
        /// </summary>
        private EcellObject m_stepper;
        #endregion

        /// <summary>
        /// The constructor for AddStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter ID added the stepper.</param>
        /// <param name="stepper">The stepper object.</param>
        public AddStepperAction(string paramID, EcellObject stepper)
        {
            m_paramID = paramID;
            m_stepper = stepper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AddStepperAction:" + m_paramID;
        }
        /// <summary>
        /// Execute to add the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.AddStepperID(m_paramID, m_stepper, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DeleteStepperID(m_paramID, m_stepper, false);
        }
    }

    /// <summary>
    /// Action class to delete stepper.
    /// </summary>
    public class DeleteStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter ID deleted the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The deleted stepper.
        /// </summary>
        private EcellObject m_stepper;
        #endregion

        /// <summary>
        /// The constructor for DeleteStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter ID deleted the stepper.</param>
        /// <param name="stepper">The deleted stepper.</param>
        public DeleteStepperAction(string paramID, EcellObject stepper)
        {
            m_paramID = paramID;
            m_stepper = stepper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DeleteStepperAction:" + m_paramID;
        }
        /// <summary>
        /// Execute to delete the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.DeleteStepperID(m_paramID, m_stepper, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.AddStepperID(m_paramID, m_stepper, false);
        }
    }

    /// <summary>
    /// Action class to update the parameter of simulation.
    /// </summary>
    public class UpdateStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter id updated the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The updated stepper.
        /// </summary>
        private List<EcellObject> m_newStepperList;
        /// <summary>
        /// The original stepper.
        /// </summary>
        private List<EcellObject> m_oldStepperList;
        #endregion

        /// <summary>
        /// The constructor for UpdateStepperAction.
        /// </summary>
        public UpdateStepperAction()
        {
            m_newStepperList = new List<EcellObject>();
            m_oldStepperList = new List<EcellObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "UpdateStepperAction:" + m_paramID;
        }

        /// <summary>
        /// The constructor for UpdateStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter id updated the stepper.</param>
        /// <param name="newStepper">The updated stepper.</param>
        /// <param name="oldStepper">The old stepper.</param>
        public UpdateStepperAction(string paramID, List<EcellObject> newStepper, List<EcellObject> oldStepper)
        {
            m_paramID = paramID;
            m_newStepperList = newStepper;
            m_oldStepperList = oldStepper;
        }
        /// <summary>
        /// Execute to update the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.UpdateStepperID(m_paramID, m_newStepperList, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.UpdateStepperID(m_paramID, m_oldStepperList, false);
        }
    }

    /// <summary>
    /// Action class to create the parameter of simulation.
    /// </summary>
    public class NewSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The created parameter id.
        /// </summary>
        private string m_paramID;
        #endregion

        /// <summary>
        /// The constructor for NewSimParamAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The created paramter ID.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public NewSimParamAction(string paramID, bool isAnchor)
        {
            m_paramID = paramID;
            m_isAnchor = isAnchor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NewSimParamAction:" + m_paramID;
        }
        /// <summary>
        /// Execute to create the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.CreateSimulationParameter(m_paramID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DeleteSimulationParameter(m_paramID, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to delete the parameter of simulation.
    /// </summary>
    public class DeleteSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The deleted parameter ID.
        /// </summary>
        private string m_paramID;
        #endregion

        /// <summary>
        /// The constructor for DeleteSimParamAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The deleted parameter ID.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public DeleteSimParamAction(string paramID, bool isAnchor)
        {
            m_paramID = paramID;
            m_isAnchor = isAnchor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DeleteSimParamAction:" + m_isAnchor.ToString() + ", " + m_paramID;
        }
        /// <summary>
        /// Execute to delete the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.DeleteSimulationParameter(m_paramID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.CreateSimulationParameter(m_paramID, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to setup parameter of simulation.
    /// </summary>
    public class SetSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// New parameter ID.
        /// </summary>
        private string m_newParamID;
        /// <summary>
        /// Old parameter ID.
        /// </summary>
        private string m_oldParamID;
        #endregion

        /// <summary>
        /// The constructor for SetSimParamAction with initial parameters.
        /// </summary>
        /// <param name="newParamID">new parameter ID</param>
        /// <param name="oldParamID">old parameter ID</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public SetSimParamAction(string newParamID, string oldParamID, bool isAnchor)
        {
            m_newParamID = newParamID;
            m_oldParamID = oldParamID;
            m_isAnchor = isAnchor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NewSimParamAction:" + m_isAnchor.ToString() + ", " + m_oldParamID + ", " + m_newParamID;
        }
        /// <summary>
        /// Execute to set the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.SetSimulationParameter(m_newParamID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.SetSimulationParameter(m_oldParamID, false, m_isAnchor);
        }
    }
}
