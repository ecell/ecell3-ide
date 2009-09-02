import clr
clr.AddReference("EcellLib")
import Ecell as Ecell



class JobStub:
    def __init__(self, aCommandManager, aJobManager, aName, aID):
        '''
        Constructor.
        Return None:
        '''
        self.theCommandManager = aCommandManager
        self.theJobManager = aJobManager
        self.theJobID = aID
        self.theName = aName
        self.theJob = self.theCommandManager.CreateJobStrub(aName, aID)


    def create(self):
        '''
        Create the job entity.
        Return None :
        '''
        self.theJob.Create()


    def delete(self):
        '''
        Delete the job entity.
        Return None :
        '''
        self.theJob.Delete()


    def getStatus(self):
        '''
        Get the status of job.
        Return int :
        '''
        return self.theJob.GetStatus()


    def getProcessID(self):
        '''
        Get the process ID of job.
        Return int :
        '''
        return self.theJob.GetProcessID()


    def GetStdOut(self):
        '''
        Get the message at StdOut.
        Retrun str :
        '''
        return self.theJob.GetStdOut()


    def GetStdErr(self):
        '''
        Get the message at StdErr.
        Retrun str :
        '''
        return self.theJob.GetStdErr()


class JobManager:
    def __init__(self, aCommantManager, aConc, aEnv):
        '''
        Constructor
        aCommandManager : CommandManager
        aConc(str) : the numner of concurrency
        aEnv(str) : Environment string
        Return None :
        '''
        self.theManager = aCommandManager
        self.theJobManager = aCommandManager.JobManager
        self.setEnvironment(aEnv)
        self.setConcurrency(aConc)


    def setEnvironment(self, env):
        '''
        Set the environment.
        Return None:
        '''
        self.theJobManager.SetCurrentEnvironment(env)


    def getEnvironment(self):
        '''
        Get the environment.
        Return envitonment string:
        '''
        return self.theJobManager.getCurrentEnvironment()


    def setConcurrency(self, aConc):
        '''
        Set the concurrency
        Return None:
        '''
        self.theJobManager.Conurrency = aConc


    def getConcurrency(self, aConc):
        '''
        Get the concurrency
        Return concurency string:
        '''
        return self.theJobManager.Conurrency


    def setTmpRootDir(self, tmpRoot):
        '''
        Set the temporary root directory.
        Return None:
        '''
        self.theJobManager.TmpRootDir = tmpRoot


    def getTmpRootDir(self):
        '''
        Get the temporary root directory.
        Return str :
        '''
        return self.theJobManager.TmpRootDir


    def clearSessionProxy(self, jobid):
        '''
        Clear the job.
        Return None:
        '''
        self.theJobManager.ClearJob(jobid)


    def run(self):
        '''
        Run the jobs.
        Return None:
        '''
        try:
            self.theJobManager.Run()
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def stop(self, aID):
        '''
        Stop the jobs by job id.
        If job id = 0, all job is stopped.
        Return None:
        '''
        try:
            self.theJobManager.Stop(aID)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def RunSimParameterRange(self, groupName, topdir, modelName, num, count):
        '''
        Run the simulation by using the initial parameter within the range of parameters.
        groupName : str : group name.
        topdir : str : top directory.
        modelName : str : model name.
        num : int : the number of samples
        count : double : the simulation time
        Return the list of int : the list of job id
        '''
        try:
            list = []
            for value in self.thManager.RunSimParameterRange(groupName, topdir, modelName, num, count):
                list.append(value)
            return list
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def RunSimParameterMatrix(self, groupName, topdir, modelName, count):
        '''
        Run the simulation by using the initial parameter within the range of parameters.
        groupName : str : group name.
        topdir : str : top directory.
        modelName : str : model name.
        count : double : the simulation time
        Return the list of int : the list of job id
        '''
        try:
            list = []
            for value in self.theManager.RunSimParameterMatrix(groupName, topdir, modelName, count):
                list.append(value)
            return list
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def registerSessionProxy(self, script, arg, extFile):
        '''
        Regist the job.
        script : str : the script file name
        arg : str : the argument of script
        extFile : the list of str : the list of output file
        Return int (job id):
        '''
        try:
            return self.theJobManager.RegisterJob(script, arg, extFile)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


