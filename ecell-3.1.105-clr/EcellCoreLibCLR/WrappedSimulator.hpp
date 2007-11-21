#include "libecs/libecs.hpp"
#include "WrappedPolymorph.hpp"
#include "WrappedDataPointVector.hpp"

#undef GetCurrentTime

namespace EcellCoreLib
{
	using namespace System;
	using namespace System::Collections::Generic;
	using namespace System::Windows::Forms;
	using namespace System::Runtime::InteropServices;

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

	public ref class WrappedException: public ApplicationException {};

	public ref class WrappedLibecsException: public WrappedException
	{
	private:
		String^ m_className;
		String^ m_message;
	public:
		WrappedLibecsException(const libecs::Exception& msg)
			: m_className(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>((msg.getClassName())))),
  			  m_message(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(msg.message().c_str())))
		{
		}

		property String^ Message
		{
		public:
			virtual String^ get() new
			{
				return m_message;
			}
		}

		property String^ Source
		{
		public:
			virtual String^ get() new
			{
				return m_className;
			}
		}
	};

	public ref class WrappedStdException: public WrappedException
	{
	private:
		String^ m_message;
		String^ m_className;

	public:
		WrappedStdException(const std::exception& e)
			: m_message(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(e.what()))),
  			  m_className(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(typeid(e).name())))
		{
		}


		property String^ Message
		{
		public:
			virtual String^ get() new
			{
				return m_message;
			}
		}

		property String^ Source
		{
		public:
			virtual String^ get() new
			{
				return m_className;
			}
		}
	};

    public ref class WrappedSimulator
    {
    private:
        libemc::Simulator* m_simulator;
		libemc::EventCheckerSharedPtr* m_eventChecker;
		libemc::EventHandlerSharedPtr* m_eventHandler;

	private:
		std::vector<libecs::Polymorph> ExchangeType(System::Collections::IEnumerable^ l_list);

	public:
        static int s_flagSettable = 0;
        static int s_flagGettable = 1;
        static int s_flagLoadable = 2;
        static int s_flagSavable = 3;
        WrappedSimulator(array<String^> ^ l_dmPath);
        ~WrappedSimulator();
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
		void LoadEntityProperty(String ^ l_fullPNString, System::Collections::IEnumerable^ l_list);
		void LoadEntityProperty(String ^ l_fullPNString, WrappedPolymorph^ l_wrappedPolymorph);
		void LoadStepperProperty(String ^ l_stepperID, String ^ l_propertyName, System::Collections::IEnumerable^ l_list);
        void LoadStepperProperty(String ^ l_stepperID, String ^ l_propertyName, WrappedPolymorph^ l_wrappedPolymorph);
        void Run();
        void Run(Double aDuration);
		void SetEntityProperty(String ^ l_fullPNString, System::Collections::IEnumerable^ l_list);
        void SetEntityProperty(String ^ l_fullPNString, WrappedPolymorph^ l_value);
		void SetStepperProperty(String ^ l_stepperID, String ^ l_propertyName, System::Collections::IEnumerable^ l_list);
        void SetStepperProperty(String ^ aStepperID, String ^ aPropertyName, WrappedPolymorph^ aValue);
        void Step(Int32 aNumSteps);
        void Stop();
        void Suspend();
    };
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
