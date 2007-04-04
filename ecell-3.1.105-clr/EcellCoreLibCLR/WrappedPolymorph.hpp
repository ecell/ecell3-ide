#pragma once

#include "libecs/Polymorph.hpp"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;


namespace EcellCoreLib {
    public ref class WrappedPolymorph {
    private:
        libecs::Polymorph * thePolymorph;
    public:
        WrappedPolymorph(double aValue);
        WrappedPolymorph(int aValue);
        WrappedPolymorph(String ^ aValue);
        WrappedPolymorph(List<WrappedPolymorph ^> ^ aWrappedPolymorphList);
        WrappedPolymorph( libecs::Polymorph * aPolymorph );
        libecs::Polymorph * GetPolymorph();
        double CastToDouble();
        int CastToInt();
        List<WrappedPolymorph ^> ^ CastToList();
        static libecs::Polymorph CastToPolymorph(List<WrappedPolymorph ^> ^ aWrappedPolymorphList);
        String ^ CastToString();
        bool IsDouble();
        bool IsInt();
        bool IsString();
        bool IsList();

        /*
        int AsInteger();
        double AsReal();
        String ^ AsString();
        int Size();
         */
    };
}
