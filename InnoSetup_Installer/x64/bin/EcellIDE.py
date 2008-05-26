import re
import sys
import time
import clr
import System.IO
clr.AddReferenceToFile("EcellLib.dll")
import EcellLib


print ""
print "# Imports following modules by default."
print "import sys"
print "import clr"
print "import EcellLib"
print ""


class Session:
    'Session class'

    def __init__(self, aCommandManager = None):
        if aCommandManager is None:
            self.theCommandManager = EcellLib.CommandManager.GetInstance()
        else:
            self.theCommandManager = aCommandManager
        self.execNumpyFileName = "ExecNumpy.bat"

    def createEntityStub(self, aFullID):
        return EntityStub(self.theCommandManager, aFullID)

    def createLogger(self, aFullPN):
        self.theCommandManager.CreateLogger(aFullPN)

    def createLoggerStub(self, aFullPN):
        return LoggerStub(self.theCommandManager, aFullPN)

    def createModel(self, aModelName):
        self.theCommandManager.CreateModel(aModelName)

    def createProject(self, aProjectName, aComment):
        self.theCommandManager.CreateProject(aProjectName, aComment)

    def createSimulationParameterStub(self, aParameterID):
        return SimulationParameterStub(self.theCommandManager, aParameterID)

    def createStepperStub(self, anID):
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
        return self.theCommandManager.GetCurrentProjectID()

    def getCurrentSimulationParameterID(self):
        return self.theCommandManager.GetCurrentSimulationParameterID()

    def getCurrentTime(self):
        return self.theCommandManager.GetCurrentSimulationTime()

    def getEntityList(self, anEntityType, aSystemPath):
        tuple = ()
        for value in self.theCommandManager.GetEntityList(anEntityType, aSystemPath):
            tuple = tuple + (value,)
        return tuple

    def getEntityProperty(self, aFullPNString):
        return self.theCommandManager.GetEntityProperty(aFullPNString)

    def getLoggerList(self):
        tuple = ()
        for value in self.theCommandManager.GetLoggerList():
            tuple = tuple + (value,)
        return tuple

    def getLoggerData(
            self,
            aFillPN,
            aStartTime = 0.0,
            anEndTime = 0.0):
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
        list = []
        for value in self.theCommandManager.GetProcessList():
            list.append(value)
        list.sort()
        return list

    def getSimulationParameterIDList(self):
        return self.theCommandManager.GetSimulationParameterIDList()

    def getStepperList(self):
        tuple = ()
        for value in self.theCommandManager.GetStepperList():
            tuple = tuple + (value,)
        return tuple

    def getVariableList(self):
        list = []
        for value in self.theCommandManager.GetVariableList():
            if(re.match(".*:SIZE", value) == None):
                list.append(value)
        list.sort()
        return list

    def initialize(self):
        self.theCommandManager.Initialize()

    def interact(self):
        self.theCommandManager.Interact()

    def isActive(self):
        self.theCommandManager.IsActive()

    def loadModel(self, aEmlFileName):
        self.theCommandManager.LoadModel(aEmlFileName)

    def refresh(self):
        self.theCommandManager.Refresh()

    def run(self, aTime = 0):
        self.theCommandManager.Run(aTime)

    def runNotSuspend(self, aTime = 0):
        self.theCommandManager.RunNotSuspend(aTime)

    def saveLoggerData(
            self,
            aFullPN,
            aSaveDirectory,
            aStartTime = 0.0,
            anEndTime = 0.0):
        self.theCommandManager.SaveLoggerData(aFullPN, aSaveDirectory, aStartTime, anEndTime)

    def saveModel(self, aModelName):
        self.theCommandManager.SaveModel(aModelName)

    def setEntityProperty(self, aFullPNString, aValue):
        self.theCommandManager.SetEntityProperty(aFullPNString, aValue)

    def step(self, num = 1):
        self.theCommandManager.Step(num)

    def stepNotSuspend(self, num = 1):
        self.theCommandManager.StepNotSuspend(num)

    def stop(self):
        self.theCommandManager.Stop()

    def updateInitialCondition(self, keyType, dic):
        self.theCommandManager.UpdateInitialCondition(keyType, dic)


class EntityStub:

    def __init__(self, aCommandManager, aFullID):
        self.theCommandManager = aCommandManager
        self.theEntity = self.theCommandManager.CreateEntityStub(aFullID)

    def create(self, aClassName):
        self.theEntity.Create(aClassName)

    def delete(self):
        self.theEntity.Delete()
        self.theEntity = None

    def exists(self):
        return self.theEntity.Exists()

    def getClassname(self):
        return self.theEntity.GetClassName()

    def getName(self):
        return self.theEntity.GetName()

    def getProperty(self, aPropertyName):
        return self.theEntity.GetProperty(aPropertyName)

    def getPropertyAttributes(self, aPropertyName):
        tuple = ()
        for value in self.theEntity.GetPropertyAttributes(aPropertyName):
            tuple = tuple + (value,)
        return tuple

    def getPropertyList(self):
        tuple = ()
        for value in self.theEntity.GetPropertyList():
            tuple = tuple + (value,)
        return tuple

    def setProperty(self, aPropertyName, aValue):
        self.theEntity.SetProperty(aPropertyName, aValue)


