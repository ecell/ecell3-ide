import re
import sys
import time
import clr
import System.IO
clr.AddReferenceToFile("EcellLib.dll")
import Ecell


class Session:
    'Session class'

    def __init__(self, aCommandManager = None):
	'''
	Constructor.
	Initialize the parameters and execute the library of python.
	aCommandManager(CommandManager) -- CommandManager. The defaule is none.

	Return None :
	'''
        if aCommandManager is None:
            self.theCommandManager = EcellLib.CommandManager.GetInstance()
        else:
            self.theCommandManager = aCommandManager
        self.execNumpyFileName = "ExecNumpy.bat"

    def createEntityStub(self, aFullID):
	'''
	Create the entity having the input ID.	
	aFullID(str) -- input the full ID.
	               (for example, Process:/cell:Translate_CI)

	Return EntityStub : the created entity object.
	'''
        return EntityStub(self.theCommandManager, aFullID)

    def createLogger(self, aFullPN):
	'''
	Create the logger entity.
	This function do not have the return object.
	aFullID(str) -- input the full ID.
	               (for example, Process:/cell:Translate_CI:Activity)

	Return None :
	'''
        self.theCommandManager.CreateLogger(aFullPN)

    def createLoggerStub(self, aFullPN):
	'''
	Create the logger entity.
	aFullID(str) -- input the full ID.
	               (for example, Process:/cell:Translate_CI:Activity)

	Return LoggerStub : the created logger entity object.
	'''
        return LoggerStub(self.theCommandManager, aFullPN)

    def createModel(self, aModelName):
	'''
	Create the model on the current project.
	aModelName(str) -- input the name of model.

	Return None :
	'''
        self.theCommandManager.CreateModel(aModelName)

    def createProject(self, aProjectName, aComment):
	'''
	Create the project.
	aProjectName(str) -- input the name of project.
	aComment(str) -- input the comment of project.

	Return None :
	'''
        self.theCommandManager.CreateProject(aProjectName, aComment)

    def createSimulationParameterStub(self, aParameterID):
	'''
	Create the simulation parameter of current project.
	aParameterID(str) -- input the parameter name of current project.

	Return SimulationParameterStub : the created simulation parameter entity object.
	'''
        return SimulationParameterStub(self.theCommandManager, aParameterID)

    def createStepperStub(self, anID):
	'''
	Create the stepper entity on the current project.
	anID(str) -- input the ID of stepper.

	Return StepperStub : the created stepper entity object.
	'''
        return StepperStub(self.theCommandManager, anID)

    def execNumpy(self, methodName, matrix, methodArgumentList):
        tmpFileName = time.time()
        try:
            f = open(str(tmpFileName), "w")
            if(matrix != None):
                f.write(str(matrix) + "\n\n")
            if(methodArgumentList != None):
                for methodArgument in methodArgumentList:
                    f.write(str(methodArgument) + "\n\n")
            f.close()
        except IOError, e:
            raise TypeError, " Not found the tmp file named [" + tmpFileName + "]. { " + str(e) + " }"

        self.theCommandManager.Exec(self.execNumpyFileName, methodName + " " + str(tmpFileName))

        info = ""
        try:
            f = open(str(tmpFileName), "r")
            info = f.read()
            f.close()
        except IOError, e:
            raise TypeError, " Not found the tmp file named [" + tmpFileName + "]. { " + str(e) + " }"

        System.IO.File.Delete(str(tmpFileName))

        return info

    def getCurrentProjectID(self):
	'''
	Get the name of current project.

	Return str : the name of current project.
	'''
        return self.theCommandManager.GetCurrentProjectID()

    def getCurrentSimulationParameterID(self):
	'''
	Get the parameter id of current project.

	Return str : the parameter id.
	'''
        return self.theCommandManager.GetCurrentSimulationParameterID()

    def getCurrentTime(self):
	'''
	Get the current time.

	Return str : the current time string.
	'''
        return self.theCommandManager.GetCurrentSimulationTime()

    def getEntityList(self, anEntityType, aSystemPath):
	'''
	Get the list of entity in the set system.
	anEntityType("Process", "Variable", "System") -- the type of entity.
	aSystemPath(str) -- the path of system.

	Return the list of EntityStub : the list of entity.
	'''
        tuple = ()
        for value in self.theCommandManager.GetEntityList(anEntityType, aSystemPath):
            tuple = tuple + (value,)
        return tuple

    def getEntityProperty(self, aFullPNString):
        return self.theCommandManager.GetEntityProperty(aFullPNString)

    def getLoggerList(self):
	'''
	Get the list of working logger entity.

	Return the list of LoggerStub : the list of logger entity.
	'''
        tuple = ()
        for value in self.theCommandManager.GetLoggerList():
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
        for value in self.theCommandManager.GetLogData(aFillPN, aStartTime, anEndTime):
            if(previousTime == value.time):
                continue
            childTuple = ()
            childTuple = childTuple + (value.time,) + (value.value,)
            if value.avg != None and value.avg > 0:
                childTuple = childTuple + (value.avg,) + (value.min,) + (value.max,)
            tuple = tuple + (childTuple,)
            previousTime = value.time
        return tuple

    def getNextEvent(self):
        tuple = ()
        for value in self.theCommandManager.GetNextEvent():
            tuple = tuple + (value,)
        return tuple

    def getProcessList(self):
	'''
	Get the list of pecess in the current project.

	Return the list of EntityStub : the list of process entity object.
	'''
        list = []
        for value in self.theCommandManager.GetProcessList():
            list.append(value)
        list.sort()
        return list

    def getSimulationParameterIDList(self):
	'''
	Get the list of simulation parameter id in the current project.

	Return the list of str : the list of parameter id.
	'''
        return self.theCommandManager.GetSimulationParameterIDList()

    def getStepperList(self):
	'''
	Get the list of stepper in the current project.

	Return the list of StepperStub : the list of stepper entity object.
	'''
        tuple = ()
        for value in self.theCommandManager.GetStepperList():
            tuple = tuple + (value,)
        return tuple

    def getVariableList(self):
	'''
	Get the list of variable in the current project.

	Return the list of EntityStub : the list of variable entity object.
	'''
        list = []
        for value in self.theCommandManager.GetVariableList():
            if(re.match(".*:SIZE", value) == None):
                list.append(value)
        list.sort()
        return list

    def initialize(self):
	'''
	Load the model into the simulator, and prepare the simulation of the current project.

	Return None :
	'''
        self.theCommandManager.Initialize()

    def interact(self):
        self.theCommandManager.Interact()

    def isActive(self):
	'''
	Get whether the simulation of the current project is running.

	Return bool : whether the simulation of the current project is running.
	'''
        self.theCommandManager.IsActive()

    def loadModel(self, aEmlFileName):
	'''
	Load the model from the input file. The file format of this file is eml.
	aEmlFileName(str) -- input the eml file.

	Return None :
	'''
        self.theCommandManager.LoadModel(aEmlFileName)

    def refresh(self):
	'''
	Close the current project.

	Return None :
	'''
        self.theCommandManager.Refresh()

    def run(self, aTime = 0):
	'''
	Run the simulation of current project.
	aTime(double) -- input the simulation time.

	Return None :
	'''
        self.theCommandManager.Run(aTime)

    def runNotSuspend(self, aTime = 0):
	'''
	Run the simulation of current project without suspend.
	aTime(double) -- input the simulation time.

	Return None :
	'''
        self.theCommandManager.RunNotSuspend(aTime)

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
        self.theCommandManager.SaveLoggerData(aFullPN, aSaveDirectory, aStartTime, anEndTime)

    def saveModel(self, aModelName):
	'''
	Save the model data.
	aModelName(str) -- input the name of saved model.

	Return None :
	'''
        self.theCommandManager.SaveModel(aModelName)

    def setEntityProperty(self, aFullPNString, aValue):
	'''
	Set the property value of entity.
	aFullPNString(str) -- input the set property name.
	aValue(str) -- input the set value.

	Return None :
	'''
        self.theCommandManager.SetEntityProperty(aFullPNString, aValue)

    def step(self, num = 1):
	'''
	Step the simulation on the current project.
	num(int) -- input the number of step.

	Return None :
	'''
        self.theCommandManager.Step(num)

    def stepNotSuspend(self, num = 1):
	'''
	Step the simulation on the current project without the simulation suspend.
	num(int) -- input the number of step.

	Return None :
	'''
        self.theCommandManager.StepNotSuspend(num)

    def stop(self):
	'''
	Reset the simulation of the current project.
	
	Return None :
	'''
        self.theCommandManager.Stop()

    def updateInitialCondition(self, keyType, dic):
	'''
	Set the initial condiction of simulation on the current project.
	keyType("Proceee", "Variable" or "System") -- input the type of entity.
	dic(the dictionary of string and double) -- input the initial condition of entity.

	Return None :
	'''
        self.theCommandManager.UpdateInitialCondition(keyType, dic)


