#include "boost/shared_array.hpp"

#include "libemc/Simulator.hpp"
#include "WrappedSimulator.hpp"

using namespace System;
using namespace System::IO;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;


namespace EcellCoreLib {
	static void freeHGlobalBuffer(void *ptr)
	{
		Marshal::FreeHGlobal((IntPtr)ptr);
	}

	class WrappedCString: public boost::shared_array<char>
	{
	public:
		inline WrappedCString(const WrappedCString& that)
			: boost::shared_array<char>(that)
		{
		}

		inline WrappedCString(String^ str)
			: boost::shared_array<char>(
				reinterpret_cast<char*>(
					(void*)Marshal::StringToHGlobalAnsi(str)),
				reinterpret_cast<void(*)(char*)>(&freeHGlobalBuffer))
		{
		}

		inline operator std::string() const
		{
			return std::string(get());
		}
	};

    WrappedSimulator::WrappedSimulator(array<String^>^ l_dmPaths)
		: m_eventHandler(new libemc::EventHandlerSharedPtr(new EventHandler())),
	      m_eventChecker(new libemc::EventCheckerSharedPtr(new EventChecker()))
    {
        try
        {
            libecs::initialize();
        }
        catch(libecs::Exception l_ex)
        {
            throw gcnew Exception("Failed to initialize \"libecs\".");
        }
		if (l_dmPaths != nullptr)
		{
			libecs::setDMSearchPath(
				WrappedCString(String::Join(
						Path::PathSeparator.ToString(), l_dmPaths)));
		}
        m_simulator = new libemc::Simulator();
		m_simulator->setEventHandler(libemc::EventHandlerSharedPtr(new EventHandler()));
		m_simulator->setEventChecker(libemc::EventCheckerSharedPtr(new EventChecker()));
    }

    WrappedSimulator::~WrappedSimulator()
    {
		delete m_simulator;
		delete m_eventHandler;
		delete m_eventChecker;
        libecs::finalize();
    }

    void WrappedSimulator::CreateEntity(String^ l_className, String^ l_fullIDString)
    {
		try
		{
            m_simulator->createEntity(
				WrappedCString(l_className),
				WrappedCString(l_fullIDString));
		}
		catch (const libecs::Exception& l_ex)
		{
			throw gcnew WrappedLibecsException(l_ex);
        }
		catch (const std::exception& e)
		{
			throw gcnew WrappedStdException(e);
		}
    }

    void WrappedSimulator::CreateLogger(String^ l_fullPNString)
    {
        try
        {
            m_simulator->createLogger(WrappedCString(l_fullPNString));
        }
		catch (const libecs::Exception& l_ex)
		{
			throw gcnew WrappedLibecsException(l_ex);
		}
    }