class Session:
    def __init__( self ):
        '''
        Constructor
        Initialize the parameters and execute the library of python.
        Return None :
        '''
        if Ecell.CommandManager.s_instance is None:
            theEnv =  Ecell.ApplicationEnvironment()
        self.theManager = Ecell.CommandManager.s_instance


    def loadModel( self, aModel ):
        '''
        Load the model from the model file.
        aModel(str) -- input the model file name.
        This method can throw exceptions.
        Return None :
        '''
        try:
            self.theManager.LoadModel(aModel)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def saveModel( self ):
        '''
        Save the model to the project.
        This method can throw exceptions.
        Return None :
        '''
        try:
            self.theManager.SaveMode()
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def run( self, aTime = "" ):
        '''
        Run the simulation of current project.
        aTime(double) -- input the simulation time.
                             if aTime is null or 0.0, simulation do not suspend.
        This method can throw exceptions.
        Return None :
        '''
        try:
            if not aTime:
                self.theManager.Run(0.0)
            else:
                self.theManager.Run(aTime)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def step(self, num = 1):
        '''
        Step the simulation on the current project.
        num(int) -- input the number of step.
        Return None :
        '''
        try:
            self.theCommandManager.Step(num)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def stop( self ):
        '''
        Stop the simulation of current project.
        This method can throw exceptions.
        Return None :
        '''
        try:
            self.theManager.Stop()
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def getCurrentTime( self ):
        '''
        Get the current time of simulation.
        This method can throw exceptions.
        Return str: the current time of simulation.
        '''
        self.theManager.GetCurrentTime()


    def createEntityStub( self, aFullID ):
        '''
        Create the entity having the input ID.	
        aFullID(str) -- input the full ID.
	               (for example, Process:/cell:Translate_CI)
        Return EntityStub : the created entity object.
        '''
        return EntityStub(self.theManager, aFullID)


    def createStepperStub( self, anID ):
        '''
        Create the stepper entity on the current project.
        anID(str) -- input the ID of stepper.
        This method can throw exceptions.
        Return StepperStub : the created stepper entity object.
        '''
        return StepperStub(self.theManager, anID)


    def createLogger( self, aFullPN ):
        '''
        Create the logger entity.
        This function do not have the return object.
        aFullID(str) -- input the full ID.
	               (for example, Process:/cell:Translate_CI:Activity)
        This method can throw exceptions.
        Return None :
        '''
        try:
            self.theManager.CreateLogger(aFullPN)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def createLoggerStub( self, aFullPN ):
        '''
        Create the logger entity.
        aFullID(str) -- input the full ID.
	               (for example, Process:/cell:Translate_CI:Activity)
        This method can throw exceptions.
        Return LoggerStub : the created logger entity object.
        '''
        return LoggerStub(self.theManager, aFullPN)


    def saveLoggerData(
            self,
            aFullPN,
            aSaveDirectory,
            aStartTime = 0.0,
            anEndTime = 0.0):
        '''
        Save the log data of property while the simulation is running.
        If aStartTime and anEndTime is set 0.0, this function save the all log data.
        aFullPN(str) -- input the property name of log data.
        aSaveDirectory(str) -- input the directory name to save the log data.
        aStartTime(double) -- input the start time to get the log data. The default is 0.0.
        aEndTime(double) -- input the end time to get the log data. The default is 0.0.
        Return None :
        '''
        try:
            self.theManager.SaveLoggerData(aFullPN, aSaveDirectory, aStartTime, anEndTime)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def getEntityList( self, anEntityType, aSystemPath ):
        '''
        Get the list of entity in the set system.
        anEntityType("Process", "Variable", "System") -- the type of entity.
        aSystemPath(str) -- the path of system.
        Return the list of EntityStub : the list of entity.
        '''
        tuple = ()
        for value in self.theManager.GetEntityList(anEntityType, aSystemPath):
            tuple = tuple + (value,)
        return tuple


    def getLoggerList( self ):
        '''
        Get the list of working logger entity.
        Return the list of LoggerStub : the list of logger entity.
        '''
        tuple = ()
        for value in self.theManager.GetLoggerList():
            tuple = tuple + (value,)
        return tuple


    def createProject( self, aProjectName, aComment ):
        '''
        Create the project(IDE originial).
        aProjectName(str) -- input the name of project.
        aComment(str) -- input the comment of project.
        Return None :
        '''
        try:
            self.theManager.CreateProject(aProjectName, aComment)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def loadSBML( self, aSBMLFile ):
        '''
        Load the SBML file(IDE original).
        aSBMLFile(str) -- input the file name.
        Return None :
        '''
        try:
            self.theManager.LoadSBML(aSBMLFile)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def getCurrentProjectID( self ):
        '''
        Get the name of current project(IDE original).
        Return str : the name of current project.
        '''
        return self.theManager.GetCurrentProjectID()


    def getCurrentSimulationParameterID( self ):
        '''
        Get the parameter id of current project(IDE original).
        Return str : the parameter id.
        '''
        return self.theManager.GetCurrentSimulationParameterID()


    def setObservedData(self, fullPN, max, min, differ, rate):
        '''
        Set the observed data.
        fullPN : str
        max : float
        min : float
        differ : float
        rate : float
        Return None:
        '''
        try:
            self.theManager.SetObservedData(fullPN, max, min, differ, rate)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def setParameterData(self, fullPN, max, min, step):
        '''
        Set the parameter data.
        fullPN : str
        max : float
        min : float
        step : float
        Return None:
        '''
        try:
            self.theManager.SetParameterData(fullPN, max, min, step)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def removeObservedData(self, fullPN):
        '''
        Remove the observed data.
        fullPN : str
        Return None:
        '''
        try:
            self.theManager.RemoveObservedData(fullPN)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def removeParameterData(self, fullPN):
        '''
        Remove the parameter data.
        fullPN : str
        Return None:
        '''
        try:
            self.theManager.RemoveParameterData(fullPN)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def createJobGroup(self, analysisName):
        '''
        Return the group name.
        analysisName : str : analysis name.
        Return the string : the group name.
        '''
        try:
            return self.theManager.CreateJobGroup(analysisName)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def getProcessList(self):
        '''
        Get the list of pecess in the current project.
        Return the list of EntityStub : the list of process entity object.
        '''
        list = []
        for value in self.theManager.GetProcessList():
            list.append(value)
        list.sort()
        return list


    def getStepperList(self):
        '''
        Get the list of stepper in the current project.
        Return the list of StepperStub : the list of stepper entity object.
        '''
        tuple = ()
        for value in self.theManager.GetStepperList():
            tuple = tuple + (value,)
        return tuple


    def getVariableList(self):
        '''
        Get the list of variable in the current project.
        Return the list of EntityStub : the list of variable entity object.
        '''
        list = []
        for value in self.theManager.GetVariableList():
            if(re.match(".*:SIZE", value) == None):
                list.append(value)
        list.sort()
        return list


    def createSimulationParameterStub(self, aParameterID):
        '''
        Create the simulation parameter of current project.
        aParameterID(str) -- input the parameter name of current project.
        Return SimulationParameterStub : the created simulation parameter entity object.
        '''
        return SimulationParameterStub(self.theCommandManager, aParameterID)


    def getNextEvent(self):
        tuple = ()
        for value in self.theCommandManager.GetNextEvent():
            tuple = tuple + (value,)
        return tuple


    def getLoggerData(
            self,
            aFillPN,
            aStartTime = 0.0,
            anEndTime = 0.0):
        '''
        Get the log data while the simulation.
        If aStartTime and anEndTime is set 0.0, this function get the all log data.
        aFillPN(str) -- input the property name of log data.
	               (for example, Process:/cell:Translate_CI:Activity)
        aStartTime(double) -- the start time to get the log data. the default 0.0.
        aEndTime(double) --- the end time to get the log data, the default 0.0.
        Return the list of Tuple : the list of log data.
        '''
        tuple = ()
        previousTime = -1.0
        for value in self.theManager.GetLogData(aFillPN, aStartTime, anEndTime):
            if(previousTime == value.time):
                continue
            childTuple = ()
            childTuple = childTuple + (value.time,) + (value.value,)
            if value.avg != None and value.avg > 0:
                childTuple = childTuple + (value.avg,) + (value.min,) + (value.max,)
            tuple = tuple + (childTuple,)
            previousTime = value.time
        return tuple


    def getEntityProperty(self, aFullPNString):
        '''
        Get the property value from FullPN.
        return str:
        '''
        return self.theManager.GetEntityProperty(aFullPNString)


    def setEntityProperty(self, aFullPNString, aValue):
        '''
        Set the property value of entity.
        aFullPNString(str) -- input the set property name.
        aValue(str) -- input the set value.
        Return None :
        '''
        self.theManager.SetEntityProperty(aFullPNString, aValue)