class EntityStub:

    def __init__(self, aCommandManager, aFullID):
	'''
	Constructor.
	Initialize of this entity.
	aCommandManager(CommandManager) -- input CommandManager.
	aFullID(str) -- input the ID of entity.

	Return None :
	'''
        self.theCommandManager = aCommandManager
        self.theEntity = self.theCommandManager.CreateEntityStub(aFullID)

    def create(self, aClassName):
	'''
	Create the entity object.
	aClassName(str) -- input the class name of entity object.

	Return None :
	'''
        self.theEntity.Create(aClassName)

    def delete(self):
	'''
	Delete the current entity object.
	
	Return None :
	'''
        self.theEntity.Delete()
        self.theEntity = None

    def exists(self):
	'''
	Check whether this object have the entity object.
	
	Return bool :  whether this object have the entity object.
	'''
        return self.theEntity.Exists()

    def getClassname(self):
	'''
	Get the class name of entity object.

	Return str : the class name.
	'''
        return self.theEntity.GetClassName()

    def getName(self):
	'''
	Get the name of entity object.

	Return str : the name of entity.
	'''
        return self.theEntity.GetName()

    def getProperty(self, aPropertyName):
	'''
	Get the property value.
	aPropertyName(str) -- input the property name.
	
	Return str : the property value.
	'''
        return self.theEntity.GetProperty(aPropertyName)

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

    def setProperty(self, aPropertyName, aValue):
	'''
	Set the property value of this entity.

	aPropertyName(str) -- input the name of property.
	aValue(str) -- input the value of property.

	Return None :
	'''
        self.theEntity.SetProperty(aPropertyName, aValue)


