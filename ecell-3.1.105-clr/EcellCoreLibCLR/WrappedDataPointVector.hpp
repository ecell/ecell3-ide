#pragma once

#include "libecs/DataPointVector.hpp"

using namespace System;

namespace EcellCoreLib {
    public ref class WrappedDataPointVector
    {
    private:
        libecs::LongDataPoint * m_longDataPoint;
        libecs::DataPoint * m_dataPoint;
        int m_arraySize;
        bool m_longFlag;
    public:
        WrappedDataPointVector(boost::shared_ptr<libecs::DataPointVector> * l_boostDataPointVector);
        int GetArraySize();
        double GetTime(int l_point);
        double GetValue(int l_point);
        double GetAvg(int l_point);
        double GetMin(int l_point);
        double GetMax(int l_point);
    };
}