class EntityStub:
    def __init__( self, aCommandManager, aFullID ):
        '''
        Constructor.
        Initialize of this entity.
        aCommandManager(CommandManager) -- input CommandManager.
        aFullID(str) -- input the ID of entity.
        Return None :
        '''
        self.theManager = aCommandManager
        self.theEntity = self.theManager.CreateEntityStub(aFullID)
        self.theFullIDString = aFullID


    def getName( self ):
        '''
        Get the name of entity object.
        Return str : the name of entity.
        '''
        return self.theEntity.GetName()


    def create( self, aClassName ):
        '''
        Create the entity object.
        aClassName(str) -- input the class name of entity object.
        Return None :
        '''
        try:
            self.theEntity.Create(aClassName)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def delete( self ):
        '''
        Delete the current entity object.
        Return None :
        '''
        self.theEntity.Delete()
        self.theEntity = None


    def getClassname( self ):
        '''
        Get the class name of entity object.
        Return str : the class name.
        '''
        return self.theEntity.GetClassName()


    def exists( self ):
        '''
        Check whether this object have the entity object.
        Return bool :  whether this object have the entity object.
        '''
        return self.theEntity.Exists()


    def setProperty( self, aPropertyName, aValue ):
        '''
        Set the property value of this entity.
        aPropertyName(str) -- input the name of property.
        aValue(str) -- input the value of property.
        Return None :
        '''
        try:
            self.theEntity.SetProperty(aPropertyName, str(aValue))
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def __setitem__( self, aPropertyName, aValue ):
        '''
        Set the property value of this entity.
        aPropertyName(str) -- input the name of property.
        aValue(str) -- input the value of property.
        Return None :
        '''
        self.SetProperty(aPropertyName, aValue)


    def getProperty( self, aPropertyName ):
        '''
        Get the property value.
        aPropertyName(str) -- input the property name.
        Return str : the property value.
        '''
        try:
            return self.theEntity.GetProperty(aPropertyName)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def __getitem__( self, aPropertyName ):
        '''
        Get the property value.
        aPropertyName(str) -- input the property name.
        Return str : the property value.
        '''
        return self.getProperty(aPropertyName)


    def getPropertyAttributes(self, aPropertyName):
        '''
        Get the permission attribute of property.
        The attribute is "Settable", "Gettable", "Loadable" and "Saveable".
        aPropertyName(str) -- input the property name.
        Return the list of Tuple : the list of attribute(bool).
        '''
        tuple = ()
        for value in self.theEntity.GetPropertyAttributes(aPropertyName):
            tuple = tuple + (value,)
        return tuple


    def getPropertyList(self):
        '''
        Get the list of property name.
        Return the list of str : the list of property naem.
        '''
        tuple = ()
        for value in self.theEntity.GetPropertyList():
            tuple = tuple + (value,)
        return tuple