class LoggerStub:

    def __init__(self, aCommandManager, aFullPN):
	'''
	Constructor.
	aCommandManager(CommandManager) -- input CommandManager.
	aFullPN(str) -- input the name of entity.

	Return None :
	'''
        self.theCommandManager = aCommandManager
        self.theLogger = self.theCommandManager.CreateLoggerStub(aFullPN)

    def create(self):
	'''
	Create the logger entity.

	Return None :
	'''
        self.theLogger.Create()

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

    def getData(self, aStartTime = 0.0, anEndTime = 0.0):
	'''
	Get the log data from the start time to the end time.
	aStartTime(double) -- input the start time to get the log data.
	anEndTime(double) -- input the end time to get the log data.

	Return the list of Tuple : the list of log data.
	'''
        tuple = ()
        for value in self.theLogger.GetData(aStartTime, anEndTime):
            childTuple = ()
            childTuple = childTuple + (value.time,) + (value.value,)
            if value.avg != None and value.avg > 0:
                childTuple = childTuple + (value.avg,) + (value.min,) + (value.max,)
            tuple = tuple + (childTuple,)
        return tuple

    def getEndTime(self):
	'''
	Get the end time of simulation.
	
	Return double : the end time.
	'''
        return self.theLogger.GetEndTime()

    def getLoggerPolicy(self):
	'''
	Get the logger policy of this logger entity.

	Return the list of Tuple : the reload step count, the reload intervale, the action when the disk is full and the max disk space.
	'''
        loggerPolicy = self.theLogger.GetLoggerPolicy()
        return (loggerPolicy.m_reloadStepCount,) + (loggerPolicy.m_reloadInterval,) + (loggerPolicy.m_diskFullAction,) + (loggerPolicy.m_maxDiskSpace,)

    def getName(self):
	'''
	Get the name of logger.

	Return  str : the name of logger.
	'''
        return self.theLogger.GetName()

    def getSize(self):
	'''
	Get the size of logger.

	Return int : the size of logger.
	'''
        return self.theLogger.GetSize()

    def getStartTime(self):
	''' 
	Get the start time of simulation.

	Return double : the start time.
	'''
        return self.theLogger.GetStartTime()

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
        self.theCommandManager = aCommandManager
        self.theSimulationParameter = self.theCommandManager.CreateSimulationParameterStub(aParameterID)

    def create(self):
	'''
	Create the simulation parameter object.

	Return None :
	'''
        self.theSimulationParameter.Create()

    def createStepperStub(self, aStepperID):
	'''
	Create the stepper having the stepper ID in the current project.
	aStepperID(str) -- input the name of stepper.

	Return None :
	'''
        self.theSimulationParameter.CreateStepperStub(aStepperID)

    def delete(self):
	'''
	Delete the stepper from the current project.

	Return None :
	'''
        self.theSimulationParameter.Delete()
        self.theSimulationParameter = None

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


