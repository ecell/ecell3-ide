#pragma warning(disable:4793)
#include <boost/shared_array.hpp>
#include <boost/python.hpp>
#include <libecs/libecs.hpp>
#include <dmtool/ModuleMaker.hpp>
#include <dmtool/SharedModuleMakerInterface.hpp>
#include <libecs/Model.hpp>
#include <libecs/System.hpp>
#include <libecs/Variable.hpp>
#include <libecs/Process.hpp>

#include "WrappedDataPointVector.hpp"
#include "WrappedException.hpp"

#undef GetCurrentTime

namespace py = boost::python;

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
        DMInfo(String^ typeName, String^ moduleName, String^ fileName, String^ description)
            : typeName_(typeName), moduleName_(moduleName), fileName_(fileName), description_(description)
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

        property String^ Description
        {
            String^ get()
            {
                return description_;
            }
        }
    private:
        String^ typeName_;
        String^ moduleName_;
        String^ fileName_;
        String^ description_;
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

	public ref class ModuleMakerEntry
	{
	public:
		ModuleMakerEntry()
			: theEntry(libecs::createDefaultModuleMaker()) {}

		ModuleMakerEntry(ModuleMaker<libecs::EcsObject>* data)
			: theEntry(data) {}

		property ModuleMaker<libecs::EcsObject>* Maker
		{
			ModuleMaker<libecs::EcsObject>* get()
			{
				return theEntry;
			}
		}

	private:
		ModuleMaker<libecs::EcsObject>* theEntry;
	};

	inline py::object generic_getattr( py::object anObj, const char* aName )
	{
		py::handle<> aRetval( py::allow_null( PyObject_GenericGetAttr(
			anObj.ptr(),
			py::handle<>(
				PyString_InternFromString(
					const_cast< char* >( aName ) ) ).get() ) ) );
		if ( !aRetval )
		{
			py::throw_error_already_set();
		}

		return py::object( aRetval );
	}

	template< typename T >
	class PythonDynamicModule;

	struct PythonEntityBaseBase
	{
	protected:
		static void appendDictToSet( std::set< libecs::String >& retval, std::string const& aPrivPrefix, PyObject* aObject )
		{
			py::handle<> aSelfDict( py::allow_null( PyObject_GetAttrString( aObject, const_cast< char* >( "__dict__" ) ) ) );
			if ( !aSelfDict )
			{
				PyErr_Clear();
				return;
			}

			if ( !PyMapping_Check( aSelfDict.get() ) )
			{
				return;
			}

			py::handle<> aKeyList( PyMapping_Items( aSelfDict.get() ) );
			BOOST_ASSERT( PyList_Check( aKeyList.get() ) );
			for ( py::ssize_t i( 0 ), e( PyList_GET_SIZE( aKeyList.get() ) ); i < e; ++i )
			{
				py::handle<> aKeyValuePair( py::borrowed( PyList_GET_ITEM( aKeyList.get(), i ) ) );
				BOOST_ASSERT( PyTuple_Check( aKeyValuePair.get() ) && PyTuple_GET_SIZE( aKeyValuePair.get() ) == 2 );
				py::handle<> aKey( py::borrowed( PyTuple_GET_ITEM( aKeyValuePair.get(), 0 ) ) );
				BOOST_ASSERT( PyString_Check( aKey.get() ) );
				if ( PyString_GET_SIZE( aKey.get() ) >= static_cast< py::ssize_t >( aPrivPrefix.size() )
						&& memcmp( PyString_AS_STRING( aKey.get() ), aPrivPrefix.data(), aPrivPrefix.size() ) == 0 )
				{
					continue;
				}

				if ( PyString_GET_SIZE( aKey.get() ) >= 2
						&& memcmp( PyString_AS_STRING( aKey.get() ), "__", 2 ) == 0 )
				{
					continue;
				}

				py::handle<> aValue( py::borrowed( PyTuple_GET_ITEM( aKeyValuePair.get(), 1 ) ) );
				// if ( !PolymorphRetriever::isConvertible( aValue.get() ) )
				// {
				//	continue;
				// }

				retval.insert( libecs::String( PyString_AS_STRING( aKey.get() ), PyString_GET_SIZE( aKey.get() ) ) );
			}
		}

		static void addAttributesFromBases( std::set< libecs::String >& retval, std::string const& aPrivPrefix, PyObject* anUpperBound, PyObject* tp )
		{
			BOOST_ASSERT( PyType_Check( tp ) );

			if ( anUpperBound == tp )
			{
				return;
			}

			py::handle<> aBasesList( py::allow_null( PyObject_GetAttrString( tp, const_cast< char* >( "__bases__" ) ) ) );
			if ( !aBasesList )
			{
				PyErr_Clear();
				return;
			}

			if ( !PyTuple_Check( aBasesList.get() ) )
			{
				return;
			}

			for ( py::ssize_t i( 0 ), ie( PyTuple_GET_SIZE( aBasesList.get() ) ); i < ie; ++i )
			{
				py::handle<> aBase( py::borrowed( PyTuple_GET_ITEM( aBasesList.get(), i ) ) );
				appendDictToSet( retval, aPrivPrefix, aBase.get() );
				addAttributesFromBases( retval, aPrivPrefix, anUpperBound, aBase.get() );
			}
		}

		static void removeAttributesFromBases( std::set< libecs::String >& retval, PyObject *tp )
		{
			BOOST_ASSERT( PyType_Check( tp ) );

			py::handle<> aBasesList( py::allow_null( PyObject_GetAttrString( tp, const_cast< char* >( "__bases__" ) ) ) );
			if ( !aBasesList )
			{
				PyErr_Clear();
				return;
			}

			if ( !PyTuple_Check( aBasesList.get() ) )
			{
				return;
			}

			for ( py::ssize_t i( 0 ), ie( PyTuple_GET_SIZE( aBasesList.get() ) ); i < ie; ++i )
			{
				py::handle<> aBase( py::borrowed( PyTuple_GET_ITEM( aBasesList.get(), i ) ) );
				removeAttributesFromBases( retval, aBase.get() );

				py::handle<> aBaseDict( py::allow_null( PyObject_GetAttrString( aBase.get(), const_cast< char* >( "__dict__" ) ) ) );
				if ( !aBaseDict )
				{
					PyErr_Clear();
					return;
				}

				if ( !PyMapping_Check( aBaseDict.get() ) )
				{
					return;
				}

				py::handle<> aKeyList( PyMapping_Keys( aBaseDict.get() ) );
				BOOST_ASSERT( PyList_Check( aKeyList.get() ) );
				for ( py::ssize_t j( 0 ), je( PyList_GET_SIZE( aKeyList.get() ) ); j < je; ++j )
				{
					py::handle<> aKey( py::borrowed( PyList_GET_ITEM( aKeyList.get(), i ) ) );
					BOOST_ASSERT( PyString_Check( aKey.get() ) );
					libecs::String aKeyStr( PyString_AS_STRING( aKey.get() ), PyString_GET_SIZE( aKey.get() ) );  
					retval.erase( aKeyStr );
				}
			}
		}

		PythonEntityBaseBase() {}
	};

	template< typename Tderived_, typename Tbase_ >
	class PythonEntityBase: public Tbase_, public PythonEntityBaseBase, public py::wrapper< Tbase_ >
	{
	public:
		virtual ~PythonEntityBase()
		{
			py::decref( py::detail::wrapper_base_::owner( this ) );
		}

		PythonDynamicModule< Tderived_ > const& getModule() const
		{
			return theModule;
		}

		const libecs::Polymorph defaultGetProperty( libecs::String const& aPropertyName ) const
		{
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			py::handle<> aValue( py::allow_null( PyObject_GenericGetAttr( aSelf, py::handle<>( PyString_InternFromString( const_cast< char* >( aPropertyName.c_str() ) ) ).get() ) ) );
			if ( !aValue )
			{
				PyErr_Clear();
				THROW_EXCEPTION_INSIDE( libecs::NoSlot, 
						"failed to retrieve property attributes "
						"for [" + aPropertyName + "]" );
			}

			return py::extract< libecs::Polymorph >( aValue.get() );
		}

		const libecs::PropertyAttributes defaultGetPropertyAttributes( libecs::String const& aPropertyName ) const
		{
			return libecs::PropertyAttributes( libecs::PropertySlotBase::POLYMORPH, true, true, true, true, true );
		}


		void defaultSetProperty( libecs::String const& aPropertyName, libecs::Polymorph const& aValue )
		{
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			PyObject_GenericSetAttr( aSelf, py::handle<>( PyString_InternFromString( const_cast< char* >( aPropertyName.c_str() ) ) ).get(), py::object( aValue ).ptr() );
			if ( PyErr_Occurred() )
			{
				PyErr_Clear();
				THROW_EXCEPTION_INSIDE( libecs::NoSlot, 
								"failed to set property [" + aPropertyName + "]" );
			}
		}

		const libecs::StringVector defaultGetPropertyList() const
		{
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			std::set< libecs::String > aPropertySet;

			if ( thePrivPrefix.empty() )
			{
				PyObject* anOwner( py::detail::wrapper_base_::owner( this ) );
				BOOST_ASSERT( anOwner != NULL );
				thePrivPrefix = libecs::String( "_" ) + anOwner->ob_type->tp_name;
			}

			appendDictToSet( aPropertySet, thePrivPrefix, aSelf );

			PyObject* anUpperBound(
					reinterpret_cast< PyObject* >(
						py::objects::registered_class_object(
							typeid( Tbase_ ) ).get() ) );
			addAttributesFromBases( aPropertySet, thePrivPrefix, anUpperBound,
					reinterpret_cast< PyObject* >( aSelf->ob_type ) );
			removeAttributesFromBases( aPropertySet, anUpperBound );

			libecs::StringVector retval;
			for ( std::set< libecs::String >::iterator i( aPropertySet.begin() ), e( aPropertySet.end() ); i != e; ++i )
			{
				retval.push_back( *i );
			}

			return retval;
		}

		libecs::PropertyInterface< Tbase_ > const& _getPropertyInterface() const;

		libecs::PropertySlotBase const* getPropertySlot( libecs::String const& aPropertyName ) const
		{
			return _getPropertyInterface().getPropertySlot( aPropertyName ); \
		}

		virtual void setProperty( libecs::String const& aPropertyName, libecs::Polymorph const& aValue )
		{
		}

		const libecs::Polymorph getProperty( libecs::String const& aPropertyName ) const
		{
			return _getPropertyInterface().getProperty( *this, aPropertyName );
		}

		void loadProperty( libecs::String const& aPropertyName, libecs::Polymorph const& aValue )
		{
			return _getPropertyInterface().loadProperty( *this, aPropertyName, aValue );
		}

		const libecs::Polymorph saveProperty( libecs::String const& aPropertyName ) const
		{
			return _getPropertyInterface().saveProperty( *this, aPropertyName );
		}

		const libecs::StringVector getPropertyList() const
		{
			return _getPropertyInterface().getPropertyList( *this );
		}

		libecs::PropertySlotProxy* createPropertySlotProxy( libecs::String const& aPropertyName )
		{
			return _getPropertyInterface().createPropertySlotProxy( *this, aPropertyName );
		}

		const libecs::PropertyAttributes
			getPropertyAttributes( libecs::String const& aPropertyName ) const
		{
			return _getPropertyInterface().getPropertyAttributes( *this, aPropertyName );
		}

		virtual libecs::PropertyInterfaceBase const& getPropertyInterface() const
		{
			return _getPropertyInterface();
		}

		PythonEntityBase( PythonDynamicModule< Tderived_ > const& aModule )
			: theModule( aModule ) {}

		static void addToRegistry()
		{
			py::objects::register_dynamic_id< Tderived_ >();
			py::objects::register_dynamic_id< Tbase_ >();
			py::objects::register_conversion< Tderived_, Tbase_ >( false );
			py::objects::register_conversion< Tbase_, Tderived_ >( true );
		}

	protected:

		PythonDynamicModule< Tderived_ > const& theModule;
		mutable libecs::String thePrivPrefix;
	};

	class PythonProcess: public PythonEntityBase< PythonProcess, libecs::Process >
	{
	public:
		virtual ~PythonProcess() {}

		LIBECS_DM_INIT_PROP_INTERFACE()
		{
			INHERIT_PROPERTIES( libecs::Process );
		}

		virtual void initialize()
		{
			Process::initialize();
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			generic_getattr( py::object( py::borrowed( aSelf ) ), "initialize" )();
			theFireMethod = generic_getattr( py::object( py::borrowed( aSelf ) ), "fire" );
		}

		virtual void fire()
		{
			py::object retval( theFireMethod() );
			if ( retval )
			{
				setActivity( py::extract< libecs::Real >( retval ) );
			} 
		}

		virtual const bool isContinuous() const
		{
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			py::handle<> anIsContinuousDescr( py::allow_null( PyObject_GenericGetAttr( reinterpret_cast< PyObject* >( aSelf->ob_type ), py::handle<>( PyString_InternFromString( "IsContinuous" ) ).get() ) ) );
			if ( !anIsContinuousDescr )
			{
				PyErr_Clear();
				return Process::isContinuous();
			}

			descrgetfunc aDescrGetFunc( anIsContinuousDescr.get()->ob_type->tp_descr_get );
			if ( ( anIsContinuousDescr.get()->ob_type->tp_flags & Py_TPFLAGS_HAVE_CLASS ) && aDescrGetFunc )
			{
				return py::extract< bool >( py::handle<>( aDescrGetFunc( anIsContinuousDescr.get(), aSelf, reinterpret_cast< PyObject* >( aSelf->ob_type ) ) ).get() );
			}

			return py::extract< bool >( anIsContinuousDescr.get() );
		}

		PythonProcess( PythonDynamicModule< PythonProcess > const& aModule )
			: PythonEntityBase< PythonProcess, Process >( aModule ) {}

		py::object theFireMethod;
	};


	class PythonVariable: public PythonEntityBase< PythonVariable, libecs::Variable >
	{
	public:
		LIBECS_DM_INIT_PROP_INTERFACE()
		{
			INHERIT_PROPERTIES( libecs::Variable );
		}

		virtual void initialize()
		{
			libecs::Variable::initialize();
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			py::getattr( py::object( py::borrowed( aSelf ) ), "initialize" )();
			theOnValueChangingMethod = py::handle<>( py::allow_null( PyObject_GenericGetAttr( aSelf, py::handle<>( PyString_InternFromString( const_cast< char* >( "onValueChanging" ) ) ).get() ) ) );
			if ( !theOnValueChangingMethod )
			{
				PyErr_Clear();
			}
		}

		virtual SET_METHOD( libecs::Real, Value )
		{
			if ( theOnValueChangingMethod )
			{
				if ( !PyCallable_Check( theOnValueChangingMethod.get() ) )
				{
					PyErr_SetString( PyExc_TypeError, "object is not callable" );
					py::throw_error_already_set();
				}

				py::handle<> aResult( PyObject_CallFunction( theOnValueChangingMethod.get(), "f", value ) );
				if ( !aResult )
				{
					py::throw_error_already_set();
				}
				else
				{
					if ( !PyObject_IsTrue( aResult.get() ) )
					{
						return;
					}
				}
			}
			else
			{
				PyErr_Clear();
			}
			libecs::Variable::setValue( value );
		}

		PythonVariable( PythonDynamicModule< PythonVariable > const& aModule )
			: PythonEntityBase< PythonVariable, Variable >( aModule ) {}

		py::handle<> theOnValueChangingMethod;
	};


	class PythonSystem: public PythonEntityBase< PythonSystem, libecs::System >
	{
	public:
		LIBECS_DM_INIT_PROP_INTERFACE()
		{
			INHERIT_PROPERTIES( libecs::System );
		}

		virtual void initialize()
		{
			libecs::System::initialize();
			PyObject* aSelf( py::detail::wrapper_base_::owner( this ) );
			py::getattr( py::object( py::borrowed( aSelf ) ), "initialize" )();
		}

		PythonSystem( PythonDynamicModule< PythonSystem > const& aModule )
			: PythonEntityBase< PythonSystem, libecs::System >( aModule ) {}
	};

	template<typename T_>
	struct DeduceEntityType
	{
		static libecs::EntityType value;
	};

	template<>
	libecs::EntityType DeduceEntityType< PythonProcess >::value( libecs::EntityType::PROCESS );

	template<>
	libecs::EntityType DeduceEntityType< PythonVariable >::value( libecs::EntityType::VARIABLE );

	template<>
	libecs::EntityType DeduceEntityType< PythonSystem >::value( libecs::EntityType::SYSTEM );

	template< typename T_ >
	class PythonDynamicModule: public DynamicModule< libecs::EcsObject >
	{
	public:
		typedef DynamicModule< libecs::EcsObject > Base;

		struct make_ptr_instance: public py::objects::make_instance_impl< T_, py::objects::pointer_holder< T_*, T_ >, make_ptr_instance > 
		{
			typedef py::objects::pointer_holder< T_*, T_ > holder_t;

			template <class Arg>
			static inline holder_t* construct(void* storage, PyObject* arg, Arg& x)
			{
				py::detail::initialize_wrapper( py::incref( arg ), boost::get_pointer(x) );
				return new (storage) holder_t(x);
			}

			template<typename Ptr>
			static inline PyTypeObject* get_class_object(Ptr const& x)
			{
				return static_cast< T_ const* >( boost::get_pointer(x) )->getModule().getPythonType();
			}
		};

		virtual libecs::EcsObject* createInstance() const;

		virtual const char* getFileName() const
		{
			const char* aRetval( 0 );
			try
			{
				py::handle<> aPythonModule( PyImport_Import( py::getattr( thePythonClass, "__module__" ).ptr() ) );
				if ( !aPythonModule )
				{
					py::throw_error_already_set();
				}
				aRetval = py::extract< const char * >( py::getattr( py::object( aPythonModule ), "__file__" ) );
			}
			catch ( py::error_already_set )
			{
				PyErr_Clear();
			}
			return aRetval;
		}

		virtual const char *getModuleName() const 
		{
			return reinterpret_cast< PyTypeObject* >( thePythonClass.ptr() )->tp_name;
		}

		virtual const DynamicModuleInfo* getInfo() const
		{
			return &thePropertyInterface;
		}

		PyTypeObject* getPythonType() const
		{
			return reinterpret_cast< PyTypeObject* >( thePythonClass.ptr() );
		}
	 
		PythonDynamicModule( py::object aPythonClass )
			: Base( DM_TYPE_DYNAMIC ),
			   thePythonClass( aPythonClass ),
			   thePropertyInterface( getModuleName(),
									 DeduceEntityType< T_ >::value.asString() )
		{
		}

	private:
		py::object thePythonClass;
		libecs::PropertyInterface< T_ > thePropertyInterface;
	};

	template< typename Tderived_, typename Tbase_ >
	inline libecs::PropertyInterface< Tbase_ > const&
	PythonEntityBase< Tderived_, Tbase_ >::_getPropertyInterface() const
	{
		return *reinterpret_cast< libecs::PropertyInterface< Tbase_ > const* >( theModule.getInfo() );
	}

	template< typename T_ >
	libecs::EcsObject* PythonDynamicModule< T_ >::createInstance() const
	{
		T_* retval( new T_( *this ) );
		py::handle<> aNewObject( make_ptr_instance::execute( retval ) );

		if ( !aNewObject )
		{
			delete retval;
			std::string anErrorStr( "Instantiation failure" );
			PyObject* aPyErrObj( PyErr_Occurred() );
			if ( aPyErrObj )
			{
				anErrorStr += "(";
				anErrorStr += aPyErrObj->ob_type->tp_name;
				anErrorStr += ": ";
				py::handle<> aPyErrStrRepr( PyObject_Str( aPyErrObj ) );
				BOOST_ASSERT( PyString_Check( aPyErrStrRepr.get() ) );
				anErrorStr.insert( anErrorStr.size(),
					PyString_AS_STRING( aPyErrStrRepr.get() ),
					PyString_GET_SIZE( aPyErrStrRepr.get() ) );
				anErrorStr += ")";
				PyErr_Clear();
			}
			throw std::runtime_error( anErrorStr );
		}

		return retval;
	}

	struct CompositeModuleMaker: public ModuleMaker< libecs::EcsObject >,
                                 public SharedModuleMakerInterface
    {
        virtual ~CompositeModuleMaker() {}

        virtual void setSearchPath( const std::string& path )
        {
            SharedModuleMakerInterface* anInterface(
                dynamic_cast< SharedModuleMakerInterface* >( &theDefaultModuleMaker ) );
            if ( anInterface )
            {
                anInterface->setSearchPath( path );
            }
        }

        virtual const std::string getSearchPath() const
        {
            SharedModuleMakerInterface* anInterface(
                dynamic_cast< SharedModuleMakerInterface* >( &theDefaultModuleMaker ) );
            if ( anInterface )
            {
                return anInterface->getSearchPath();
            }
			return "";
        }

        virtual const Module& getModule( const std::string& aClassName, bool forceReload = false )
        {
            ModuleMap::iterator i( theRealModuleMap.find( aClassName ) );
            if ( i == theRealModuleMap.end() )
            {
                const Module& retval( theDefaultModuleMaker.getModule( aClassName, forceReload ) );
                theRealModuleMap[ retval.getModuleName() ] = const_cast< Module* >( &retval );
                return retval;
            }
            return *(*i).second;
        }

        virtual void addClass( Module* dm )
        {
            assert( dm != NULL && dm->getModuleName() != NULL );
            this->theRealModuleMap[ dm->getModuleName() ] = dm;
			ModuleMaker< libecs::EcsObject >::addClass( dm );
        }

        virtual const ModuleMap& getModuleMap() const
        {
            return theRealModuleMap;
        }

		CompositeModuleMaker( ModuleMaker< libecs::EcsObject >& aDefaultModuleMaker )
            : theDefaultModuleMaker( aDefaultModuleMaker ) {}

    private:
		ModuleMaker< libecs::EcsObject >& theDefaultModuleMaker;
        ModuleMap theRealModuleMap;
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
			Py_Initialize();
			PythonVariable::addToRegistry();
			PythonProcess::addToRegistry();
			PythonSystem::addToRegistry();
        }

        WrappedSimulator(System::Collections::IEnumerable^ l_dmPath)
            try: theEventCheckInterval(30),
			     theDefaultPropertiedObjectMaker( libecs::createDefaultModuleMaker() ),
				 thePropertiedObjectMaker( new CompositeModuleMaker( *theDefaultPropertiedObjectMaker ) ),
				 theModel( new libecs::Model( *thePropertiedObjectMaker ) )
        {
			theModel->setup();
            StringBuilder^ dmPathRepr = gcnew StringBuilder();
            for each (String^ i in l_dmPath) {
                dmPathRepr->Append(i);
                dmPathRepr->Append(static_cast<wchar_t>(libecs::Model::PATH_SEPARATOR));
            }
            --dmPathRepr->Length;
            theModel->setDMSearchPath(WrappedCString(dmPathRepr->ToString()));
        }
        catch ( std::exception const& e )
        {
            delete theModel;
            delete thePropertiedObjectMaker;
			delete theDefaultPropertiedObjectMaker;
            throw gcnew WrappedStdException( e );
        }

        ~WrappedSimulator()
        {
            delete theModel;
            delete thePropertiedObjectMaker;
			delete theDefaultPropertiedObjectMaker;
        }

        void Initialize()
		{
		    // calling the model's initialize(), which is non-const,
		    // is semantically a const operation at the simulator level.
            try
            {
    		    theModel->initialize();
            }
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException( e );
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException( e );
            }
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
				String^ module = FromCString(i->second->getModuleName());

                retval->Add(DMInfo(
                    FromCString( info->getTypeName() ),
                    module,
                    FromCString( i->second->getFileName() ),
					(String^)GetInfoField(*info, "Description" ) ) );
            }

            return retval;
        }

		String^ GetDescription(String^ className)
		{
			try
			{
				return (String^)GetInfoField( theModel->getPropertyInterface( WrappedCString( className ) ), "Description" );
			}
			catch (std::exception const& e)
			{
				throw gcnew WrappedStdException(e);
			}
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
                            for (libecs::System::Variables::const_iterator
                                    i = aSystemPtr->getVariables().begin(),
                                    end = aSystemPtr->getVariables().end();
                                 i != end; ++i)
                            {
                                retval->Add(FromCString(i->second->getID()));
                            }
                        }
                        break;

                    case libecs::EntityType::PROCESS:
                        {
                            for (libecs::System::Processes::const_iterator
                                    i = aSystemPtr->getProcesses().begin(),
                                    end = aSystemPtr->getProcesses().end();
                                 i != end; ++i)
                            {
                                retval->Add(FromCString(i->second->getID()));
                            }
                        }
                        break;
                    case libecs::EntityType::SYSTEM:
                        {
                            for (libecs::System::Systems::const_iterator
                                    i = aSystemPtr->getSystems().begin(),
                                    end = aSystemPtr->getSystems().end();
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
				libecs::StepperCptr anEntityPtr(theModel->getStepper(WrappedCString(aStepperID)));
                return FromPolymorph(anEntityPtr->getProperty(WrappedCString(aPropertyName)));
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

            libecs::Stepper const* aSystemStepper(theModel->getSystemStepper());
            try
            { 
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
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
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

            libecs::Stepper * aSystemStepper(theModel->getSystemStepper() );
            aSystemStepper->setCurrentTime( aStopTime );
            aSystemStepper->setStepInterval( 0.0 );

            theModel->getScheduler().updateEvent( 0, aStopTime );

            try
            {
                for (;;)
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
            
            try
            { 
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
            catch (libecs::Exception const& e)
            {
                throw gcnew WrappedLibecsException(e);
            }
            catch (std::exception const& e)
            {
                throw gcnew WrappedStdException(e);
            }
        }

        void Stop()
        {
            theRunningFlag = false;
            theModel->flushLoggers();
        }

    private:
		Object^ GetInfoField(libecs::PropertyInterfaceBase const& aPropIface, String^ aPropName)
		{
			std::auto_ptr<DynamicModuleInfo::EntryIterator> anInfo( aPropIface.getInfoFields() );
            while ( anInfo->next() )
			{
				if(FromCString(anInfo->current().first) != "Description")
					continue;

				return FromPolymorph( *reinterpret_cast< const libecs::Polymorph* >( anInfo->current().second ) );
			}
			return nullptr;
		}
		
		libecs::LoggerPtr getLogger(String^ aFullPNString)
        {
            return theModel->getLoggerBroker().getLogger(libecs::FullPN(WrappedCString(aFullPNString)));
        }

        void setDirty()
        {
            theDirtyFlag = true;
        }

        const bool IsDirty()
        {
            return theDirtyFlag;
        }

        inline void HandleEvent()
        {
            if (theEventHandler != nullptr)
                theEventHandler(this, nullptr);
            ClearDirty();
        }

        void ClearDirty()
        {
            if (IsDirty())
            {
                Initialize();
                // interruptAll();
                theDirtyFlag = false;
            }
        }

        void Start()
        {
            ClearDirty();
            theRunningFlag = true;
        }

    private:
        bool                          theRunningFlag;

        mutable bool                  theDirtyFlag;

        libecs::Integer               theEventCheckInterval;

		ModuleMaker<libecs::EcsObject>* theDefaultPropertiedObjectMaker;
		CompositeModuleMaker*         thePropertiedObjectMaker;
        libecs::Model*                theModel;

        System::EventHandler^         theEventHandler;
   };
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
