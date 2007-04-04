#include <iostream>
#include "libecs/VVector.h"
#include "libemc/Simulator.hpp"
#include "WrappedSimulator.hpp"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;


namespace EcellCoreLib {
    WrappedSimulator::WrappedSimulator()
    {
        m_simulator = new libemc::Simulator();
    }

    void WrappedSimulator::CreateEntity(String ^ l_className, String ^ l_fullIDString)
    {
        try
        {
            std::string l_stdClassName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_className);
            std::string l_stdFullIDString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullIDString);
            WrappedSimulator::m_simulator -> createEntity(l_stdClassName, l_stdFullIDString);
        }
        catch(libecs::Exception l_ex)
        {
            throw gcnew Exception(
                "Can't create the \"" + l_fullIDString + "\" entity of the \"" + l_className + "\" class. [" +
                gcnew String(l_ex.message().c_str()) + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't create the \"" + l_fullIDString + "\" entity of the \"" + l_className + "\" class. [" +
                l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::CreateLogger(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            WrappedSimulator::m_simulator -> createLogger(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't create the \"" + l_fullPNString + "\" logger. [" + l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::CreateLogger(String ^ l_fullPNString, WrappedPolymorph ^ l_paramList)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            libecs::Polymorph * l_polymorph = l_paramList -> GetPolymorph();
            WrappedSimulator::m_simulator -> createLogger(l_stdFullPNString, * l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't create the \"" + l_fullPNString + "\" logger. [" + l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::CreateStepper(String ^ l_className, String ^ l_ID)
    {
        try
        {
            std::string l_stdClassName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_className);
            std::string l_stdID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_ID);
            WrappedSimulator::m_simulator -> createStepper(l_stdClassName, l_stdID);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't create the \"" + l_ID + "\" stepper of the \"" + l_className + "\" class. [" +
                l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::DeleteEntity(String ^ l_fullIDString)
    {
        try
        {
            std::string l_stdFullIDString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullIDString);
            WrappedSimulator::m_simulator -> deleteEntity(l_stdFullIDString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't delete the \"" + l_fullIDString + "\" entity. [" + l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::DeleteStepper(String ^ l_ID)
    {
        try
        {
            std::string l_stdID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_ID);
            WrappedSimulator::m_simulator -> deleteStepper(l_stdID);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't delete the \"" + l_ID + "\" stepper. [" + l_ex -> ToString() + "]");
        }
    }

    std::vector<libecs::Polymorph> WrappedSimulator::ExchangeType(ArrayList ^ l_arrayList)
    {
        try
        {
            std::vector<libecs::Polymorph> l_vector;
            for(int i = 0; i < l_arrayList -> Count; i++)
            {
                if((l_arrayList[i] -> GetType()) -> Equals(ArrayList::typeid))
                {
                    std::vector<libecs::Polymorph> l_subVector
                            = WrappedSimulator::ExchangeType((ArrayList ^)l_arrayList[i]);
                    l_vector.push_back(libecs::Polymorph(l_subVector));
                }
                else
                {
                    std::string l_value = (char *)(void *)Marshal::StringToHGlobalAnsi((String ^)l_arrayList[i]);
                    l_vector.push_back(libecs::Polymorph(l_value));
                }
            }
            return l_vector;
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't exchange the \"Arraylist\" for the \"PolymorphVector\". [" + l_ex -> ToString() + "]");
        }
    }

    Double WrappedSimulator::GetCurrentTime()
    {
        try
        {
            return WrappedSimulator::m_simulator -> getCurrentTime();
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the current simulation time. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetDMInfo()
    {
        try
        {
            libecs::Polymorph libecsPolymorph = WrappedSimulator::m_simulator -> getDMInfo();
            return gcnew WrappedPolymorph( & libecsPolymorph );
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the DB information. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetEntityList(String ^ l_entityTypeString, String ^ l_systemPathString)
    {
        try
        {
            std::string l_stdEntityTypeString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_entityTypeString);
            std::string l_stdSystemPathString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_systemPathString);
            libecs::Polymorph l_polymorph
                    = WrappedSimulator::m_simulator -> getEntityList(l_stdEntityTypeString, l_stdSystemPathString);
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_entityTypeString + "\" entity list of the \"" +
                l_systemPathString + "\" system. [" +
                l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetEntityProperty(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            libecs::Polymorph l_polymorph = WrappedSimulator::m_simulator -> getEntityProperty(l_stdFullPNString);
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_fullPNString + "\" entity property. [" + l_ex -> ToString() + "]");
        }
    }

    List<bool> ^ WrappedSimulator::GetEntityPropertyAttributes(String ^ l_fullPNString)
    {
        try
        {
            List<bool> ^ l_list = gcnew List<bool>();
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            std::vector<libecs::Polymorph> l_vector
                    = (std::vector<libecs::Polymorph>)
                            (WrappedSimulator::m_simulator -> getEntityPropertyAttributes(l_stdFullPNString));
            if(l_vector[WrappedSimulator::s_flagSettable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagGettable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagLoadable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagSavable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            return l_list;
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the attributes of the \"" + l_fullPNString + "\" entity property. [" +
                l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetEntityPropertyList(String ^ l_fullIDString)
    {
        try
        {
            std::string l_stdFullIDString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullIDString);
            libecs::Polymorph l_polymorph = WrappedSimulator::m_simulator -> getEntityPropertyList(l_stdFullIDString);
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_fullIDString + "\" entity property list. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedDataPointVector ^ WrappedSimulator::GetLoggerData(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            boost::shared_ptr<libecs::DataPointVector> l_dataPointVector
                    = WrappedSimulator::m_simulator -> getLoggerData(l_stdFullPNString);
            return gcnew WrappedDataPointVector(& l_dataPointVector);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_fullPNString + "\" logger data. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedDataPointVector ^ WrappedSimulator::GetLoggerData(
            String ^ l_fullPNString,
            Double l_startTime,
            Double l_endTime)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            boost::shared_ptr<libecs::DataPointVector> l_dataPointVector
                    = WrappedSimulator::m_simulator -> getLoggerData(l_stdFullPNString, l_startTime, l_endTime);
            return gcnew WrappedDataPointVector(& l_dataPointVector);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_fullPNString + "\" logger data. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedDataPointVector ^ WrappedSimulator::GetLoggerData(
            String ^ l_fullPNString,
            Double l_startTime,
            Double l_endTime,
            Double l_interval)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            boost::shared_ptr<libecs::DataPointVector> l_dataPointVector
                    = WrappedSimulator::m_simulator -> getLoggerData(l_stdFullPNString, l_startTime, l_endTime, l_interval);
            return gcnew WrappedDataPointVector(& l_dataPointVector);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_fullPNString + "\" logger data. [" + l_ex -> ToString() + "]");
        }
    }

    Double WrappedSimulator::GetLoggerEndTime(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return WrappedSimulator::m_simulator -> getLoggerEndTime(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the end time of the \"" + l_fullPNString + "\" logger. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetLoggerList()
    {
        try
        {
            libecs::Polymorph l_polymorph = WrappedSimulator::m_simulator -> getLoggerList();
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the logger list. [" + l_ex -> ToString() + "]");
        }
    }

    Double WrappedSimulator::GetLoggerMinimumInterval(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return WrappedSimulator::m_simulator -> getLoggerMinimumInterval(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the minimum interval of the \"" + l_fullPNString + "\" logger. [" +
                l_ex -> ToString() + "]");
        }
    }

    Int32 WrappedSimulator::GetLoggerSize(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return WrappedSimulator::m_simulator -> getLoggerSize(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the size of the \"" + l_fullPNString + "\" logger. [" +
                l_ex -> ToString() + "]");
        }
    }

    Double WrappedSimulator::GetLoggerStartTime(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return WrappedSimulator::m_simulator -> getLoggerStartTime(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the start time of the \"" + l_fullPNString + "\" logger. [" +
                l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetNextEvent()
    {
        try
        {
            libecs::Polymorph l_polymorph = WrappedSimulator::m_simulator -> getNextEvent();
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't obtain a next event. [" + l_ex -> ToString() + "]");
        }
    }

    String ^ WrappedSimulator::GetStepperClassName(String ^ l_stepperID)
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdClassName = WrappedSimulator::m_simulator -> getStepperClassName(l_stdStepperID);
            return gcnew String(l_stdClassName.c_str());
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the name of the \"" + l_stepperID + "\" stepper. [" +
                l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetStepperList()
    {
        try
        {
            libecs::Polymorph l_polymorph = WrappedSimulator::m_simulator -> getStepperList();
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the stepper list. [" + l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetStepperProperty(String ^ l_stepperID, String ^ l_propertyName)
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdPropertyName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_propertyName);
            libecs::Polymorph l_polymorph = WrappedSimulator::m_simulator -> getStepperProperty(l_stdStepperID, l_stdPropertyName);
            return gcnew WrappedPolymorph(& l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_propertyName + "\" property of the \"" + l_stepperID + "\" stepper. [" +
                l_ex -> ToString() + "]");
        }
    }

    List<bool> ^ WrappedSimulator::GetStepperPropertyAttributes(String ^ l_stepperID, String ^ l_propertyName)
    {
        try
        {
            List<bool> ^ l_list = gcnew List<bool>();
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdPropertyName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_propertyName);
            std::vector<libecs::Polymorph> l_vector = (std::vector<libecs::Polymorph>)(WrappedSimulator::m_simulator -> getStepperPropertyAttributes(l_stdStepperID, l_stdPropertyName));
            if(l_vector[WrappedSimulator::s_flagSettable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagGettable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagLoadable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagSavable].asInteger() == 0)
            {
                l_list -> Add(false);
            }
            else
            {
                l_list -> Add(true);
            }
            return l_list;
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the \"" + l_propertyName + "\" attribute of the \"" + l_stepperID + "\" stepper. [" +
                l_ex -> ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetStepperPropertyList(String ^ l_stepperID)
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            libecs::Polymorph l_libecsPolymorph =
                WrappedSimulator::m_simulator -> getStepperPropertyList(l_stdStepperID);
            return gcnew WrappedPolymorph(& l_libecsPolymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the property list of the \"" + l_stepperID + "\" stepper. [" +
                l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::Initialize()
    {
        try
        {
            // WrappedSimulator::m_simulator -> initialize();
            // WrappedSimulator::m_simulator -> getEntityProperty("System::/:Size");
            WrappedSimulator::m_simulator -> saveEntityProperty("System::/:Name");
        }
        catch (libecs::Exception l_ex)
        {
            throw gcnew Exception ("Can't initialize the simulator. [" + gcnew String(l_ex.message().c_str()) + "]");
        }
        catch (Exception ^ l_ex)
        {
            throw gcnew Exception ("Can't initialize the simulator. [" + l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::LoadEntityProperty(String ^ l_fullPNString, ArrayList ^ l_arrayList)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            std::vector<libecs::Polymorph> l_vector = WrappedSimulator::ExchangeType(l_arrayList);
            WrappedSimulator::m_simulator -> loadEntityProperty(l_stdFullPNString, l_vector);
        }
        catch(boost::bad_lexical_cast l_ex)
        {
            throw gcnew Exception(
                "Can't load the \"" + l_fullPNString + "\" entity property. [" +
                gcnew String(l_ex.what()) + "]");
        }
        catch(libecs::Exception l_ex)
        {
            throw gcnew Exception(
                "Can't load the \"" + l_fullPNString + "\" entity property. [" +
                gcnew String(l_ex.message().c_str()) + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't load the \"" + l_fullPNString + "\" entity property. [" +
                l_ex -> ToString() + "]");
        }
    }

    void WrappedSimulator::LoadEntityProperty(String ^ l_fullPNString, WrappedPolymorph ^ l_wrappedPolymorph)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            WrappedSimulator::m_simulator ->
                    loadEntityProperty(l_stdFullPNString, * (l_wrappedPolymorph -> GetPolymorph()));
        }
        catch(boost::bad_lexical_cast l_ex)
        {
            throw gcnew Exception(
                "Can't load the \"" + l_fullPNString + "\" entity property. [" +
                gcnew String(l_ex.what()) + "]");
        }
        catch(libecs::Exception l_ex)
        {
            throw gcnew Exception(
                "Can't load the \"" + l_fullPNString + "\" entity property. [" +
                gcnew String(l_ex.message().c_str()) + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't load the \"" + l_fullPNString + "\" entity property. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::LoadStepperProperty(String ^ l_stepperID, String ^ l_propertyName, ArrayList ^ l_arrayList)
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdPropertyName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_propertyName);
            std::vector<libecs::Polymorph> l_vector = WrappedSimulator::ExchangeType(l_arrayList);
            WrappedSimulator::m_simulator -> loadStepperProperty(l_stdStepperID, l_stdPropertyName, l_vector);
        }
        catch(boost::bad_lexical_cast l_ex)
        {
            throw gcnew Exception("Can't load the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + gcnew String(l_ex.what()) + "]");
        }
        catch(libecs::Exception l_ex)
        {
            throw gcnew Exception("Can't load the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + gcnew String(l_ex.message().c_str()) + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't load the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::LoadStepperProperty(
        String ^ l_stepperID, String ^ l_propertyName, WrappedPolymorph ^ l_wrappedPolymorph)
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdPropertyName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_propertyName);
            WrappedSimulator::m_simulator ->
                loadStepperProperty(l_stdStepperID, l_stdPropertyName, * (l_wrappedPolymorph -> GetPolymorph()));
        }
        catch(boost::bad_lexical_cast l_ex)
        {
            throw gcnew Exception("Can't load the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + gcnew String(l_ex.what()) + "]");
        }
        catch(libecs::Exception l_ex)
        {
            throw gcnew Exception("Can't load the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + gcnew String(l_ex.message().c_str()) + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't load the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::Run()
    {
        try
        {
            this -> m_eventChecker = new EventChecker();
            WrappedSimulator::m_simulator -> setEventChecker(libemc::EventCheckerSharedPtr(this -> m_eventChecker));
            WrappedSimulator::m_simulator -> setEventHandler(libemc::EventHandlerSharedPtr(new EventHandler()));
            WrappedSimulator::m_simulator -> run();
        }
        catch(vvector_full l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_write_error l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_read_error l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_init_error l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't execute the simulator. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::Run(Double aDuration)
    {
        try
        {
            if (this -> m_eventChecker != nullptr && this -> m_eventChecker -> GetSuspendFlag())
            {
                this -> m_eventChecker -> SetSuspendFlag(false);
            }
            else
            {
                this -> m_eventChecker = new EventChecker();
                WrappedSimulator::m_simulator -> setEventChecker(libemc::EventCheckerSharedPtr(this -> m_eventChecker));
                WrappedSimulator::m_simulator -> setEventHandler(libemc::EventHandlerSharedPtr(new EventHandler()));
                WrappedSimulator::m_simulator -> run(aDuration);
            }
        }
        catch(vvector_full l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_write_error l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_read_error l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_init_error l_ex)
        {
            throw gcnew Exception(
                "Can't execute the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't execute the simulator. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::SetEntityProperty(String ^ l_fullPNString, ArrayList ^ l_arrayList)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            std::vector<libecs::Polymorph> l_vector = WrappedSimulator::ExchangeType(l_arrayList);
            WrappedSimulator::m_simulator -> setEntityProperty(l_stdFullPNString, l_vector );
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't set the \"" + l_fullPNString + "\" entity property. [" +
                l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::SetEntityProperty(String ^ l_fullPNString, WrappedPolymorph ^ l_value)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            libecs::Polymorph * l_polymorph = l_value -> GetPolymorph();
            WrappedSimulator::m_simulator -> setEntityProperty(l_stdFullPNString, * l_polymorph);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't set the \"" + l_fullPNString + "\" entity property. [" +
                l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::SetStepperProperty(String ^ l_stepperID, String ^ l_propertyName, ArrayList ^ l_arrayList )
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdPropertyName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_propertyName);
            std::vector<libecs::Polymorph> l_vector = WrappedSimulator::ExchangeType(l_arrayList);
            WrappedSimulator::m_simulator -> setStepperProperty(l_stdStepperID, l_stdPropertyName, l_vector);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't set the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::SetStepperProperty(
            String ^ l_stepperID,
            String ^ l_propertyName,
            WrappedPolymorph ^ l_value )
    {
        try
        {
            std::string l_stdStepperID = (char *)(void *)Marshal::StringToHGlobalAnsi(l_stepperID);
            std::string l_stdPropertyName = (char *)(void *)Marshal::StringToHGlobalAnsi(l_propertyName);
            libecs::Polymorph * l_libecsPolymorph = l_value -> GetPolymorph();
            WrappedSimulator::m_simulator -> setStepperProperty(l_stdStepperID, l_stdPropertyName, * l_libecsPolymorph );
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't set the \"" + l_propertyName + "\" stepper property of the \"" +
                l_stepperID + "\" stepper. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::Step(Int32 l_numSteps)
    {
        try
        {
            WrappedSimulator::m_simulator -> step(l_numSteps);
        }
        catch(vvector_full l_ex)
        {
            throw gcnew Exception(
                "Can't step the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_write_error l_ex)
        {
            throw gcnew Exception(
                "Can't step the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_read_error l_ex)
        {
            throw gcnew Exception(
                "Can't step the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(vvector_init_error l_ex)
        {
            throw gcnew Exception(
                "Can't step the simulator. [" + (gcnew String(l_ex.what()))->Replace("\n", "") + "]");
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't step the simulator. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::Stop()
    {
        try
        {
            WrappedSimulator::m_simulator -> stop();
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception("Can't stop the simulator. [" + l_ex->ToString() + "]");
        }
    }

    void WrappedSimulator::Suspend()
    {
        if (WrappedSimulator::m_eventChecker == nullptr)
        {
            return;
        }
        WrappedSimulator::m_eventChecker -> SetSuspendFlag(true);
        /*
        if (WrappedSimulator::m_eventChecker -> GetSuspendFlag())
        {
            WrappedSimulator::m_eventChecker -> SetSuspendFlag(false);
        }
        else
        {
            WrappedSimulator::m_eventChecker -> SetSuspendFlag(true);
        }
         */
    }

    /*
    WrappedPolymorph ^ WrappedSimulator::saveStepperProperty( String ^ aStepperID, String ^ aPropertyName ) {
        std::string stdStepperID = ( char* )( void * )Marshal::StringToHGlobalAnsi( aStepperID );
        std::string stdPropertyName = ( char* )( void * )Marshal::StringToHGlobalAnsi( aPropertyName );
        libecs::Polymorph aPolymorph = WrappedSimulator::m_simulator -> saveStepperProperty( stdStepperID, stdPropertyName );
        return gcnew WrappedPolymorph( & aPolymorph );
    }

    String ^ WrappedSimulator::getEntityClassName( String ^ aFullIDString ) {
        std::string stdFullIDString = ( char* )( void * )Marshal::StringToHGlobalAnsi( aFullIDString );
        std::string stdClassName = WrappedSimulator::m_simulator -> getEntityClassName( stdFullIDString );
        return gcnew String( stdClassName.c_str() );
    }

    Boolean WrappedSimulator::isEntityExist( String ^ aFullIDString ) {
        std::string stdFullIDString = ( char* )( void * )Marshal::StringToHGlobalAnsi( aFullIDString );
        return WrappedSimulator::m_simulator -> isEntityExist( stdFullIDString );
    }

    WrappedPolymorph ^ WrappedSimulator::saveEntityProperty( String ^ aFullPNString ) {
        std::string stdFullPNString = ( char* )( void * )Marshal::StringToHGlobalAnsi( aFullPNString );
        libecs::Polymorph libecsPolymorph = WrappedSimulator::m_simulator -> saveEntityProperty( stdFullPNString );
        return gcnew WrappedPolymorph( & libecsPolymorph );
    }

    WrappedPolymorph ^ WrappedSimulator::getLoggerPolicy( String ^ aFullPNString ) {
        std::string stdFullPNString = ( char* )( void * )Marshal::StringToHGlobalAnsi( aFullPNString );
        libecs::Polymorph libecsPolymorph = WrappedSimulator::m_simulator -> getLoggerPolicy( stdFullPNString );
        return gcnew WrappedPolymorph( & libecsPolymorph );
    }

    void WrappedSimulator::setLoggerMinimumInterval( String ^ aFullPNString, Double anInterval ) {
        std::string stdFullPNString = ( char* )( void * )Marshal::StringToHGlobalAnsi( aFullPNString );
        WrappedSimulator::m_simulator -> setLoggerMinimumInterval( stdFullPNString, anInterval );
    }

    void WrappedSimulator::setLoggerPolicy( String ^ aFullPNString, WrappedPolymorph ^ aParamList ) {
        std::string stdFullPNString = ( char* )( void * )Marshal::StringToHGlobalAnsi( aFullPNString );
        libecs::Polymorph * aPolymorph = aParamList -> GetPolymorph();
        WrappedSimulator::m_simulator -> setLoggerPolicy( stdFullPNString, * aPolymorph );
    }


    WrappedPolymorph ^ WrappedSimulator::getNextEvent() {
        libecs::Polymorph libecsPolymorph = WrappedSimulator::m_simulator -> getNextEvent();
        return gcnew WrappedPolymorph( & libecsPolymorph );
    }

    void WrappedSimulator::setEventChecker( WrappedEventChecker ^  aEventChecker ) {
        boost::shared_ptr< libemc::EventChecker > * libemcEventChecker = aEventChecker ->getLibemcEventChecker();
        WrappedSimulator::m_simulator -> setEventChecker( * libemcEventChecker );
    }

    void WrappedSimulator::setEventHandler( WrappedEventHandler ^ anEventHandler ) {
        boost::shared_ptr< libemc::EventHandler > * libemcEventHandler = anEventHandler ->getLibemcEventHandler();
        WrappedSimulator::m_simulator -> setEventHandler( * libemcEventHandler );
    }

     */
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
