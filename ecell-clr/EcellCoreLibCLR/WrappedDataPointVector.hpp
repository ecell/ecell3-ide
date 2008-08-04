#pragma once

#include <libecs/DataPointVector.hpp>

using namespace System;

namespace EcellCoreLib {
    public ref class WrappedDataPointVector
    {
    private:
		libecs::DataPointVectorSharedPtr* m_dataPointVector;
    public:
		virtual ~WrappedDataPointVector();
		WrappedDataPointVector(const libecs::DataPointVectorSharedPtr& l_boostDataPointVector);
        int GetArraySize();
        double GetTime(int l_point);
        double GetValue(int l_point);
        double GetAvg(int l_point);
        double GetMin(int l_point);
        double GetMax(int l_point);
    };
}
