#pragma warning(disable:4793)
#include <boost/shared_array.hpp>
#include <libecs/libecs.hpp>
#include <dmtool/ModuleMaker.hpp>
#include <libecs/Model.hpp>
#include <libecs/System.hpp>
#include <libecs/Variable.hpp>
#include <libecs/Process.hpp>

#include "WrappedDataPointVector.hpp"
#include "WrappedException.hpp"

#undef GetCurrentTime

namespace EcellCoreLib {
    using namespace System;
    using namespace System::Collections::Generic;
    using namespace System::Windows::Forms;
    using namespace System::Text;
    using namespace System::Runtime::InteropServices;

    class WrappedCString: public boost::shared_array<char>
    {
    public:
        inline WrappedCString(const WrappedCString& that)
            : boost::shared_array<char>(that)
        {
        }

        inline WrappedCString(String^ str)
            : boost::shared_array<char>(
                static_cast<char*>((void*)Marshal::StringToHGlobalAnsi(str)),
                reinterpret_cast<void(*)(char*)>(&freeHGlobalBuffer))
        {
        }

        inline operator std::string() const
        {
            return std::string(get());
        }

    private:
        static void freeHGlobalBuffer(void *ptr)
        {
            Marshal::FreeHGlobal((IntPtr)ptr);
        }
    };

    static String^ FromCString(std::string const& str)
    {
        return Marshal::PtrToStringAnsi((IntPtr)(char *)(str.data()), str.size());
    }

    static String^ FromCString(const char* str)
    {
        return Marshal::PtrToStringAnsi((IntPtr)(char *)(str));
    }

    static libecs::Polymorph ToPolymorph(Object^ obj)
    {

        if (dynamic_cast<double^>(obj) != nullptr)
        {
            const double val = safe_cast<double>(obj);
            return libecs::Polymorph(val);
        }
        else if (dynamic_cast<float^>(obj) != nullptr)
        {
            const double val = safe_cast<float>(obj);
            return libecs::Polymorph(val);
        }
        else if (dynamic_cast<int^>(obj) != nullptr)
        {
            return libecs::Polymorph(safe_cast<long>(obj));
        }
        else if (dynamic_cast<long^>(obj) != nullptr)
        {
            return libecs::Polymorph(safe_cast<long>(obj));
        }
        else if (dynamic_cast<String^>(obj) != nullptr)
        {
            char* ptr = static_cast<char*>(
                (void*)Marshal::StringToHGlobalAnsi(safe_cast<String^>(obj)));
            try
            {
                return libecs::Polymorph(ptr);
            }
            finally
            {
                Marshal::FreeHGlobal(safe_cast<IntPtr>(ptr));
            }
        }
        else if (dynamic_cast<System::Collections::IEnumerable^>(obj) != nullptr)
        {
            libecs::PolymorphVector v;
            for each (Object^ i in safe_cast<System::Collections::IEnumerable^>(obj))
            {
                v.push_back(ToPolymorph(i)); 
            }
            return libecs::Polymorph(v);
        }
        throw gcnew NotSupportedException();
    }

    static Object^ FromPolymorph(libecs::Polymorph pol)
    {
        switch (pol.getType())
        {
        case libecs::PolymorphValue::REAL:
            return pol.as<libecs::Real>();
            break;
        case libecs::PolymorphValue::INTEGER:
            return pol.as<libecs::Integer>();
            break;
        case libecs::PolymorphValue::STRING:
            return Marshal::PtrToStringAnsi(
                (IntPtr)(char*)static_cast<const char*>(pol.as<libecs::PolymorphValue::RawString const&>()),
                pol.as<libecs::PolymorphValue::RawString const&>().size());
            break;
        case libecs::PolymorphValue::TUPLE:
            {
                typedef libecs::PolymorphValue::Tuple Tuple;
                Tuple const& tuple(pol.as<Tuple const&>());
                array<Object^>^ objs = gcnew array<Object^>(tuple.size());
                for (Tuple::size_type i = 0; i < tuple.size(); ++i)
                {
                    objs[i] = FromPolymorph(tuple[i]);
                }
                return objs;
            }
        }
        throw gcnew NotSupportedException();
    }

