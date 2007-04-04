//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Simulation Environment package
//
//                Copyright (C) 2000-2001 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Gabor Bereczki <gabor.bereczki@talk21.com>
//


#if !defined(__PHYSICALLOGGER_HPP)
#define __PHYSICALLOGGER_HPP

#include "libecs.hpp"

#include "VVector.h"

#include "Exceptions.hpp"
#include "DataPoint.hpp"
#include "DataPointVector.hpp"

namespace libecs
{


  /** @addtogroup logging
   *@{
   */

  /** @file */


  class PhysicalLogger
  {

    //    DECLARE_TYPE( _DATAPOINT, DATAPOINT );
    //    typedef vvector<DATAPOINT> Vector;

    typedef vvector<DataPoint> Vector;
    
  public:

    DECLARE_TYPE( Vector::size_type, VectorIterator );
    DECLARE_TYPE( Vector::size_type, size_type );

    PhysicalLogger()
    {
      setMaxSize( 0 ); // no limit
    }

    virtual ~PhysicalLogger()
    {
      ; // do nothing
    }
	
    void push( DataPointCref aDataPoint )
    {
      theVector.push_back( aDataPoint );
    }

    void setEndPolicy( Integer anEndPolicy )
    {
      theVector.setEndPolicy ( anEndPolicy );
    }

    int getEndPolicy() const
    {
      return theVector.getEndPolicy();
    }

    /// set max storage size in Kbytes.

    void setMaxSize( size_type aMaxSize )
    {
      theMaxSize = aMaxSize;
      theVector.setMaxSize( ( theMaxSize * 1024 ) /
                            sizeof( DataPoint ) );
    }

    size_type getMaxSize() const
    {
      return theMaxSize;
    }

    size_type lower_bound( const size_type start,
			   const size_type end,
			   const Real time ) const;

    size_type upper_bound( const size_type start,
			   const size_type end,
			   const Real time ) const;

    size_type lower_bound_linear( const size_type start,
				  const size_type end,
				  const Real time ) const;

    size_type upper_bound_linear( const size_type start,
				  const size_type end,
				  const Real time ) const;

    size_type lower_bound_linear_backwards( const size_type start,
					    const size_type end,
					    const Real time ) const;

    size_type lower_bound_linear_estimate( const size_type start,
					   const size_type end,
					   const Real time,
					   const Real time_per_step ) const;

    size_type upper_bound_linear_estimate( const size_type start,
					   const size_type end,
					   const Real time,
					   const Real time_per_step ) const;
    
    DataPointVectorSharedPtr getVector( const size_type start,
					const size_type end ) const;

    ECELL_API size_type size() const;

    bool empty() const;


    LongDataPoint at( size_type index) const
    {
      return theVector[ index ];
    }
    
    size_type begin() const
    {
      return 0;
    }
    
    
    size_type end() const
    {
      if ( size() > 0 )
	{
	  return size() - 1;
	}
      else
	{
	  return 0;
	}
    }


    LongDataPoint front() const
    {
      if ( empty() )
	{
	  return DataPoint();
	}
      
      return at( begin() );
    }
    
    LongDataPoint back() const
    {
      if ( empty() )
	{
	  return DataPoint();
	}
      
      return at( end() );
    }
  
    Real getAverageInterval() const;

  private:

    // this mutable can be removed if vvector supports const operations
    mutable Vector      theVector;

    size_type      theMaxSize;

  };


  //@}

} // namespace libecs


#endif /* __PHYSICALLOGGER_HPP */
