#include "WrappedPolymorph.hpp"


namespace EcellCoreLib
{
	using namespace System;
	using namespace System::Collections::Generic;
	using namespace System::Runtime::InteropServices;

    WrappedPolymorph::WrappedPolymorph(double aValue)
		: thePolymorph(new libecs::Polymorph((libecs::Real)aValue))
	{
    }

    WrappedPolymorph::WrappedPolymorph(int aValue)
		: thePolymorph(new libecs::Polymorph((libecs::Integer)aValue))
	{
    }

    WrappedPolymorph::WrappedPolymorph(String^ aValue)
    {
		char* str = reinterpret_cast<char*>((void *)Marshal::StringToHGlobalAnsi(aValue));
		try
		{
	        thePolymorph = new libecs::Polymorph(str);
		}
		finally
		{
			Marshal::FreeHGlobal((IntPtr)str);
		}
    }

    WrappedPolymorph::WrappedPolymorph(List<WrappedPolymorph^>^ aWrappedPolymorphList)
		: thePolymorph(WrappedPolymorph::CastToPolymorph(aWrappedPolymorphList))
    {
    }

    WrappedPolymorph::WrappedPolymorph(const libecs::Polymorph& aPolymorph)
		: thePolymorph(new libecs::Polymorph(aPolymorph))
    {
    }

	WrappedPolymorph::~WrappedPolymorph()
	{
		delete thePolymorph;
	}

    double WrappedPolymorph::CastToDouble()
    {
        if(this->IsDouble())
        {
            return thePolymorph->asReal();
        }
        else
        {
			// XXX: this is not valid
            return 0.0;
        }
    }

    int WrappedPolymorph::CastToInt()
    {
        if(this->IsInt())
        {
            return thePolymorph->asInteger();
        }
        else
        {
			// XXX: this is not valid
            return 0;
        }
    }

    List<WrappedPolymorph^>^ WrappedPolymorph::CastToList()
    {
        if (this->IsList())
        {
			libecs::PolymorphVector v(*thePolymorph);
            List<WrappedPolymorph^>^ l_list = gcnew List<WrappedPolymorph^>();

			for (libecs::PolymorphVector::const_iterator i = v.begin(); i < v.end(); ++i)
			{
				l_list->Add(gcnew WrappedPolymorph(*i));
			}
				
            return l_list;
        }
        else
        {
            return nullptr;
        }
    }

	libecs::Polymorph* WrappedPolymorph::CastToPolymorph(IEnumerable<WrappedPolymorph^>^ aWrappedPolymorphList)
    {
		libecs::PolymorphVector aPolymorphVector;

		for (IEnumerator<WrappedPolymorph^>^ i = aWrappedPolymorphList->GetEnumerator();
				i->MoveNext(); )
        {
			WrappedPolymorph^ aWrappedPolymorph = i->Current;

            if(aWrappedPolymorph->IsDouble())
            {
                aPolymorphVector.push_back(libecs::Polymorph((libecs::Real)aWrappedPolymorph->CastToDouble()));
            }
            else if(aWrappedPolymorph->IsInt())
            {
                aPolymorphVector.push_back(libecs::Polymorph((libecs::Integer)aWrappedPolymorph->CastToInt()));
            }
            else if(aWrappedPolymorph->IsString())
            {
                char* aValue = reinterpret_cast<char*>(
						(void *)Marshal::StringToHGlobalAnsi(
							aWrappedPolymorph->CastToString()));
				try
				{
	                aPolymorphVector.push_back(libecs::Polymorph(aValue));
				}
				finally
				{
					Marshal::FreeHGlobal((IntPtr)aValue);
				}
            }
            else
            {
				libecs::Polymorph* m = CastToPolymorph(aWrappedPolymorph->CastToList());
                aPolymorphVector.push_back(*m);
				delete m;
            }
        }
        return new libecs::Polymorph(aPolymorphVector);
    }

    String^ WrappedPolymorph::CastToString()
    {
        if(this->IsString())
        {
            return gcnew String((thePolymorph->asString()).c_str());
        }
        else
        {
            return nullptr;
        }
    }

    const libecs::Polymorph& WrappedPolymorph::GetPolymorph()
    {
        return *thePolymorph;
    }

    bool WrappedPolymorph::IsDouble()
    {
        return thePolymorph->getType() == libecs::Polymorph::REAL;
    }

    bool WrappedPolymorph::IsInt()
    {
        return thePolymorph->getType() == libecs::Polymorph::INTEGER;
    }

    bool WrappedPolymorph::IsList()
    {
        return thePolymorph->getType() == libecs::Polymorph::POLYMORPH_VECTOR;
    }

    bool WrappedPolymorph::IsString()
    {
        return thePolymorph->getType() == libecs::Polymorph::STRING;
    }

	String^ WrappedPolymorph::ToString()
	{
		return Marshal::PtrToStringAnsi(
				(IntPtr)const_cast<char *>(thePolymorph->asString().c_str()));
	}
}
