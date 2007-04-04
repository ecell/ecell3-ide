#include "WrappedPolymorph.hpp"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;


namespace EcellCoreLib
{
    WrappedPolymorph::WrappedPolymorph(double aValue)
    {
        WrappedPolymorph::thePolymorph = new libecs::Polymorph((libecs::Real)aValue);
    }

    WrappedPolymorph::WrappedPolymorph(int aValue)
    {
        WrappedPolymorph::thePolymorph = new libecs::Polymorph((libecs::Integer)aValue);
    }

    WrappedPolymorph::WrappedPolymorph(String ^ aValue)
    {
        WrappedPolymorph::thePolymorph = new libecs::Polymorph((char*)(void *)Marshal::StringToHGlobalAnsi(aValue));
    }

    WrappedPolymorph::WrappedPolymorph(List<WrappedPolymorph ^> ^ aWrappedPolymorphList)
    {
        std::vector<libecs::Polymorph> * aPolymorphVector = new std::vector<libecs::Polymorph>();
        for(int i=0; i<aWrappedPolymorphList->Count; i++)
        {
            if(aWrappedPolymorphList[i]->IsDouble())
            {
                aPolymorphVector->push_back(* (new libecs::Polymorph((libecs::Real)aWrappedPolymorphList[i]->CastToDouble())));
            }
            else if(aWrappedPolymorphList[i]->IsInt())
            {
                aPolymorphVector->push_back(* (new libecs::Polymorph((libecs::Integer)aWrappedPolymorphList[i]->CastToInt())));
            }
            else if(aWrappedPolymorphList[i]->IsString())
            {
                std::string aValue = (char*)(void *)Marshal::StringToHGlobalAnsi(aWrappedPolymorphList[i]->CastToString());
                aPolymorphVector->push_back(* (new libecs::Polymorph(aValue)));
            }
            else
            {
                aPolymorphVector->push_back(CastToPolymorph(aWrappedPolymorphList[i]->CastToList()));
            }
        }
        WrappedPolymorph::thePolymorph = new libecs::Polymorph(* aPolymorphVector);
    }

    WrappedPolymorph::WrappedPolymorph( libecs::Polymorph * aPolymorph )
    {
        WrappedPolymorph::thePolymorph = new libecs::Polymorph();
        * WrappedPolymorph::thePolymorph = * aPolymorph;
    }

    double WrappedPolymorph::CastToDouble()
    {
        if(this->IsDouble())
        {
            return WrappedPolymorph::thePolymorph->asReal();
        }
        else
        {
            return 0.0;
        }
    }

    int WrappedPolymorph::CastToInt()
    {
        if(this->IsInt())
        {
            return WrappedPolymorph::thePolymorph->asInteger();
        }
        else
        {
            return 0;
        }
    }

    List<WrappedPolymorph ^> ^ WrappedPolymorph::CastToList()
    {
        if(this->IsList())
        {
            List<WrappedPolymorph ^> ^ l_list = gcnew List<WrappedPolymorph ^>();
            for(unsigned int i=0; i<((std::vector<libecs::Polymorph>)*WrappedPolymorph::thePolymorph).size(); i++)
            {
                l_list->Add(gcnew WrappedPolymorph(&(libecs::Polymorph)(((std::vector<libecs::Polymorph>)*WrappedPolymorph::thePolymorph)[i])));
            }
            return l_list;
        }
        else
        {
            return nullptr;
        }
    }

    libecs::Polymorph WrappedPolymorph::CastToPolymorph(List<WrappedPolymorph ^> ^ aWrappedPolymorphList)
    {
        std::vector<libecs::Polymorph> * aPolymorphVector = new std::vector<libecs::Polymorph>();
        for(int i=0; i<aWrappedPolymorphList->Count; i++)
        {
            if(aWrappedPolymorphList[i]->IsDouble())
            {
                aPolymorphVector->push_back(* (new libecs::Polymorph((libecs::Real)aWrappedPolymorphList[i]->CastToDouble())));
            }
            else if(aWrappedPolymorphList[i]->IsInt())
            {
                aPolymorphVector->push_back(* (new libecs::Polymorph((libecs::Integer)aWrappedPolymorphList[i]->CastToInt())));
            }
            else if(aWrappedPolymorphList[i]->IsString())
            {
                std::string aValue = (char*)(void *)Marshal::StringToHGlobalAnsi(aWrappedPolymorphList[i]->CastToString());
                aPolymorphVector->push_back(* (new libecs::Polymorph(aValue)));
            }
            else
            {
                aPolymorphVector->push_back(CastToPolymorph(aWrappedPolymorphList[i]->CastToList()));
            }
        }
        return * (new libecs::Polymorph(* aPolymorphVector));
    }

    String ^ WrappedPolymorph::CastToString()
    {
        if(this->IsString())
        {
            return gcnew String((WrappedPolymorph::thePolymorph->asString()).c_str());
        }
        else
        {
            return nullptr;
        }
    }

    libecs::Polymorph * WrappedPolymorph::GetPolymorph()
    {
        return WrappedPolymorph::thePolymorph;
    }

    bool WrappedPolymorph::IsDouble()
    {
        if(WrappedPolymorph::thePolymorph->getType() == libecs::Polymorph::REAL)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool WrappedPolymorph::IsInt()
    {
        if(WrappedPolymorph::thePolymorph->getType() == libecs::Polymorph::INTEGER)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool WrappedPolymorph::IsList()
    {
        if(WrappedPolymorph::thePolymorph->getType() == libecs::Polymorph::POLYMORPH_VECTOR)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool WrappedPolymorph::IsString()
    {
        if(WrappedPolymorph::thePolymorph->getType() == libecs::Polymorph::STRING)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
    int WrappedPolymorph::AsInteger() {
        return WrappedPolymorph::thePolymorph -> asInteger();
    }

    double WrappedPolymorph::AsReal() {
        return WrappedPolymorph::thePolymorph -> asReal();
    }

    String ^ WrappedPolymorph::AsString() {
        return gcnew String( ( WrappedPolymorph::thePolymorph -> asString() ).c_str() );
    }

    int WrappedPolymorph::Size() {
        if( IsList() ) {
//            std::vector<libecs::Polymorph> aVector = ( std::vector<libecs::Polymorph> )( * WrappedPolymorph::thePolymorph );
//            int i = aVector.size();
            return ( ( std::vector<libecs::Polymorph> )( * WrappedPolymorph::thePolymorph ) ).size();
        } else {
            return 0;
        }
    }
     */
}
