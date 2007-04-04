#include "WrappedDataPointVector.hpp"

using namespace System;

namespace EcellCoreLib
{
    WrappedDataPointVector::WrappedDataPointVector(boost::shared_ptr<libecs::DataPointVector> * l_boostDataPointVector)
    {
        libecs::DataPointVector * l_dataPointVector = l_boostDataPointVector -> get();
        WrappedDataPointVector::m_arraySize = l_dataPointVector -> end();
        if(l_dataPointVector -> getPointSize() == 2 )
        {
            WrappedDataPointVector::m_dataPoint = new libecs::DataPoint[WrappedDataPointVector::m_arraySize];
            for(int i = 0; i < WrappedDataPointVector::m_arraySize; i++)
            {
                WrappedDataPointVector::m_dataPoint[i] = l_dataPointVector -> asShort(i);
            }
            WrappedDataPointVector::m_longFlag = false;
        }
        else
        {
            WrappedDataPointVector::m_longDataPoint = new libecs::LongDataPoint[WrappedDataPointVector::m_arraySize];
            for(int i = 0; i < WrappedDataPointVector::m_arraySize; i++)
            {
                WrappedDataPointVector::m_longDataPoint[i] = l_dataPointVector -> asLong(i);
            }
            WrappedDataPointVector::m_longFlag = true;
        }
    }

    int WrappedDataPointVector::GetArraySize()
    {
        return WrappedDataPointVector::m_arraySize;
    }

    double WrappedDataPointVector::GetTime(int l_point)
    {
        if(WrappedDataPointVector::m_longFlag)
        {
            return WrappedDataPointVector::m_longDataPoint[l_point].getTime();
        }
        else
        {
            return WrappedDataPointVector::m_dataPoint[l_point].getTime();
        }
    }

    double WrappedDataPointVector::GetValue(int l_point)
    {
        if(WrappedDataPointVector::m_longFlag)
        {
            return WrappedDataPointVector::m_longDataPoint[l_point].getValue();
        }
        else
        {
            return WrappedDataPointVector::m_dataPoint[l_point].getValue();
        }
    }

    double WrappedDataPointVector::GetAvg(int l_point)
    {
        if(WrappedDataPointVector::m_longFlag)
        {
            return WrappedDataPointVector::m_longDataPoint[l_point].getAvg();
        }
        else
        {
            return Double::NaN;
        }
    }

    double WrappedDataPointVector::GetMin(int l_point)
    {
        if(WrappedDataPointVector::m_longFlag)
        {
            return WrappedDataPointVector::m_longDataPoint[l_point].getMin();
        }
        else
        {
            return Double::NaN;
        }
    }

    double WrappedDataPointVector::GetMax(int l_point)
    {
        if(WrappedDataPointVector::m_longFlag)
        {
            return WrappedDataPointVector::m_longDataPoint[l_point].getMax();
        }
        else
        {
            return Double::NaN;
        }
    }
}
