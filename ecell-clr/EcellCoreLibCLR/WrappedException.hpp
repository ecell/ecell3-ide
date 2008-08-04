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
		String^ m_className;
		String^ m_message;
	public:
		WrappedLibecsException(const libecs::Exception& msg)
			: m_className(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>((msg.getClassName())))),
  			  m_message(Marshal::PtrToStringAnsi((IntPtr)const_cast<char*>(msg.message().c_str())))
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