    public value struct DMInfo
    {
        DMInfo(String^ typeName, String^ moduleName, String^ fileName)
            : typeName_(typeName), moduleName_(moduleName), fileName_(fileName)
        {
        }

        property String^ TypeName
        {
            String^ get()
            {
                return typeName_;
            }
        }

        property String^ ModuleName
        {
            String^ get()
            {
                return moduleName_;
            }
        }

        property String^ FileName
        {
            String^ get()
            {
                return fileName_;
            }
        }
    private:
        String^ typeName_;
        String^ moduleName_;
        String^ fileName_;
    };

    public enum PropertyType
    {
        Real       = libecs::PropertySlotBase::REAL,
        Integer    = libecs::PropertySlotBase::INTEGER,
        StringType = libecs::PropertySlotBase::STRING,
        Polymorph  = libecs::PropertySlotBase::POLYMORPH
    };

    public value struct PropertyAttributes
    {
    public:
        PropertyAttributes(libecs::PropertyAttributes const& attrs)
            : type((PropertyType)attrs.getType()),
              settable(attrs.isSetable()),
              gettable(attrs.isGetable()),
              loadable(attrs.isLoadable()),
              savable(attrs.isSavable()),
              dynamic(attrs.isDynamic())
        {
        }

        PropertyAttributes(PropertyType type,
                           bool isSettable, bool isGettable,
                           bool isLoadable, bool isSavable, bool isDynamic)
            : type(type), settable(isSettable), gettable(isGettable),
              loadable(isLoadable), savable(isSavable), dynamic(isDynamic)
        {
        }

        property PropertyType Type
        {
            PropertyType get()
            {
                return type;
            }
        }

        property bool Settable
        {
            bool get()
            {
                return settable;
            }
        }

        property bool Gettable
        {
            bool get()
            {
                return gettable;
            }
        }

        property bool Loadable
        {
            bool get()
            {
                return loadable;
            }
        }

        property bool Savable
        {
            bool get()
            {
                return savable;
            }
        }

        property bool Dynamic
        {
            bool get()
            {
                return dynamic;
            }
        }    

    private:
        PropertyType type;
        bool settable;
        bool gettable;
        bool loadable;
        bool savable;
        bool dynamic;
    };

    public value struct EventDescriptor
    {
        EventDescriptor(double time, String^ id)
            : time_(time), id_(id) {}

        property double Time
        {
            double get()
            {
                return time_;
            }
        }

        property String^ StepperID
        {
            String^ get()
            {
                return id_;
            }
        }

    private:
        double time_;
        String^ id_;
    };

