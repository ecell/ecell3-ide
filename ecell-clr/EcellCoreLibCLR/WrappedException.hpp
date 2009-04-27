#include <libecs/libecs.hpp>

#undef GetCurrentTime

namespace EcellCoreLib
{
	using namespace System;
	using namespace System::Runtime::InteropServices;

	public ref class WrappedException: public ApplicationException {};

	public ref class WrappedLibecsException: public WrappedException
	{
	private:
		String^ const m_className;
		String^ const m_message;
        String^ const m_fullID;
	public:
		WrappedLibecsException(const libecs::Exception& msg)
			: m_className(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>((msg.getClassName())))),
  			  m_message(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(msg.message().c_str()))),
              m_fullID(GetFullIDFromEcsObject(msg.getEcsObject()))
		{
		}

		virtual property String^ Message
		{
		public:
			String^ get() override
			{
				return m_message;
			}
		}

		virtual property String^ Source
		{
		public:
			String^ get() override
			{
				return m_className;
			}
		}

        property String^ FullID
        {
        public:
            String^ get()
            {
                return m_fullID;
            }
        }

    private:
        static String^ GetFullIDFromEcsObject(libecs::EcsObject const* obj)
        {
            {
                libecs::Entity const* ent = dynamic_cast<libecs::Entity const*>(obj);
                if (ent)
                {
                    std::string fullIDStr(ent->getFullID().asString());
                    return Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(fullIDStr.c_str()));
                }
            }
            {
                /* 本来 Stepper の FullID というものは存在しないが、仮に
                 * Stepper:[Stepper の ID 名] という形で表現する */
                libecs::Stepper const* stp = dynamic_cast<libecs::Stepper const*>(obj);
                if (stp)
                {
                    std::string fullIDStr(std::string("Stepper:") + stp->getID());
                    return Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(fullIDStr.c_str()));
                }
            }

            return nullptr;
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
			virtual String^ get() override
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
}