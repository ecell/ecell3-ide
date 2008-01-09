#pragma warning(disable:4793)
#include "WrappedDataPointVector.hpp"

using namespace System;

namespace EcellCoreLib
{
	WrappedDataPointVector::WrappedDataPointVector(const libecs::DataPointVectorSharedPtr& dataPointVector)
		: m_dataPointVector(new libecs::DataPointVectorSharedPtr(dataPointVector))
    {
    }

	WrappedDataPointVector::~WrappedDataPointVector()
	{
		delete m_dataPointVector;
	}

    int WrappedDataPointVector::GetArraySize()
    {
		return m_dataPointVector->get()->getSize();
    }

    double WrappedDataPointVector::GetTime(int l_point)
    {
		return m_dataPointVector->get()->getElementSize() == sizeof(libecs::DataPoint) ?
			m_dataPointVector->get()->asShort(l_point).getTime():
			m_dataPointVector->get()->asLong(l_point).getTime();
    }

    double WrappedDataPointVector::GetValue(int l_point)
    {
		return m_dataPointVector->get()->getElementSize() == sizeof(libecs::DataPoint) ?
			m_dataPointVector->get()->asShort(l_point).getValue():
			m_dataPointVector->get()->asLong(l_point).getValue();
    }

    double WrappedDataPointVector::GetAvg(int l_point)
    {
		return m_dataPointVector->get()->getElementSize() == sizeof(libecs::DataPoint) ?
			m_dataPointVector->get()->asShort(l_point).getAvg():
			m_dataPointVector->get()->asLong(l_point).getAvg();
    }

    double WrappedDataPointVector::GetMin(int l_point)
    {
		return m_dataPointVector->get()->getElementSize() == sizeof(libecs::DataPoint) ?
			m_dataPointVector->get()->asShort(l_point).getMin():
			m_dataPointVector->get()->asLong(l_point).getMin();
    }

    double WrappedDataPointVector::GetMax(int l_point)
    {
		return m_dataPointVector->get()->getElementSize() == sizeof(libecs::DataPoint) ?
			m_dataPointVector->get()->asShort(l_point).getMax():
			m_dataPointVector->get()->asLong(l_point).getMax();
    }
}