    public ref class WrappedSimulator
    {
    public:
        property System::EventHandler^ EventHandler
        {
            System::EventHandler^ get()
            {
                return theEventHandler;
            }
            void set(System::EventHandler^ value)
            {
                theEventHandler = value;
            }
        }

        property unsigned EventCheckInterval
        {
            unsigned get()
            {
                return theEventCheckInterval;
            }
            void set(unsigned value)
            {
                theEventCheckInterval = value;
            }
        }
    public:
        static WrappedSimulator()
        {
            libecs::initialize();
        }

        WrappedSimulator(System::Collections::IEnumerable^ l_dmPath)
            try: thePropertiedObjectMaker( libecs::createDefaultModuleMaker() ),
                 theModel( new libecs::Model( *thePropertiedObjectMaker ) ),
                 theEventCheckInterval(30)
        {
            StringBuilder^ dmPathRepr = gcnew StringBuilder();
            for each (String^ i in l_dmPath) {
                dmPathRepr->Append(i);
                dmPathRepr->Append(static_cast<wchar_t>(libecs::Model::PATH_SEPARATOR));
            }
            --dmPathRepr->Length;
            theModel->setDMSearchPath(WrappedCString(dmPathRepr->ToString()));
        }
        catch ( std::exception const& )
        {
            delete theModel;
            delete thePropertiedObjectMaker;
            throw;
        }

        ~WrappedSimulator()
        {
            delete theModel;
            delete thePropertiedObjectMaker;
        }

        void Initialize()
		{
		    // calling the model's initialize(), which is non-const,
		    // is semantically a const operation at the simulator level.
		    theModel->initialize();
		}

        void CreateEntity(String^ l_classname, String^ l_fullIDString)
        {
            try
            {
                theModel->createEntity(
                    WrappedCString(l_classname),
                    libecs::FullID(WrappedCString(l_fullIDString)));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void CreateLogger(String ^ l_fullPNString)
        {
            CreateLogger(l_fullPNString, 1, 0, true, 0);
        }

        void CreateLogger(String ^ l_fullPNString, libecs::Integer minSteps, libecs::Real minInterval, bool continueOnError, libecs::Integer maxSpace)
        {
            try
            {
                theModel->getLoggerBroker().createLogger(
                    libecs::FullPN(WrappedCString(l_fullPNString)),
                    libecs::Logger::Policy(
                        minSteps, minInterval,
                        continueOnError, maxSpace));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void CreateStepper(String ^ l_classname, String ^ l_ID)
        {
            try
            {
                theModel->createStepper(WrappedCString(l_classname), WrappedCString(l_ID));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        libecs::Time GetCurrentTime()
        {
            return theModel->getCurrentTime();
        }

        IList<DMInfo>^ GetDMInfo()
        {
            typedef ModuleMaker<libecs::EcsObject>::ModuleMap ModuleMap;
            const ModuleMap& modules(thePropertiedObjectMaker->getModuleMap());

            System::Collections::Generic::List<DMInfo>^ retval = gcnew List<DMInfo>();

            for (ModuleMap::const_iterator i = modules.begin(); i != modules.end(); ++i)
            {
                const libecs::PropertyInterfaceBase* info(
                    reinterpret_cast< const libecs::PropertyInterfaceBase *>(
                        i->second->getInfo() ) );
                retval->Add(DMInfo(
                    FromCString(info->getTypeName()),
                    FromCString(i->second->getModuleName()),
                    FromCString(i->second->getFileName())));
            }

            return retval;
        }

        IList<String^>^ GetEntityList(String^ l_entityTypeString, String^ l_systemPathString)
        {
            try
            {
                const libecs::EntityType anEntityType((std::string)WrappedCString(l_entityTypeString));
                const libecs::SystemPath aSystemPath((std::string)WrappedCString(l_systemPathString));

                List<String^>^ retval = gcnew List<String^>();

                if (aSystemPath.size() == 0)
                {
                    if (anEntityType == libecs::EntityType::SYSTEM)
                    {
                        retval->Add("/");
                    }
                }
                else
                {
                    libecs::System const* aSystemPtr = theModel->getSystem(aSystemPath);

                    switch (anEntityType)
                    {
                    case libecs::EntityType::VARIABLE:
                        {
                            for (libecs::System::VariableMap::const_iterator
                                    i = aSystemPtr->getVariableMap().begin(),
                                    end = aSystemPtr->getVariableMap().end();
                                 i != end; ++i)
                            {
                                retval->Add(FromCString(i->second->getID()));
                            }
                        }
                        break;

                    case libecs::EntityType::PROCESS:
                        {
                            for (libecs::System::ProcessMap::const_iterator
                                    i = aSystemPtr->getProcessMap().begin(),
                                    end = aSystemPtr->getProcessMap().end();
                                 i != end; ++i)
                            {
                                retval->Add(FromCString(i->second->getID()));
                            }
                        }
                        break;
                    case libecs::EntityType::SYSTEM:
                        {
                            for (libecs::System::SystemMap::const_iterator
                                    i = aSystemPtr->getSystemMap().begin(),
                                    end = aSystemPtr->getSystemMap().end();
                                 i != end; ++i)
                            {
                                retval->Add(FromCString(i->second->getID()));
                            }
                        }
                        break;
                    }
                }
                return retval;
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        Object^ GetEntityProperty(String^ l_fullPNString)
        {
            try
            {
                libecs::FullPN aFullPN((std::string)WrappedCString(l_fullPNString));
                libecs::EntityCptr anEntityPtr(theModel->getEntity(aFullPN.getFullID()));
                return FromPolymorph(anEntityPtr->getProperty(aFullPN.getPropertyName()));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        PropertyAttributes GetEntityPropertyAttributes(String^ l_fullPNString)
		{
            try
            {
                libecs::FullPN aFullPN((std::string)WrappedCString(l_fullPNString));
                libecs::EntityCptr anEntityPtr(theModel->getEntity(aFullPN.getFullID()));
                return PropertyAttributes(anEntityPtr->getPropertyAttributes(aFullPN.getPropertyName()));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
		}

        IList<String^>^ GetEntityPropertyList(String^ l_fullIDString)
        {
            try
            {
                libecs::EntityCptr anEntityPtr(theModel->getEntity(libecs::FullID((std::string)WrappedCString(l_fullIDString))));
                const libecs::StringVector v(anEntityPtr->getPropertyList());
                List<String^>^ retval = gcnew List<String^>(v.size());
                for (libecs::StringVector::const_iterator i = v.begin(); i != v.end(); ++i)
                {
                    retval->Add(FromCString(*i));
                }
                return retval;
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        WrappedDataPointVector^ GetLoggerData(String ^ l_fullPNString)
        {
            try
            {
                return gcnew WrappedDataPointVector(getLogger(l_fullPNString)->getData());
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        WrappedDataPointVector^
        GetLoggerData(String ^ l_fullPNString, double l_startTime, double l_endTime)
        {
            try
            {
                return gcnew WrappedDataPointVector(getLogger(l_fullPNString)->getData(l_startTime, l_endTime));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        WrappedDataPointVector^
        GetLoggerData(String ^ l_fullPNString, double l_startTime, double l_endTime, double l_interval)
        {
            try
            {
                return gcnew WrappedDataPointVector(getLogger(l_fullPNString)->getData(l_startTime, l_endTime, l_interval));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        double GetLoggerStartTime(String^ aFullPNString)
        {
            try
            {
                return getLogger(aFullPNString)->getStartTime();
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        double GetLoggerEndTime(String^ aFullPNString)
        {
            try
            {
                return getLogger(aFullPNString)->getEndTime();
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        long GetLoggerSize(String^ aFullPNString)
        {
            try
            {
                return getLogger(aFullPNString)->getSize();
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        IList<String^>^ GetLoggerList()
        {
            try
            {
                List<String^>^ retval = gcnew List<String^>();
                libecs::LoggerBroker const& loggerBroker(theModel->getLoggerBroker());
                for (libecs::LoggerBroker::const_iterator
                        i(loggerBroker.begin()), end(loggerBroker.end());
                    i != end; ++i)
                {
                    libecs::FullPNCref aFullPN( (*i).first );
                    retval->Add(FromCString(aFullPN.getString()));
                }

                return retval;
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        EventDescriptor GetNextEvent()
        {
            libecs::StepperEventCref aNextEvent(theModel->getTopEvent());

            return EventDescriptor(aNextEvent.getTime(), FromCString(aNextEvent.getStepper()->getID()));
        }

        String^ GetStepperClassName(String^ aStepperID)
        {
            return FromCString(theModel->getStepper(WrappedCString(aStepperID))->getClassName());
        }

        IList<String^>^ GetStepperList()
        {
            try
            {
                libecs::Model::StepperMapCref aStepperMap(theModel->getStepperMap());
                List<String^>^ retval = gcnew List<String^>(aStepperMap.size());
               
                for(libecs::Model::StepperMap::const_iterator i( aStepperMap.begin() );
                     i != aStepperMap.end(); ++i)
                {
                    retval->Add(FromCString(i->first));
                }
                return retval;
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        Object^ GetStepperProperty(String^ aStepperID, String^ aPropertyName)
        {
            try
            {
                return FromPolymorph(theModel->getStepper(WrappedCString(aStepperID))->getProperty(WrappedCString(aPropertyName)));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        PropertyAttributes GetStepperPropertyAttributes(String^ l_stepperID, String^ l_propertyName)
        {
            try
            {
                return PropertyAttributes(theModel->getStepper(WrappedCString(l_stepperID))->getPropertyAttributes(WrappedCString(l_propertyName)));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        IList<String^>^ GetStepperPropertyList(String^ aStepperID)
        {
            try
            {
                libecs::StepperCptr anStepperPtr(theModel->getStepper(((std::string)WrappedCString(aStepperID))));
                const libecs::StringVector v(anStepperPtr->getPropertyList());
                List<String^>^ retval = gcnew List<String^>(v.size());
                for (libecs::StringVector::const_iterator i = v.begin(); i != v.end(); ++i)
                {
                    retval->Add(FromCString(*i));
                }
                return retval;
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void LoadEntityProperty(String^ l_fullPNString, Object^ l_value)
        {
            try
            {
                libecs::FullPN aFullPN((std::string)WrappedCString(l_fullPNString));
                libecs::EntityPtr anEntityPtr(theModel->getEntity(aFullPN.getFullID()));
                setDirty();
                anEntityPtr->loadProperty(aFullPN.getPropertyName(), ToPolymorph(l_value));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void LoadStepperProperty(String^ l_stepperID, String^ l_propName, Object^ l_value)
        {
            try
            {
                libecs::StepperPtr aStepperPtr(theModel->getStepper((std::string)WrappedCString(l_stepperID)));
                setDirty();
                aStepperPtr->loadProperty((std::string)WrappedCString(l_propName), ToPolymorph(l_value));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void Run()
        {
            Start();

            do
            {
                unsigned int aCounter = theEventCheckInterval;
                do
                {
                    theModel->step();
                    --aCounter;
                }
                while( aCounter != 0 );
                HandleEvent();
            } while( theRunningFlag );
        }

        void Run(Double aDuration)
        {
            if( aDuration <= 0.0 )
            {
                throw gcnew System::ArgumentException(
                    "duration must be greater than 0. (" + aDuration + " given.)");
            }

            Start();

            const libecs::Real aStopTime(theModel->getCurrentTime() + aDuration );

            // setup SystemStepper to step at aStopTime

            libecs::StepperPtr aSystemStepper(theModel->getSystemStepper() );
            aSystemStepper->setCurrentTime( aStopTime );
            aSystemStepper->setStepInterval( 0.0 );

            theModel->getScheduler().updateEvent( 0, aStopTime );

            // runWithEvent();
            runWithoutEvent();
        }

        void SetEntityProperty(String ^ l_fullPNString, Object^ l_value)
        {
            try
            {
                libecs::FullPN aFullPN((std::string)WrappedCString(l_fullPNString));
                libecs::EntityPtr anEntityPtr(theModel->getEntity(aFullPN.getFullID()));
                setDirty();
                anEntityPtr->setProperty(aFullPN.getPropertyName(), ToPolymorph(l_value));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void SetStepperProperty(String^ l_stepperID, String^ l_propName, Object^ l_value)
        {
            try
            {
                libecs::StepperPtr aStepperPtr(theModel->getStepper((std::string)WrappedCString(l_stepperID)));
                setDirty();
                aStepperPtr->setProperty((std::string)WrappedCString(l_propName), ToPolymorph(l_value));
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void Step(Int32 aNumSteps)
        {
            if( aNumSteps <= 0 )
            {
                throw gcnew System::ArgumentException(
                     "step( n ): n must be 1 or greater. (" + aNumSteps + " given.)" );
            }

            Start();

            libecs::Integer aCounter( aNumSteps );
            
            for (;;)
            {
                theModel->step();
                
                --aCounter;
                
                if (aCounter == 0)
                {
                    Stop();
                    break;
                }

                if (aCounter % theEventCheckInterval == 0)
                {
                    HandleEvent();

                    if (!theRunningFlag)
                    {
                        break;
                    }
                }
            }
        }

        void Stop()
        {
            theRunningFlag = false;
            theModel->flushLoggers();
        }

    private:
        libecs::LoggerPtr getLogger(String^ aFullPNString)
        {
            return theModel->getLoggerBroker().getLogger(libecs::FullPN(WrappedCString(aFullPNString)));
        }

        void setDirty()
        {
            theDirtyFlag = true;
        }

        const bool isDirty()
        {
            return theDirtyFlag;
        }

        inline void HandleEvent()
        {
            if (theEventHandler != nullptr)
                theEventHandler(this, nullptr);
            clearDirty();
        }

        void clearDirty()
        {
            if (isDirty())
            {
                Initialize();
                // interruptAll();
                theDirtyFlag = false;
            }
        }

        void Start()
        {
            clearDirty();
            theRunningFlag = true;
        }

        void runWithEvent()
        {
            libecs::StepperCptr const aSystemStepper = theModel->getSystemStepper();
            do
            {
                unsigned int aCounter(theEventCheckInterval);
                do 
                {
                    if (theModel->getTopEvent().getStepper() == aSystemStepper)
                    {
                        theModel->step();
                        Stop();
                        return;
                    }
                    else
                    {
                        theModel->step();
                    }

                    --aCounter;
                } while( aCounter != 0 );

                HandleEvent();
            }
            while( theRunningFlag );
        }

        void runWithoutEvent()
        {
            libecs::StepperCptr const aSystemStepper = theModel->getSystemStepper();
            do
            {
                if (theModel->getTopEvent().getStepper() == aSystemStepper)
                {
                    theModel->step();
                    Stop();
                    return;    // the only exit
                }
                else
                {
                    theModel->step();
                }
            }
            while( 1 );
        }

    private:
        bool                          theRunningFlag;

        mutable bool                  theDirtyFlag;

        libecs::Integer               theEventCheckInterval;

        ModuleMaker<libecs::EcsObject> *thePropertiedObjectMaker;
        libecs::Model                 *theModel;

        System::EventHandler^         theEventHandler;
    };
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