class StepperStub:

    def __init__(self, aCommandManager, anID):
	'''
	Constructor.
	aCommandManager(CommandManager) -- input CommandManager.
	anID(str) -- input the name of stepper.

	Return None :
	'''
        self.theCommandManager = aCommandManager
        self.theStepper = self.theCommandManager.CreateStepperStub(anID)

    def create(self, aClassName):
	'''
	Create the stepper haveing the classname of stepper.
	aClassName(str) -- the classname of stepper.

	Return None :
	'''
        self.theStepper.Create(aClassName)

    def delete(self):
	'''
	Delete the stepper object.
	
	Return None :
	'''
        self.theStepper.Delete()
        self.theStepper = None

    def exists(self):
	'''
	Get whether the stepper exist in the current project.

	Return None :
	'''
        return self.theStepper.Exists()

    def getClassname(self):
	'''
	Get the classname of this stepper.

	Return str : the class name.
	'''
        return self.theStepper.GetClassName()

    def getName(self):
	'''
	Get the name of this stepper.
	
	Return str : the name of stepper.
	'''
        return self.theStepper.GetName()

    def getProperty(self, aPropertyName):
	'''
	Get the property of this stepper.
	aPropertyName(str) -- input the name of property.

	Return str : the value of property.
	'''
        return self.theStepper.GetProperty(aPropertyName)

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

    def getPropertyList(self):
	'''
	Get the list of property of this stepper.
	
	Return the list of Tuple : the list of property name.
	'''
        tuple = ()
        for value in self.Setepper.GetPropertyList():
            tuple = tuple = (value,)
        return tuple

    def setProperty(self, aPropertyName, aValue):
	'''
	Set the value of property for this stepper.
	aPropertyName(str) -- input the name of property.
	aValue(str) -- input the value of property.

	Return None :
	'''
        self.theStepper.SetProperty(aPropertyName, aValue)

    def getSimulationParameterID():
	'''
	Get the simulation parameter ID.
	
	Return the current simulation parameter ID.
	'''
        return self.theStepper.GetSimulationParameterID()
