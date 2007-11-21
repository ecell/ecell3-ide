#pragma once

#include "libecs/Polymorph.hpp"

namespace EcellCoreLib {
	using namespace System;
	using namespace System::Collections::Generic;

    public ref class WrappedPolymorph {
    private:
        libecs::Polymorph* thePolymorph;
    public:
        WrappedPolymorph(double aValue);
        WrappedPolymorph(int aValue);
        WrappedPolymorph(String ^ aValue);
        WrappedPolymorph(List<WrappedPolymorph^>^ aWrappedPolymorphList);
        WrappedPolymorph(const libecs::Polymorph& aPolymorph);
		virtual ~WrappedPolymorph();
        const libecs::Polymorph& GetPolymorph();
        double CastToDouble();
        int CastToInt();
        List<WrappedPolymorph^>^ CastToList();
        static libecs::Polymorph* CastToPolymorph(IEnumerable<WrappedPolymorph^>^ aWrappedPolymorphList);
        String^ CastToString();
        bool IsDouble();
        bool IsInt();
        bool IsString();
        bool IsList();
		virtual String^ ToString() new;
    };
}