    void WrappedSimulator::CreateLogger(String^ l_fullPNString, WrappedPolymorph^ l_paramList)
    {
        try
        {
            m_simulator->createLogger(
					WrappedCString(l_fullPNString),
					l_paramList->GetPolymorph());
        }
		catch(const libecs::Exception& l_ex)
        {
			throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::CreateStepper(String ^ l_className, String ^ l_ID)
    {
		try
		{
            m_simulator->createStepper(
					WrappedCString(l_className),
					WrappedCString(l_ID));
		}
		catch (const libecs::Exception& e)
		{
			throw gcnew WrappedLibecsException(e);
		}
		catch (const std::exception& e)
		{
			throw gcnew WrappedStdException(e);
		}
    }

    void WrappedSimulator::DeleteEntity(String^ l_fullIDString)
    {
        try
        {
            m_simulator->deleteEntity(WrappedCString(l_fullIDString));
        }
		catch (const libecs::Exception& e)
		{
			throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::DeleteStepper(String^ l_ID)
    {
        try
        {
            m_simulator->deleteStepper(WrappedCString(l_ID));
        }
		catch (const libecs::Exception& e)
		{
			throw gcnew WrappedLibecsException(e);
        }
	}

	libecs::PolymorphVector WrappedSimulator::ExchangeType(System::Collections::IEnumerable^ l_list)
    {
        try
        {
			libecs::PolymorphVector l_vector;
			for (System::Collections::IEnumerator^ i = l_list->GetEnumerator(); i->MoveNext(); delete i)
            {
				System::Object^ elem = i->Current;
				if (String::typeid->IsInstanceOfType(elem))
                {
                    l_vector.push_back(libecs::Polymorph(WrappedCString(elem->ToString())));
                }
				else if (System::Collections::IEnumerable::typeid->IsInstanceOfType(elem))
                {
                    l_vector.push_back(
							libecs::Polymorph(
								WrappedSimulator::ExchangeType(
									(System::Collections::IEnumerable^)elem)));
                }
			}
            return l_vector;
		}
		catch (Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't exchange the \"System::Collections::IEnumerable\" for the \"PolymorphVector\". [" + l_ex->ToString() + "]");
        }
    }

    Double WrappedSimulator::GetCurrentTime()
    {
        try
        {
            return m_simulator->getCurrentTime();
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
		}
    }

    WrappedPolymorph^ WrappedSimulator::GetDMInfo()
    {
        try
        {
            return gcnew WrappedPolymorph(m_simulator->getDMInfo());
        }
		catch (const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    WrappedPolymorph^ WrappedSimulator::GetEntityList(String^ l_entityTypeString, String^ l_systemPathString)
    {
        try
        {
            return gcnew WrappedPolymorph(
					m_simulator->getEntityList(
						WrappedCString(l_entityTypeString),
						WrappedCString(l_systemPathString)));
        }
		catch (const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    WrappedPolymorph^ WrappedSimulator::GetEntityProperty(String^ l_fullPNString)
    {
        try
        {
            return gcnew WrappedPolymorph(
					m_simulator->getEntityProperty(WrappedCString(l_fullPNString)));
        }
		catch(const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    List<bool>^ WrappedSimulator::GetEntityPropertyAttributes(String^ l_fullPNString)
    {
        try
        {
            List<bool> ^ l_list = gcnew List<bool>();
            libecs::PolymorphVector l_vector(
					m_simulator->getEntityPropertyAttributes(
						WrappedCString(l_fullPNString)));
            if(l_vector[WrappedSimulator::s_flagSettable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagGettable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagLoadable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagSavable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            return l_list;
        }
		catch(const libecs::Exception e)
		{
			throw gcnew WrappedLibecsException(e);
        }
    }

    WrappedPolymorph^ WrappedSimulator::GetEntityPropertyList(String ^ l_fullIDString)
    {
        try
        {
            return gcnew WrappedPolymorph(
					 m_simulator->getEntityPropertyList(WrappedCString(l_fullIDString)));
        }
		catch(const libecs::Exception e)
		{
			throw gcnew WrappedLibecsException(e);
        }
	}

    WrappedDataPointVector^ WrappedSimulator::GetLoggerData(String ^ l_fullPNString)
    {
        try
        {
            return gcnew WrappedDataPointVector(
					m_simulator->getLoggerData(WrappedCString(l_fullPNString)));
        }
		catch(const libecs::Exception e)
		{
			throw gcnew WrappedLibecsException(e);
        }
	}

    WrappedDataPointVector^ WrappedSimulator::GetLoggerData(
            String ^ l_fullPNString,
            Double l_startTime,
            Double l_endTime)
    {
        try
        {
            return gcnew WrappedDataPointVector(m_simulator->getLoggerData(
					WrappedCString(l_fullPNString), l_startTime, l_endTime));
        }
		catch(const libecs::Exception e)
		{
			throw gcnew WrappedLibecsException(e);
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
            return gcnew WrappedDataPointVector(
					m_simulator->getLoggerData(
						WrappedCString(l_fullPNString),
						l_startTime, l_endTime, l_interval));
        }
		catch(const libecs::Exception e)
		{
			throw gcnew WrappedLibecsException(e);
        }
	}

    Double WrappedSimulator::GetLoggerEndTime(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return m_simulator->getLoggerEndTime(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the end time of the \"" + l_fullPNString + "\" logger. [" + l_ex->ToString() + "]");
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetLoggerList()
    {
        try
        {
            return gcnew WrappedPolymorph(m_simulator->getLoggerList());
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the logger list. [" + l_ex->ToString() + "]");
        }
    }

    Double WrappedSimulator::GetLoggerMinimumInterval(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return m_simulator->getLoggerMinimumInterval(l_stdFullPNString);
        }
        catch(Exception ^ l_ex)
        {
            throw gcnew Exception(
                "Can't obtain the minimum interval of the \"" + l_fullPNString + "\" logger. [" +
                l_ex->ToString() + "]");
        }
    }

    Int32 WrappedSimulator::GetLoggerSize(String ^ l_fullPNString)
    {
        try
        {
            std::string l_stdFullPNString = (char *)(void *)Marshal::StringToHGlobalAnsi(l_fullPNString);
            return m_simulator->getLoggerSize(l_stdFullPNString);
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    Double WrappedSimulator::GetLoggerStartTime(String ^ l_fullPNString)
    {
        try
		{
            return m_simulator->getLoggerStartTime(WrappedCString(l_fullPNString));
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetNextEvent()
    {
        try
        {
            return gcnew WrappedPolymorph(m_simulator->getNextEvent());
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    String ^ WrappedSimulator::GetStepperClassName(String ^ l_stepperID)
    {
        try
        {
			return Marshal::PtrToStringAnsi(
					(IntPtr)const_cast<char*>(
						m_simulator->getStepperClassName(
							WrappedCString(l_stepperID)).c_str()));
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
	}

    WrappedPolymorph ^ WrappedSimulator::GetStepperList()
    {
        try
        {
            return gcnew WrappedPolymorph(m_simulator->getStepperList());
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    WrappedPolymorph ^ WrappedSimulator::GetStepperProperty(String ^ l_stepperID, String ^ l_propertyName)
    {
        try
        {
            return gcnew WrappedPolymorph(
					m_simulator->getStepperProperty(
						WrappedCString(l_stepperID),
						WrappedCString(l_propertyName)));
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    List<bool> ^ WrappedSimulator::GetStepperPropertyAttributes(String ^ l_stepperID, String ^ l_propertyName)
    {
        try
        {
            List<bool> ^ l_list = gcnew List<bool>();
            libecs::PolymorphVector l_vector = m_simulator->getStepperPropertyAttributes(
					WrappedCString(l_stepperID),
					WrappedCString(l_propertyName));
            if(l_vector[WrappedSimulator::s_flagSettable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagGettable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagLoadable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            if(l_vector[WrappedSimulator::s_flagSavable].asInteger() == 0)
            {
                l_list->Add(false);
            }
            else
            {
                l_list->Add(true);
            }
            return l_list;
        }
		catch(const libecs::Exception& l_ex)
        {
			throw gcnew WrappedLibecsException(l_ex);
        }
    }

    WrappedPolymorph^ WrappedSimulator::GetStepperPropertyList(String ^ l_stepperID)
    {
        try
        {
            return gcnew WrappedPolymorph(
					m_simulator->getStepperPropertyList(
						WrappedCString(l_stepperID)));
        }
		catch(const libecs::Exception& l_ex)
        {
            throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::Initialize()
    {
        try
        {
            // m_simulator->initialize();
            // m_simulator->getEntityProperty("System::/:Size");
            m_simulator->saveEntityProperty("System::/:Name");
        }
        catch (const libecs::Exception& l_ex)
        {
            throw gcnew WrappedLibecsException(l_ex);
        }
		catch (const std::exception& l_ex)
        {
            throw gcnew WrappedStdException(l_ex);
        }
    }

    void WrappedSimulator::LoadEntityProperty(String ^ l_fullPNString, System::Collections::IEnumerable ^ l_list)
    {
        try
        {
            m_simulator->loadEntityProperty(
					WrappedCString(l_fullPNString),
					WrappedSimulator::ExchangeType(l_list));
        }
        catch(const libecs::Exception& l_ex)
        {
			throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::LoadEntityProperty(String^ l_fullPNString, WrappedPolymorph^ l_wrappedPolymorph)
    {
        try
        {
            m_simulator->loadEntityProperty(
					WrappedCString(l_fullPNString),
					l_wrappedPolymorph->GetPolymorph());
        }
        catch(const libecs::Exception& l_ex)
        {
			throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::LoadStepperProperty(String ^ l_stepperID, String ^ l_propertyName, System::Collections::IEnumerable^ l_list)
    {
        try
        {
            m_simulator->loadStepperProperty(
					WrappedCString(l_stepperID),
					WrappedCString(l_propertyName),
					WrappedSimulator::ExchangeType(l_list));
        }
        catch (const libecs::Exception& l_ex)
        {
            throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::LoadStepperProperty(
        String ^ l_stepperID, String ^ l_propertyName, WrappedPolymorph ^ l_wrappedPolymorph)
    {
        try
        {
            m_simulator->loadStepperProperty(
					WrappedCString(l_stepperID),
					WrappedCString(l_propertyName),
					l_wrappedPolymorph->GetPolymorph());
        }
        catch(const libecs::Exception& l_ex)
        {
			throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::Run()
    {
        try
        {
            m_simulator->run();
        }
		catch(const libecs::Exception& l_ex)
        {
            throw gcnew WrappedLibecsException(l_ex);
        }
    }

    void WrappedSimulator::Run(Double aDuration)
    {
        try
        {
            if (reinterpret_cast<EventChecker*>(m_eventChecker->get())->GetSuspendFlag())
            {
                reinterpret_cast<EventChecker*>(m_eventChecker->get())->SetSuspendFlag(false);
            }
            else
            {
                m_simulator->run(aDuration);
            }
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::SetEntityProperty(String ^ l_fullPNString, System::Collections::IEnumerable ^ l_list)
    {
        try
        {
            m_simulator->setEntityProperty(
					WrappedCString(l_fullPNString),
					WrappedSimulator::ExchangeType(l_list));
        }
		catch(const libecs::Exception& e)
        {
            throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::SetEntityProperty(String ^ l_fullPNString, WrappedPolymorph ^ l_value)
    {
        try
        {
            m_simulator->setEntityProperty(WrappedCString(l_fullPNString), l_value->GetPolymorph());
        }
		catch (const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::SetStepperProperty(String ^ l_stepperID, String ^ l_propertyName, System::Collections::IEnumerable ^ l_list )
    {
        try
        {
            m_simulator->setStepperProperty(
					WrappedCString(l_stepperID),
					WrappedCString(l_propertyName),
					WrappedSimulator::ExchangeType(l_list));
        }
		catch(const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::SetStepperProperty(
            String^ l_stepperID,
            String^ l_propertyName,
            WrappedPolymorph^ l_value )
    {
        try
        {
            m_simulator->setStepperProperty(
					WrappedCString(l_stepperID),
					WrappedCString(l_propertyName),
					l_value->GetPolymorph());
        }
 		catch(const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::Step(Int32 l_numSteps)
    {
        try
        {
            m_simulator->step(l_numSteps);
        }
 		catch(const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::Stop()
    {
        try
        {
            m_simulator->stop();
        }
 		catch(const libecs::Exception& e)
        {
			throw gcnew WrappedLibecsException(e);
        }
    }

    void WrappedSimulator::Suspend()
    {
        reinterpret_cast<EventChecker*>(m_eventChecker->get())->SetSuspendFlag(true);
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