class StepperStub:
    def __init__( self, aCommandManager, anID ):
        '''
        Constructor.
        aCommandManager(CommandManager) -- input CommandManager.
        anID(str) -- input the name of stepper.
        Return None :
        '''
        self.theManager = aCommandManager
        self.theStepper = self.theManager.CreateStepperStub(anID)


    def getName( self ):
        '''
        Get the name of this stepper.
        Return str : the name of stepper.
        '''
        return self.theStepper.GetName()


    def create( self, aClassName ):
        '''
        Create the stepper haveing the classname of stepper.
        aClassName(str) -- the classname of stepper.
        This method can throw exceptions.
        Return None :
        '''
        try:
            self.theStepper.Create(aClassName)
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def delete( self ):
        '''
        Delete the stepper object.
        This method can throw exceptions.
        Return None :
        '''
        self.theStepper.Delete()
        self.theStepper = None


    def exists( self ):
        '''
        Get whether the stepper exist in the current project.
        Return bool :
        '''
        return self.theStepper.Exists()


    def getClassname( self ):
        '''
        Get the classname of this stepper.
        Return str : the class name.
        '''
        return self.theStepper.GetClassName()


    def getProperty( self, aPropertyName ):
        '''
        Get the property of this stepper.
        aPropertyName(str) -- input the name of property.
        This method can throw exceptions.
        Return str : the value of property.
        '''
        return self.theStepper.GetProperty(aPropertyName)


    def __getitem__( self, aPropertyName ):
        '''
        Get the property of this stepper.
        aPropertyName(str) -- input the name of property.
        This method can throw exceptions.
        Return str : the value of property.
        '''
        return self.getProperty( aPropertyName )


    def setProperty( self, aPropertyName, aValue ):
        '''
        Set the value of property for this stepper.
        aPropertyName(str) -- input the name of property.
        aValue(str) -- input the value of property.
        This method can throw exceptions.
        Return None :
        '''
        try:
            self.theStepper.SetProperty(aPropertyName, str(aValue))
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def __setitem__( self, aPropertyName, aValue ):
        '''
        Set the value of property for this stepper.
        aPropertyName(str) -- input the name of property.
        aValue(str) -- input the value of property.
        This method can throw exceptions.
        Return None :
        '''
        return self.setProperty( aPropertyName, aValue )


    def getPropertyList(self):
        '''
        Get the list of property of this stepper.
        Return the list of Tuple : the list of property name.
        '''
        tuple = ()
        for value in self.Setepper.GetPropertyList():
            tuple = tuple = (value,)
        return tuple


    def getPropertyAttributes(self, aPropertyName):
        '''
        Get the permission attribute of property.
        The attribute is "Settable", "Gettable", "Loadable" and "Saveable".
        aPropertyName(str) -- input the property name.
        Return the list of Tuple : the list of attribute(bool).
        '''
        tuple = ()
        for value in self.theStepper.GetPropertyAttributes(aPropertyName):
            tuple = tuple = (value,)
        return tuple




