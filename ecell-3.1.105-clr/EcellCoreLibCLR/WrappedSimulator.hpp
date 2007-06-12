#include "WrappedPolymorph.hpp"
#include "WrappedDataPointVector.hpp"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Windows::Forms;

#undef GetCurrentTime

namespace EcellCoreLib
{
    class EventChecker : public libemc::EventChecker
    {
    private:
        bool m_suspendFlag;

    public:
        EventChecker()
        {
            this -> m_suspendFlag = false;
        }

        bool GetSuspendFlag()
        {
            return this -> m_suspendFlag;
        }

        virtual bool operator()(void) const
        {
            Application::DoEvents();
            return this -> m_suspendFlag;
        }

        void SetSuspendFlag(bool l_flag)
        {
            this -> m_suspendFlag = l_flag;
        }
    };

    class EventHandler : public libemc::EventHandler
    {
    public:
        virtual void operator()(void) const
        {
        }
        bool get()
        {
            return true;
        }
    };

    public ref class WrappedSimulator
    {
    private:
        libemc::Simulator * m_simulator;
        std::vector<libecs::Polymorph> ExchangeType(ArrayList ^ l_arrayList);
        EventChecker * m_eventChecker;
    public:
        static int s_flagSettable = 0;
        static int s_flagGettable = 1;
        static int s_flagLoadable = 2;
        static int s_flagSavable = 3;
        WrappedSimulator();
        WrappedSimulator(String ^ l_dmPath);
        void CreateEntity(String ^ l_classname, String ^ l_fullIDString);
        void CreateLogger(String ^ l_fullPNString);
        void CreateLogger(String ^ l_fullPNString, WrappedPolymorph ^ l_paramList);
        void CreateStepper(String ^ l_classname, String ^ l_ID);
        void DeleteEntity(String ^ l_fullIDString);
        void DeleteStepper(String ^ l_ID);
        Double GetCurrentTime();
        WrappedPolymorph ^ GetDMInfo();
        WrappedPolymorph ^ GetEntityList(String ^ l_entityTypeString, String ^ l_systemPathString);
        WrappedPolymorph ^ GetEntityProperty(String ^ l_fullPNString);
        List<bool> ^ GetEntityPropertyAttributes(String ^ l_fullPNString);
        WrappedPolymorph ^ GetEntityPropertyList(String ^ l_fullIDString);
        WrappedDataPointVector ^ GetLoggerData(String ^ l_fullPNString);
        WrappedDataPointVector ^ GetLoggerData(String ^ l_fullPNString, Double l_startTime, Double l_endTime);
        WrappedDataPointVector
                ^ GetLoggerData(String ^ l_fullPNString, Double l_startTime, Double l_endTime, Double l_interval);
        Double GetLoggerEndTime(String ^ aFullPNString);
        WrappedPolymorph ^ GetLoggerList();
        Double GetLoggerMinimumInterval(String ^ aFullPNString);
        Int32 GetLoggerSize(String ^ aFullPNString);
        Double GetLoggerStartTime(String ^ aFullPNString);
        WrappedPolymorph ^ GetNextEvent();
        String ^ GetStepperClassName(String ^ aStepperID);
        WrappedPolymorph ^ GetStepperList();
        WrappedPolymorph ^ GetStepperProperty(String ^ aStepperID, String ^ aPropertyName);
        List<bool> ^ GetStepperPropertyAttributes(String ^ l_stepperID, String ^ l_propertyName);
        WrappedPolymorph ^ GetStepperPropertyList(String ^ aStepperID);
        void Initialize();
        void LoadEntityProperty(String ^ l_fullPNString, ArrayList ^ l_arrayList);
        void LoadEntityProperty(String ^ l_fullPNString, WrappedPolymorph ^ l_wrappedPolymorph);
        void LoadStepperProperty(String ^ l_stepperID, String ^ l_propertyName, ArrayList ^ l_arrayList);
        void LoadStepperProperty(String ^ l_stepperID, String ^ l_propertyName, WrappedPolymorph ^ l_wrappedPolymorph);
        void Run();
        void Run(Double aDuration);
        void SetEntityProperty(String ^ l_fullPNString, ArrayList ^ l_arrayList);
        void SetEntityProperty(String ^ l_fullPNString, WrappedPolymorph ^ l_value);
        void SetStepperProperty(String ^ l_stepperID, String ^ l_propertyName, ArrayList ^ l_arrayList);
        void SetStepperProperty(String ^ aStepperID, String ^ aPropertyName, WrappedPolymorph ^ aValue);
        void Step(Int32 aNumSteps);
        void Stop();
        void Suspend();
        /*
        WrappedPolymorph ^ saveStepperProperty( String ^ aStepperID, String ^ aPropertyName );
        String ^ getEntityClassName( String ^ aFullIDString );
        Boolean isEntityExist( String ^ aFullIDString );
        WrappedPolymorph ^ saveEntityProperty( String ^ aFullPNString );
        WrappedPolymorph ^ getLoggerPolicy( String ^ aFullPNString );
        void setLoggerMinimumInterval( String ^ aFullPNString, Double anInterval );
        void setLoggerPolicy( String ^ aFullPNString, WrappedPolymorph ^ aParamList );
        void setEventChecker( WrappedEventChecker ^ aEventChecker );
        void setEventHandler( WrappedEventHandler ^ anEventHandler );
         */
    };
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