class LoggerStub:

    def __init__(self, aCommandManager, aFullPN):
        self.theCommandManager = aCommandManager
        self.theLogger = self.theCommandManager.CreateLoggerStub(aFullPN)

    def create(self):
        self.theLogger.Create()

    def delete(self):
        self.theLogger.Delete()

    def exists(self):
        self.theLogger.Exists()

    def getData(self, aStartTime = 0.0, anEndTime = 0.0):
        tuple = ()
        for value in self.theLogger.GetData(aStartTime, anEndTime):
            childTuple = ()
            childTuple = childTuple + (value.time,) + (value.value,)
            if value.avg != None and value.avg > 0:
                childTuple = childTuple + (value.avg,) + (value.min,) + (value.max,)
            tuple = tuple + (childTuple,)
        return tuple

    def getEndTime(self):
        return self.theLogger.GetEndTime()

    def getLoggerPolicy(self):
        loggerPolicy = self.theLogger.GetLoggerPolicy()
        return (loggerPolicy.m_reloadStepCount,) + (loggerPolicy.m_reloadInterval,) + (loggerPolicy.m_diskFullAction,) + (loggerPolicy.m_maxDiskSpace,)

    def getName(self):
        return self.theLogger.GetName()

    def getSize(self):
        return self.theLogger.GetSize()

    def getStartTime(self):
        return self.theLogger.GetStartTime()

    def setLoggerPolicy(
            self,
            aStepCount,
            aLogInterval,
            aHDDFullAction,
            aMaxHDDSpace):
        self.theLogger.SetLoggerPolicy(aStepCount, aLogInterval, aHDDFullAction, aMaxHDDSpace)


class SimulationParameterStub:

    def __init__(self, aCommandManager, aParameterID):
        self.theCommandManager = aCommandManager
        self.theSimulationParameter = self.theCommandManager.CreateSimulationParameterStub(aParameterID)

    def create(self):
        self.theSimulationParameter.Create()

    def createStepperStub(self, aStepperID):
        self.theSimulationParameter.CreateStepperStub(aStepperID)

    def delete(self):
        self.theSimulationParameter.Delete()
        self.theSimulationParameter = None

    def exists(self):
        return self.theSimulationParameter.Exists()

    def getLoggerPolicy(self):
        loggerPolicy = self.theSimulationParameter.GetLoggerPolicy()
        return (loggerPolicy.m_reloadStepCount,) + (loggerPolicy.m_reloadInterval,) + (loggerPolicy.m_diskFullAction,) + (loggerPolicy.m_maxDiskSpace,)

    def getInitialCondition(self, aType):
        tuple = ()
        initialCondition = self.theSimulationParameter.GetInitialCondition(aType)
        for key in initialCondition.Keys:
            tuple = tuple + ((key, initialCondition[key]),)
        return tuple

    def getSimulationParameterID(self):
        return self.theSimulationParameter.GetSimulationParameterID()

    def getStepperIDList(self):
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
        self.theSimulationParameter.SetLoggerPolicy(aStepCount, aLogInterval, aHDDFullAction, aMaxHDDSpace)


class StepperStub:

    def __init__(self, aCommandManager, anID):
        self.theCommandManager = aCommandManager
        self.theStepper = self.theCommandManager.CreateStepperStub(anID)

    def create(self, aClassName):
        self.theStepper.Create(aClassName)

    def delete(self):
        self.theStepper.Delete()
        self.theStepper = None

    def exists(self):
        return self.theStepper.Exists()

    def getClassname(self):
        return self.theStepper.GetClassName()

    def getName(self):
        return self.theStepper.GetName()

    def getProperty(self, aPropertyName):
        return self.theStepper.GetProperty(aPropertyName)

    def getPropertyAttributes(self, aPropertyName):
        tuple = ()
        for value in self.theStepper.GetPropertyAttributes(aPropertyName):
            tuple = tuple = (value,)
        return tuple

    def getPropertyList(self):
        tuple = ()
        for value in self.Setepper.GetPropertyList():
            tuple = tuple = (value,)
        return tuple

    def setProperty(self, aPropertyName, aValue):
        self.theStepper.SetProperty(aPropertyName, aValue)

    def getSimulationParameterID():
        return self.theStepper.GetSimulationParameterID()