class LoggerStub:
    def __init__( self, aCommandManager, aFullPN ):
        '''
        Constructor.
        aCommandManager(CommandManager) -- input CommandManager.
        aFullPN(str) -- input the name of entity.
        This method can throw exceptions.
        Return None :
        '''
        self.theManager = aCommandManager
        self.theLogger = self.theManager.CreateLoggerStub(aFullPN)


    def getName(self):
        '''
        Get the name of logger.
        Return  str : the name of logger.
        '''
        return self.theLogger.GetName()


    def create( self ):
        '''
        Create the logger entity.
        Return None :
        '''
        try :
            self.theLogger.Create()
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def delete(self):
        '''
        Delete the logger entity.
        Return None :
        '''
        self.theLogger.Delete()


    def exists(self):
        '''
        Check whether this logger entity exists,
        Return None:
        '''
        self.theLogger.Exists()


    def getData( self, aStartTime=None, anEndTime=None, anInterval=None ):
        '''
        Get the log data from the start time to the end time.
        aStartTime(double) -- input the start time to get the log data.
        anEndTime(double) -- input the end time to get the log data.
        anInterval(double) -- input the interval to get the log data.
        Return the list of Tuple : the list of log data.
        '''
        try:
            if anStartTime == None:
                anStartTime = self.getStartTime()
            if anEndTime == None:
                anEndTime = self.getEndTime()
            tuple = ()
            if anInterval == None:
                for value in self.theManager.GetData(aStartTime, anEndTime):
                    childTuple = ()
                    childTuple = childTuple + (value.time,) + (value.value,)
                    if value.avg != None and value.avg > 0:
                        childTuple = childTuple + (value.avg,) + (value.min,) + (value.max,)
                    tuple = tuple + (childTuple,)
            else:
                for value in self.theManager.GetData(aStartTime, anEndTime, anInterval):
                    childTuple = ()
                    childTuple = childTuple + (value.time,) + (value.value,)
                    if value.avg != None and value.avg > 0:
                        childTuple = childTuple + (value.avg,) + (value.min,) + (value.max,)
                    tuple = tuple + (childTuple,)
            return tuple
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e

    def getStartTime(self):
        ''' 
        Get the start time of simulation.
        Return double : the start time.
        '''
        return self.theLogger.GetStartTime()


    def getEndTime(self):
        '''
        Get the end time of simulation.
        Return double : the end time.
        '''
        return self.theLogger.GetEndTime()


    def getSize(self):
        '''
        Get the size of logger.
        Return int : the size of logger.
        '''
        return self.theLogger.GetSize()


    def setLoggerPolicy(
            self,
            aStepCount,
            aLogInterval,
            aHDDFullAction,
            aMaxHDDSpace):
        '''
        Set the property of logger policy.
        aStepCount(int) -- input the step count to save the log data.
        aLogInterval(double) -- input the interval to save the log data.
        aHDDFullAction(int) -- input the action when the disk is full.
	                         0 : throw Exception.
	                         1 : override the earlist.
        aMaxHDDSpace(int) -- input the max space of HDD. If 0 is set, the max space is no limit.
        Return None :
        '''
        self.theLogger.SetLoggerPolicy(aStepCount, aLogInterval, aHDDFullAction, aMaxHDDSpace)




class SimulationParameterStub:

    def __init__(self, aCommandManager, aParameterID):
        '''
        Constructor.
        aCommandManager(CommandManager) -- input CommandManager.
        aParameterID(str) -- input the parameter id.
        Return None :
        '''
        self.theManager = aCommandManager
        self.theSimulationParameter = self.theManager.CreateSimulationParameterStub(aParameterID)


    def create(self):
        '''
	   Create the simulation parameter object.
        Return None :
        '''
        try:
            self.theSimulationParameter.Create()
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def delete(self):
        '''
        Delete the stepper from the current project.
       Return None :
        '''
        try:
            self.theSimulationParameter.Delete()
            self.theSimulationParameter = None
        except Exception, e:
            self.theManager.SetMessage(e.Message)
            print e.Message
            raise e


    def exists(self):
        '''
        Check whether the current project have the simulation parameter.
        Return bool : whether the current project have the simulation parameter.
        '''
        return self.theSimulationParameter.Exists()


    def getLoggerPolicy(self):
        '''
        Get the logger policy of current project.
        return the list of Tuple : the reload step count, the reload intervale, the action when the disk is full and the max disk space.
        '''
        loggerPolicy = self.theSimulationParameter.GetLoggerPolicy()
        return (loggerPolicy.m_reloadStepCount,) + (loggerPolicy.m_reloadInterval,) + (loggerPolicy.m_diskFullAction,) + (loggerPolicy.m_maxDiskSpace,)


    def getInitialCondition(self, aType):
        '''
        Get the initial condition of entity object.
        aType(str) -- "Process", "Variable" or "System".
        return the list of Tuple : the dictionary of key string and value string.
        '''
        tuple = ()
        initialCondition = self.theSimulationParameter.GetInitialCondition(aType)
        for key in initialCondition.Keys:
            tuple = tuple + ((key, initialCondition[key]),)
        return tuple


    def getSimulationParameterID(self):
        '''
        Get the simulation parameter ID of current project.
        Return str : the current simulation parameter.
        '''
        return self.theSimulationParameter.GetSimulationParameterID()


    def getStepperIDList(self):
        '''
        Get the list of stepper in the current project.
        Return the list of Tuple : the list of stepper name.
        '''
        tuple = ()
        for value in self.theSimulationParameter.GetStepperIDList():
            tuple = tuple + (value,)
        return tuple


    def setLoggerPolicy(
            self,
            aStepCount,
            aLogInterval,
            aHDDFullAction,
            aMaxHDDSpace):
        '''
        Set the property of logger policy.
        aStepCount(int) -- input the step count to save the log data.
        aLogInterval(double) -- input the interval to save the log data.
        aHDDFullAction(int) -- input the action when the disk is full.
	                         0 : throw Exception.
	                         1 : override the earlist.
        aMaxHDDSpace(int) -- input the max space of HDD. If 0 is set, the max space is no limit.
        Return None :
        '''
        self.theSimulationParameter.SetLoggerPolicy(aStepCount, aLogInterval, aHDDFullAction, aMaxHDDSpace)


    def updateInitialCondition(self, keyType, dic):
        '''
        Set the initial condiction of simulation on the current project.
        keyType("Proceee", "Variable" or "System") -- input the type of entity.
        dic(the dictionary of string and double) -- input the initial condition of entity.
        Return None :
        '''
        self.theManager.UpdateInitialCondition(keyType, dic)




if __name__ == "__main__":
    pass